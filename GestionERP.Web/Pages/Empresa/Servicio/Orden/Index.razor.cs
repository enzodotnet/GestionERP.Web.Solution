using Microsoft.AspNetCore.Components;
using GestionERP.Web.Models.Dtos.Servicio;
using GestionERP.Web.Components;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Services;
using GestionERP.Web.Handlers;
using System.Security.Claims;
using GestionERP.Web.Models.Dtos.Principal;
using Telerik.Blazor;
using Microsoft.AspNetCore.Components.Routing; 

namespace GestionERP.Web.Pages.Empresa.Servicio.Orden;

public partial class Index : IDisposable
{
    #region Propiedades
    private const string codigoModulo = "03";
    private const string codigoServicio = "S109";
    private const string rutaServicio = "/ordenes";
    private string rutaEmpresa = "";

    private IEnumerable<OrdenListarDto> ListaOrdenes { get; set; }
    private bool EsVisibleDialogEliminar { get; set; }
    private string CodigoRegistro { get; set; }
    public string CodigoEjercicio { get; set; }
    public string CodigoPeriodo { get; set; }
    private Guid? RegistroId { get; set; } 
    private bool EsAsignadoEliminar { get; set; }
    private bool EsAsignadoEditar { get; set; }
    private bool EsAsignadoEmitirDirecta { get; set; }
    private bool EsAsignadoEmitirPorSolicitud { get; set; }
    private bool IsInitGrid { get; set; }
    private bool IsLoadingAction { get; set; }
    private int NumeroMesPeriodo { get; set; }
    public string FlagOrigen { get; set; }
    private bool IsAuthUser { get; set; }
    private IEnumerable<OrdenFlag> Origenes { get; set; }
    private IEnumerable<EmpresaPeriodoCatalogoDto> CatalogoPeriodos { get; set; }
    private IEnumerable<EmpresaEjercicioCatalogoDto> CatalogoEjercicios { get; set; }
    public EmpresaConsultaPorCodigoWebDto Empresa { get; set; }
    [Parameter] public string CodigoWebEmpresa { get; set; }
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    #endregion

    [Inject] public IServicioOrden IOrden { get; set; }
    [Inject] public IPrincipalEmpresa IEmpresa { get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        { 
            Notify.ShowLoading(mensaje: "Listando registro(s)");

            Origenes = OrdenFlag.Origenes();

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio);
            if (!IsAuthUser) return;

            Empresa = await IEmpresa.ConsultaPorCodigoWeb(CodigoWebEmpresa);
                             
            EsAsignadoEditar = await IPermiso.ConsultaEsAsignadoPorSesion(OrdenAcceso.Editar, Empresa.Codigo);
            EsAsignadoEliminar = await IPermiso.ConsultaEsAsignadoPorSesion(OrdenAcceso.Eliminar, Empresa.Codigo);
            EsAsignadoEmitirDirecta = await IPermiso.ConsultaEsAsignadoPorSesion(OrdenAcceso.EmitirDirecta, Empresa.Codigo);
            EsAsignadoEmitirPorSolicitud = await IPermiso.ConsultaEsAsignadoPorSesion(OrdenAcceso.EmitirPorSolicitud, Empresa.Codigo);

            rutaEmpresa = INavigation.Uri.Replace(INavigation.BaseUri, "").Split("?")[0].Replace(rutaServicio, "");

            CatalogoEjercicios = await IEmpresa.CatalogoEjercicios(Empresa.Codigo) ?? [];
            CodigoEjercicio = await IEmpresa.ConsultaEjercicioCodigoPorAnio(Empresa.Codigo, DateTime.Now.Year);
            CatalogoPeriodos = [];
            if (!string.IsNullOrEmpty(CodigoEjercicio))
            {
                CatalogoPeriodos = await IEmpresa.CatalogoPeriodos(Empresa.Codigo, CodigoEjercicio) ?? [];
                CodigoPeriodo = await IEmpresa.ConsultaPeriodoCodigoPorFecha(Empresa.Codigo, DateTime.Now);
                NumeroMesPeriodo = CatalogoPeriodos.Where(x => x.CodigoPeriodo == CodigoPeriodo).Select(x => x.NumeroMes).FirstOrDefault();
            } 
            await Listar();
        }
        catch (Exception ex)
        {
            if (ex is HttpRequestException)
                Notify.ShowError("NC");
            else if (ex is HttpResponseException)
            {
                string codeError = (ex as HttpResponseException).Code; 
				if (codeError == "AU")
                    INavigation.NavigateTo($"{rutaEmpresa}/inicio");
                Notify.ShowError(codeError, ex);
            }
            else 
                Notify.ShowError("FA", ex);
        }
        finally
        {
            Notify.ShowLoading(false);
        }    
    }

    protected async Task Eliminar()
    {
        try
        {
            EsVisibleDialogEliminar = false;
            IsLoadingAction = true;

            IsAuthUser = (await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio, User.FindFirst("code").Value)).esValido;
            if (!IsAuthUser) return;

            Notify.ShowLoading(mensaje: "Eliminación en progreso");

            await IOrden.Eliminar(Empresa.Codigo, (Guid) RegistroId);
            await Listar();
 
            Notify.Show($"La orden {CodigoRegistro} ha sido eliminada con éxito de la empresa", "success");
        }
        catch (Exception ex)
        {
            if (ex is HttpRequestException)
                Notify.ShowError("NC");
            else if (ex is HttpResponseException)
                Notify.ShowError((ex as HttpResponseException).Code, ex);
            else
                Notify.ShowError("FA", ex);
        }
        finally
        {
            IsLoadingAction = false;
            Notify.ShowLoading(false);
        }
    }

    private async Task RefrescarLista()
    {
        try
        {
            IsAuthUser = (await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio, User.FindFirst("code").Value)).esValido;
            if (!IsAuthUser) return;

            Notify.ShowLoading(mensaje: "Refrescando lista");

            await Listar(); 
        }
        catch (Exception ex)
        {
            if (ex is HttpRequestException)
                Notify.ShowError("NC");
            else if (ex is HttpResponseException)
            {
                string codeError = (ex as HttpResponseException).Code; 
				if (codeError == "AU")
                    INavigation.NavigateTo($"{rutaEmpresa}/inicio");
                Notify.ShowError(codeError, ex);
            }
            else 
                Notify.ShowError("FA", ex);
        }
        finally
        {
            Notify.ShowLoading(false);
        }
    }

    private async Task Cerrar(LocationChangingContext context)
    {
        if (IsAuthUser && IsLoadingAction && !await Dialog.ConfirmAsync("¿Está seguro de salir del formulario de listado sin haber culminado la eliminación?", "Saliendo del formulario"))
            context.PreventNavigation();
    }

    private async Task OnComboEjercicioValueChanged(string value)
    {
        CodigoEjercicio = value;
        if (!string.IsNullOrEmpty(CodigoEjercicio))
        {
            CatalogoPeriodos = await IEmpresa.CatalogoPeriodos(Empresa.Codigo, CodigoEjercicio) ?? [];
            CodigoPeriodo = CatalogoPeriodos.Where(x => x.NumeroMes == NumeroMesPeriodo).Select(x => x.CodigoPeriodo).FirstOrDefault();
            await RefrescarLista();
        }
        else
        { 
            CatalogoPeriodos = null;
        }
    }

    private async Task OnComboPeriodoValueChanged(string value)
    {
        CodigoPeriodo = value;
        NumeroMesPeriodo = CatalogoPeriodos.Where(x => x.CodigoPeriodo == CodigoPeriodo).Select(x => x.NumeroMes).FirstOrDefault();
        await RefrescarLista();
    }

    protected void IrEmitirDirecta() => INavigation.NavigateTo(INavigation.GetUriWithQueryParameters($"{rutaEmpresa}{rutaServicio}/emitir/directa", new Dictionary<string, object> { ["returnpage"] = "index" }));
    
    protected void IrEmitirPorSolicitud() => INavigation.NavigateTo(INavigation.GetUriWithQueryParameters($"{rutaEmpresa}{rutaServicio}/emitir/solicitud", new Dictionary<string, object> { ["returnpage"] = "index" }));

    protected void IrVer(Guid id) => INavigation.NavigateTo($"{rutaEmpresa}{rutaServicio}/{id}");

    protected void IrEditar(Guid id) => INavigation.NavigateTo(INavigation.GetUriWithQueryParameters($"{rutaEmpresa}{rutaServicio}/{id}/editar", new Dictionary<string, object> { ["returnpage"] = "index" }));

    protected void MostrarEliminar(bool visible, Guid? id = null)
    {
        RegistroId = id;
        CodigoRegistro = visible ? ListaOrdenes.Where(x => x.Id == (Guid) id).Select(x => x.Codigo).FirstOrDefault().Trim() : null;
        EsVisibleDialogEliminar = visible;
    }

    private async Task Listar()
    {
        ListaOrdenes = await IOrden.Listar(Empresa.Codigo, CodigoEjercicio, CodigoPeriodo, FlagOrigen); 
        IsInitGrid = true; 
    }

    private async Task OnComboOrigenValueChanged(string value)
    {
        FlagOrigen = value; 
        await RefrescarLista();
    }

    public void Dispose() => GC.SuppressFinalize(this); 
}
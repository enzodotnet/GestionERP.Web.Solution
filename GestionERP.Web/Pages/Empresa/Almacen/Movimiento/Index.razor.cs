using Microsoft.AspNetCore.Components; 
using GestionERP.Web.Models.Dtos.Almacen;
using GestionERP.Web.Components;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Services;
using GestionERP.Web.Handlers;
using System.Security.Claims; 
using GestionERP.Web.Models.Dtos.Principal; 
using Telerik.Blazor;
using Microsoft.AspNetCore.Components.Routing;

namespace GestionERP.Web.Pages.Empresa.Almacen.Movimiento;

public partial class Index : IDisposable
{
    #region Propiedades
    private const string codigoModulo = "04";
    private const string codigoServicio = "S130";
    private const string rutaServicio = "/movimientos";
    private string rutaEmpresa = "";
     
    private IEnumerable<MovimientoListarDto> ListaMovimientos { get; set; }
    private bool EsVisibleDialogEliminar { get; set; }
    private string CodigoRegistro { get; set; }
    public string CodigoEjercicio { get; set; }
    public string CodigoPeriodo { get; set; }
    private Guid? RegistroId { get; set; } 
    private bool EsAsignadoEliminar { get; set; } 
    private bool EsAsignadoRegistrarIngreso { get; set; }
    private bool EsAsignadoRegistrarSalida { get; set; }
    private bool EsAsignadoRegistrarTransferencia { get; set; }
    private bool IsInitGrid { get; set; }
    private bool IsLoadingAction { get; set; }
    private int NumeroMesPeriodo { get; set; }
    public string FlagTipoRegistro { get; set; }
    private bool IsAuthUser { get; set; }
    private IEnumerable<MovimientoFlag> TiposRegistro { get; set; }
    private IEnumerable<EmpresaPeriodoCatalogoDto> CatalogoPeriodos { get; set; }
    private IEnumerable<EmpresaEjercicioCatalogoDto> CatalogoEjercicios { get; set; }
    public EmpresaConsultaPorCodigoWebDto Empresa { get; set; }
    [Parameter] public string CodigoWebEmpresa { get; set; } 
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    #endregion

    [Inject] public IAlmacenMovimiento IMovimiento { get; set; }
    [Inject] public IPrincipalEmpresa IEmpresa { get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        { 
            Notify.ShowLoading(mensaje: "Listando registro(s)");

            TiposRegistro = MovimientoFlag.TiposRegistro();

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio);
            if (!IsAuthUser) return;

            Empresa = await IEmpresa.ConsultaPorCodigoWeb(CodigoWebEmpresa);
                              
            EsAsignadoEliminar = await IPermiso.ConsultaEsAsignadoPorSesion(MovimientoAcceso.Eliminar, Empresa.Codigo);
            EsAsignadoRegistrarIngreso = await IPermiso.ConsultaEsAsignadoPorSesion(MovimientoAcceso.RegistrarIngreso, Empresa.Codigo);
            EsAsignadoRegistrarSalida = await IPermiso.ConsultaEsAsignadoPorSesion(MovimientoAcceso.RegistrarSalida, Empresa.Codigo);
            EsAsignadoRegistrarTransferencia = await IPermiso.ConsultaEsAsignadoPorSesion(MovimientoAcceso.RegistrarTransferencia, Empresa.Codigo);

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

            await IMovimiento.Eliminar(Empresa.Codigo, (Guid) RegistroId); 
            await Listar();

            Notify.Show($"El movimiento {CodigoRegistro} ha sido eliminada con éxito de la empresa", "success");
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

    protected void IrRegistrarIngreso() => INavigation.NavigateTo(INavigation.GetUriWithQueryParameters($"{rutaEmpresa}{rutaServicio}/registrar/ingreso", new Dictionary<string, object> { ["returnpage"] = "index" }));

    protected void IrRegistrarSalida() => INavigation.NavigateTo(INavigation.GetUriWithQueryParameters($"{rutaEmpresa}{rutaServicio}/registrar/salida", new Dictionary<string, object> { ["returnpage"] = "index" }));

    protected void IrRegistrarTransferencia() => INavigation.NavigateTo(INavigation.GetUriWithQueryParameters($"{rutaEmpresa}{rutaServicio}/registrar/transferencia", new Dictionary<string, object> { ["returnpage"] = "index" }));

    protected void IrVer(Guid id) => INavigation.NavigateTo($"{rutaEmpresa}{rutaServicio}/{id}");
     
    protected void MostrarEliminar(bool visible, Guid? id = null)
    {
        RegistroId = id;
        CodigoRegistro = visible ? ListaMovimientos.Where(x => x.Id == (Guid) id).Select(x => string.Concat(x.CodigoPeriodo, "-", x.Codigo)).FirstOrDefault().Trim() : null;
        EsVisibleDialogEliminar = visible;
    }

    private async Task OnComboTipoRegistroValueChanged(string value)
    {
        FlagTipoRegistro = value;
        await RefrescarLista();
    }

    private async Task Listar()
    {
        ListaMovimientos = await IMovimiento.Listar(Empresa.Codigo, CodigoEjercicio, CodigoPeriodo, FlagTipoRegistro); 
        IsInitGrid = true;
    } 

    public void Dispose() => GC.SuppressFinalize(this); 
}
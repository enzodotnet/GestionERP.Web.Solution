using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Models.Dtos.Principal.Types;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components;
using GestionERP.Web.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Routing;
using Telerik.Blazor;

namespace GestionERP.Web.Pages.Principal.CuentaContable;

public partial class View : IDisposable
{
    #region Propiedades
    private const string codigoServicio = "S004";
    public CuentaContableObtenerDto CuentaContableObtener { get; set; }
    public CuentaContableDetalleObtenerDto Detalle { get; set; }
    public CuentaContableDestinoObtenerDto DestinoObtener { get; set; }
    public bool EsVisibleModalDetalle { get; set; } = false;
    public bool EsVisibleModalDestino { get; set; } = false;
    private IEnumerable<CuentaContableTipoCuentaCorrienteType> TiposCuentaCorriente { get; set; }
    private IEnumerable<CuentaContableTipoEntidadFinancieraType> TiposEntidadFinanciera { get; set; }
    private IEnumerable<CuentaContableTipoNaturalezaType> TiposNaturaleza { get; set; }
    private IEnumerable<CuentaContableTipoType> Tipos { get; set; }
    private IEnumerable<CuentaContableFormatoFuncionType> FormatosFuncion { get; set; }
    private IEnumerable<CuentaContableDetalleTipoType> DetalleTipos { get; set; }
    private IEnumerable<CuentaContableDestinoTipoType> DestinoTipos { get; set; }
    private IEnumerable<CuentaContableDetalleTipoCambioType> DetalleTiposCambio { get; set; }
    private IEnumerable<CuentaContableDetalleRegistroType> DetalleRegistros { get; set; }
    private bool EsVisibleDialogEliminar { get; set; }
    private bool EsAsignadoEliminar { get; set; }
    private bool EsAsignadoEditar { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool IsAuthUser { get; set; }
    [Parameter] public Guid? Id { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    #endregion

    [Inject] public IPrincipalCuentaContable ICuentaContable { get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Notify.ShowLoading(mensaje: "Obteniendo registro"); 
            Detalle = new();
            DestinoObtener = new();

            TiposCuentaCorriente = CuentaContableTipoCuentaCorrienteType.ObtenerTipos();
            TiposEntidadFinanciera = CuentaContableTipoEntidadFinancieraType.ObtenerTipos();
            TiposNaturaleza = CuentaContableTipoNaturalezaType.ObtenerTipos();
            Tipos = CuentaContableTipoType.ObtenerTipos();
            FormatosFuncion = CuentaContableFormatoFuncionType.ObtenerTipos();
            DetalleTipos = CuentaContableDetalleTipoType.ObtenerTipos();
            DetalleTiposCambio = CuentaContableDetalleTipoCambioType.ObtenerTipos();
            DetalleRegistros = CuentaContableDetalleRegistroType.ObtenerTipos();
            DestinoTipos = CuentaContableDestinoTipoType.ObtenerTipos();

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio);
            if (!IsAuthUser) return;
                   
            EsAsignadoEditar = await IPermiso.ConsultaEsAsignadoPorSesion(CuentaContableAcceso.Editar);
            EsAsignadoEliminar = await IPermiso.ConsultaEsAsignadoPorSesion(CuentaContableAcceso.Eliminar);

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(CuentaContableAcceso.VerRegistros))
            {
                INavigation.NavigateTo("inicio");
                Notify.Show("No tiene permiso para ver registros del servicio principal de [Cuentas Contables]", "error");
                return;
            }

            CuentaContableObtener = await ICuentaContable.Obtener((Guid) Id); 
            if (CuentaContableObtener is null)
            {
                INavigation.NavigateTo("cuentas-contables");
                Notify.Show($"El registro de la [Cuenta Contable] consultado a visualizar no está disponible", "error");
            }
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
            Notify.ShowLoading(false);
        }
    }

    private async Task Eliminar()
    {
        try
        {
            EsVisibleDialogEliminar = false;
            IsLoadingAction = true;
            
            IsAuthUser = (await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio, codigoUser: User.FindFirst("code").Value)).esValido;
            if (!IsAuthUser) return;

            Notify.ShowLoading(mensaje: "Eliminación en progreso"); 

            await ICuentaContable.Eliminar((Guid) Id);
            IsLoadingAction = false;
            INavigation.NavigateTo("cuentas-contables");
            Notify.Show($"La cuenta contable {CuentaContableObtener.Codigo.Trim()} ha sido eliminado con éxito", "success");
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

    public async Task Cerrar(LocationChangingContext context)
    {
        if (IsAuthUser && IsLoadingAction && !await Dialog.ConfirmAsync("¿Está seguro de salir del formulario sin haber culminado la acción?", "Saliendo del formulario"))
            context.PreventNavigation();
    }

    public void VerItemDetalle(CuentaContableDetalleObtenerDto item)
    {
        Detalle = item;
        VisibleDetalleChangedHandler(true);
    }

    public void VerItemDestino(CuentaContableDestinoObtenerDto item)
    {
        DestinoObtener = item;
        EsVisibleModalDestino = true;
    }

    private void Volver() => INavigation.NavigateTo("cuentas-contables");

    private void VisibleDetalleChangedHandler(bool esVisible) => EsVisibleModalDetalle = esVisible;

    private void VisibleDestinoChangedHandler(bool esVisible) => EsVisibleModalDestino = esVisible;

    private void IrEditar() => INavigation.NavigateTo(INavigation.GetUriWithQueryParameters($"cuentas-contables/{Id}/editar", new Dictionary<string, object> { ["returnpage"] = "view" }));

    public void Dispose() => GC.SuppressFinalize(this);
}
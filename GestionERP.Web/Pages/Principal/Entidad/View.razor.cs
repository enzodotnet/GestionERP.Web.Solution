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

namespace GestionERP.Web.Pages.Principal.Entidad;

public partial class View : IDisposable
{
    #region Propiedades
    private const string codigoServicio = "S002";
    public EntidadObtenerDto EntidadObtener { get; set; }
    public EntidadDireccionObtenerDto DireccionObtener { get; set; }
    public EntidadDireccionObtenerEsPredeterminadoDto DireccionPredeterminadoObtener { get; set; }
    public EntidadRepresentanteObtenerDto RepresentanteObtener { get; set; }
    public EntidadVehiculoObtenerDto VehiculoObtener { get; set; }
    public bool EsVisibleListaDireccion { get; set; }
    public bool EsVisibleListaVehiculo { get; set; }
    public bool EsVisibleModalDireccion { get; set; } = false;
    public bool EsVisibleModalRepresentante { get; set; } = false;
    public bool EsVisibleModalVehiculo { get; set; } = false;
    private IEnumerable<EntidadTipoPersonaType> TiposPersona { get; set; }
    private IEnumerable<EntidadFichaSunatCondicionContribuyenteType> CondicionesContribuyente { get; set; }
    private bool EsVisibleDialogEliminar { get; set; }
    private bool EsAsignadoEliminar { get; set; }
    private bool EsAsignadoEditar { get; set; }
    public bool EsVisibleTabFichaSunat { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool IsAuthUser { get; set; }
    [Parameter] public Guid? Id { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    #endregion

    [Inject] public IPrincipalEntidad IEntidad { get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Notify.ShowLoading(mensaje: "Obteniendo registro"); 
            DireccionObtener = new();
            DireccionPredeterminadoObtener = new();
            RepresentanteObtener = new();
            VehiculoObtener = new();

            TiposPersona = EntidadTipoPersonaType.ObtenerTipos();
            CondicionesContribuyente = EntidadFichaSunatCondicionContribuyenteType.ObtenerTipos();

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio);
            if (!IsAuthUser) return;
                
            EsAsignadoEditar = await IPermiso.ConsultaEsAsignadoPorSesion(EntidadAcceso.Editar);
            EsAsignadoEliminar = await IPermiso.ConsultaEsAsignadoPorSesion(EntidadAcceso.Eliminar);

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(EntidadAcceso.VerRegistros))
            {
                INavigation.NavigateTo("inicio");
                Notify.Show("No tiene permiso para ver registros del servicio principal de [Entidades]", "error");
                return;
            }

            EntidadObtener = await IEntidad.Obtener((Guid) Id);
            if (EntidadObtener is null)
            {
                INavigation.NavigateTo("entidades");
                Notify.Show($"El registro de la [Entidad] consultado a visualizar no está disponible", "error");
                return;
            }

            DireccionPredeterminadoObtener = await IEntidad.ObtenerDireccionEsPredeterminado(EntidadObtener.Codigo) ?? new(); 

            EsVisibleTabFichaSunat = EntidadObtener.FlagTipoPersona == "JU";
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

            await IEntidad.Eliminar((Guid) Id);
            IsLoadingAction = false;
            INavigation.NavigateTo("entidades");
            Notify.Show($"La entidad {EntidadObtener.Codigo} ha sido eliminada con éxito", "success");
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

    public void VerItemDireccion(EntidadDireccionObtenerDto item)
    {
        DireccionObtener = item;
        EsVisibleModalDireccion = true;
    }

    public void VerItemRepresentante(EntidadRepresentanteObtenerDto item)
    {
        RepresentanteObtener = item;
        EsVisibleModalRepresentante = true;
    }

    public void VerItemVehiculo(EntidadVehiculoObtenerDto item)
    {
        VehiculoObtener = item;
        EsVisibleModalVehiculo = true;
    }

    private void Volver() => INavigation.NavigateTo("entidades");

    private void VisibleListaDireccionChangedHandler(bool esVisible) => EsVisibleListaDireccion = esVisible;

    private void VisibleDireccionChangedHandler(bool esVisible) => EsVisibleModalDireccion = esVisible;

    private void VisibleListaVehiculoChangedHandler(bool esVisible) => EsVisibleListaVehiculo = esVisible;

    private void VisibleRepresentanteChangedHandler(bool esVisible) => EsVisibleModalRepresentante = esVisible;

    private void VisibleVehiculoChangedHandler(bool esVisible) => EsVisibleModalVehiculo = esVisible;

    private void IrEditar() => INavigation.NavigateTo(INavigation.GetUriWithQueryParameters($"entidades/{Id}/editar", new Dictionary<string, object> { ["returnpage"] = "view" }));

    public void Dispose() => GC.SuppressFinalize(this);
}
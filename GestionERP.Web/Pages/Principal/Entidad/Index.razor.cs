using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components;
using GestionERP.Web.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Routing;
using Telerik.Blazor;

namespace GestionERP.Web.Pages.Principal.Entidad;

public partial class Index : IDisposable
{
    #region Propiedades
    private const string codigoServicio = "S002";
    private IEnumerable<EntidadListarDto> ListaEntidades { get; set; }
    public bool EsTransportista { get; set; }
    private bool EsVisibleDialogEliminar { get; set; }
    private string CodigoRegistro { get; set; }
    private Guid? RegistroId { get; set; } 
    private bool EsAsignadoEliminar { get; set; }
    private bool EsAsignadoEditar { get; set; }
    private bool EsAsignadoInsertar { get; set; }
    private bool IsInitGrid { get; set; }
    private bool IsAuthUser { get; set; }
    private bool IsLoadingAction { get; set; }
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
            Notify.ShowLoading(mensaje: "Listando registro(s)");

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio);
            if (!IsAuthUser) return;
                  
            EsAsignadoEditar = await IPermiso.ConsultaEsAsignadoPorSesion(EntidadAcceso.Editar);
            EsAsignadoEliminar = await IPermiso.ConsultaEsAsignadoPorSesion(EntidadAcceso.Eliminar);
            EsAsignadoInsertar = await IPermiso.ConsultaEsAsignadoPorSesion(EntidadAcceso.Insertar);

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(EntidadAcceso.VerRegistros))
            {
                INavigation.NavigateTo("inicio");
                Notify.Show("No tiene permiso para ver registros del servicio principal de [Entidades]", "error");
                return;
            }

            await Listar();
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

            await IEntidad.Eliminar((Guid) RegistroId);
            await Listar();
 
            Notify.Show($"La entidad {CodigoRegistro} ha sido eliminada con éxito", "success");
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

    private void IrInsertar() => INavigation.NavigateTo(INavigation.GetUriWithQueryParameters("entidades/insertar", new Dictionary<string, object> { ["returnpage"] = "index" }));

    private void IrVer(Guid id) => INavigation.NavigateTo($"entidades/{id}");

    private void IrEditar(Guid id) => INavigation.NavigateTo(INavigation.GetUriWithQueryParameters($"entidades/{id}/editar", new Dictionary<string, object> { ["returnpage"] = "index" }));

    public async Task Cerrar(LocationChangingContext context)
    {
        if (IsAuthUser && IsLoadingAction && !await Dialog.ConfirmAsync("¿Está seguro de salir del formulario sin haber culminado la acción?", "Saliendo del formulario"))
            context.PreventNavigation();
    }

    private void MostrarEliminar(bool visible, Guid? id = null)
    {
        RegistroId = id;
        CodigoRegistro = visible ? ListaEntidades.Where(x => x.Id == (Guid) id).Select(x => x.Codigo).FirstOrDefault().Trim() : null;
        EsVisibleDialogEliminar = visible;
    }

    private async Task Listar(bool esTransportista = false)
    {
        ListaEntidades = await IEntidad.Listar(esTransportista); 
        IsInitGrid = true; 
    }

    private async Task OnCheckBoxEsTransportistaValueChanged(bool value)
    {
        EsTransportista = value;
        await Listar(EsTransportista);
    }

    public void Dispose() => GC.SuppressFinalize(this);
}
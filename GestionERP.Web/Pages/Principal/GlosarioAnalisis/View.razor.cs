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

namespace GestionERP.Web.Pages.Principal.GlosarioAnalisis;

public partial class View : IDisposable
{
    #region Propiedades
    private const string codigoServicio = "S018";
    public GlosarioAnalisisObtenerDto GlosarioAnalisisObtener { get; set; }
    private IEnumerable<GlosarioAnalisisRegistroType> Registros { get; set; }
    private IEnumerable<GlosarioAnalisisIdiomaOriginalType> IdiomasOriginal { get; set; }
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

    [Inject] public IPrincipalGlosarioAnalisis IGlosarioAnalisis { get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; } 
    [Inject] public NavigationManager INavigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Notify.ShowLoading(mensaje: "Obteniendo registro"); 

            Registros = GlosarioAnalisisRegistroType.ObtenerTipos();
            IdiomasOriginal = GlosarioAnalisisIdiomaOriginalType.ObtenerTipos();

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio);
            if (!IsAuthUser) return;
                
            EsAsignadoEditar = await IPermiso.ConsultaEsAsignadoPorSesion(GlosarioAnalisisAcceso.Editar);
            EsAsignadoEliminar = await IPermiso.ConsultaEsAsignadoPorSesion(GlosarioAnalisisAcceso.Eliminar);

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(GlosarioAnalisisAcceso.VerRegistros))
            {
                INavigation.NavigateTo("inicio");
                Notify.Show("No tiene permiso para ver registros del servicio principal de [Glosarios de Análisis]", "error");
                return;
            }

            GlosarioAnalisisObtener = await IGlosarioAnalisis.Obtener((Guid) Id);
            if (GlosarioAnalisisObtener is null)
            {
                INavigation.NavigateTo("glosarios-analisis");
                Notify.Show("El registro del [Glosario de Análisis] consultado a visualizar no está disponible", "error");
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

            await IGlosarioAnalisis.Eliminar((Guid) Id);
            IsLoadingAction = false;
            INavigation.NavigateTo("glosarios-analisis");
            Notify.Show($"El glosario de análisis {GlosarioAnalisisObtener.Codigo.Trim()} ha sido eliminado con éxito", "success");
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

    private void Volver() => INavigation.NavigateTo("glosarios-analisis");

    private void IrEditar() => INavigation.NavigateTo(INavigation.GetUriWithQueryParameters($"glosarios-analisis/{Id}/editar", new Dictionary<string, object> { ["returnpage"] = "view" }));

    public void Dispose() => GC.SuppressFinalize(this);
}
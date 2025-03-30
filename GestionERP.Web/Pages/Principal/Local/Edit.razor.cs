
using AutoMapper;
using Blazored.FluentValidation;
using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components;
using GestionERP.Web.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Telerik.Blazor;
using Telerik.Blazor.Components;
using GestionERP.Web.Global;

namespace GestionERP.Web.Pages.Principal.Local;

public partial class Edit : IDisposable
{
    #region Propiedades
    private const string codigoServicio = "S012";
    public FluentValidationValidator validator;
    public LocalEditarDto LocalEditar { get; set; }
    public LocalObtenerDto LocalObtener { get; set; }
    public bool EsVisibleAlert { get; set; }
    public string MensajeAlert { get; set; } 
    private EditContext EditContext { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool IsAuthUser { get; set; }
    private bool IsModified { get; set; }
    private bool EsVisibleVolver { get; set; }
    private bool IsInitPage { get; set; }
    public TelerikNotification Alert { get; set; }
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    [Parameter] public Guid? Id { get; set; }
    [Parameter][SupplyParameterFromQuery(Name = "returnpage")] public string ReturnPage { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    #endregion

    [Inject] public IPrincipalLocal ILocal { get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }
    [Inject] public IMapper IMapper { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Notify.ShowLoading(mensaje: "Obteniendo registro"); 
            LocalEditar = new();

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio);
            if (!IsAuthUser) return;
                
            EsVisibleVolver = !string.IsNullOrEmpty(ReturnPage) && ReturnPage is "index" or "view";
                    
            if (!await IPermiso.ConsultaEsAsignadoPorSesion(LocalAcceso.Editar))
            {
                INavigation.NavigateTo("locales");
                Notify.Show("No tiene permiso para editar registros de [Locales]", "error");
                return;
            }

            LocalObtener = await ILocal.Obtener((Guid) Id); 
            if (LocalObtener is null)
            {
                INavigation.NavigateTo("locales");
                Notify.Show("El registro del [Local] consultado a editar no existe", "error");
                return;
            }
            LocalEditar = IMapper.Map<LocalEditarDto>(LocalObtener);

            EditContext = new EditContext(LocalEditar);
            IsInitPage = true;
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

    private async Task Editar()
    {
        try
        {
            IsLoadingAction = true;

            IsAuthUser = (await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio, codigoUser: User.FindFirst("code").Value)).esValido;
            if (!IsAuthUser) return;
            if (!EditContext.Validate())
            {
                Fnc.MostrarAlerta(Alert, Cnf.MsgErrorInvalidEditContext, "error");
                return;
            }
             
            Notify.ShowLoading(mensaje: "Actualización en progreso");

            await ILocal.Editar((Guid) Id, LocalEditar);

            IsModified = false;
            Notify.Show("El local ha sido editado con éxito", "success");
            INavigation.NavigateTo($"locales/{Id}");
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

    private async Task Cerrar(LocationChangingContext context)
    {
        if (IsAuthUser && IsModified && !await Dialog.ConfirmAsync("¿Está seguro de salir del formulario de editar sin haber actualizado?", "Saliendo del formulario"))
            context.PreventNavigation();
    }


    private void Volver() => INavigation.NavigateTo($"locales{(ReturnPage == "view" ? $"/{Id}" : "")}");

    #region Catalogos
    private void CargarItemCatalogoRegion(RegionCatalogoDto item)
    {
        if (LocalObtener.CodigoRegion != item.CodigoRegion)
        {
            if (item.CodigoRegion.Trim() != LocalObtener.CodigoRegion?.Trim())
            {
                LocalObtener.CodigoProvincia = LocalObtener.CodigoDistrito = null;
                LocalObtener.NombreProvincia = LocalObtener.NombreDistrito = "";
            }
            LocalObtener.CodigoRegion = item.CodigoRegion;
            LocalObtener.NombreRegion = item.NombreRegion;
            IsModified = true;
        } 
    }

    private void CargarItemCatalogoProvinciaPorRegion(ProvinciaCatalogoPorRegionDto item)
    {
        if (LocalObtener.CodigoRegion != item.CodigoProvincia)
        {
            if (item.CodigoProvincia.Trim() != LocalObtener.CodigoProvincia?.Trim())
            {
                LocalEditar.CodigoDistrito = null;
                LocalObtener.NombreDistrito = "";
            }
            LocalObtener.CodigoProvincia = item.CodigoProvincia;
            LocalObtener.NombreProvincia = item.NombreProvincia;
            IsModified = true;
        } 
    }

    private void CargarItemCatalogoDistritoPorProvincia(DistritoCatalogoPorProvinciaDto item)
    {
        if (LocalEditar.CodigoDistrito != item.CodigoDistrito)
        {
            LocalEditar.CodigoDistrito = item.CodigoDistrito;
            LocalObtener.NombreDistrito = item.NombreDistrito; 
            IsModified = true;
        } 
    }
    #endregion

    public void Dispose() => GC.SuppressFinalize(this);
}
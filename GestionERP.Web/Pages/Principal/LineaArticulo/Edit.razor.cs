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

namespace GestionERP.Web.Pages.Principal.LineaArticulo;

public partial class Edit : IDisposable
{
    #region Propiedades
    private const string codigoServicio = "S028";
    public FluentValidationValidator validator;
    public LineaArticuloObtenerDto LineaArticuloObtener { get; set; }
    public LineaArticuloEditarDto LineaArticuloEditar { get; set; }
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

    [Inject] public IPrincipalLineaArticulo ILineaArticulo { get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }
    [Inject] public IMapper IMapper { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Notify.ShowLoading(mensaje: "Obteniendo registro");
            LineaArticuloEditar = new();

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio);
            if (!IsAuthUser) return;
                
            EsVisibleVolver = !string.IsNullOrEmpty(ReturnPage) && ReturnPage is "index" or "view";
                
            if (!await IPermiso.ConsultaEsAsignadoPorSesion(LineaArticuloAcceso.Editar))
            {
                INavigation.NavigateTo("lineas-articulo");
                Notify.Show("No tiene permiso para editar registros de [Líneas de Artículo]", "error");
                return;
            }

            LineaArticuloObtener = await ILineaArticulo.Obtener((Guid) Id);
            if (LineaArticuloObtener is null)
            {
                INavigation.NavigateTo("lineas-articulo");
                Notify.Show("El registro de la [Línea de Artículo] consultado a editar no existe", "error");
                return;
            }

            LineaArticuloEditar = IMapper.Map<LineaArticuloEditarDto>(LineaArticuloObtener);
            EditContext = new EditContext(LineaArticuloEditar);
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

            await ILineaArticulo.Editar((Guid) Id, LineaArticuloEditar);
            
            IsModified = false;
            Notify.Show("La línea de artículo ha sido editada con éxito", "success");
            INavigation.NavigateTo($"lineas-articulo/{Id}");
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
     
    private void Volver() => INavigation.NavigateTo($"lineas-articulo{(ReturnPage == "view" ? $"/{Id}" : "")}");

    public void Dispose() => GC.SuppressFinalize(this);
}
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

namespace GestionERP.Web.Pages.Principal.TipoImpuesto;

public partial class Edit : IDisposable
{
    #region Propiedades
    private const string codigoServicio = "S017";
    public FluentValidationValidator validator;
    public TipoImpuestoObtenerDto TipoImpuestoObtener { get; set; }
    public TipoImpuestoEditarDto TipoImpuestoEditar { get; set; }
    private EditContext EditContext { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool IsAuthUser { get; set; }
    private bool IsModified { get; set; }
    private bool EsVisibleVolver { get; set; }
    private bool IsInitPage { get; set; }
    private MonedaObtenerPorTipoDto MN { get; set; }
    public TelerikNotification Alert { get; set; }
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    [Parameter] public Guid? Id { get; set; }
    [Parameter][SupplyParameterFromQuery(Name = "returnpage")] public string ReturnPage { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    #endregion

    [Inject] public IPrincipalTipoImpuesto ITipoImpuesto { get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public IPrincipalMoneda IMoneda { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }
    [Inject] public IMapper IMapper { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Notify.ShowLoading(mensaje: "Obteniendo registro");
            TipoImpuestoEditar = new();
            MN = new(); 

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio);
            if (!IsAuthUser) return;
                
            EsVisibleVolver = !string.IsNullOrEmpty(ReturnPage) && ReturnPage is "index" or "view";
                
            if (!await IPermiso.ConsultaEsAsignadoPorSesion(TipoImpuestoAcceso.Editar))
            {
                INavigation.NavigateTo("tipos-impuesto");
                Notify.Show("No tiene permiso para editar registros de [Tipos de Impuesto]", "error");
                return;
            }

            TipoImpuestoObtener = await ITipoImpuesto.Obtener((Guid) Id);
            if (TipoImpuestoObtener is null)
            {
                INavigation.NavigateTo("tipos-impuesto");
                Notify.Show("El registro del [Tipo de Impuesto] consultado a editar no existe", "error");
                return;
            }

            MN = await IMoneda.ObtenerPorTipo("MN");

            TipoImpuestoEditar = IMapper.Map<TipoImpuestoEditarDto>(TipoImpuestoObtener);
            EditContext = new EditContext(TipoImpuestoEditar);
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

            await ITipoImpuesto.Editar((Guid) Id, TipoImpuestoEditar);

            IsModified = false;
            Notify.Show("El tipo de impuesto ha sido editado con éxito", "success");
            INavigation.NavigateTo($"tipos-impuesto/{Id}");
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


    private void Volver() => INavigation.NavigateTo($"tipos-impuesto{(ReturnPage == "view" ? $"/{Id}" : "")}");

    public void Dispose() => GC.SuppressFinalize(this);
}
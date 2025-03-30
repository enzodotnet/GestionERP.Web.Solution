using Blazored.FluentValidation;
using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Models.Dtos.Principal.Types;
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

namespace GestionERP.Web.Pages.Principal.Documento;

public partial class Insert : IDisposable
{
    #region Propiedades
    private const string codigoServicio = "S007";
    public FluentValidationValidator validator;
    public DocumentoInsertarDto DocumentoInsertar { get; set; }
    public DocumentoObtenerDto DocumentoObtener { get; set; }
    private IEnumerable<TipoDocumentoAtributoType> Atributos { get; set; }
    private IEnumerable<DocumentoTipoEntidadType> TiposEntidad { get; set; }
    private EditContext EditContext { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool IsAuthUser { get; set; }
    private bool IsModified { get; set; }
    private bool EsVisibleVolver { get; set; }
    public TelerikNotification Alert { get; set; }
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    [Parameter][SupplyParameterFromQuery(Name = "returnpage")] public string ReturnPage { get; set; } 
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    #endregion

    [Inject] public IPrincipalDocumento IDocumento { get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        { 
            DocumentoObtener = new();
            DocumentoInsertar = new();

            Atributos = TipoDocumentoAtributoType.ObtenerTipos();
            TiposEntidad = DocumentoTipoEntidadType.ObtenerTipos();

            EditContext = new EditContext(DocumentoInsertar);

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio);
            if (!IsAuthUser) return;
                
            EsVisibleVolver = !string.IsNullOrEmpty(ReturnPage) && ReturnPage is "index";

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(DocumentoAcceso.Insertar))
            {
                INavigation.NavigateTo("documentos");
                Notify.Show("No tiene permiso para insertar registros de [Documentos]", "error");
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
    }

    private async Task Insertar()
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

            Notify.ShowLoading(mensaje: "Inserción en progreso");

            Guid id = await IDocumento.Insertar(DocumentoInsertar);

            IsModified = false;
            Notify.Show("El documento ha sido insertado con éxito", "success");
            INavigation.NavigateTo($"documentos/{id}"); 
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
        if (IsAuthUser && IsModified && !await Dialog.ConfirmAsync("¿Está seguro de salir del formulario de insertar sin haber guardado?", "Saliendo del formulario"))
            context.PreventNavigation();
    }


    private void ValueCodigoUpperChanged(object codigo) => DocumentoInsertar.Codigo = codigo?.ToString().ToUpper().Trim();

    private void Volver() => INavigation.NavigateTo("documentos");

    #region Catalogos 
    private void CargarItemCatalogoTipoComprobante(TipoComprobanteCatalogoDto item)
    {
        DocumentoInsertar.CodigoTipoComprobante = item.CodigoTipoComprobante;
        DocumentoObtener.NombreTipoComprobante = item.NombreTipoComprobante;
        IsModified = true;
    }

    private void CargarItemCatalogoTipoDocumento(TipoDocumentoCatalogoDto item)
    {
        DocumentoInsertar.CodigoTipoDocumento = item.CodigoTipoDocumento;
        DocumentoObtener.NombreTipoDocumento = item.NombreTipoDocumento;
        DocumentoObtener.FlagAtributo = item.FlagAtributo;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoTipoDocumento"));
        IsModified = true;
    }
    #endregion

    public void Dispose() => GC.SuppressFinalize(this);
} 
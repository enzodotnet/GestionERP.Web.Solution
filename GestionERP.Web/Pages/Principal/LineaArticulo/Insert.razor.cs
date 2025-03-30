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

public partial class Insert : IDisposable
{
    #region Propiedades
    private const string codigoServicio = "S028";
    public FluentValidationValidator validator;
    public LineaArticuloInsertarDto LineaArticuloInsertar { get; set; }
    public LineaArticuloObtenerDto LineaArticuloObtener { get; set; }
    private EditContext EditContext { get; set; }
    private bool IsAuthUser { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool EnabledCodigo { get; set; }
    private string MaskCodigo { get; set; }
    private bool IsModified { get; set; }
    private bool EsVisibleVolver { get; set; }
    public TelerikNotification Alert { get; set; }
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    [Parameter][SupplyParameterFromQuery(Name = "returnpage")] public string ReturnPage { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    #endregion

    [Inject] public IPrincipalLineaArticulo ILineaArticulo { get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        { 
            LineaArticuloInsertar = new();
            LineaArticuloObtener = new();

            EditContext = new EditContext(LineaArticuloInsertar);

            MaskCodigo = "";

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio);
            if (!IsAuthUser) return;
                
            EsVisibleVolver = !string.IsNullOrEmpty(ReturnPage) && ReturnPage is "index";

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(LineaArticuloAcceso.Insertar))
            {
                INavigation.NavigateTo("lineas-articulo");
                Notify.Show("No tiene permiso para insertar registros de [Líneas de Artículo]", "error");
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

            Guid id = await ILineaArticulo.Insertar(LineaArticuloInsertar);
            
            IsModified = false;
            Notify.Show("La línea de artículo ha sido insertada con éxito", "success");
            INavigation.NavigateTo($"lineas-articulo/{id}");
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
     
    private void ValueCodigoUpperChanged(object codigo) => LineaArticuloInsertar.Codigo = codigo?.ToString().ToUpper().Trim();

    private void Volver() => INavigation.NavigateTo("lineas-articulo");

    private void CargarItemCatalogoSegmento(SegmentoArticuloCatalogoDto item)
    {
        LineaArticuloInsertar.CodigoSegmentoArticulo = item.CodigoSegmentoArticulo;
        LineaArticuloObtener.NombreSegmentoArticulo = item.NombreSegmentoArticulo;
        
        string preCodigo = LineaArticuloInsertar.CodigoSegmentoArticulo;
        MaskCodigo = @$"\{preCodigo[0]}\{preCodigo[1]}\{preCodigo[2]}\{preCodigo[3]}\{preCodigo[4]}AAA";
        EnabledCodigo = true;
        if (LineaArticuloInsertar.Codigo is not null)
        {
            LineaArticuloInsertar.Codigo = LineaArticuloInsertar.Codigo.Replace(LineaArticuloInsertar.Codigo[..5], preCodigo);
        }
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoSegmentoArticulo"));
        IsModified = true;
    }

    public void Dispose() => GC.SuppressFinalize(this);
}
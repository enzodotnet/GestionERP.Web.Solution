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

namespace GestionERP.Web.Pages.Principal.SegmentoArticulo;

public partial class Insert : IDisposable
{
    #region Propiedades
    private const string codigoServicio = "S027";
    public FluentValidationValidator validator;
    public SegmentoArticuloInsertarDto SegmentoArticuloInsertar { get; set; }
    public SegmentoArticuloObtenerDto SegmentoArticuloObtener { get; set; }
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

    [Inject] public IPrincipalSegmentoArticulo ISegmentoArticulo { get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        { 

            SegmentoArticuloInsertar = new();
            SegmentoArticuloObtener = new();

            EditContext = new EditContext(SegmentoArticuloInsertar);

            MaskCodigo = "";

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio);
            if (!IsAuthUser) return; 
                
            EsVisibleVolver = !string.IsNullOrEmpty(ReturnPage) && ReturnPage is "index";

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(SegmentoArticuloAcceso.Insertar))
            {
                INavigation.NavigateTo("segmentos-articulos");
                Notify.Show("No tiene permiso para insertar registros de [Segmentos de Artículo]", "error");
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

            Guid id = await ISegmentoArticulo.Insertar(SegmentoArticuloInsertar);
            
            IsModified = false;
            Notify.Show("El segmento de artículo ha sido insertado con éxito", "success");
            INavigation.NavigateTo($"segmentos-articulos/{id}");
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
        if (IsAuthUser && IsModified && !await Dialog.ConfirmAsync("¿Está seguro de salir del formulario de insertar sin haber guardado?","Deshacer acción"))
            context.PreventNavigation();
    }


    private void ValueCodigoUpperChanged(object codigo) => SegmentoArticuloInsertar.Codigo = codigo?.ToString().ToUpper().Trim();

    private void Volver() => INavigation.NavigateTo("segmentos-articulos");

    private void CargarItemCatalogoGrupo(GrupoArticuloCatalogoDto item)
    { 
        SegmentoArticuloInsertar.CodigoGrupoArticulo = item.CodigoGrupoArticulo;
        SegmentoArticuloObtener.NombreGrupoArticulo = item.NombreGrupoArticulo;
        
        string preCodigo = SegmentoArticuloInsertar.CodigoGrupoArticulo;
        MaskCodigo = @$"\{preCodigo[0]}\{preCodigo[1]}AAA";
        EnabledCodigo = true;
        if (SegmentoArticuloInsertar.Codigo is not null)
        {
            SegmentoArticuloInsertar.Codigo = SegmentoArticuloInsertar.Codigo.Replace(SegmentoArticuloInsertar.Codigo[..2], preCodigo);
        }
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoGrupoArticulo"));
        IsModified = true;  
    }

    public void Dispose() => GC.SuppressFinalize(this);  
}
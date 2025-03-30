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

namespace GestionERP.Web.Pages.Principal.Articulo;

public partial class Insert : IDisposable
{
    #region Propiedades
    private const string codigoServicio = "S004";
    public FluentValidationValidator validator;
    public ArticuloInsertarDto ArticuloInsertar { get; set; }
    [Parameter][SupplyParameterFromQuery(Name = "returnpage")] public string ReturnPage { get; set; }
    public ArticuloObtenerDto ArticuloObtener { get; set; } 
    private IEnumerable<ArticuloTipoUnidadType> Categorias { get; set; }
    private EditContext EditContext { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool IsAuthUser { get; set; }
    private bool IsModified { get; set; }
    private bool EsVisibleVolver { get; set; }
    public TelerikNotification Alert { get; set; }
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    #endregion

    [Inject] public IPrincipalArticulo IArticulo { get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        { 
            ArticuloObtener = new();
            ArticuloInsertar = new();

            Categorias = ArticuloTipoUnidadType.ObtenerTipos();

            EditContext = new EditContext(ArticuloInsertar);

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio);
            if (!IsAuthUser) return;
                
            EsVisibleVolver = !string.IsNullOrEmpty(ReturnPage) && ReturnPage is "index";

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(ArticuloAcceso.Insertar))
            {
                INavigation.NavigateTo("articulos");
                Notify.Show("No tiene permiso para insertar registros de [Artículos]", "error");
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

            Guid id = await IArticulo.Insertar(ArticuloInsertar);

            IsModified = false;
            Notify.Show("El artículo ha sido insertado con éxito", "success");
            INavigation.NavigateTo($"articulos/{id}"); 
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


    private void Volver() => INavigation.NavigateTo("articulos");

    #region Catalogos
    private void CargarItemCatalogoGrupo(GrupoArticuloCatalogoDto item)
    {
        if (item.CodigoGrupoArticulo.Trim() != ArticuloObtener.CodigoGrupoArticulo?.Trim())
        {
            ArticuloObtener.CodigoSegmentoArticulo = ArticuloInsertar.CodigoLineaArticulo = null;
            ArticuloObtener.NombreSegmentoArticulo = ArticuloObtener.NombreLineaArticulo = "";
        }
        ArticuloObtener.CodigoGrupoArticulo = item.CodigoGrupoArticulo;
        ArticuloObtener.NombreGrupoArticulo = item.NombreGrupoArticulo;
        IsModified = true;
    }

    private void CargarItemCatalogoSegmentoPorGrupo(SegmentoArticuloCatalogoPorGrupoDto item)
    {
        if (item.CodigoSegmentoArticulo.Trim() != ArticuloObtener.CodigoSegmentoArticulo?.Trim())
        {
            ArticuloInsertar.CodigoLineaArticulo = null;
            ArticuloObtener.NombreLineaArticulo = "";
        }
        ArticuloObtener.CodigoSegmentoArticulo = item.CodigoSegmentoArticulo;
        ArticuloObtener.NombreSegmentoArticulo = item.NombreSegmentoArticulo;
        IsModified = true;
    }

    private void CargarItemCatalogoLineaPorSegmento(LineaArticuloCatalogoPorSegmentoDto item)
    {
        ArticuloInsertar.CodigoLineaArticulo = item.CodigoLineaArticulo;
        ArticuloObtener.NombreLineaArticulo = item.NombreLineaArticulo;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoLineaArticulo"));
        IsModified = true;
    }

    private void CargarItemCatalogoFamiliaProducto(FamiliaProductoCatalogoDto item)
    {
        ArticuloInsertar.CodigoFamiliaProducto = item.CodigoFamiliaProducto;
        ArticuloObtener.NombreFamiliaProducto = item.NombreFamiliaProducto;
        IsModified = true;
    }

    private void CargarItemCatalogoUnidadMedida(UnidadMedidaCatalogoDto item)
    {
        ArticuloInsertar.CodigoUnidadMedida = item.CodigoUnidadMedida;
        ArticuloObtener.NombreUnidadMedida = item.NombreUnidadMedida;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoUnidadMedida"));
        IsModified = true;
    }
    #endregion

    public void Dispose() => GC.SuppressFinalize(this);
} 
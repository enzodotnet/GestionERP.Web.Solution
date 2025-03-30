using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components;
using GestionERP.Web.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Routing;
using Telerik.Blazor;

namespace GestionERP.Web.Pages.Principal.LineaArticulo;

public partial class Index : IDisposable
{
    #region Propiedades
    private const string codigoServicio = "S028";
    private IEnumerable<LineaArticuloListarDto> ListaLineasArticulo { get; set; }
    private bool EsVisibleDialogEliminar { get; set; }
    private string CodigoRegistro { get; set; }
    private Guid? RegistroId { get; set; } 
    private bool EsAsignadoEliminar { get; set; }
    private bool EsAsignadoEditar { get; set; }
    private bool EsAsignadoInsertar { get; set; }
    private string TituloIndex { get; set; }
    private bool IsInitGrid { get; set; }
    private bool IsAuthUser { get; set; }
    private bool IsLoadingAction { get; set; }
    [Parameter][SupplyParameterFromQuery(Name = "codigoSegmentoArticulo")] public string CodigoSegmentoArticulo { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public DialogFactory Dialog { get; set; }
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
            Notify.ShowLoading(mensaje: "Listando registro(s)");

            TituloIndex = "Lista de líneas de artículo";
            CodigoSegmentoArticulo ??= "";

            if (CodigoSegmentoArticulo != "")
                TituloIndex += " por segmento";

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio);
            if (!IsAuthUser) return;
                
            EsAsignadoEditar = await IPermiso.ConsultaEsAsignadoPorSesion(LineaArticuloAcceso.Editar);
            EsAsignadoEliminar = await IPermiso.ConsultaEsAsignadoPorSesion(LineaArticuloAcceso.Eliminar);
            EsAsignadoInsertar = await IPermiso.ConsultaEsAsignadoPorSesion(LineaArticuloAcceso.Insertar);

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(LineaArticuloAcceso.VerRegistros))
            {
                INavigation.NavigateTo("inicio");
                Notify.Show("No tiene permiso para ver registros del servicio principal de [Líneas de Artículo]", "error");
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

            await ILineaArticulo.Eliminar((Guid) RegistroId);
            await Listar();
 
            Notify.Show($"La línea de artículo {CodigoRegistro} ha sido eliminada con éxito", "success");
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

    private void IrInsertar() => INavigation.NavigateTo(INavigation.GetUriWithQueryParameters("lineas-articulo/insertar", new Dictionary<string, object> { ["returnpage"] = "index" }));

    private void IrVer(Guid id) => INavigation.NavigateTo($"lineas-articulo/{id}");

    private void IrEditar(Guid id) => INavigation.NavigateTo(INavigation.GetUriWithQueryParameters($"lineas-articulo/{id}/editar", new Dictionary<string, object> { ["returnpage"] = "index" }));

    public async Task Cerrar(LocationChangingContext context)
    {
        if (IsAuthUser && IsLoadingAction && !await Dialog.ConfirmAsync("¿Está seguro de salir del formulario sin haber culminado la acción?", "Saliendo del formulario"))
            context.PreventNavigation();
    }

    private void MostrarEliminar(bool visible, Guid? id = null)
    {
        RegistroId = id;
        CodigoRegistro = visible ? ListaLineasArticulo.Where(x => x.Id == (Guid) id).Select(x => x.Codigo).FirstOrDefault().Trim() : null;
        EsVisibleDialogEliminar = visible;
    }

    private async Task Listar()
    {
        ListaLineasArticulo = await ILineaArticulo.Listar(CodigoSegmentoArticulo); 
        IsInitGrid = true; 
    }

    public void Dispose() => GC.SuppressFinalize(this);
}
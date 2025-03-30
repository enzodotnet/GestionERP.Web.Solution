using Microsoft.AspNetCore.Components;
using GestionERP.Web.Models.Dtos.Servicio;
using GestionERP.Web.Components;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Services;
using GestionERP.Web.Handlers;
using System.Security.Claims;
using Telerik.Blazor.Components; 
using Microsoft.AspNetCore.Components.Routing; 
using Telerik.Blazor; 
using GestionERP.Web.Models.Dtos.Principal;
using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components.Forms;
using GestionERP.Web.Global;
using AutoMapper;

namespace GestionERP.Web.Pages.Empresa.Servicio.Orden;

public partial class Acept : IDisposable
{
    #region Propiedades
    private const string codigoModulo = "03";
    private const string codigoServicio = "S109";
    private const string rutaAccion = "/aceptar";
    private const string rutaServicio = "/ordenes";
    private string rutaEmpresa = "";
	public FluentValidationValidator validator;

	private IEnumerable<OrdenCatalogoAceptarDto> CatalogoOrdenes { get; set; }
    private OrdenObtenerAceptarDto OrdenAceptar { get; set; }
    public TelerikNotification AlertModalInsertar { get; set; }
    public TelerikNotification AlertModalAceptaciones { get; set; }
    public TelerikNotification AlertModalDetalle { get; set; }
    public TelerikNotification AlertDialogEliminar { get; set; }
    private IEnumerable<OrdenFlag> EstadosAceptacion { get; set; }
	private EditContext EditContextAceptacion { get; set; }
    public OrdenDetalleAceptacionInsertarDto AceptacionInsertar { get; set; }
    public OrdenDetalleAceptacionEliminarDto AceptacionEliminar { get; set; }
    public OrdenDetalleObtenerAceptarDto ItemDetalle { get; set; }
    public string FlagEstadoAceptacion { get; set; }
    private bool EsAsignadoAceptar { get; set; }
    private string OrigenAccionModal { get; set; }
    private string TituloAccionLoading { get; set; }
    private int NumeroAceptacionEliminar { get; set; }
    private bool EsVisibleDialogEliminar { get; set; }
    private bool EsAsignadoRetirarAceptacion { get; set; }
    private bool IsLoadingInsertar { get; set; }
    private bool IsLoadingModalDetalle { get; set; }
    private bool IsLoadingModalAceptaciones { get; set; }
    private bool IsModifiedAceptacion { get; set; }
	private bool IsInitGrid { get; set; }
    private bool IsAuthUser { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool EsVisibleModalDetalle { get; set; }
    private bool EsVisibleModalAceptaciones { get; set; }
    private bool EsVisibleModalInsertar { get; set; }
    private bool EsRecargableCatalogo { get; set; }
    private bool EsRecargableItemCatalogo { get; set; }
    private string VerboAccionAceptacion { get; set; }
    public string CodigoEjercicio { get; set; }
    private IEnumerable<EmpresaEjercicioCatalogoDto> CatalogoEjercicios { get; set; }
    public EmpresaConsultaPorCodigoWebDto Empresa { get; set; }
    [Parameter] public string CodigoWebEmpresa { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    #endregion

    [Inject] public IServicioOrden IOrden { get; set; }
    [Inject] public IPrincipalEmpresa IEmpresa { get; set; } 
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }
    [Inject] public IMapper IMapper { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Notify.ShowLoading(mensaje: "Listando catálogo");
             
            FlagEstadoAceptacion = "PA";
            EstadosAceptacion = OrdenFlag.EstadosAceptacion();

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio);
            if (!IsAuthUser) return;

            Empresa = await IEmpresa.ConsultaPorCodigoWeb(CodigoWebEmpresa);
            rutaEmpresa = INavigation.Uri.Replace(INavigation.BaseUri, "").Split("?")[0].Replace(rutaServicio, "").Replace(rutaAccion, "");

            EsAsignadoAceptar = await IPermiso.ConsultaEsAsignadoPorSesion(OrdenAcceso.Aceptar, Empresa.Codigo);
            EsAsignadoRetirarAceptacion = await IPermiso.ConsultaEsAsignadoPorSesion(OrdenAcceso.RetirarAceptacion, Empresa.Codigo);
             
            CatalogoEjercicios = await IEmpresa.CatalogoEjercicios(Empresa.Codigo) ?? [];
            CodigoEjercicio = await IEmpresa.ConsultaEjercicioCodigoPorAnio(Empresa.Codigo, DateTime.Now.Year);

            await CargarCatalogoOrdenes();
        }
        catch (Exception ex)
        {
            if (ex is HttpRequestException)
                Notify.ShowError("NC");
            else if (ex is HttpResponseException)
            {
                string codeError = (ex as HttpResponseException).Code; 
				if (codeError == "AU")
                    INavigation.NavigateTo($"{rutaEmpresa}/inicio");
                Notify.ShowError(codeError, ex);
            }
            else 
                Notify.ShowError("FA", ex);
        }
        finally
        {
            Notify.ShowLoading(false);
        }
    }

    private async Task Insertar()
    {
        try
        { 
            IsLoadingAction = true;
            IsAuthUser = (await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio, User.FindFirst("code").Value)).esValido;
            if (!IsAuthUser)
            {
                OcultarModals();
                return;
            } 
            if (!EditContextAceptacion.Validate())
            {
                Fnc.MostrarAlerta(AlertModalInsertar, Cnf.MsgErrorInvalidEditContext, "error");
                return;
            }
            if (!VerificarRegistroEsValido()) return;

            TituloAccionLoading = "Inserción en progreso";
            IsLoadingInsertar = true;
            StateHasChanged();
            
            await IOrden.InsertarDetalleAceptacion(Empresa.Codigo, AceptacionInsertar);
            EsRecargableCatalogo = EsRecargableItemCatalogo = true;
            await CerrarInsertarAceptacion();

            Fnc.MostrarAlerta(OrigenAccionModal == "CD" ? AlertModalDetalle : AlertModalAceptaciones, "La aceptación al ítem de la orden de servicio ha sido insertada con éxito", "success");
        }
        catch (HttpRequestException)
        {
            Fnc.MostrarAlerta(AlertModalInsertar, Cnf.MsgErrorNotConnectApi, "error");
        }
        catch (HttpResponseException ex)
        {
            if (ex.Code == "AU")
            {
                Fnc.MostrarAlerta(OrigenAccionModal == "CD" ? AlertModalDetalle : AlertModalAceptaciones, ex.Message, "error");
            }
            else 
            {
                EsRecargableCatalogo = EsRecargableItemCatalogo = true;
                Fnc.MostrarAlerta(AlertModalInsertar, $"{(ex.Code == "NF" ? Cnf.MsgErrorNotFoundAPi : ex.Message)}", "error");
            } 
        }
        catch (Exception)
        {
            Fnc.MostrarAlerta(AlertModalInsertar, Cnf.MsgErrorFuncAppWeb, "error");
        }
        finally
        {
            IsLoadingAction = IsLoadingInsertar = false;
        }
    }

    private async Task Eliminar()
    {
        try
        {
            if (string.IsNullOrEmpty(AceptacionEliminar.Motivo) || AceptacionEliminar.Motivo.Trim() == "")
            {
                Fnc.MostrarAlerta(AlertDialogEliminar, "Es necesario que ingrese el motivo por el cual va a eliminar la aceptación", "error");
                return;
            }

            IsLoadingAction = true;
            EsVisibleDialogEliminar = false;

            IsAuthUser = (await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio, User.FindFirst("code").Value)).esValido;
            if (!IsAuthUser)
            {
                OcultarModals();
                return;
            }
                
            TituloAccionLoading = "Eliminación en progreso";
            IsLoadingModalAceptaciones = true;
            StateHasChanged();
             
            await IOrden.EliminarDetalleAceptacion(Empresa.Codigo, AceptacionEliminar);
            EsRecargableCatalogo = EsRecargableItemCatalogo = true;
            await CerrarEliminarAceptacion();

            Fnc.MostrarAlerta(AlertModalAceptaciones, "La aceptación al ítem de la orden de servicio ha sido eliminada con éxito", "success");
        }
        catch (HttpRequestException)
        {
            Fnc.MostrarAlerta(AlertModalAceptaciones, Cnf.MsgErrorNotConnectApi, "error");
        }
        catch (HttpResponseException ex)
        { 
            EsRecargableCatalogo = EsRecargableItemCatalogo = true;
            Fnc.MostrarAlerta(AlertModalAceptaciones, $"{(ex.Code == "NF" ? Cnf.MsgErrorNotFoundAPi : ex.Message)}", "error");
        }
        catch (Exception)
        {
            Fnc.MostrarAlerta(AlertModalAceptaciones, Cnf.MsgErrorFuncAppWeb, "error");
        }
        finally
        {
            IsLoadingAction = IsLoadingModalAceptaciones = false;
        }
    }

    private bool VerificarRegistroEsValido()
    {
        bool esValido = true;
        if (AceptacionInsertar.ImporteBruto > ItemDetalle.ImporteBrutoSaldo)
        {
            Fnc.MostrarAlerta(AlertModalInsertar, "El importe bruto a aceptar no puede ser mayor al saldo", "error");
            esValido = false;
        }
        return esValido;
    }

    private async Task RefrescarCatalogoOrdenes()
    {
        try
        {
            IsAuthUser = (await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio, User.FindFirst("code").Value)).esValido;
            if (!IsAuthUser) return;

            Notify.ShowLoading(mensaje: "Refrescando catalogo");

            await CargarCatalogoOrdenes();
        }
        catch (Exception ex)
        {
            if (ex is HttpRequestException)
                Notify.ShowError("NC");
            else if (ex is HttpResponseException)
            {
                string codeError = (ex as HttpResponseException).Code; 
				if (codeError == "AU")
                    INavigation.NavigateTo($"{rutaEmpresa}/inicio");
                Notify.ShowError(codeError, ex);
            }
            else 
                Notify.ShowError("FA", ex);
        }
        finally
        {
            Notify.ShowLoading(false);
        }
    }

    private async Task RefrescarItemCatalogoOrden()
    {
        try
        { 
            TituloAccionLoading = "Obteniendo registro";

            if (OrigenAccionModal == "CD")
                IsLoadingModalDetalle = true;
            else
                IsLoadingModalAceptaciones = true;

            IsAuthUser = (await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio, User.FindFirst("code").Value)).esValido;
            if (!IsAuthUser)
            {
                OcultarModals();
                return;
            }

            StateHasChanged();
             
            OrdenAceptar = await IOrden.ObtenerAceptar(Empresa.Codigo, OrdenAceptar.OrdenId);
            if (OrdenAceptar is null)
            {
                OcultarModals();
                Notify.Show("El registro de la [Orden de Servicio] visualizada ya no está disponible", "warning");
                return;
            }  
            if (OrigenAccionModal == "CA")
            {
                ItemDetalle = OrdenAceptar.Detalles.Where(x => x.OrdenDetalleId == ItemDetalle.OrdenDetalleId).FirstOrDefault();
                if (ItemDetalle is null)
                {
                    EsVisibleModalAceptaciones = false;
                    Fnc.MostrarAlerta(AlertModalDetalle, "El registro del ítem de la [Orden de Servicio] visualizada ya no está disponible", "error");
                }
            } 
        }
        catch (HttpRequestException)
        {
            Fnc.MostrarAlerta(OrigenAccionModal == "CD" ? AlertModalDetalle : AlertModalAceptaciones, Cnf.MsgErrorNotConnectApi, "error");
        }
        catch (HttpResponseException ex)
        {
            if (ex.Code == "AU")
            {
                INavigation.NavigateTo($"{rutaEmpresa}/inicio");
                Notify.ShowError(ex.Code, ex);
            }
            else 
            {
                Fnc.MostrarAlerta(OrigenAccionModal == "CD" ? AlertModalDetalle : AlertModalAceptaciones, ex.Message, "error");
            }
        }
        catch (Exception)
        {
            Fnc.MostrarAlerta(OrigenAccionModal == "CD" ? AlertModalDetalle : AlertModalAceptaciones, Cnf.MsgErrorFuncAppWeb, "error");
        }
        finally 
        {
            IsLoadingModalDetalle = IsLoadingModalAceptaciones = false;
            TituloAccionLoading = "";
        }
    }

    private void OcultarModals() => EsVisibleModalDetalle = EsVisibleModalInsertar = EsVisibleModalAceptaciones = false;

    public async Task Cerrar(LocationChangingContext context)
    {
        if (IsAuthUser && IsLoadingAction && (!await Dialog.ConfirmAsync($"¿Está seguro de salir sin haber culminado la acción de {VerboAccionAceptacion} aceptación?", "Saliendo del formulario")))
            context.PreventNavigation(); 
    } 

    public static void OnCellImporteSaldoRenderHandler(GridCellRenderEventArgs args) => args.Class = "cell-main";

    private void VerItemDetalleCatalogo(OrdenCatalogoAceptarDto item)
    {
        OrdenAceptar = IMapper.Map<OrdenObtenerAceptarDto>(item);
        EsRecargableCatalogo = false;
        EsVisibleModalDetalle = true;
    }

    private void VerAceptacionesItemDetalle(OrdenDetalleObtenerAceptarDto item)
    {
        ItemDetalle = item;
        EsRecargableItemCatalogo = false;
        EsVisibleModalAceptaciones = true;
    }

    private async Task VisibleDetalleChangedHandler(bool esVisible)
    {
        if (!esVisible)
        {
            EsVisibleModalDetalle = false;

            if (EsRecargableCatalogo)
                await RefrescarCatalogoOrdenes();
        }
    }

    private void VisibleListaAceptacionesChangedHandler(bool esVisible) => EsVisibleModalAceptaciones = esVisible;
    
    private async Task CerrarAceptacion()
    {
        if (!IsModifiedAceptacion || await Dialog.ConfirmAsync("¿Está seguro de cerrar el formulario y que la aceptación no se grabe?", "Deshaciendo inserción"))
            await CerrarInsertarAceptacion();
    }

    private async Task CerrarInsertarAceptacion()
    {
        IsModifiedAceptacion = false;
        EsVisibleModalInsertar = false;
        VerboAccionAceptacion = "";

        if (EsRecargableItemCatalogo) 
            await RefrescarItemCatalogoOrden();
    }

    public void MostrarInsertarAceptacion(OrdenDetalleObtenerAceptarDto item, string origenModalAccion)
	{
        AceptacionInsertar = new()
        {
            OrdenDetalleId = item.OrdenDetalleId,
            FechaAceptacion = DateTime.Now
        };
        OrigenAccionModal = origenModalAccion; //D: Listado del detalle, A: Listado de aceptaciones
        EsRecargableItemCatalogo = false;
        VerboAccionAceptacion = "insertar";
        ItemDetalle = item;
		EditContextAceptacion = new EditContext(AceptacionInsertar);
	    EsVisibleModalInsertar = true;
	}

    protected void MostrarEliminarAceptacion(bool visible, OrdenDetalleAceptacionObtenerAceptarDto item = null)
    {
        if (item is not null)
        {
            NumeroAceptacionEliminar = item.NumeroAceptacion;
            AceptacionEliminar = new()
            {
                OrdenDetalleAceptacionId = item.OrdenDetalleAceptacionId
            };
            OrigenAccionModal = "CA";
            VerboAccionAceptacion = "eliminar";
            EsRecargableItemCatalogo = false;
        }
        EsVisibleDialogEliminar = visible;
    }

    private async Task CerrarEliminarAceptacion()
    {  
        NumeroAceptacionEliminar = 0;
        AceptacionEliminar = null;
        EsVisibleDialogEliminar = false;
        VerboAccionAceptacion = "";

        if (EsRecargableItemCatalogo) 
            await RefrescarItemCatalogoOrden();
    }

    private async Task CargarCatalogoOrdenes()
    {
        CatalogoOrdenes = await IOrden.CatalogoAceptar(Empresa.Codigo, CodigoEjercicio, FlagEstadoAceptacion);
        IsInitGrid = true; 
    }

    private async Task OnComboEjercicioValueChanged(string value)
    {
        CodigoEjercicio = value;
        if (!string.IsNullOrEmpty(CodigoEjercicio))
            await CargarCatalogoOrdenes();
    }

    private async Task OnComboEstadoAceptacionValueChanged(string value)
    {
        FlagEstadoAceptacion = value;
        await RefrescarCatalogoOrdenes();
    }

    public void Dispose() => GC.SuppressFinalize(this); 
}
using GestionERP.Web.Handlers; 
using GestionERP.Web.Models.Dtos.Almacen;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components; 
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Telerik.Blazor;
using Telerik.Blazor.Components;
using Telerik.SvgIcons;
using AutoMapper; 
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Global;
using GestionERP.Web.Services;
using GestionERP.Web.Models.Dtos.Compra; 

namespace GestionERP.Web.Pages.Empresa.Almacen.Movimiento;

public partial class Insert : IDisposable
{
    #region Propiedades
    private const string codigoModulo = "04";
    private const string codigoServicio = "S130";
    private const string rutaAccion = "/registrar";
    private string rutaEmpresa = "";
    private string pathBaseUri = "";
    private const string rutaServicio = "/movimientos";
     
	public MovimientoInsertarDto MovimientoInsertar { get; set; }
    public MovimientoObtenerDto Movimiento { get; set; }
    public MovimientoDetalleInsertarDto DetalleInsertar { get; set; }
    public MovimientoDetalleObtenerDto Detalle { get; set; }
    public List<MovimientoDetalleGrid> GridDetalles { get; set; }
	public MovimientoDetalleGrid ItemGridDetalle { get; set; }
	private EditContext EditContext { get; set; }
    private EditContext DetalleContext { get; set; }
    public MovimientoDetalleGridValidator GridDetalleValidator { get; set; }
    public MovimientoInsertarValidator Validator { get; set; }
    public MovimientoDetalleInsertarValidator DetalleValidator { get; set; }
    public TipoMovimientoConsultaPorCodigoDto TipoMovimiento { get; set; }
    private MonedaConsultaPorTipoDto MN { get; set; }
    private MonedaConsultaPorTipoDto ME { get; set; }
    public bool IsInitPage { get; set; }
    public bool IsEditingGridDetalle { get; set; } 
    public string CodigoTipoMovimiento { get; set; }
    public List<DateTime> FechasCerradoOperacion { get; set; }
    public EmpresaEjercicioConsultaIntervaloFechaDto FechaIntervalo { get; set; }
	private IEnumerable<MovimientoFlag> TiposRegistro { get; set; }  
    public List<OrdenCatalogoIngresarDto> CatalogoOrdenesIngresar { get; set; }
    public List<OrdenDetalleCatalogoIngresarDto> CatalogoDetallesOrden { get; set; }
    public IEnumerable<OrdenCatalogoIngresarDto> ItemsSelectedOrden { get; set; }
    public IEnumerable<OrdenDetalleCatalogoIngresarDto> ItemsSelectedDetalleOrden { get; set; }
    public string CodigoEjercicio { get; set; }
    public string CodigoEjercicioFiltro { get; set; }
    private IEnumerable<EmpresaEjercicioCatalogoDto> CatalogoEjercicios { get; set; }  
	public bool EsVisibleBotonOperacion { get; set; }
    public bool EsVisibleBotonCatalogoDetallesReferencia { get; set; }
    public bool EsVisibleBotonCatalogoReferencias { get; set; }
    public bool EsVisibleBotonQuitarReferencia { get; set; }
    public bool EsVisibleDialogReferencia { get; set; }
    public bool EsVisibleCatalogoOrdenes { get; set; }
    public bool EsVisibleCatalogoDetallesOrden { get; set; }
    private bool IsAuthUser { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool IsModified { get; set; }
    private bool IsModifiedDetalle { get; set; }
    private bool IsLoadingCatalogoDetallesReferencia { get; set; }
    private bool IsLoadingCatalogoReferencias { get; set; }
    public string VerboAccionModal { get; set; }
    public string DescripcionAccionModal { get; set; }
    public string TipoAccionModal { get; set; }
    public string TipoAccionDialog { get; set; }
    public string OrigenDialog { get; set; }
    public string OrigenModal { get; set; }
    public bool EsVisibleDialogDetalle { get; set; }
    public bool EsVisibleModalDetalle { get; set; }
    public string CodigoItemAccion { get; set; }
    public string CodigoPermisoRegistrar { get; set; }
    public string TituloTipoRegistro { get; set; }
    public ISvgIcon IconoAccionModal { get; set; } 
    public TelerikNotification AlertDetalle { get; set; }
    public TelerikNotification Alert { get; set; }
    public TelerikNotification AlertCatalogoOrdenes { get; set; }
    public TelerikNotification AlertCatalogoDetallesOrden { get; set; }
    private TelerikGrid<MovimientoDetalleGrid> GridDetalleRef { get; set; } 
    private TelerikGrid<OrdenDetalleCatalogoIngresarDto> GridCatalogoDetalleOrdenRef { get; set; }
    public GridSelectionMode SelectionModeCatalogoDetalleReferencia { get; set; }
    private bool EsSeleccionableCatalogoDetalleReferencia { get; set; }
    [Parameter][SupplyParameterFromQuery(Name = "returnpage")] public string ReturnPage { get; set; } 
    public EmpresaConsultaPorCodigoWebDto Empresa { get; set; }
    private bool EsVisibleVolver { get; set; }
    [Parameter] public string CodigoWebEmpresa { get; set; }
    [Parameter] public string TipoRegistro { get; set; }
    public string TipoRegistroTemp { get; set; }
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    #endregion

    [Inject] public IAlmacenMovimiento IMovimiento { get; set; }
	[Inject] public ICompraOrden IOrden { get; set; } 
    [Inject] public IPrincipalEmpresa IEmpresa { get; set; } 
    [Inject] public IPrincipalTipoCambioDia ITipoCambioDia { get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public IPrincipalMoneda IMoneda { get; set; }
    [Inject] public IPrincipalTipoMovimiento ITipoMovimiento { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public UtilService IUtil { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }
    [Inject] public IMapper IMapper { get; set; }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);
        if (!string.IsNullOrEmpty(TipoRegistro))
        {
            if (!string.IsNullOrEmpty(TipoRegistroTemp) && TipoRegistro != TipoRegistroTemp)
                await IniciarVistaRegistro();
            TipoRegistroTemp = TipoRegistro;
        }
    }

    protected override async Task OnInitializedAsync()
    {
        TiposRegistro = MovimientoFlag.TiposRegistro();
        Validator = new();
        await IniciarVistaRegistro();
    }

    protected async Task IniciarVistaRegistro()
    {
        try
        {
            Notify.ShowLoading(mensaje: "Iniciando formulario");

            MovimientoInsertar = new();
            Movimiento = new();
            GridDetalles = [];
            ItemGridDetalle = new();
            DetalleInsertar = new();
            Detalle = new();

            TipoMovimiento = new();
            FechasCerradoOperacion = []; 
            FechaIntervalo = new();

            TiposRegistro = MovimientoFlag.TiposRegistro();  
            EsVisibleBotonOperacion = true;

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio);
            if (!IsAuthUser) return;

            Empresa = await IEmpresa.ConsultaPorCodigoWeb(CodigoWebEmpresa);
            EsVisibleVolver = !string.IsNullOrEmpty(ReturnPage) && ReturnPage is "index";

            pathBaseUri = $"/{INavigation.ToBaseRelativePath(INavigation.Uri).Split("?")[0]}";
            rutaEmpresa = INavigation.Uri.Replace(INavigation.BaseUri, "").Split("?")[0].Replace(rutaServicio, "").Replace(rutaAccion, "").Replace(string.Concat("/", TipoRegistro ?? ""), "");

            if (!VerificarTipoRegistroEsValido())
            {
                INavigation.NavigateTo($"{rutaEmpresa}/inicio");
                Notify.Show("La ruta al que intenta acceder para registar [Movimiento] no tiene un tipo de registro válido", "error");
                return;
            }

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(MovimientoAcceso.RegistrarIngreso, Empresa.Codigo))
            {
                INavigation.NavigateTo($"{rutaEmpresa}/inicio");
                Notify.Show($"No tiene permiso para registrar [Movimientos] de {TituloTipoRegistro}", "error");
                return;
            }

            CatalogoEjercicios = await IEmpresa.CatalogoEjercicios(Empresa.Codigo) ?? [];
            CodigoEjercicio = await IEmpresa.ConsultaEjercicioCodigoPorAnio(Empresa.Codigo, DateTime.Now.Year);

            FechaIntervalo = await IEmpresa.ConsultaEjercicioIntervaloFecha(Empresa.Codigo);
            (await IEmpresa.ConsultaModuloPeriodoFechasEsCerradoOperacion(Empresa.Codigo, codigoModulo))?.ToList().ForEach(x => FechasCerradoOperacion.Add(x.Fecha));
            MovimientoInsertar.FechaHoraOperacion = !FechasCerradoOperacion.Any(x => x == DateTime.Now.Date) ? DateTime.Now.Date : null;
            if (MovimientoInsertar.FechaHoraOperacion.HasValue)
            {
                Movimiento.FechaHoraOperacion = (DateTime)MovimientoInsertar.FechaHoraOperacion;
            }

            await ConsultaTipoCambioDia();

            ME = await IMoneda.ConsultaPorTipo("ME");
            MN = await IMoneda.ConsultaPorTipo("MN");
             
            EditContext = new EditContext(MovimientoInsertar);
            IsInitPage = true;
            StateHasChanged();
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

    private async Task Insertar()
    {
        try
        {
            IsLoadingAction = true;
            ActualizarHoraOperacion();

            IsAuthUser = (await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio, User.FindFirst("code").Value)).esValido;
            if (!IsAuthUser) return;
            if (!EditContext.Validate())
            {
                Fnc.MostrarAlerta(Alert, Cnf.MsgErrorInvalidEditContext, "error");
                return;
            } 
            if (!VerificarRegistroEsValido()) return;

            Notify.ShowLoading(mensaje: "Registro en progreso");

            Guid id = await IMovimiento.Insertar(Empresa.Codigo, MovimientoInsertar);

            IsModified = false;
            Notify.Show($"El movimiento de {TituloTipoRegistro} ha sido registrado con éxito en la empresa", "success");
            INavigation.NavigateTo($"{rutaEmpresa}{rutaServicio}/{id}");
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

    private bool VerificarTipoRegistroEsValido()
    {
        bool esValido = false;
        if (!string.IsNullOrEmpty(TipoRegistro) && TipoRegistro is "ingreso" or "salida" or "transferencia")
        {
            if (TipoRegistro is "ingreso")
            {
                CodigoPermisoRegistrar = MovimientoAcceso.RegistrarIngreso;
                MovimientoInsertar.FlagTipoRegistro = "I"; 
            }
            else if (TipoRegistro is "salida")
            {
                CodigoPermisoRegistrar = MovimientoAcceso.RegistrarSalida;
                MovimientoInsertar.FlagTipoRegistro = "S"; 
            }
            else
            {
                CodigoPermisoRegistrar = MovimientoAcceso.RegistrarTransferencia;
                MovimientoInsertar.FlagTipoRegistro = "T"; 
            }
            TituloTipoRegistro = TipoRegistro;
            esValido = true;
        }
        return esValido;
    }

    private bool VerificarRegistroEsValido()
    {
        bool esValido  = true;
        if (GridDetalles.Count == 0)
        {
            Fnc.MostrarAlerta(Alert, "Es obligatorio que agregue ítem(s) al detalle de la orden de compra", "error");
            esValido = false;
        }
        else 
        {
            if (IsEditingGridDetalle)
            {
                Fnc.MostrarAlerta(Alert, "Es necesario que se termine de editar la celda del detalle", "error");
                esValido = false;
            }
            else
            {
                if (GridDetalles.Any(x => !x.Cantidad.HasValue))
                {
                    Fnc.MostrarAlerta(Alert, "Está pendiente completar alguna celda de [cantidad] del detalle de la orden de compra", "error");
                    esValido = false;
                } 
            }  
        }
		
		return esValido;
    }

    private async Task Cerrar(LocationChangingContext context)
    {
        if (IsAuthUser && IsModified && pathBaseUri != context.TargetLocation && !await Dialog.ConfirmAsync("¿Está seguro de salir del formulario de emitir sin haber guardado?", "Saliendo del formulario"))
            context.PreventNavigation();
    }

    private async Task ConsultaTipoCambioDia()
    {
        DateTime? fechaTipoCambioDia = MovimientoInsertar.FechaHoraOperacion?.Date;
        TipoCambioDiaConsultaPorFechaDto tipoCambioDia = fechaTipoCambioDia.HasValue ? await ITipoCambioDia.ConsultaPorFecha((DateTime)fechaTipoCambioDia, "V") : new();
        MovimientoInsertar.CodigoTipoCambioDia = tipoCambioDia.Codigo;
        MovimientoInsertar.MontoTipoCambioDia = tipoCambioDia.Monto;
    }

    private void IniciarAccionModal(string tipoAccion, string origenModal)
    {
        TipoAccionModal = tipoAccion;
        OrigenModal = origenModal;
        switch (tipoAccion)
        {
            case "I":
                IconoAccionModal = SvgIcon.TableAdd;
                VerboAccionModal = "Agregar";
                DescripcionAccionModal = "Agregando";
                break;
            case "M":
                IconoAccionModal = SvgIcon.TableCellProperties;
                VerboAccionModal = "Modificar";
                DescripcionAccionModal = "Modificando";
                break;
            case "V":
                DescripcionAccionModal = "Visualizando";
                break;
        }
        switch (origenModal)
        {
            case "detalle":
                if (tipoAccion is "I" or "M")
                    DetalleContext = new EditContext(DetalleInsertar);
                EsVisibleModalDetalle = true;
                break;
        }
    }

    private void IniciarAccionDialog(string tipoAccion, string origenDialog)
    {
        TipoAccionDialog = tipoAccion;
        OrigenDialog = origenDialog;
        if(origenDialog is "detalle")
            EsVisibleDialogDetalle = true;
    }

    public void CerrarModal()
    {
        if (OrigenModal is "detalle")
            IsModifiedDetalle = EsVisibleModalDetalle = false; 
        VerboAccionModal = DescripcionAccionModal = TipoAccionModal = CodigoItemAccion = "";
        StateHasChanged();
    }

    private void Volver() => INavigation.NavigateTo($"{rutaEmpresa}{rutaServicio}");
 
    public void CerrarDialog()
    {
        if (OrigenDialog is "detalle")
            EsVisibleDialogDetalle = false;
        TipoAccionDialog = OrigenDialog = "";
        StateHasChanged();
    }

    private void ActualizarHoraOperacion() => MovimientoInsertar.FechaHoraOperacion = MovimientoInsertar.FechaHoraOperacion?.Date.Add(DateTime.Now.TimeOfDay);

    #region CatalogoReferencia
    public async Task MostrarCatalogoReferencia()
    {
        if (AgregarItemDetalleEsValido())
        {
            IsLoadingCatalogoReferencias = true; 
            CodigoEjercicioFiltro = CodigoEjercicio; 
            switch (CodigoTipoMovimiento)
            {
                case "M05": //Ingreso por orden de compra
                    ItemsSelectedOrden = [];
                    await CargarCatalogoOrdenes();
                    EsVisibleCatalogoOrdenes = true;
                    break;
                default:
                    IsLoadingCatalogoReferencias = false;
                    break;
            }
        }
    }

    private async Task OnComboEjercicioValueChanged(string value)
    {
        CodigoEjercicioFiltro = value;
        if (!string.IsNullOrEmpty(CodigoEjercicioFiltro))
        {
            switch (CodigoTipoMovimiento)
            {
                case "M05": //Ingreso por orden de compra 
                    await CargarCatalogoOrdenes(); 
                    break; 
            }
        } 
    }

    private void QuitarItemReferencia()
    { 
        switch (CodigoTipoMovimiento)
        {
            case "M05": //Ingreso por orden de compra 
                OrdenCatalogoIngresarDto itemNuevo = new();
                IMapper.Map(itemNuevo, Movimiento);
                IMapper.Map(itemNuevo, MovimientoInsertar);
                 
                EsVisibleBotonCatalogoReferencias = EsVisibleBotonOperacion = true;
                EsVisibleDialogReferencia = false;
                break;
        }
    }

    public async Task MostrarCatalogoDetallesReferencia()
    {
        IsLoadingCatalogoDetallesReferencia = true;
        switch (CodigoTipoMovimiento)
        {
            case "M05": //Ingreso por orden de compra
                ItemsSelectedDetalleOrden = [];
                await CargarCatalogoDetalles();
                EsVisibleCatalogoDetallesOrden = true;
                break;
            default:
                IsLoadingCatalogoDetallesReferencia = false;
                break;
        }
    }

    #region CatalogoOrdenesIngresar  
    private async Task VisibleCatalogoOrdenesChangedHandler(bool esVisible)
    {
        if (!esVisible)
        {
            if (ItemsSelectedOrden.Any() && !await Dialog.ConfirmAsync("¿Está seguro de salir del catálogo y que la orden seleccionada no se cargue?", "Saliendo de catálogo"))
                return;

            EsVisibleCatalogoOrdenes = false;
            CatalogoOrdenesIngresar = null;
        }
    }

    private void OnRowDoubleClickCatalogoOrdenIngresarHandler(GridRowClickEventArgs args) => AgregarItemOrden(args.Item as OrdenCatalogoIngresarDto);

    private async Task CargarCatalogoOrdenes()
    {
        try
        {
            IsAuthUser = (await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio, User.FindFirst("code").Value)).esValido;
            if (!IsAuthUser) return;

            ItemsSelectedOrden = [];
            CatalogoOrdenesIngresar = (List<OrdenCatalogoIngresarDto>)await IOrden.CatalogoIngresar(Empresa.Codigo, CodigoEjercicioFiltro);
        }
        catch (HttpRequestException)
        {
            if (EsVisibleCatalogoOrdenes)
                Fnc.MostrarAlerta(AlertCatalogoOrdenes, Cnf.MsgErrorNotConnectApi, "error");
            else
                Notify.ShowError("NC");
        }
        catch (HttpResponseException ex)
        {
            if (EsVisibleCatalogoOrdenes)
                Fnc.MostrarAlerta(AlertCatalogoOrdenes, $"{(ex.Code == "NF" ? Cnf.MsgErrorNotFoundAPi : ex.Message)}", "error");
            else
                Notify.ShowError(ex.Code, ex);
        }
        catch (Exception ex)
        {
            if (EsVisibleCatalogoOrdenes)
                Fnc.MostrarAlerta(AlertCatalogoOrdenes, Cnf.MsgErrorFuncAppWeb, "error");
            else
                Notify.ShowError("FA", ex);
        }
        finally
        {
            IsLoadingCatalogoReferencias = false;
            Notify.ShowLoading(false);
        }
    }

    private void SeleccionarItemOrden()
    {
        if (!ItemsSelectedOrden.Any())
        {
            Fnc.MostrarAlerta(AlertCatalogoOrdenes, "Es necesario que seleccione al menos un ítem del catálogo", "warning");
            return;
        }

        AgregarItemOrden(ItemsSelectedOrden.First());
    }

    private void AgregarItemOrden(OrdenCatalogoIngresarDto item)
    {
        IMapper.Map(item, Movimiento);
        IMapper.Map(item, MovimientoInsertar);

        //IsEditAmountMN = item.CodigoMoneda == MN.Codigo;
        //IsEditAmountME = item.CodigoMoneda == ME.Codigo;

        EditContext.NotifyFieldChanged(EditContext.Field("CodigoDocumentoReferencia"));
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoMoneda"));
        Validator.MsgErrorEntidad = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidad"));
        Validator.MsgErrorCentroCosto = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoCentroCosto"));
        Validator.MsgErrorLocal = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoLocal"));
         
        EsVisibleBotonCatalogoDetallesReferencia = EsVisibleBotonQuitarReferencia = true;
        EsVisibleCatalogoOrdenes = EsVisibleBotonCatalogoReferencias = EsVisibleBotonOperacion = false;
    }

    private async Task VisibleCatalogoDetallesOrdenChangedHandler(bool esVisible)
    {
        if (!esVisible)
        {
            if (ItemsSelectedDetalleOrden.Any() && !CatalogoDetallesOrden.Any(x => x.IsErrorSelected) && !await Dialog.ConfirmAsync("¿Está seguro de salir del catálogo y que los ítems seleccionados no se carguen?", "Saliendo de catálogo"))
                return;

            EsVisibleCatalogoDetallesOrden = false;
            CatalogoDetallesOrden = null;
        }
    }

    private async Task CargarCatalogoDetalles()
    {
        try
        {
            IsAuthUser = (await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio, User.FindFirst("code").Value)).esValido;
            if (!IsAuthUser) return;
            ItemsSelectedDetalleOrden = [];

            CatalogoDetallesOrden = (List<OrdenDetalleCatalogoIngresarDto>)await IOrden.CatalogoDetallesIngresar(Empresa.Codigo, Movimiento.DocumentoReferencia);

            SelectionModeCatalogoDetalleReferencia = CatalogoDetallesOrden is null ? GridSelectionMode.None : GridSelectionMode.Multiple;
            EsSeleccionableCatalogoDetalleReferencia = CatalogoDetallesOrden is not null;
            GridCatalogoDetalleOrdenRef?.Rebind();
        }
        catch (HttpRequestException)
        {
            if (EsVisibleCatalogoDetallesOrden)
                Fnc.MostrarAlerta(AlertCatalogoDetallesOrden, Cnf.MsgErrorNotConnectApi, "error");
            else
                Notify.ShowError("NC");
        }
        catch (HttpResponseException ex)
        {
            if (EsVisibleCatalogoDetallesOrden)
                Fnc.MostrarAlerta(AlertCatalogoDetallesOrden, $"{(ex.Code == "NF" ? Cnf.MsgErrorNotFoundAPi : ex.Message)}", "error");
            else
                Notify.ShowError(ex.Code, ex);
        }
        catch (Exception ex)
        {
            if (EsVisibleCatalogoDetallesOrden)
                Fnc.MostrarAlerta(AlertCatalogoDetallesOrden, Cnf.MsgErrorFuncAppWeb, "error");
            else
                Notify.ShowError("FA", ex);
        }
        finally
        {
            IsLoadingCatalogoDetallesReferencia = false;
            Notify.ShowLoading(false);
        }
    }

    public static void OnRowCatalogoDetalleOrdenRenderHandler(GridRowRenderEventArgs args) => args.Class = (args.Item as OrdenDetalleCatalogoIngresarDto).IsErrorSelected ? "row-error" : "";

    protected void OnSelectCatalogoDetalle(IEnumerable<OrdenDetalleCatalogoIngresarDto> itemsSeleccionado)
    {
        ItemsSelectedDetalleOrden = itemsSeleccionado;

        if (CatalogoDetallesOrden is not null)
        {
            if (ItemsSelectedDetalleOrden.Any())
            {
                foreach (OrdenDetalleCatalogoIngresarDto item in CatalogoDetallesOrden.Where(x => x.IsErrorSelected))
                    item.IsErrorSelected = ItemsSelectedDetalleOrden.Any(i => i.CodigoArticulo == item.CodigoArticulo);
            }
            else
            {
                CatalogoDetallesOrden.ForEach(x => { x.IsErrorSelected = false; });
            }
        }
    }

    private async Task AgregarItemsDetalleOrden()
    {
        if (!ItemsSelectedDetalleOrden.Any())
        {
            Fnc.MostrarAlerta(AlertCatalogoDetallesOrden, "Es necesario que seleccione al menos un ítem del catálogo", "warning");
            return;
        }

        CatalogoDetallesOrden.ForEach(x => { x.IsErrorSelected = false; });
        if (GridDetalles.Count > 0)
        {
            bool esRepetidoItemsDetalle = false;
            foreach (OrdenDetalleCatalogoIngresarDto item in ItemsSelectedDetalleOrden)
            {
                if (GridDetalles.Any(x => x.CodigoArticulo == item.CodigoArticulo))
                {
                    esRepetidoItemsDetalle = true;
                    Fnc.MostrarAlerta(AlertCatalogoDetallesOrden, $"El artículo {item.CodigoArticulo} ya ha sido agregado en el detalle", "warning");
                    CatalogoDetallesOrden[CatalogoDetallesOrden.FindIndex(i => i.CodigoArticulo == item.CodigoArticulo)].IsErrorSelected = true;
                }
            }
            if (esRepetidoItemsDetalle)
                return;
        }

        GridState<MovimientoDetalleGrid> detalleState = GridDetalleRef.GetState();
        foreach (OrdenDetalleCatalogoIngresarDto itemCatalogo in ItemsSelectedDetalleOrden)
        {
            DetalleInsertar = new();
            Detalle = new();

            IMapper.Map(itemCatalogo, Detalle);
            IMapper.Map(Detalle, DetalleInsertar);

            ItemGridDetalle = IMapper.Map<MovimientoDetalleGrid>(Detalle);

            MovimientoInsertar.Detalles.Add(DetalleInsertar);
            GridDetalles.Add(ItemGridDetalle);
            detalleState.InsertedItem = ItemGridDetalle;
            await GridDetalleRef.SetStateAsync(detalleState);
        }

        detalleState.InsertedItem =  detalleState.OriginalEditItem = detalleState.EditItem = null;
        await GridDetalleRef.SetStateAsync(detalleState);

        //ModificarImportesTotales();
        EsVisibleBotonQuitarReferencia = GridDetalles.Count == 0;
        EsVisibleCatalogoDetallesOrden = false;
    } 
    #endregion
    #endregion
     
    #region Detalle
    public void InvalidarAccionDetalle(EditContext _) => Fnc.MostrarAlerta(AlertDetalle, Cnf.MsgErrorInvalidEditContext, "error");

	private async Task CerrarDetalle()
	{
        if (TipoAccionModal is not "V" && IsModifiedDetalle && !await Dialog.ConfirmAsync("¿Está seguro de cerrar el formulario y que los datos no se carguen?", "Saliendo del formulario"))
            return;
        CerrarModal();
	}

	private bool AgregarItemDetalleEsValido()
    {
		bool esValido = true;
		if (string.IsNullOrEmpty(MovimientoInsertar.CodigoOperacionLogistica))
		{
			Fnc.MostrarAlerta(Alert, "Es necesario que seleccione la [Operación Logística]", "error");
			esValido = false;
		}
        return esValido;
	}

    public void MostrarAgregarDetalle()
    { 
        if (AgregarItemDetalleEsValido())
        {
            DetalleInsertar = new();
            Detalle = new();
            DetalleValidator = new();
            IniciarAccionModal("I", "detalle");
        }
    }

    public void MostrarModificarDetalle(MovimientoDetalleGrid item = null)
    {
		if (TipoAccionModal is not "V")
			Detalle = IMapper.Map<MovimientoDetalleObtenerDto>(item);

		DetalleInsertar = IMapper.Map<MovimientoDetalleInsertarDto>(Detalle);
		CodigoItemAccion = Detalle.CodigoArticulo; 
        DetalleValidator = new()
        {
            UnidadConversion = Detalle.UnidadConversion
        };
		IniciarAccionModal("M", "detalle");
	}

    public async Task ActivarModificacionDetalle(MovimientoDetalleGrid item)
    {
        GridState<MovimientoDetalleGrid> detalleState = GridDetalleRef.GetState();
        detalleState.InsertedItem = null;
        detalleState.OriginalEditItem = item; 
        detalleState.EditItem = (MovimientoDetalleGrid) item.Clone();
        detalleState.EditItem.IsEditingItem = true; 
        IsEditingGridDetalle = true;
        CodigoItemAccion = item.CodigoArticulo;
        GridDetalleValidator = new()
        {
            UnidadConversion = item.UnidadConversion
        };
        await GridDetalleRef.SetStateAsync(detalleState);  
    }

    private void MostrarQuitarDetalle(string codigoItem)
    {
        CodigoItemAccion = codigoItem;
        IniciarAccionDialog("Q", "detalle");
    }

	public void VerItemDetalle(MovimientoDetalleGrid item)
	{
		Detalle = IMapper.Map<MovimientoDetalleObtenerDto>(item);
		CodigoItemAccion = item.CodigoArticulo;
		IniciarAccionModal("V", "detalle");
	} 
     
    private void OnChangeDetalleCantidadHandler(object value)
    {
        decimal? cantidad = (decimal?)value;
        IsModifiedDetalle = Fnc.VerifyContextIsChanged(Detalle.Cantidad != cantidad, DetalleContext, "Cantidad");
        //ModificarImportesItem(cantidad, DetalleInsertar.PrecioUnitario, DetalleInsertar.EsAfectoImpuesto);
    }

    private void OnChangeDetallePrecioUnitarioHandler(object value)
    {
        decimal? precioUnitario = (decimal?)value;
        //IsModifiedDetalle = Fnc.VerifyContextIsChanged(Detalle.PrecioUnitario != precioUnitario, DetalleContext, "PrecioUnitario");
        //ModificarImportesItem(DetalleInsertar.Cantidad, precioUnitario, DetalleInsertar.EsAfectoImpuesto);
    }

    public static void OnCellCantidadRenderHandler(GridCellRenderEventArgs args) => args.Class = !(args.Item as MovimientoDetalleGrid).Cantidad.HasValue ? "cell-error" : "cell-editable";

    public static void OnCellAlmacenRenderHandler(GridCellRenderEventArgs args) => args.Class = string.IsNullOrEmpty((args.Item as MovimientoDetalleGrid).CodigoAlmacen) ? "cell-error" : "cell-editable";

    //public static void OnCellPrecioUnitarioRenderHandler(GridCellRenderEventArgs args) => args.Class = !(args.Item as MovimientoDetalleGrid).PrecioUnitario.HasValue ? "cell-error" : "cell-editable";

    public async Task CancelDetalleHandler(GridCommandEventArgs args) => await DesactivarModificacionDetalle();

    private async Task DesactivarModificacionDetalle()
    { 
        GridState<MovimientoDetalleGrid> detalleState = GridDetalleRef.GetState();
        detalleState.InsertedItem = detalleState.OriginalEditItem = detalleState.EditItem = null;
        IsEditingGridDetalle = false;
        CodigoItemAccion = "";
        await GridDetalleRef.SetStateAsync(detalleState);
    }

    private async Task GrabarModificacionDetalle()
    {
        GridState<MovimientoDetalleGrid> detalleState = GridDetalleRef.GetState();
        UpdateItemDetalleHandler(new GridCommandEventArgs() { Item = detalleState.EditItem is not null ? detalleState.EditItem : detalleState.InsertedItem});
        await DesactivarModificacionDetalle();
    }

    public void UpdateItemDetalleHandler(GridCommandEventArgs args)
    {
		MovimientoDetalleGrid item = (MovimientoDetalleGrid) args.Item;   
        int index = GridDetalles.FindIndex(i => i.CodigoArticulo == item.CodigoArticulo);

        if (args.Field is null)
        {
            MovimientoInsertar.Detalles[index] = IMapper.Map<MovimientoDetalleInsertarDto>(item);
            GridDetalles[index] = item;
            GridDetalles[index].IsEditingItem = false;
        }
        else if (args.Field == "Cantidad")
        {
            MovimientoInsertar.Detalles[index].Cantidad = GridDetalles[index].Cantidad = item.Cantidad;
        }
            

        //ModificarImportesItem(item.Cantidad, item.PrecioUnitario, item.EsAfectoImpuesto, index);
        ModificarImportesTotales();
        IsEditingGridDetalle = false;
    }

    private void ModificarImportesItem(decimal? cantidad, decimal? precioUnitario, bool esAfectoImpuesto, int indexGrid = -1)
    {
        decimal? importeBruto = null, importeImpuesto = null, importeNeto = null;
        //if (cantidad.HasValue && precioUnitario.HasValue)
        //{
        //    importeBruto = Math.Round((decimal)(cantidad * precioUnitario), 2, MidpointRounding.AwayFromZero);
        //    importeImpuesto = Movimiento.EsAfectoImpuesto ? Math.Round((decimal)(importeBruto * (esAfectoImpuesto ? MovimientoInsertar.PorcentajeImpuesto: 0)), 2, MidpointRounding.AwayFromZero) : 0;
        //    importeNeto = importeBruto + importeImpuesto;
        //}
        //if (indexGrid > -1)
        //{
        //    MovimientoInsertar.Detalles[indexGrid].ImporteBruto = GridDetalles[indexGrid].ImporteBruto = importeBruto;
        //    MovimientoInsertar.Detalles[indexGrid].ImporteImpuesto = GridDetalles[indexGrid].ImporteImpuesto = importeImpuesto;
        //    MovimientoInsertar.Detalles[indexGrid].ImporteNeto = GridDetalles[indexGrid].ImporteNeto = importeNeto;
        //}
        //else
        //{
        //    DetalleInsertar.ImporteBruto = importeBruto;
        //    DetalleInsertar.ImporteImpuesto = importeImpuesto;
        //    DetalleInsertar.ImporteNeto = importeNeto;
        //}
    }

    private void RecalcularImportes()
    {
        if (GridDetalles.Count > 0) 
        {
            foreach ((MovimientoDetalleGrid item, int index) in GridDetalles.Select((item, index) => (item, index))) 
                //ModificarImportesItem(item.Cantidad, item.PrecioUnitario, item.EsAfectoImpuesto, index); 

            ModificarImportesTotales(); 
        }
    }

    private void ModificarImportesTotales()
    {
        bool esGridVacio = GridDetalles.Count == 0;
        //MovimientoInsertar.TotalImporteBruto = esGridVacio ? 0 : GridDetalles.Sum(x => x.ImporteBruto);
        //MovimientoInsertar.TotalImporteImpuesto = esGridVacio ? 0 : GridDetalles.Sum(x => x.ImporteImpuesto);
        //MovimientoInsertar.TotalImporteNeto = esGridVacio ? 0 : GridDetalles.Sum(x => x.ImporteNeto);
    }

    private void ValueChangeArticuloHandler(string value)
    {
        DetalleInsertar.CodigoArticulo = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(DetalleValidator.MsgErrorArticulo) && (Detalle.CodigoArticulo ?? "").Trim() != (DetalleInsertar.CodigoArticulo ?? ""))
	        Detalle.CodigoArticulo = DetalleValidator.MsgErrorArticulo = null;
    }

    private async Task OnChangeArticuloHandler(object value)
    {
		string codigo = (string)(value ?? "");
		if (DetalleContext.IsValid(DetalleContext.Field("CodigoArticulo")))
		{
			if ((Detalle.CodigoArticulo ?? "") == codigo) goto exit;
            if (GridDetalles.Any(x => x.CodigoArticulo.Trim() == codigo))
            {
                DetalleValidator.MsgErrorArticulo = "El código ya existe en el detalle del registro";
            }
            else
            {  
                (ArticuloObtenerPorCodigoEmpresaProcesoDocumentoDto item, DetalleValidator.MsgErrorArticulo) = await IUtil.ObtenerArticuloPorCodigoEmpresaProcesoDocumento(AlertDetalle, codigo, Empresa.Codigo, CodigoTipoMovimiento);
			    if (item is not null)
			    {
			        Detalle.CodigoArticulo = item.CodigoArticulo.Trim();
			        Detalle.NombreArticulo = item.NombreArticulo;
			        Detalle.UnidadConversion = DetalleValidator.UnidadConversion = item.UnidadConversion; 
			        Detalle.CodigoUnidadMedida = item.CodigoUnidadMedida;
			        Detalle.NombreUnidadMedida = item.NombreUnidadMedida;
			        Detalle.Presentacion = item.Presentacion;
			        Detalle.CodigoTipoArticulo = item.CodigoTipoArticulo;
			        Detalle.NombreTipoArticulo = item.NombreTipoArticulo;
			        if (DetalleInsertar.Cantidad.HasValue)
				        DetalleContext.NotifyFieldChanged(DetalleContext.Field("Cantidad"));
         //           if (Detalle.EsAfectoImpuesto != item.EsAfectoImpuesto)
         //           {
         //               DetalleInsertar.EsAfectoImpuesto = Detalle.EsAfectoImpuesto = item.EsAfectoImpuesto;
					    //if (DetalleInsertar.Cantidad.HasValue && DetalleContext.IsValid(DetalleContext.Field("Cantidad")) && DetalleInsertar.PrecioUnitario.HasValue)
						   // ModificarImportesItem(DetalleInsertar.Cantidad, DetalleInsertar.PrecioUnitario, DetalleInsertar.EsAfectoImpuesto);
         //           }
				    goto exit;
			    }
            } 
            DetalleContext.NotifyFieldChanged(DetalleContext.Field("CodigoArticulo"));
		}
        Detalle.CodigoArticulo = codigo;
		Detalle.NombreArticulo = Detalle.CodigoUnidadMedida = Detalle.NombreUnidadMedida = Detalle.CodigoTipoArticulo = Detalle.NombreTipoArticulo = Detalle.Presentacion = null;
		Detalle.UnidadConversion = DetalleValidator.UnidadConversion = null;
		if (string.IsNullOrEmpty(codigo)) 
            DetalleContext.MarkAsUnmodified(DetalleContext.Field("CodigoArticulo")); 
		if (DetalleInsertar.Cantidad.HasValue)
			DetalleContext.NotifyFieldChanged(DetalleContext.Field("Cantidad"));
	exit:
		IsModifiedDetalle = DetalleContext.IsModified();
	} 

    private async Task AccionarDetalle(string tipoAccion)
    {
        GridState<MovimientoDetalleGrid> detalleState = GridDetalleRef.GetState();
        int index = GridDetalles.FindIndex(i => i.CodigoArticulo == CodigoItemAccion);

        switch (tipoAccion)
        {
            case "I" or "M":
				ItemGridDetalle = IMapper.Map<MovimientoDetalleGrid>(IMapper.Map(DetalleInsertar, Detalle));
				if (tipoAccion == "I")
                {
                    MovimientoInsertar.Detalles.Add(DetalleInsertar);
                    GridDetalles.Add(ItemGridDetalle);
                    detalleState.InsertedItem = ItemGridDetalle;
                }
                else
                {
                    MovimientoInsertar.Detalles[index] = DetalleInsertar;
                    GridDetalles[index] = ItemGridDetalle; 
				} 
                break;
            case "Q":
                MovimientoInsertar.Detalles.RemoveAt(index);
                GridDetalles.RemoveAt(index);
                CerrarDialog();
                break;
        };
        CerrarModal();
        ModificarImportesTotales();
        
		EsVisibleBotonOperacion = GridDetalles.Count == 0;

		await GridDetalleRef.SetStateAsync(detalleState);
    }
    #endregion

    #region Catalogos
    private async Task CargarItemCatalogoOperacionLogistica(OperacionLogisticaCatalogoPorEmpresaSesionDto item)
    {
        MovimientoInsertar.CodigoOperacionLogistica = item.CodigoOperacionLogistica;
        Movimiento.NombreOperacionLogistica = item.NombreOperacionLogistica;
        CodigoTipoMovimiento = item.CodigoTipoMovimiento;
        
		EditContext.NotifyFieldChanged(EditContext.Field("CodigoOperacionLogistica"));
        TipoMovimiento = await ITipoMovimiento.ConsultaPorCodigo(CodigoTipoMovimiento) ?? new();
        Validator.EsRequeridoReferencia = TipoMovimiento.EsRequeridoReferencia;
        EsVisibleBotonCatalogoReferencias = TipoMovimiento.EsRequeridoReferencia;

        ActualizarHoraOperacion();
        IsModified = EditContext.IsModified();
    }

    private void CargarItemCatalogoLocal(LocalCatalogoPorEmpresaDto item)
    {
        MovimientoInsertar.CodigoLocal = Movimiento.CodigoLocal = item.CodigoLocal;
        Movimiento.NombreLocal = item.NombreLocal;
        Validator.MsgErrorLocal = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoLocal"));
        ActualizarHoraOperacion();
        IsModified = EditContext.IsModified();
    }

    private void CargarItemCatalogoCentroCosto(CentroCostoCatalogoDto item)
    {
        MovimientoInsertar.CodigoCentroCosto = Movimiento.CodigoCentroCosto = item.CodigoCentroCosto.Trim();
        Movimiento.NombreCentroCosto = item.NombreCentroCosto;
        Validator.MsgErrorCentroCosto = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoCentroCosto"));
        ActualizarHoraOperacion();
        IsModified = EditContext.IsModified();
    }

    private void CargarItemCatalogoEntidad(EntidadCatalogoPorEmpresaDto item)
    {
        MovimientoInsertar.CodigoEntidad = Movimiento.CodigoEntidad = item.CodigoEntidad.Trim();
        Movimiento.NombreEntidad = item.NombreEntidad;
        Validator.MsgErrorEntidad = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidad"));
        ActualizarHoraOperacion();
        IsModified = EditContext.IsModified();
    }

    private void CargarItemCatalogoMoneda(MonedaCatalogoDto item)
    {
        MovimientoInsertar.CodigoMoneda = Movimiento.CodigoMoneda = item.CodigoMoneda;
        Movimiento.NombreMoneda = item.NombreMoneda;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoMoneda"));
        ActualizarHoraOperacion();
        IsModified = EditContext.IsModified();
    }
    #endregion

    #region EditContextHandlers

    private void ValueChangeEntidadHandler(string value)
    {
        MovimientoInsertar.CodigoEntidad = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorEntidad) && (Movimiento.CodigoEntidad ?? "").Trim() != (MovimientoInsertar.CodigoEntidad ?? ""))
            Movimiento.CodigoEntidad = Validator.MsgErrorEntidad = null;
    }

	private async Task OnChangeEntidadHandler(object value)
    {
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoEntidad")))
		{ 
            if ((Movimiento.CodigoEntidad ?? "") == codigo) goto exit;
			(EntidadObtenerPorCodigoEmpresaDto item, Validator.MsgErrorEntidad) = await IUtil.ObtenerEntidadPorCodigoEmpresa(Alert, codigo, Empresa.Codigo, TipoMovimiento.FlagTipoEntidad);
            if (item is not null)
            {				
                Movimiento.CodigoEntidad = item.CodigoEntidad.Trim();
			    Movimiento.NombreEntidad = item.NombreEntidad; 
                goto exit;
            }
            EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidad"));
		}
        Movimiento.CodigoEntidad = codigo;
        Movimiento.NombreEntidad = null;
        if (string.IsNullOrEmpty(codigo)) EditContext.MarkAsUnmodified(EditContext.Field("CodigoEntidad"));
	exit:
        ActualizarHoraOperacion(); 
        IsModified = EditContext.IsModified();
	}

    private void ValueChangeCentroCostoHandler(string value)
    {
        MovimientoInsertar.CodigoCentroCosto = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorCentroCosto) && (Movimiento.CodigoCentroCosto ?? "").Trim() != (MovimientoInsertar.CodigoCentroCosto ?? ""))
            Movimiento.CodigoCentroCosto = Validator.MsgErrorCentroCosto = null;
    }

    private async Task OnChangeCentroCostoHandler(object value)
    {
        string codigo = (string)(value ?? "");
        if (EditContext.IsValid(EditContext.Field("CodigoCentroCosto")))
        {
            if (!string.IsNullOrEmpty(codigo))
            {
                if ((Movimiento.CodigoCentroCosto ?? "") == codigo) goto exit;
                (CentroCostoObtenerPorCodigoDto item, Validator.MsgErrorCentroCosto) = await IUtil.ObtenerCentroCostoPorCodigo(Alert, codigo, Empresa.Codigo);
                if (item is not null)
                {
                    Movimiento.CodigoCentroCosto = item.CodigoCentroCosto.Trim();
                    Movimiento.NombreCentroCosto = item.NombreCentroCosto;
                    goto exit;
                }
                EditContext.NotifyFieldChanged(EditContext.Field("CodigoCentroCosto"));
            }
            else
            {
                MovimientoInsertar.CodigoCentroCosto = null;
                EditContext.MarkAsUnmodified(EditContext.Field("CodigoCentroCosto"));
            }
        }
        Movimiento.CodigoCentroCosto = codigo;
        Movimiento.NombreCentroCosto = null;
    exit:
        ActualizarHoraOperacion();
        IsModified = EditContext.IsModified();
    }

    private void ValueChangeLocalHandler(string value)
    {
        MovimientoInsertar.CodigoLocal = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorLocal) && (Movimiento.CodigoLocal ?? "").Trim() != (MovimientoInsertar.CodigoLocal ?? ""))
            Movimiento.CodigoLocal = Validator.MsgErrorLocal = null;
    }

    private async Task OnChangeLocalHandler(object value)
    { 
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoLocal")))
		{ 
			if (!string.IsNullOrEmpty(codigo))
			{ 
                if ((Movimiento.CodigoLocal ?? "") == codigo) goto exit;
                (LocalObtenerPorCodigoEmpresaDto item, Validator.MsgErrorLocal) = await IUtil.ObtenerLocalPorCodigoEmpresa(Alert, codigo, Empresa.Codigo);
                if (item is not null)
                {
                    Movimiento.CodigoLocal = item.CodigoLocal;
                    Movimiento.NombreLocal = item.NombreLocal; 
                    goto exit;
                } 
                EditContext.NotifyFieldChanged(EditContext.Field("CodigoLocal"));    
			}
			else
            {
                MovimientoInsertar.CodigoLocal = null;
				EditContext.MarkAsUnmodified(EditContext.Field("CodigoLocal"));
            }
		}
        Movimiento.CodigoLocal = codigo;
		Movimiento.NombreLocal = null;
	exit:
        ActualizarHoraOperacion();
        IsModified = EditContext.IsModified();
    }

    private void OnChangeObservacionHandler(object value)
    {
        ActualizarHoraOperacion();
        IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "Observacion");
    }

    private void OnChangeDescripcionReferenciaHandler(object value)
    {
        ActualizarHoraOperacion();
        IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "DescripcionReferencia");
    }

    private void OnChangeComentarioHandler(object value)
    {
        ActualizarHoraOperacion();
        IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "Comentario");
    }

    private async Task OnChangeFechaHoraOperacionHandler(object value)
    {
        IsModified = Fnc.VerifyContextIsChanged((value is null && Movimiento.FechaHoraOperacion.HasValue) || (!Movimiento.FechaHoraOperacion.HasValue && value is not null) || (Movimiento.FechaHoraOperacion.HasValue && value is not null && Movimiento.FechaHoraOperacion != (DateTime?)value), EditContext, "FechaHoraOperacion");
        await ConsultaTipoCambioDia();
        //ActualizarMontosPorTipoCambioDia();
    }

    private void OnFechaHoraOperacionPopupClose(DatePickerCloseEventArgs _) => ActualizarHoraOperacion();
    #endregion

    public void Dispose() => GC.SuppressFinalize(this);
}
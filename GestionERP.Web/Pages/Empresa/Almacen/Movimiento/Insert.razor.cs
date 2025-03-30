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
    public bool IsInitPage { get; set; }
    public bool IsEditingGridDetalle { get; set; }
    public string CodigoLocalNumerador { get; set; }
    public string CodigoTipoMovimiento { get; set; }
    public List<DateTime> FechasCerradoOperacion { get; set; }
    public EmpresaEjercicioConsultaIntervaloFechaDto FechaIntervalo { get; set; }
	private IEnumerable<MovimientoFlag> TiposRegistro { get; set; }  
    public List<OrdenCatalogoIngresarDto> CatalogoOrdenesIngresar { get; set; }
    public IEnumerable<OrdenCatalogoIngresarDto> ItemsSelectedOrden { get; set; }
    public string CodigoEjercicio { get; set; }  
    private IEnumerable<EmpresaEjercicioCatalogoDto> CatalogoEjercicios { get; set; }  
	public bool EsVisibleBotonOperacion { get; set; }
    public bool EsVisibleCatalogoOrdenes { get; set; } 
    private bool IsAuthUser { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool IsModified { get; set; }
    private bool IsModifiedDetalle { get; set; }
    private bool IsLoadingCatalogoOrdenes { get; set; }
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
    private TelerikGrid<MovimientoDetalleGrid> GridDetalleRef { get; set; }
    private TelerikGrid<OrdenCatalogoIngresarDto> GridCatalogoRef { get; set; }
    public GridSelectionMode SelectionModeCatalogoOrden { get; set; }
    private bool EsSeleccionableCatalogoOrden { get; set; }
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
    [Inject] public IPrincipalUsuario IUsuario { get; set; } 
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
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

            FechaIntervalo = await IEmpresa.ConsultaEjercicioIntervaloFecha(Empresa.Codigo);
            (await IEmpresa.ConsultaModuloPeriodoFechasEsCerradoOperacion(Empresa.Codigo, codigoModulo))?.ToList().ForEach(x => FechasCerradoOperacion.Add(x.Fecha));
            MovimientoInsertar.FechaHoraOperacion = !FechasCerradoOperacion.Any(x => x == DateTime.Now.Date) ? DateTime.Now.Date : null;
            if (MovimientoInsertar.FechaHoraOperacion.HasValue)
                Movimiento.FechaHoraOperacion = (DateTime)MovimientoInsertar.FechaHoraOperacion; 

            Validator = new();
            EditContext = new EditContext(MovimientoInsertar);
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

    private async Task Insertar()
    {
        try
        {
            IsLoadingAction = true;

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
        if (string.IsNullOrEmpty(MovimientoInsertar.CodigoLocal) && string.IsNullOrEmpty(MovimientoInsertar.DescripcionReferencia))
        {
            Fnc.MostrarAlerta(Alert, "Es necesario que especifique un local de recepción o describa el lugar de recojo", "error");
            esValido = false;
        }
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

	#region CatalogoOrdenesIngresar
	public async Task MostrarCatalogoOrdenes()
	{
        if (AgregarItemDetalleEsValido())
        {
            IsLoadingCatalogoOrdenes = true;

			CatalogoEjercicios = await IEmpresa.CatalogoEjercicios(Empresa.Codigo) ?? [];
			CodigoEjercicio = await IEmpresa.ConsultaEjercicioCodigoPorAnio(Empresa.Codigo, DateTime.Now.Year);
			ItemsSelectedOrden = [];

			await CargarCatalogoOrdenes(); 
			EsVisibleCatalogoOrdenes = true; 
		}
	}

	private async Task VisibleCatalogoOrdenesChangedHandler(bool esVisible)
	{
		if (!esVisible)
		{
            //if (ItemsSelectedOrden.Any() && !CatalogoOrdenesIngresar.Any(x => x.IsErrorSelected) && !await Dialog.ConfirmAsync("¿Está seguro de salir del catálogo y que los ítems seleccionados no se carguen?", "Saliendo de catálogo"))
            //    return;

            EsVisibleCatalogoOrdenes = false;
            CatalogoOrdenesIngresar = null;
		}
	}

	private async Task OnComboEjercicioValueChanged(string value)
	{
		CodigoEjercicio = value;
		if (!string.IsNullOrEmpty(CodigoEjercicio))
			await CargarCatalogoOrdenes();
	}

	private async Task CargarCatalogoOrdenes()
	{
		try
		{
            IsAuthUser = (await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio, User.FindFirst("code").Value)).esValido;
            if (!IsAuthUser) return;

            ItemsSelectedOrden = [];

			//CatalogoOrdenesIngresar = (List<SolicitudCatalogoAtenderDto>) await ISolicitud.CatalogoAtender(Empresa.Codigo, CodigoEjercicio, CodigoProcesoDocumento, CodigoLocalNumerador);
			 
			SelectionModeCatalogoOrden = CatalogoOrdenesIngresar is null ? GridSelectionMode.None : GridSelectionMode.Multiple;
			EsSeleccionableCatalogoOrden = CatalogoOrdenesIngresar is not null;
			GridCatalogoRef?.Rebind();
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
            IsLoadingCatalogoOrdenes = false;
			Notify.ShowLoading(false);
		}
	}

	public static void OnRowCatalogoOrdenRenderHandler(GridRowRenderEventArgs args) => args.Class = (args.Item as OrdenCatalogoIngresarDto).IsErrorSelected ? "row-error" : "";

	protected void OnSelectCatalogoOrden(IEnumerable<OrdenCatalogoIngresarDto> itemsSeleccionado)
	{
		ItemsSelectedOrden = itemsSeleccionado;
		//CatalogoOrdenesIngresar?.ForEach(x => { x.IsErrorSelected = false; });
	}

	private async Task AgregarItemsOrden()
    {
        if (!ItemsSelectedOrden.Any())
        {
            Fnc.MostrarAlerta(AlertCatalogoOrdenes, "Es necesario que seleccione al menos un ítem del catálogo", "warning");
            return;
        }

		bool isErrorSelectedCatalogo = false;
		//foreach (SolicitudCatalogoAtenderDto item in ItemsSelectedOrden)
		//{
		//	if (GridDetalles.Any(x => x.CodigoArticulo == item.CodigoArticulo))
		//	{
		//		Fnc.MostrarAlerta(AlertCatalogoOrdenes, $"El ítem {item.CodigoArticulo} ya ha sido agregado en el detalle", "error");
		//		item.IsErrorSelected = isErrorSelectedCatalogo = true;
		//	}
		//}
		//List<IGrouping<string, SolicitudCatalogoAtenderDto>> elements = ItemsSelectedOrden.GroupBy(x => x.CodigoArticulo).Where(x => x.Count() > 1).ToList();
		//if (elements.Count > 0)
		//{
		//	Fnc.MostrarAlerta(AlertCatalogoOrdenes, "Existen ítems seleccionados de igual código", "error");
		//	elements.ForEach(x => x.ToList().ForEach(x => x.IsErrorSelected = true));
		//	isErrorSelectedCatalogo = true;
		//}

		if (isErrorSelectedCatalogo)
			return;

		

        //if (string.IsNullOrEmpty(CodigoLocalNumerador))
        //{
        //    string codigoLocal = ItemsSelectedOrden.Select(x => x.CodigoLocal).First();
        //    if (string.IsNullOrEmpty(MovimientoInsertar.CodigoLocal) && !string.IsNullOrEmpty(codigoLocal) && !ItemsSelectedOrden.Any(x => x.CodigoLocal != codigoLocal))
        //    {
        //        MovimientoInsertar.CodigoLocal = codigoLocal;
        //        Movimiento.NombreLocal = ItemsSelectedOrden.Select(x => x.NombreLocal).First();
        //    }
        //}

  //      string codigoEntidadProveedor = ItemsSelectedOrden.Where(x => !string.IsNullOrEmpty(x.CodigoEntidad)).Select(x => x.CodigoEntidad).FirstOrDefault();
  //      if (string.IsNullOrEmpty(MovimientoInsertar.CodigoEntidad) && !string.IsNullOrEmpty(codigoEntidadProveedor) && !ItemsSelectedOrden.Where(x => !string.IsNullOrEmpty(x.CodigoEntidad)).Any(x => x.CodigoEntidad != codigoEntidadProveedor))
		//{
  //          SolicitudCatalogoAtenderDto itemCatalogo = ItemsSelectedOrden.Where(x => x.CodigoEntidad == codigoEntidadProveedor).First();
  //          MovimientoInsertar.CodigoEntidad = Movimiento.CodigoEntidad = itemCatalogo.CodigoEntidad;
  //          Movimiento.NombreEntidad = itemCatalogo.NombreEntidad;
  //          Validator.MsgErrorEntidad = null;
  //          EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidad"));
  //      }

		GridState<MovimientoDetalleGrid> detalleState = GridDetalleRef.GetState();
        foreach (OrdenCatalogoIngresarDto itemCatalogo in ItemsSelectedOrden)
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

        EsVisibleBotonOperacion = GridDetalles.Count == 0; 
        EsVisibleCatalogoOrdenes = false;
    }

    #endregion

    #region Detalle
    public void InvalidarAccionDetalle(EditContext editContext) => Fnc.MostrarAlerta(AlertDetalle, Cnf.MsgErrorInvalidEditContext, "error");

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

    //public static void OnCellPrecioUnitarioRenderHandler(GridCellRenderEventArgs args) => args.Class = !(args.Item as MovimientoDetalleGrid).PrecioUnitario.HasValue ? "cell-error" : "cell-editable";

    public void EditDetalleHandler(GridCommandEventArgs args)
    {
        IsEditingGridDetalle = args.Field is "Cantidad" or "PrecioUnitario";
        if (IsEditingGridDetalle)
        { 
            GridDetalleValidator = new()
            {
                UnidadConversion = (args.Item as MovimientoDetalleGrid).UnidadConversion
            };
        }
    }

    public void CancelDetalleHandler(GridCommandEventArgs args) => IsEditingGridDetalle = !(args.Field is "Cantidad" or "PrecioUnitario");

    public void UpdateItemDetalleHandler(GridCommandEventArgs args)
    {
		MovimientoDetalleGrid item = (MovimientoDetalleGrid) args.Item;   
        int index = GridDetalles.FindIndex(i => i.CodigoArticulo == item.CodigoArticulo);

        //if (args.Field == "PrecioUnitario") 
        //    MovimientoInsertar.Detalles[index].PrecioUnitario = GridDetalles[index].PrecioUnitario = item.PrecioUnitario; 
        //else if (args.Field == "Cantidad")
        //    MovimientoInsertar.Detalles[index].Cantidad = GridDetalles[index].Cantidad = item.Cantidad;
          
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
    private void CargarItemCatalogoOperacionLogistica(OperacionLogisticaCatalogoPorEmpresaSesionDto item)
    {
        MovimientoInsertar.CodigoOperacionLogistica = item.CodigoOperacionLogistica;
        Movimiento.NombreOperacionLogistica = item.NombreOperacionLogistica;
        CodigoTipoMovimiento = item.CodigoTipoMovimiento;

		EditContext.NotifyFieldChanged(EditContext.Field("CodigoOperacionLogistica"));
        IsModified = EditContext.IsModified();
    }

    private void CargarItemCatalogoLocal(LocalCatalogoPorEmpresaDto item)
    {
        MovimientoInsertar.CodigoLocal = Movimiento.CodigoLocal = item.CodigoLocal;
        Movimiento.NombreLocal = item.NombreLocal;
        Validator.MsgErrorLocal = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoLocal"));
        IsModified = EditContext.IsModified();
    }

    private void CargarItemCatalogoCentroCosto(CentroCostoCatalogoDto item)
    {
        MovimientoInsertar.CodigoCentroCosto = Movimiento.CodigoCentroCosto = item.CodigoCentroCosto.Trim();
        Movimiento.NombreCentroCosto = item.NombreCentroCosto;
        Validator.MsgErrorCentroCosto = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoCentroCosto"));
        IsModified = EditContext.IsModified();
    }

    private void CargarItemCatalogoEntidad(EntidadProveedorCatalogoPorEmpresaDto item)
    {
        MovimientoInsertar.CodigoEntidad = Movimiento.CodigoEntidad = item.CodigoEntidad.Trim();
        Movimiento.NombreEntidad = item.NombreEntidad;
        Validator.MsgErrorEntidad = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidad"));
        IsModified = EditContext.IsModified();
    }

    private void CargarItemCatalogoMoneda(MonedaCatalogoDto item)
    {
        MovimientoInsertar.CodigoMoneda = Movimiento.CodigoMoneda = item.CodigoMoneda;
        Movimiento.NombreMoneda = item.CodigoMoneda;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoMoneda"));
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
			(EntidadProveedorObtenerPorCodigoEmpresaDto item, Validator.MsgErrorEntidad) = await IUtil.ObtenerProveedorPorCodigoEmpresa(Alert, codigo, Empresa.Codigo, "L");
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
		IsModified = EditContext.IsModified();
	}

	private void OnChangeObservacionHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "Observacion");

    private void OnChangeDescripcionReferenciaHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "DescripcionReferencia");

    private void OnChangeMotivoHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "Motivo");
      
    private void OnChangeFechaHoraOperacionHandler(object value)
    {
        IsModified = Fnc.VerifyContextIsChanged((value is null && Movimiento.FechaHoraOperacion.HasValue) || (!Movimiento.FechaHoraOperacion.HasValue && value is not null) || (Movimiento.FechaHoraOperacion.HasValue && value is not null && Movimiento.FechaHoraOperacion != (DateTime?)value), EditContext, "FechaHoraOperacion");
    }
    #endregion

    public void Dispose() => GC.SuppressFinalize(this);
}
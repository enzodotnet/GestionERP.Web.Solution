using Blazored.FluentValidation;
using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Importacion;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components;
using GestionERP.Web.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Telerik.Blazor;
using Telerik.Blazor.Components;
using Telerik.SvgIcons;
using AutoMapper;
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Global; 

namespace GestionERP.Web.Pages.Empresa.Importacion.Pedido;

public partial class Insert : IDisposable
{
    #region Propiedades
    private const string codigoModulo = "02";
    private const string codigoServicio = "S106";
    private const string rutaAccion = "/emitir";
    private string rutaEmpresa = "";
	private string pathBaseUri = "";
    private const string rutaServicio = "/pedidos";
    public FluentValidationValidator validator;
     
	public PedidoInsertarDto PedidoInsertar { get; set; }
    public PedidoObtenerDto Pedido { get; set; }
    public PedidoDetalleInsertarDto DetalleInsertar { get; set; }
    public PedidoDetalleObtenerDto Detalle { get; set; } 
	public List<PedidoDetalleGrid> GridDetalles { get; set; }
	public PedidoDetalleGrid ItemGridDetalle { get; set; } 
	private EditContext EditContext { get; set; }
    private EditContext DetalleContext { get; set; }
    public PedidoDetalleGridValidator GridDetalleValidator { get; set; }
	public PedidoInsertarValidator Validator { get; set; }
    public PedidoDetalleInsertarValidator DetalleValidator { get; set; }
    private MonedaConsultaPorTipoDto MN { get; set; }
	private MonedaConsultaPorTipoDto ME { get; set; }
	public bool IsInitPage { get; set; }
    public bool IsEditingGridDetalle { get; set; }
    public bool EsVisibleBotonCatalogoOrdenes { get; set; }
	public bool EsVisibleCatalogoOrdenes { get; set; }
	public bool EsVisibleCatalogoDetalles { get; set; }
    public bool IsEditAmountMN { get; set; }
	public bool IsEditAmountME { get; set; }
	public bool EsVisibleBotonCatalogoArticulos { get; set; }
    public List<DateTime> FechasCerradoOperacion { get; set; }
    public string CodigoProcesoDocumento { get; set; }
    public string CodigoTipoDocumento { get; set; }
    public bool EsVisibleBotonDocumento { get; set; }
    public EmpresaEjercicioConsultaIntervaloFechaDto FechaIntervalo { get; set; }
	public IEnumerable<PedidoFlag> TiposFinanciamiento { get; set; }
	public IEnumerable<PedidoFlag> ViasTransporte { get; set; }
	public IEnumerable<PedidoFlag> Canales { get; set; }
	public IEnumerable<PedidoFlag> ModalidadesEmbarque { get; set; } 
    public List<OrdenCatalogoAtenderDto> CatalogoOrdenes { get; set; }
	public List<OrdenDetalleCatalogoAtenderDto> CatalogoDetalles { get; set; }
	public IEnumerable<OrdenCatalogoAtenderDto> ItemsSelectedOrden { get; set; }
	public IEnumerable<OrdenDetalleCatalogoAtenderDto> ItemsSelectedDetalle { get; set; }
	public string CodigoEjercicio { get; set; }  
    private IEnumerable<EmpresaEjercicioCatalogoDto> CatalogoEjercicios { get; set; }
    private bool IsAuthUser { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool IsModified { get; set; }
    private bool IsModifiedDetalle { get; set; } 
	private bool IsLoadingCatalogoOrdenes { get; set; }
	private bool IsLoadingCatalogoDetalles { get; set; }
	public string VerboAccionModal { get; set; }
    public string DescripcionAccionModal { get; set; }
    public string TipoAccionModal { get; set; }
    public string TipoAccionDialog { get; set; }
    public string OrigenDialog { get; set; }
    public string OrigenModal { get; set; }
    public bool EsVisibleDialogOrden { get; set; }
    public bool EsVisibleDialogDetalle { get; set; }
    public bool EsVisibleModalDetalle { get; set; }  
    public string CodigoItemAccion { get; set; }
    public ISvgIcon IconoAccionModal { get; set; }
    public TelerikNotification AlertDetalle { get; set; } 
	public TelerikNotification Alert { get; set; }
    public TelerikNotification AlertCatalogoOrdenes { get; set; }
	public TelerikNotification AlertCatalogoDetalles { get; set; }
	private TelerikGrid<PedidoDetalleGrid> GridDetalleRef { get; set; }
	private TelerikGrid<OrdenDetalleCatalogoAtenderDto> GridCatalogoDetalleRef { get; set; } 
	public GridSelectionMode SelectionModeCatalogoDetalle { get; set; }
	private bool EsSeleccionableCatalogoDetalle { get; set; }
    [Parameter][SupplyParameterFromQuery(Name = "returnpage")] public string ReturnPage { get; set; }
    public EmpresaConsultaPorCodigoWebDto Empresa { get; set; }
    private bool EsVisibleVolver { get; set; }
    [Parameter] public string CodigoWebEmpresa { get; set; }
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    #endregion

    [Inject] public IImportacionPedido IPedido { get; set; }
	[Inject] public IImportacionOrden IOrden { get; set; }
    [Inject] public IPrincipalEmpresa IEmpresa { get; set; }
	[Inject] public IPrincipalMoneda IMoneda { get; set; }
	[Inject] public IPrincipalTipoCambioDia ITipoCambioDia { get; set; } 
	[Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public UtilService IUtil { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }
    [Inject] public IMapper IMapper { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
			Notify.ShowLoading(mensaje: "Iniciando formulario");

			PedidoInsertar = new();
            Pedido = new();
            GridDetalles = [];
			ItemGridDetalle = new();
			DetalleInsertar = new();
            Detalle = new(); 
			 
            FechasCerradoOperacion = [];
            FechaIntervalo = new();

            CodigoTipoDocumento = "PE";
            EsVisibleBotonDocumento = true;

            ViasTransporte = PedidoFlag.ViasTransporte();
            ModalidadesEmbarque = PedidoFlag.ModalidadesEmbarque();
            Canales = PedidoFlag.Canales();
			TiposFinanciamiento = PedidoFlag.TiposFinanciamiento();
            EsVisibleBotonCatalogoOrdenes = true;

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio);
            if (!IsAuthUser) return;

            Empresa = await IEmpresa.ConsultaPorCodigoWeb(CodigoWebEmpresa);
            
            EsVisibleVolver = !string.IsNullOrEmpty(ReturnPage) && ReturnPage is "index";
            
			pathBaseUri = $"/{INavigation.ToBaseRelativePath(INavigation.Uri).Split("?")[0]}";
            rutaEmpresa = INavigation.Uri.Replace(INavigation.BaseUri, "").Split("?")[0].Replace(rutaServicio, "").Replace(rutaAccion, "");

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(PedidoAcceso.Emitir, Empresa.Codigo))
            {
                INavigation.NavigateTo($"{rutaEmpresa}/inicio");
                Notify.Show("No tiene permiso para emitir registros de órdenes de importación", "error");
                return;
            } 

            FechaIntervalo = await IEmpresa.ConsultaEjercicioIntervaloFecha(Empresa.Codigo);
            (await IEmpresa.ConsultaModuloPeriodoFechasEsCerradoOperacion(Empresa.Codigo, codigoModulo))?.ToList().ForEach(x => FechasCerradoOperacion.Add(x.Fecha));
            PedidoInsertar.FechaEmision = !FechasCerradoOperacion.Any(x => x == DateTime.Now.Date) ? DateTime.Now.Date : null;
            if (PedidoInsertar.FechaEmision.HasValue)
                Pedido.FechaEmision = (DateTime)PedidoInsertar.FechaEmision;
			
            await ConsultaTipoCambioDia();
            
            ME = await IMoneda.ConsultaPorTipo("ME");
			MN = await IMoneda.ConsultaPorTipo("MN");

            Validator = new();
            EditContext = new EditContext(PedidoInsertar);
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

            Notify.ShowLoading(mensaje: "Emisión en progreso");

            Guid id = await IPedido.Insertar(Empresa.Codigo, PedidoInsertar);

            IsModified = false;
            Notify.Show("El pedido de importación ha sido emitida con éxito en la empresa", "success");
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

    private bool VerificarRegistroEsValido()
    {
        bool esValido  = true;

		if (!PedidoInsertar.MontoTipoCambioDia.HasValue)
		{
            string fechaTipoCambio = PedidoInsertar.FechaCosto.HasValue ? PedidoInsertar.FechaCosto?.ToString("dd/MM/yyyy") : PedidoInsertar.FechaEmision?.ToString("dd/MM/yyyy");
			Fnc.MostrarAlerta(Alert, $"Es necesario el monto del tipo de cambio del día {fechaTipoCambio}", "error");
			esValido = false;
		}

		if (GridDetalles.Count == 0)
        {
            Fnc.MostrarAlerta(Alert, "Es obligatorio que agregue ítem(s) al detalle del pedido de importación", "error");
            esValido = false;
        }
        else if (IsEditingGridDetalle)
        {
            Fnc.MostrarAlerta(Alert, "Es necesario que se termine de editar la celda del detalle", "error");
            esValido = false;
		}
        else 
        {
            if (GridDetalles.Any(x => !x.Cantidad.HasValue))
            {
                Fnc.MostrarAlerta(Alert, "Está pendiente completar alguna celda de [cantidad] del detalle del pedido de importación", "error");
                esValido = false;
            } 
            if (GridDetalles.Any(x => !x.PrecioUnitario.HasValue))
            {
                Fnc.MostrarAlerta(Alert, "Está pendiente completar alguna celda del [precio unitario] del detalle del pedido de importación", "error");
                esValido = false;
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
		DateTime? fechaTipoCambioDia = PedidoInsertar.FechaCosto.HasValue ? PedidoInsertar.FechaCosto : PedidoInsertar.FechaEmision;
        TipoCambioDiaConsultaPorFechaDto tipoCambioDia = fechaTipoCambioDia.HasValue ? await ITipoCambioDia.ConsultaPorFecha((DateTime) fechaTipoCambioDia, "V") : new();
		PedidoInsertar.CodigoTipoCambioDia = tipoCambioDia.Codigo;
		PedidoInsertar.MontoTipoCambioDia = tipoCambioDia.Monto;
	}

    private decimal? ConvertirMontoPorTipoCambioDia(decimal? montoConvertir, int cantidadDecimales = 6)
    { 
        decimal? montoTipoCambioDia = PedidoInsertar.MontoTipoCambioDia;

        if (!montoTipoCambioDia.HasValue || !montoConvertir.HasValue)
            return null;

        decimal? montoConversion = null;
          
        if (Pedido.CodigoMoneda == MN.Codigo) 
			montoConversion = Math.Round((decimal)(montoConvertir / montoTipoCambioDia), cantidadDecimales, MidpointRounding.AwayFromZero);
		else if (Pedido.CodigoMoneda == ME.Codigo)
			montoConversion = Math.Round((decimal)(montoConvertir * montoTipoCambioDia), cantidadDecimales, MidpointRounding.AwayFromZero); 

		return montoConversion;
	}

	private void ActualizarMontosPorTipoCambioDia()
    { 
        foreach ((PedidoDetalleGrid item, int index) in GridDetalles.Select((item, index) => (item, index)))
        { 
			if (Pedido.CodigoMoneda == MN.Codigo) 
				GridDetalles[index].CostoEstimadoME = PedidoInsertar.Detalles[index].CostoEstimadoME = ConvertirMontoPorTipoCambioDia(item.CostoEstimadoMN);  
			else if (Pedido.CodigoMoneda == ME.Codigo) 
				GridDetalles[index].CostoEstimadoMN = PedidoInsertar.Detalles[index].CostoEstimadoMN = ConvertirMontoPorTipoCambioDia(item.CostoEstimadoME);  
		}
    }
      
	private void IniciarAccionModal(string tipoAccion, string origenModal)
    {
        TipoAccionModal = tipoAccion;
        OrigenModal = origenModal;
        switch (tipoAccion)
        {
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

	#region CatalogoOrdenes
	public async Task MostrarCatalogoOrdenes()
	{
        if (string.IsNullOrEmpty(PedidoInsertar.CodigoDocumento))
        {
            Fnc.MostrarAlerta(Alert, "Es necesario que seleccione el numerador de [Documento]", "error");
            return;
        }

        IsLoadingCatalogoOrdenes = true;

		CatalogoEjercicios = await IEmpresa.CatalogoEjercicios(Empresa.Codigo) ?? [];
		CodigoEjercicio = await IEmpresa.ConsultaEjercicioCodigoPorAnio(Empresa.Codigo, DateTime.Now.Year);
		ItemsSelectedOrden = [];

		await CargarCatalogoOrdenes();
		EsVisibleCatalogoOrdenes = true;
	}

    private void QuitarItemOrden()
    {
		OrdenCatalogoAtenderDto itemNuevo = new();
        IMapper.Map(itemNuevo, Pedido);
        IMapper.Map(itemNuevo, PedidoInsertar);

        IsEditAmountMN = IsEditAmountME = false;

        PedidoInsertar.CodigoOrdenReferencia = null;
        EsVisibleBotonCatalogoOrdenes = EsVisibleBotonDocumento = true;
        EsVisibleDialogOrden = false;
    }

    private async Task VisibleCatalogoOrdenesChangedHandler(bool esVisible)
	{
		if (!esVisible)
		{
            if (ItemsSelectedOrden.Any() && !await Dialog.ConfirmAsync("¿Está seguro de salir del catálogo y que el ítem seleccionado no se cargue?", "Saliendo de catálogo"))
                return;

			EsVisibleCatalogoOrdenes = false;
			CatalogoOrdenes = null;
		}
	}

	private void OnRowDoubleClickCatalogoOrdenHandler(GridRowClickEventArgs args) => AgregarItemOrden(args.Item as OrdenCatalogoAtenderDto);

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

			CatalogoOrdenes = (List<OrdenCatalogoAtenderDto>) await IOrden.CatalogoAtender(Empresa.Codigo, CodigoEjercicio); 
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

	private void SeleccionarItemOrden()
    {
        if (!ItemsSelectedOrden.Any())
        {
            Fnc.MostrarAlerta(AlertCatalogoOrdenes, "Es necesario que seleccione al menos un ítem del catálogo", "warning");
            return;
        }

        AgregarItemOrden(ItemsSelectedOrden.First());
	}

    private void AgregarItemOrden(OrdenCatalogoAtenderDto item)
    {
		IMapper.Map(item, Pedido);
		IMapper.Map(item, PedidoInsertar);

        IsEditAmountMN = item.CodigoMoneda == MN.Codigo;
		IsEditAmountME = item.CodigoMoneda == ME.Codigo;

        EditContext.NotifyFieldChanged(EditContext.Field("CodigoOrden"));
        IsModified = true;

        EsVisibleBotonCatalogoArticulos = true; 
        EsVisibleCatalogoOrdenes = EsVisibleBotonCatalogoOrdenes = EsVisibleBotonDocumento = false;
    }

	#endregion

	#region CatalogoDetalles
	public async Task MostrarCatalogoDetalles()
	{
        if (string.IsNullOrEmpty(PedidoInsertar.CodigoOrdenReferencia))
        {
            Fnc.MostrarAlerta(Alert, "Es necesario que seleccione el código de [Orden de Referencia]", "error");
            return;
        } 
        IsLoadingCatalogoDetalles = true;  
		ItemsSelectedDetalle = []; 
		await CargarCatalogoDetalles(); 
		EsVisibleCatalogoDetalles = true;
	}

	private async Task VisibleCatalogoDetallesChangedHandler(bool esVisible)
	{
		if (!esVisible)
		{
			if (ItemsSelectedDetalle.Any() && !CatalogoDetalles.Any(x => x.IsErrorSelected) && !await Dialog.ConfirmAsync("¿Está seguro de salir del catálogo y que los ítems seleccionados no se carguen?", "Saliendo de catálogo"))
				return;

			EsVisibleCatalogoDetalles = false;
			CatalogoDetalles = null;
		}
	}

	private async Task CargarCatalogoDetalles()
	{
		try
		{
            IsAuthUser = (await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio, User.FindFirst("code").Value)).esValido;
            if (!IsAuthUser) return;
            ItemsSelectedDetalle = [];

			CatalogoDetalles = (List<OrdenDetalleCatalogoAtenderDto>) await IOrden.CatalogoDetallesAtender(Empresa.Codigo, Pedido.CodigoOrdenReferencia);

			SelectionModeCatalogoDetalle = CatalogoDetalles is null ? GridSelectionMode.None : GridSelectionMode.Multiple;
			EsSeleccionableCatalogoDetalle = CatalogoDetalles is not null;
			GridCatalogoDetalleRef?.Rebind();
		}
		catch (HttpRequestException)
		{
			if (EsVisibleCatalogoDetalles)
                Fnc.MostrarAlerta(AlertCatalogoDetalles, Cnf.MsgErrorNotConnectApi, "error");
			else
				Notify.ShowError("NC");
		}
		catch (HttpResponseException ex)
		{
			if (EsVisibleCatalogoDetalles)
                Fnc.MostrarAlerta(AlertCatalogoDetalles, $"{(ex.Code == "NF" ? Cnf.MsgErrorNotFoundAPi : ex.Message)}", "error");
			else
				Notify.ShowError(ex.Code, ex);
		}
		catch (Exception ex)
		{
			if (EsVisibleCatalogoDetalles)
                Fnc.MostrarAlerta(AlertCatalogoDetalles, Cnf.MsgErrorFuncAppWeb, "error");
			else
				Notify.ShowError("FA", ex);
		}
		finally
		{
			IsLoadingCatalogoDetalles = false;
			Notify.ShowLoading(false);
		}
	}
     
	public static void OnRowCatalogoDetalleRenderHandler(GridRowRenderEventArgs args) => args.Class = (args.Item as OrdenDetalleCatalogoAtenderDto).IsErrorSelected ? "row-error" : "";

	protected void OnSelectCatalogoDetalle(IEnumerable<OrdenDetalleCatalogoAtenderDto> itemsSeleccionado)
	{
		ItemsSelectedDetalle = itemsSeleccionado;

		if (CatalogoDetalles is not null)
		{
			if (ItemsSelectedDetalle.Any())
			{
				foreach (OrdenDetalleCatalogoAtenderDto item in CatalogoDetalles.Where(x => x.IsErrorSelected))
					item.IsErrorSelected = ItemsSelectedDetalle.Any(i => i.CodigoArticulo == item.CodigoArticulo); 
			}
			else
			{
				CatalogoDetalles.ForEach(x => { x.IsErrorSelected = false; });
			}
		}
	}

	private async Task AgregarItemsDetalle()
	{
		if (!ItemsSelectedDetalle.Any())
		{
			Fnc.MostrarAlerta(AlertCatalogoDetalles, "Es necesario que seleccione al menos un ítem del catálogo", "warning");
			return;
		}

		CatalogoDetalles.ForEach(x => { x.IsErrorSelected = false; });
		 
		if (GridDetalles.Count > 0)
		{
			bool esRepetidoItemsDetalle = false;
			foreach (OrdenDetalleCatalogoAtenderDto item in ItemsSelectedDetalle)
			{
				if (GridDetalles.Any(x => x.CodigoArticulo == item.CodigoArticulo))
				{
					esRepetidoItemsDetalle = true;
					Fnc.MostrarAlerta(AlertCatalogoDetalles, $"El artículo {item.CodigoArticulo} ya ha sido agregado en el detalle", "warning");
					CatalogoDetalles[CatalogoDetalles.FindIndex(i => i.CodigoArticulo == item.CodigoArticulo)].IsErrorSelected = true;
				}
			}
			if (esRepetidoItemsDetalle)
				return;
		}
         
		GridState<PedidoDetalleGrid> detalleState = GridDetalleRef.GetState();

		foreach (OrdenDetalleCatalogoAtenderDto item in ItemsSelectedDetalle)
		{
            DetalleInsertar = new();
            Detalle = new();

			IMapper.Map(item, Detalle);
			IMapper.Map(Detalle, DetalleInsertar);

			ModificarImportesItem(DetalleInsertar.Cantidad, DetalleInsertar.PrecioUnitario); 
			IMapper.Map(DetalleInsertar, Detalle); 

			ItemGridDetalle = IMapper.Map<PedidoDetalleGrid>(Detalle);
			 
			PedidoInsertar.Detalles.Add(DetalleInsertar);
			GridDetalles.Add(ItemGridDetalle);
			detalleState.InsertedItem = ItemGridDetalle;
            
			await GridDetalleRef.SetStateAsync(detalleState);
		}

		ModificarImportesTotales();
		EsVisibleBotonCatalogoOrdenes = GridDetalles.Count == 0;
		EsVisibleCatalogoDetalles = false;
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

	public void MostrarModificarDetalle(PedidoDetalleGrid item = null)
    {
		if (TipoAccionModal is not "V")
			Detalle = IMapper.Map<PedidoDetalleObtenerDto>(item);

		DetalleInsertar = IMapper.Map<PedidoDetalleInsertarDto>(Detalle);
		CodigoItemAccion = Detalle.CodigoArticulo; 
        DetalleValidator = new()
        {
            FlagTipoMoneda = Pedido.CodigoMoneda == MN.Codigo ? "MN" : "ME",
            UnidadConversion = Detalle.UnidadConversion
        }; 
		IniciarAccionModal("M", "detalle");
	}

    private void MostrarQuitarDetalle(string codigoItem)
    {
        CodigoItemAccion = codigoItem;
        IniciarAccionDialog("Q", "detalle");
    }

	public void VerItemDetalle(PedidoDetalleGrid item)
	{
		Detalle = IMapper.Map<PedidoDetalleObtenerDto>(item);
		CodigoItemAccion = item.CodigoArticulo;
		IniciarAccionModal("V", "detalle");
	}
	 
    private void OnChangeDetalleObservacionHandler(object value) => IsModifiedDetalle = Fnc.VerifyContextIsChanged((Detalle.Observacion ?? "") != ((string)value ?? ""), DetalleContext, "Observacion");

    private void OnChangeDetalleCantidadHandler(object value)
    {
        decimal? cantidad = (decimal?)value;
        IsModifiedDetalle = Fnc.VerifyContextIsChanged(Detalle.Cantidad != cantidad, DetalleContext, "Cantidad");
        ModificarImportesItem(cantidad, DetalleInsertar.PrecioUnitario);
    }

    private void OnChangeDetallePrecioUnitarioHandler(object value)
    {
        decimal? precioUnitario = (decimal?)value;
        IsModifiedDetalle = Fnc.VerifyContextIsChanged(Detalle.PrecioUnitario != precioUnitario, DetalleContext, "PrecioUnitario");
        ModificarImportesItem(DetalleInsertar.Cantidad, precioUnitario);
    }

    private void OnChangeDetalleCostoEstimadoMEHandler(object value)
    {
        decimal? costoEstimadoME = (decimal?)value;
        IsModifiedDetalle = Fnc.VerifyContextIsChanged(Detalle.CostoEstimadoME != costoEstimadoME, DetalleContext, "CostoEstimadoME");
        DetalleInsertar.CostoEstimadoMN = ConvertirMontoPorTipoCambioDia(costoEstimadoME);
    }

    private void OnChangeDetalleCostoEstimadoMNHandler(object value)
    {
        decimal? costoEstimadoMN = (decimal?)value;
        IsModifiedDetalle = Fnc.VerifyContextIsChanged(Detalle.CostoEstimadoMN != costoEstimadoMN, DetalleContext, "CostoEstimadoMN");
        DetalleInsertar.CostoEstimadoME = ConvertirMontoPorTipoCambioDia(costoEstimadoMN);
    } 

	public static void OnCellCantidadRenderHandler(GridCellRenderEventArgs args) => args.Class = !(args.Item as PedidoDetalleGrid).Cantidad.HasValue ? "cell-error" : "cell-editable";

	public static void OnCellPrecioUnitarioRenderHandler(GridCellRenderEventArgs args) => args.Class = !(args.Item as PedidoDetalleGrid).PrecioUnitario.HasValue ? "cell-error" : "cell-editable";

	public void OnCellCostoEstimadoMNRenderHandler(GridCellRenderEventArgs args) => args.Class = IsEditAmountMN ? "cell-editable" : "";

	public void OnCellCostoEstimadoMERenderHandler(GridCellRenderEventArgs args) => args.Class = IsEditAmountME ? "cell-editable" : "";

    public void EditDetalleHandler(GridCommandEventArgs args)
    {
        IsEditingGridDetalle = args.Field is "Cantidad" or "PrecioUnitario" or "CostoEstimadoME" or "CostoEstimadoMN";
        if (IsEditingGridDetalle)
        {
            GridDetalleValidator = new()
            {
                UnidadConversion = (args.Item as PedidoDetalleGrid).UnidadConversion
            };
        }
    }

    public void CancelDetalleHandler(GridCommandEventArgs args) => IsEditingGridDetalle = !(args.Field is "Cantidad" or "PrecioUnitario" or "CostoEstimadoME" or "CostoEstimadoMN"); 

	public void UpdateItemDetalleHandler(GridCommandEventArgs args)
    {
		PedidoDetalleGrid item = (PedidoDetalleGrid) args.Item; 
        int index = GridDetalles.FindIndex(i => i.CodigoArticulo == item.CodigoArticulo);
		bool esActualizableImportes = false;

		if (args.Field == "PrecioUnitario")
        {
            PedidoInsertar.Detalles[index].PrecioUnitario = GridDetalles[index].PrecioUnitario = item.PrecioUnitario;
			esActualizableImportes = true;
		}
        else if (args.Field == "Cantidad")
        {
            PedidoInsertar.Detalles[index].Cantidad = GridDetalles[index].Cantidad = item.Cantidad;
			esActualizableImportes = true;
		}
		else if (args.Field == "CostoEstimadoMN")
		{
			PedidoInsertar.Detalles[index].CostoEstimadoMN = GridDetalles[index].CostoEstimadoMN = item.CostoEstimadoMN;
			PedidoInsertar.Detalles[index].CostoEstimadoME = GridDetalles[index].CostoEstimadoME = ConvertirMontoPorTipoCambioDia(item.CostoEstimadoMN); 
		}
		else if (args.Field == "CostoEstimadoME")
		{
			PedidoInsertar.Detalles[index].CostoEstimadoME = GridDetalles[index].CostoEstimadoME = item.CostoEstimadoME;
			PedidoInsertar.Detalles[index].CostoEstimadoMN = GridDetalles[index].CostoEstimadoMN = ConvertirMontoPorTipoCambioDia(item.CostoEstimadoME);
		}
		if (esActualizableImportes)
		{
			ModificarImportesItem(item.Cantidad, item.PrecioUnitario, index);
			ModificarImportesTotales();
		} 
		IsEditingGridDetalle = false;
	}

    private void ModificarImportesItem(decimal? cantidad, decimal? precioUnitario, int indexGrid = -1)
    {
        decimal? importeBruto = null;
        if (cantidad.HasValue && precioUnitario.HasValue)
            importeBruto = Math.Round((decimal)(cantidad * precioUnitario), 2, MidpointRounding.AwayFromZero);
		if (indexGrid > -1)
			PedidoInsertar.Detalles[indexGrid].ImporteBruto = GridDetalles[indexGrid].ImporteBruto = importeBruto; 
		else
			DetalleInsertar.ImporteBruto = importeBruto;
    }

    private void ModificarImportesTotales()
    {
        PedidoInsertar.TotalImporteBruto = GridDetalles.Count == 0 ? 0 : GridDetalles.Sum(x => x.ImporteBruto);
        PedidoInsertar.TotalImporteNeto = PedidoInsertar.TotalImporteBruto;
	} 

    private async Task AccionarDetalle(string tipoAccion)
    {
		GridState<PedidoDetalleGrid> detalleState = GridDetalleRef.GetState();
        int index = GridDetalles.FindIndex(i => i.CodigoArticulo == CodigoItemAccion);

        switch (tipoAccion)
        {
            case "M":
                PedidoInsertar.Detalles[index] = DetalleInsertar;
                GridDetalles[index] = IMapper.Map<PedidoDetalleGrid>(IMapper.Map(DetalleInsertar, Detalle));
                break;
            case "Q":
                PedidoInsertar.Detalles.RemoveAt(index);
                GridDetalles.RemoveAt(index);
                CerrarDialog();
                break;
        };
		CerrarModal();
        ModificarImportesTotales(); 
        await GridDetalleRef.SetStateAsync(detalleState);
    }
    #endregion

    #region Catalogos
    private void CargarItemCatalogoSerieDocumento(SerieDocumentoCatalogoPorEmpresaSesionDto item)
    {
        PedidoInsertar.CodigoDocumento = item.CodigoDocumento;
        PedidoInsertar.CodigoSerieDocumento = item.CodigoSerieDocumento;
        Pedido.NombreSerieDocumento = item.NombreSerieDocumento;
		  
        CodigoProcesoDocumento = item.CodigoProcesoDocumento;

        EditContext.NotifyFieldChanged(EditContext.Field("CodigoDocumento"));
        IsModified = EditContext.IsModified();
    }

    private void CargarItemCatalogoSituacionImportacion(SituacionImportacionCatalogoDto item)
	{
		PedidoInsertar.CodigoSituacionImportacion = Pedido.CodigoSituacionImportacion = item.CodigoSituacionImportacion;
		Pedido.NombreSituacionImportacion = item.NombreSituacionImportacion;
		Validator.MsgErrorSituacion = null;
	    EditContext.NotifyFieldChanged(EditContext.Field("CodigoSituacionImportacion"));
        IsModified = EditContext.IsModified();
	}

	private void CargarItemCatalogoRegimenImportacion(RegimenImportacionCatalogoDto item)
	{
		PedidoInsertar.CodigoRegimenImportacion = Pedido.CodigoRegimenImportacion = item.CodigoRegimenImportacion;
		Pedido.NombreRegimenImportacion = item.NombreRegimenImportacion;
		Validator.MsgErrorRegimen = null;
		EditContext.NotifyFieldChanged(EditContext.Field("CodigoRegimenImportacion"));
        IsModified = EditContext.IsModified();
	}

	private void CargarItemCatalogoEntidadAlmacen(EntidadProveedorCatalogoPorEmpresaDto item)
	{
		PedidoInsertar.CodigoEntidadAlmacen = Pedido.CodigoEntidadAlmacen = item.CodigoEntidad.Trim();
		Pedido.NombreEntidadAlmacen = item.NombreEntidad;
		Validator.MsgErrorEntidadAlmacen = null;
		EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidadAlmacen"));
        IsModified = EditContext.IsModified();
	}

	private void CargarItemCatalogoEntidadAgenciaCarga(EntidadProveedorCatalogoPorEmpresaDto item)
	{
		PedidoInsertar.CodigoEntidadAgenciaCarga = Pedido.CodigoEntidadAgenciaCarga = item.CodigoEntidad.Trim();
		Pedido.NombreEntidadAgenciaCarga = item.NombreEntidad;
		Validator.MsgErrorEntidadAgenciaCarga = null;
		EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidadAgenciaCarga"));
        IsModified = EditContext.IsModified();
	}

	private void CargarItemCatalogoEntidadAgenteAduana(EntidadProveedorCatalogoPorEmpresaDto item)
	{
		PedidoInsertar.CodigoEntidadAgenteAduana = Pedido.CodigoEntidadAgenteAduana = item.CodigoEntidad.Trim();
		Pedido.NombreEntidadAgenteAduana = item.NombreEntidad;
		Validator.MsgErrorEntidadAgenteAduana = null;
		EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidadAgenteAduana"));
        IsModified = EditContext.IsModified();
	}

	private void CargarItemCatalogoEntidadMuelle(EntidadProveedorCatalogoPorEmpresaDto item)
	{
		PedidoInsertar.CodigoEntidadMuelle = Pedido.CodigoEntidadMuelle = item.CodigoEntidad.Trim();
		Pedido.NombreEntidadMuelle = item.NombreEntidad;
		Validator.MsgErrorEntidadMuelle = null;
		EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidadMuelle"));
        IsModified = EditContext.IsModified();
	}

	private void CargarItemCatalogoEntidadTerminal(EntidadProveedorCatalogoPorEmpresaDto item)
	{
		PedidoInsertar.CodigoEntidadTerminal = Pedido.CodigoEntidadTerminal = item.CodigoEntidad.Trim();
		Pedido.NombreEntidadTerminal = item.NombreEntidad;
		Validator.MsgErrorEntidadTerminal = null;
		EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidadTerminal"));
        IsModified = EditContext.IsModified();
	}

	private void CargarItemCatalogoEntidadNaviera(EntidadProveedorCatalogoPorEmpresaDto item)
	{
		PedidoInsertar.CodigoEntidadNaviera = Pedido.CodigoEntidadNaviera = item.CodigoEntidad.Trim();
		Pedido.NombreEntidadNaviera = item.NombreEntidad;
		Validator.MsgErrorEntidadNaviera = null;
		EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidadNaviera"));
        IsModified = EditContext.IsModified();
	}

	private void CargarItemCatalogoPuertoEmbarque(PuertoCatalogoDto item)
	{
		PedidoInsertar.CodigoPuertoEmbarque = Pedido.CodigoPuertoEmbarque = item.CodigoPuerto;
		Pedido.NombrePuertoEmbarque = item.NombrePuerto;
        Pedido.CodigoPaisEmbarque = item.CodigoPais;
        Pedido.NombrePaisEmbarque = item.NombrePais;
		Validator.MsgErrorPuertoEmbarque = null;
		EditContext.NotifyFieldChanged(EditContext.Field("CodigoPuertoEmbarque"));
        IsModified = EditContext.IsModified();
	}

    private void CargarItemCatalogoPuertoLlegada(PuertoCatalogoDto item)
    {
        PedidoInsertar.CodigoPuertoLlegada = Pedido.CodigoPuertoLlegada = item.CodigoPuerto;
        Pedido.NombrePuertoLlegada = item.NombrePuerto;
        Pedido.CodigoPaisLlegada = item.CodigoPais;
        Pedido.NombrePaisLlegada = item.NombrePais;
		Validator.MsgErrorPuertoLlegada = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoPuertoLlegada"));
        IsModified = EditContext.IsModified();
    }

    private void CargarItemCatalogoTransporteImportacion(TransporteImportacionCatalogoDto item)
    {
        PedidoInsertar.CodigoTransporteImportacion = Pedido.CodigoTransporteImportacion = item.CodigoTransporteImportacion;
        Pedido.NombreTransporteImportacion = item.NombreTransporteImportacion;
		Validator.MsgErrorTransporte = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoTransporteImportacion"));
        IsModified = EditContext.IsModified();
    }
    #endregion

    #region EditContextHandlers
    private void ValueChangeEntidadAgenteAduanaHandler(string value)
    {
        PedidoInsertar.CodigoEntidadAgenteAduana = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorEntidadAgenteAduana) && (Pedido.CodigoEntidadAgenteAduana ?? "").Trim() != (PedidoInsertar.CodigoEntidadAgenteAduana ?? ""))
            Pedido.CodigoEntidadAgenteAduana = Validator.MsgErrorEntidadAgenteAduana = null;
    }

    private async Task OnChangeEntidadAgenteAduanaHandler(object value)
	{
		string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoEntidadAgenteAduana")))
		{ 
			if (!string.IsNullOrEmpty(codigo))
			{
				if ((Pedido.CodigoEntidadAgenteAduana ?? "").Trim() == codigo) goto exit;
				(EntidadProveedorObtenerPorCodigoEmpresaDto item, Validator.MsgErrorEntidadAgenteAduana) = await IUtil.ObtenerProveedorPorCodigoEmpresa(Alert, codigo, Empresa.Codigo, "A");
				if (item is not null)
				{
					Pedido.CodigoEntidadAgenteAduana = item.CodigoEntidad.Trim();
					Pedido.NombreEntidadAgenteAduana = item.NombreEntidad;
					goto exit;
				} 
				EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidadAgenteAduana"));
			}
			else
			{
				PedidoInsertar.CodigoEntidadAgenteAduana = null;
				EditContext.MarkAsUnmodified(EditContext.Field("CodigoEntidadAgenteAduana"));
			}
		}
		Pedido.CodigoEntidadAgenteAduana = codigo;
		Pedido.NombreEntidadAgenteAduana = null;
	exit:
		IsModified = EditContext.IsModified();
	}

    private void ValueChangeEntidadMuelleHandler(string value)
    {
        PedidoInsertar.CodigoEntidadMuelle = value?.Trim().ToUpper();
		if (!string.IsNullOrEmpty(Validator.MsgErrorEntidadMuelle) && (Pedido.CodigoEntidadMuelle ?? "").Trim() != (PedidoInsertar.CodigoEntidadMuelle ?? ""))
            Pedido.CodigoEntidadMuelle = Validator.MsgErrorEntidadMuelle = null;
    }

    private async Task OnChangeEntidadMuelleHandler(object value)
	{
		string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoEntidadMuelle")))
		{ 
			if (!string.IsNullOrEmpty(codigo))
			{
				if ((Pedido.CodigoEntidadMuelle ?? "").Trim() == codigo) goto exit;
				(EntidadProveedorObtenerPorCodigoEmpresaDto item, Validator.MsgErrorEntidadMuelle) = await IUtil.ObtenerProveedorPorCodigoEmpresa(Alert, codigo, Empresa.Codigo, "A");
				if (item is not null)
				{
					Pedido.CodigoEntidadMuelle = item.CodigoEntidad.Trim();
					Pedido.NombreEntidadMuelle = item.NombreEntidad;
					goto exit;
				} 
				EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidadMuelle"));
			}
			else
			{
				PedidoInsertar.CodigoEntidadMuelle = null;
				EditContext.MarkAsUnmodified(EditContext.Field("CodigoEntidadMuelle"));
			}
		}
		Pedido.CodigoEntidadMuelle = codigo;
		Pedido.NombreEntidadMuelle = null;
	exit:
		IsModified = EditContext.IsModified();
	}

    private void ValueChangeEntidadAgenciaCargaHandler(string value)
    {
        PedidoInsertar.CodigoEntidadAgenciaCarga = value?.Trim().ToUpper();
		if (!string.IsNullOrEmpty(Validator.MsgErrorEntidadAgenciaCarga) && (Pedido.CodigoEntidadAgenciaCarga ?? "").Trim() != (PedidoInsertar.CodigoEntidadAgenciaCarga ?? ""))
            Pedido.CodigoEntidadAgenciaCarga = Validator.MsgErrorEntidadAgenciaCarga = null;
    }

    private async Task OnChangeEntidadAgenciaCargaHandler(object value)
	{
		string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoEntidadAgenciaCarga")))
		{ 
			if (!string.IsNullOrEmpty(codigo))
			{
				if ((Pedido.CodigoEntidadAgenciaCarga ?? "").Trim() == codigo) goto exit;
				(EntidadProveedorObtenerPorCodigoEmpresaDto item, Validator.MsgErrorEntidadAgenciaCarga) = await IUtil.ObtenerProveedorPorCodigoEmpresa(Alert, codigo, Empresa.Codigo, "A");
				if (item is not null)
				{
					Pedido.CodigoEntidadAgenciaCarga = item.CodigoEntidad.Trim();
					Pedido.NombreEntidadAgenciaCarga = item.NombreEntidad;
					goto exit;
				} 
				EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidadAgenciaCarga"));
			}
			else
			{
				PedidoInsertar.CodigoEntidadAgenciaCarga = null;
				EditContext.MarkAsUnmodified(EditContext.Field("CodigoEntidadAgenciaCarga"));
			}
		}
		Pedido.CodigoEntidadAgenciaCarga = codigo;
		Pedido.NombreEntidadAgenciaCarga = null;
	exit:
		IsModified = EditContext.IsModified();
	}

    private void ValueChangeEntidadTerminalHandler(string value)
    {
        PedidoInsertar.CodigoEntidadTerminal = value?.Trim().ToUpper();
		if (!string.IsNullOrEmpty(Validator.MsgErrorEntidadTerminal) && (Pedido.CodigoEntidadTerminal ?? "").Trim() != (PedidoInsertar.CodigoEntidadTerminal ?? ""))
            Pedido.CodigoEntidadTerminal = Validator.MsgErrorEntidadTerminal = null;
    }

    private async Task OnChangeEntidadTerminalHandler(object value)
	{
		string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoEntidadTerminal")))
		{ 
			if (!string.IsNullOrEmpty(codigo))
			{
				if ((Pedido.CodigoEntidadTerminal ?? "").Trim() == codigo) goto exit;
				(EntidadProveedorObtenerPorCodigoEmpresaDto item, Validator.MsgErrorEntidadTerminal) = await IUtil.ObtenerProveedorPorCodigoEmpresa(Alert, codigo, Empresa.Codigo, "A");
				if (item is not null)
				{
					Pedido.CodigoEntidadTerminal = item.CodigoEntidad.Trim();
					Pedido.NombreEntidadTerminal = item.NombreEntidad;
					goto exit;
				} 
				EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidadTerminal"));
			}
			else
			{
				PedidoInsertar.CodigoEntidadTerminal = null;
				EditContext.MarkAsUnmodified(EditContext.Field("CodigoEntidadTerminal"));
			}
		}
		Pedido.CodigoEntidadTerminal = codigo;
		Pedido.NombreEntidadTerminal = null;
	exit:
		IsModified = EditContext.IsModified();
	}

    private void ValueChangeEntidadAlmacenHandler(string value)
    {
        PedidoInsertar.CodigoEntidadAlmacen = value?.Trim().ToUpper();
		if (!string.IsNullOrEmpty(Validator.MsgErrorEntidadAlmacen) && (Pedido.CodigoEntidadAlmacen ?? "").Trim() != (PedidoInsertar.CodigoEntidadAlmacen ?? ""))
            Pedido.CodigoEntidadAlmacen = Validator.MsgErrorEntidadAlmacen = null;
    }

    private async Task OnChangeEntidadAlmacenHandler(object value)
	{
		string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoEntidadAlmacen")))
		{ 
			if (!string.IsNullOrEmpty(codigo))
			{
				if ((Pedido.CodigoEntidadAlmacen ?? "").Trim() == codigo) goto exit;
				(EntidadProveedorObtenerPorCodigoEmpresaDto item, Validator.MsgErrorEntidadAlmacen) = await IUtil.ObtenerProveedorPorCodigoEmpresa(Alert, codigo, Empresa.Codigo, "A");
				if (item is not null)
				{
					Pedido.CodigoEntidadAlmacen = item.CodigoEntidad.Trim();
					Pedido.NombreEntidadAlmacen = item.NombreEntidad;
					goto exit;
				} 
				EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidadAlmacen"));
			}
			else
			{
				PedidoInsertar.CodigoEntidadAlmacen = null;
				EditContext.MarkAsUnmodified(EditContext.Field("CodigoEntidadAlmacen"));
			}
		}
		Pedido.CodigoEntidadAlmacen = codigo;
		Pedido.NombreEntidadAlmacen = null;
	exit:
		IsModified = EditContext.IsModified();
	}

    private void ValueChangeEntidadNavieraHandler(string value)
    {
        PedidoInsertar.CodigoEntidadNaviera = value?.Trim().ToUpper();
		if (!string.IsNullOrEmpty(Validator.MsgErrorEntidadNaviera) && (Pedido.CodigoEntidadNaviera ?? "").Trim() != (PedidoInsertar.CodigoEntidadNaviera ?? ""))
            Pedido.CodigoEntidadNaviera = Validator.MsgErrorEntidadNaviera = null;
    }

    private async Task OnChangeEntidadNavieraHandler(object value)
	{
		string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoEntidadNaviera")))
		{ 
			if (!string.IsNullOrEmpty(codigo))
			{
				if ((Pedido.CodigoEntidadNaviera ?? "").Trim() == codigo) goto exit;
				(EntidadProveedorObtenerPorCodigoEmpresaDto item, Validator.MsgErrorEntidadNaviera) = await IUtil.ObtenerProveedorPorCodigoEmpresa(Alert, codigo, Empresa.Codigo, "A");
				if (item is not null)
				{
					Pedido.CodigoEntidadNaviera = item.CodigoEntidad.Trim();
					Pedido.NombreEntidadNaviera = item.NombreEntidad;
					goto exit;
				} 
				EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidadNaviera"));
			}
			else
			{
				PedidoInsertar.CodigoEntidadNaviera = null;
				EditContext.MarkAsUnmodified(EditContext.Field("CodigoEntidadNaviera"));
			}
		}
		Pedido.CodigoEntidadNaviera = codigo;
		Pedido.NombreEntidadNaviera = null;
	exit:
		IsModified = EditContext.IsModified();
	}

    private void ValueChangeSituacionImportacionHandler(string value)
    {
        PedidoInsertar.CodigoSituacionImportacion = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorSituacion) && (Pedido.CodigoSituacionImportacion ?? "").Trim() != (PedidoInsertar.CodigoSituacionImportacion ?? ""))
            Pedido.CodigoSituacionImportacion = Validator.MsgErrorSituacion = null;
    }

    private async Task OnChangeSituacionImportacionHandler(object value)
	{
		string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoSituacionImportacion")))
		{ 
			if (!string.IsNullOrEmpty(codigo))
			{
				if ((Pedido.CodigoSituacionImportacion ?? "") == codigo) goto exit;
                (SituacionImportacionObtenerPorCodigoDto item, Validator.MsgErrorSituacion) = await IUtil.ObtenerSituacionImportacionPorCodigo(Alert, codigo);
				if (item is not null)
                {
                    Pedido.CodigoSituacionImportacion = item.CodigoSituacionImportacion;
                    Pedido.NombreSituacionImportacion = item.NombreSituacionImportacion;
                    goto exit;
                } 
                EditContext.NotifyFieldChanged(EditContext.Field("CodigoSituacionImportacion"));
            }
			else
			{
				PedidoInsertar.CodigoSituacionImportacion = null;
				EditContext.MarkAsUnmodified(EditContext.Field("CodigoSituacionImportacion"));
			}
		}
		Pedido.CodigoSituacionImportacion = codigo;
		Pedido.NombreSituacionImportacion = null;
	exit:
		IsModified = EditContext.IsModified();
	}

    private void ValueChangeTransporteImportacionHandler(string value)
    {
        PedidoInsertar.CodigoTransporteImportacion = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorTransporte) && (Pedido.CodigoTransporteImportacion ?? "").Trim() != (PedidoInsertar.CodigoTransporteImportacion ?? ""))
            Pedido.CodigoTransporteImportacion = Validator.MsgErrorTransporte = null;
    }

    private async Task OnChangeTransporteImportacionHandler(object value)
	{
		string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoTransporteImportacion")))
		{ 
			if (!string.IsNullOrEmpty(codigo))
			{
				if ((Pedido.CodigoTransporteImportacion ?? "") == codigo) goto exit;
                (TransporteImportacionObtenerPorCodigoDto item, Validator.MsgErrorTransporte) = await IUtil.ObtenerTransporteImportacionPorCodigo(Alert, codigo);
                if (item is not null)
                {
                    Pedido.CodigoTransporteImportacion = item.CodigoTransporteImportacion;
                    Pedido.NombreTransporteImportacion = item.NombreTransporteImportacion;
                    goto exit;
                } 
                EditContext.NotifyFieldChanged(EditContext.Field("CodigoTransporteImportacion"));
            }
			else
			{
				PedidoInsertar.CodigoTransporteImportacion = null;
				EditContext.MarkAsUnmodified(EditContext.Field("CodigoTransporteImportacion"));
			}
		}
		Pedido.CodigoTransporteImportacion = codigo;
		Pedido.NombreTransporteImportacion = null;
	exit:
		IsModified = EditContext.IsModified();
	}

    private void ValueChangeRegimenImportacionHandler(string value)
    {
        PedidoInsertar.CodigoRegimenImportacion = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorRegimen) && (Pedido.CodigoRegimenImportacion ?? "").Trim() != (PedidoInsertar.CodigoRegimenImportacion ?? ""))
            Pedido.CodigoRegimenImportacion = Validator.MsgErrorRegimen = null;
    }

    private async Task OnChangeRegimenImportacionHandler(object value)
	{
		string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoRegimenImportacion")))
		{ 
			if (!string.IsNullOrEmpty(codigo))
			{
				if ((Pedido.CodigoRegimenImportacion ?? "") != codigo) goto exit; 
				(RegimenImportacionObtenerPorCodigoDto item, Validator.MsgErrorRegimen) = await IUtil.ObtenerRegimenImportacionPorCodigo(Alert, codigo);
				if (item is not null)
				{
                    Pedido.CodigoRegimenImportacion = item.CodigoRegimenImportacion;
                    Pedido.NombreRegimenImportacion = item.NombreRegimenImportacion;
                    goto exit;
                } 
                EditContext.NotifyFieldChanged(EditContext.Field("CodigoRegimenImportacion"));
            }
			else
			{
				PedidoInsertar.CodigoRegimenImportacion = null;
				EditContext.MarkAsUnmodified(EditContext.Field("CodigoRegimenImportacion"));
			}
		}
		Pedido.CodigoRegimenImportacion = codigo;
		Pedido.NombreRegimenImportacion = null;
	exit:
		IsModified = EditContext.IsModified();
	}

    private void ValueChangePuertoEmbarqueHandler(string value)
    {
        PedidoInsertar.CodigoPuertoEmbarque = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorPuertoEmbarque) && (Pedido.CodigoPuertoEmbarque ?? "").Trim() != (PedidoInsertar.CodigoPuertoEmbarque ?? ""))
            Pedido.CodigoPuertoEmbarque = Validator.MsgErrorPuertoEmbarque = null;
    }

    private async Task OnChangePuertoEmbarqueHandler(object value)
	{
		string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoPuertoEmbarque")))
		{ 
			if (!string.IsNullOrEmpty(codigo))
			{
				if ((Pedido.CodigoPuertoEmbarque ?? "") == codigo) goto exit;
				(PuertoObtenerPorCodigoDto item, Validator.MsgErrorPuertoEmbarque) = await IUtil.ObtenerPuertoPorCodigo(Alert, codigo);
				if (item is not null)
				{
                    Pedido.CodigoPuertoEmbarque = item.CodigoPuerto;
                    Pedido.NombrePuertoEmbarque = item.NombrePuerto;
                    Pedido.CodigoPaisEmbarque = item.CodigoPais;
                    Pedido.NombrePaisEmbarque = item.NombrePais;
                    goto exit;
                } 
                EditContext.NotifyFieldChanged(EditContext.Field("CodigoPuertoEmbarque")); 
            }
			else
			{
				PedidoInsertar.CodigoPuertoEmbarque = null;
				EditContext.MarkAsUnmodified(EditContext.Field("CodigoPuertoEmbarque"));
			}
		}
		Pedido.CodigoPuertoEmbarque = codigo;
		Pedido.NombrePuertoEmbarque = Pedido.CodigoPaisEmbarque = Pedido.NombrePaisEmbarque = null;
	exit:
		IsModified = EditContext.IsModified();
	}

    private void ValueChangePuertoLlegadaHandler(string value)
    {
        PedidoInsertar.CodigoPuertoLlegada = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorPuertoLlegada) && (Pedido.CodigoPuertoLlegada ?? "").Trim() != (PedidoInsertar.CodigoPuertoLlegada ?? ""))
            Pedido.CodigoPuertoLlegada = Validator.MsgErrorPuertoLlegada = null;
    }

    private async Task OnChangePuertoLlegadaHandler(object value)
	{
		string codigo = (string)(value ?? ""); 
		if (EditContext.IsValid(EditContext.Field("CodigoPuertoLlegada")))
		{ 
			if (!string.IsNullOrEmpty(codigo))
			{
				if ((Pedido.CodigoPuertoLlegada ?? "") == codigo) goto exit;
                (PuertoObtenerPorCodigoDto item, Validator.MsgErrorPuertoEmbarque) = await IUtil.ObtenerPuertoPorCodigo(Alert, codigo);
				if (item is not null)
				{
                    Pedido.CodigoPuertoLlegada = item.CodigoPuerto;
                    Pedido.NombrePuertoLlegada = item.NombrePuerto;
                    Pedido.CodigoPaisLlegada = item.CodigoPais;
                    Pedido.NombrePaisLlegada = item.NombrePais;
                    goto exit;
                } 
                EditContext.NotifyFieldChanged(EditContext.Field("CodigoPuertoLlegada"));
            } 
			else
			{
				PedidoInsertar.CodigoPuertoLlegada = null;
				EditContext.MarkAsUnmodified(EditContext.Field("CodigoPuertoLlegada"));
			}
		}
		Pedido.CodigoPuertoLlegada = codigo;
		Pedido.NombrePuertoLlegada = Pedido.CodigoPaisLlegada = Pedido.NombrePaisLlegada = null;
	exit:
		IsModified = EditContext.IsModified();
	}

	private void OnChangeObservacionHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "Observacion");

	private void OnChangeNumeroDuaHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "NumeroDUA");

	private async Task OnChangeFechaEmisionHandler(object value)
	{
		IsModified = Fnc.VerifyContextIsChanged((value is null && Pedido.FechaEmision.HasValue) || (!Pedido.FechaEmision.HasValue && value is not null) || (Pedido.FechaEmision.HasValue && value is not null && Pedido.FechaEmision != (DateTime?)value), EditContext, "FechaEmision");
		await ConsultaTipoCambioDia();
		ActualizarMontosPorTipoCambioDia();
	}

	private async Task OnChangeFechaCostoHandler(object value)
	{
		IsModified = Fnc.VerifyContextIsChanged(value is not null, EditContext, "FechaCosto");
		await ConsultaTipoCambioDia();
		ActualizarMontosPorTipoCambioDia();
	}

	private void OnChangeFlagCanalHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "FlagCanal");

	private void OnChangeEsVentaSucesivaHandler(object value) => IsModified = Fnc.VerifyContextIsChanged((bool)value, EditContext, "EsVentaSucesiva");

	public void OnChangeEsSujetoReclamoHandler(object value)
	{
		if (!(bool)value)
		{
			PedidoInsertar.ComentarioReclamo = Pedido.ComentarioReclamo = null;
			EditContext.MarkAsUnmodified(EditContext.Field("ComentarioReclamo"));
		}
		IsModified = Fnc.VerifyContextIsChanged(Pedido.EsSujetoReclamo != (bool)value, EditContext, "EsSujetoReclamo");
	}

	private void OnChangeComentarioReclamoHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "ComentarioReclamo");

	private void OnChangeNumeroBLHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "NumeroBL");

	private void OnChangeNumeroInvoiceHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "NumeroInvoice");

	private void OnChangeFechaEmisionBLHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(value is not null, EditContext, "FechaEmisionBL");

	private void OnChangeFechaEmisionInvoiceHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(value is not null, EditContext, "FechaEmisionInvoice");

	private void OnChangeEsAfectoLiberaArancelHandler(object value) => IsModified = Fnc.VerifyContextIsChanged((bool)value, EditContext, "EsAfectoLiberaArancel");

	private void OnChangeEsRequeridoInspeccionHandler(object value) => IsModified = Fnc.VerifyContextIsChanged((bool)value, EditContext, "EsRequeridoInspeccion");

	private void OnChangeEsPrevioAforoHandler(object value) => IsModified = Fnc.VerifyContextIsChanged((bool)value, EditContext, "EsPrevioAforo");

	private void OnChangeComentarioHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "Comentario");

	private void OnChangeDescripcionEmpaqueHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "DescripcionEmpaque");

	private void OnChangeDescripcionFormaPagoHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "DescripcionFormaPago");

	private void OnChangeDescripcionMarcaHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "DescripcionMarca");

	private void OnChangeReferenciaHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "Referencia");

	private void OnChangeComentarioDocumentacionHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "ComentarioDocumentacion");

	private void OnChangeCantidadDiasSobreestadiaHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(value is not null, EditContext, "CantidadDiasSobreestadia");

	private void OnChangeCantidadDiasAlmacenajeHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(value is not null, EditContext, "CantidadDiasAlmacenaje");

	private void OnChangeFechaDireccionamientoHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(value is not null, EditContext, "FechaDireccionamiento");

	private void OnChangeFechaEntregaDocumentoHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(value is not null, EditContext, "FechaEntregaDocumento");

	private void OnChangeFechaVistoBuenoHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(value is not null, EditContext, "FechaVistoBueno");

	private void OnChangeDescripcionTransporteHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "DescripcionTransporte");

	private void OnChangeNombreNaveHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "NombreNave");

	private void OnChangeDescripcionLocalDestinoHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "DescripcionLocalDestino");

	private void OnChangeNumeroManifiestoHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "NumeroManifiesto");

	private void OnChangeFlagModalidadEmbarqueHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "FlagModalidadEmbarque");

	private void OnChangeFechaEmbarqueHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(value is not null, EditContext, "FechaEmbarque");

	private void OnChangeNombreTipoEmbarqueHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "NombreTipoEmbarque");

	private void OnChangeFlagViaTransporteHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "FlagViaTransporte");

	private void OnChangeFechaEstimadaArriboHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(value is not null, EditContext, "FechaEstimadaArribo");

	private void OnChangeFechaLlegadaHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(value is not null, EditContext, "FechaLlegada");

	private void OnChangeEsTransbordoHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!(bool)value, EditContext, "EsTransbordo");

	private void OnChangeFechaDescargaHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(value is not null, EditContext, "FechaDescarga");

	private void OnChangeFechaTarjaHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(value is not null, EditContext, "FechaTarja");

	private void OnChangeFechaDesgloseCargaHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(value is not null, EditContext, "FechaDesgloseCarga");

	private void OnChangeNombreConsolidadorHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "NombreConsolidador");

	private void OnChangeDescripcionTramiteAduanaHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "DescripcionTramiteAduana");

	private void OnChangeDescripcionModalidadHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "DescripcionModalidad");

	private void OnChangeFechaAforoCanalHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(value is not null, EditContext, "FechaAforoCanal");

	private void OnChangeFechaSobreestadiaHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(value is not null, EditContext, "FechaSobreestadia");

	private void OnChangeFechaLevanteCanalHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(value is not null, EditContext, "FechaLevanteCanal");

	private void OnChangeFechaLugarAlmacenajeHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(value is not null, EditContext, "FechaLugarAlmacenaje");

	private void OnChangeFechaInspeccionHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(value is not null, EditContext, "FechaInspeccion");

	private void OnChangeFechaRecojoDocumentoAduanaHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(value is not null, EditContext, "FechaRecojoDocumentoAduana");

	private void OnChangeFechaLevanteInspeccionHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(value is not null, EditContext, "FechaLevanteInspeccion");

	private void OnChangeNumeroPresupuestoHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "NumeroPresupuesto");

	private void OnChangeFlagTipoFinanciamientoHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "FlagTipoFinanciamiento");

	private void OnChangeFechaPagoFinanciamientoHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(value is not null, EditContext, "FechaPagoFinanciamiento");

	private void OnChangeCreditoAgenteAduanaHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "CreditoAgenteAduana");

	private void OnChangeFechaPagoAgenteAduanaHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(value is not null, EditContext, "FechaPagoAgenteAduana");

	private void OnChangeFechaEmisionDUAHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(value is not null, EditContext, "FechaEmisionDUA");

	private void OnChangeFechaCancelacionDUAHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(value is not null, EditContext, "FechaCancelacionDUA");

	private void OnChangeFechaVencimientoInvoiceHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(value is not null, EditContext, "FechaVencimientoInvoice");

	private void OnChangeCantidadDiasVenceInvoiceHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(value is not null, EditContext, "CantidadDiasVenceInvoice");

	private void OnChangeNumeroPackListHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "NumeroPackList");

	private void OnChangeNumeroPolizaHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "NumeroPoliza");

	private void OnChangeFechaPackListHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(value is not null, EditContext, "FechaPackList");

	private void OnChangeFechaEmisionPolizaHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(value is not null, EditContext, "FechaEmisionPoliza");
	#endregion

	public void Dispose() => GC.SuppressFinalize(this);
}
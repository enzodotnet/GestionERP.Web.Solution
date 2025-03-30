using AutoMapper;
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
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Global;

namespace GestionERP.Web.Pages.Empresa.Importacion.Pedido;

public partial class Edit : IDisposable
{
    #region Propiedades
    private const string codigoModulo = "02";
    private const string codigoServicio = "S106";
    private const string rutaAccion = "/editar";
    private const string rutaServicio = "/pedidos";
    private string rutaEmpresa = "";
    public FluentValidationValidator validator;

    public PedidoObtenerDto Pedido { get; set; }
    public PedidoEditarDto PedidoEditar { get; set; }
	public List<PedidoDetalleGrid> GridDetalles { get; set; }
	public PedidoDetalleGrid ItemGridDetalle { get; set; }
	public PedidoDetalleObtenerDto Detalle { get; set; }
    public PedidoDetalleEditarDto DetalleEditar { get; set; }
    public PedidoDetalleInsertarDto DetalleInsertar { get; set; }
    private EditContext EditContext { get; set; }
    private EditContext DetalleInsertarContext { get; set; }
    private EditContext DetalleEditarContext { get; set; }
    public PedidoDetalleGridValidator GridDetalleValidator { get; set; }
    public PedidoEditarValidator Validator { get; set; }
    public PedidoDetalleInsertarValidator DetalleInsertarValidator { get; set; }
    public PedidoDetalleEditarValidator DetalleEditarValidator { get; set; }
    private MonedaObtenerPorTipoDto MN { get; set; }
    private MonedaObtenerPorTipoDto ME { get; set; } 
	public bool IsEditingGridDetalle { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool IsAuthUser { get; set; }
    private bool IsModified { get; set; }
    private bool IsModifiedDetalle { get; set; } 
	public bool IsEditAmountMN { get; set; }
	public bool IsEditAmountME { get; set; }
	public TelerikNotification AlertDetalle { get; set; }
    public TelerikNotification Alert { get; set; } 
    public TelerikNotification AlertCatalogoDetalles { get; set; }
    public string VerboAccionModal { get; set; }
    public string DescripcionAccionModal { get; set; }
    public string TipoAccionModal { get; set; } 
    public string TipoAccionDialog { get; set; }
    public string OrigenDialog { get; set; }
    public string OrigenModal { get; set; }
    public bool EsVisibleDialogDetalle { get; set; }
    public bool EsVisibleModalDetalle { get; set; }
	public bool IsInitPage { get; set; }
    public bool IsChangedSituacionImportacion { get; set; }
    public bool IsChangedRegimenImportacion { get; set; }
    public bool IsChangedEntidadAgenteAduana { get; set; }
    public bool IsChangedEntidadTerminal { get; set; }
    public bool IsChangedEntidadMuelle { get; set; }
    public bool IsChangedEntidadNaviera { get; set; }
    public bool IsChangedEntidadAlmacen { get; set; }
    public bool IsChangedEntidadAgenciaCarga { get; set; }
    public bool IsChangedPuertoEmbarque { get; set; }
    public bool IsChangedPuertoLlegada { get; set; }
    public bool IsChangedTransporteImportacion { get; set; }
	public ISvgIcon IconoAccionModal { get; set; }
    private TelerikGrid<PedidoDetalleGrid> GridDetalleRef { get; set; }
    public GridSelectionMode SelectionModeCatalogoSolicitud { get; set; }
    public IEnumerable<PedidoFlag> TiposFinanciamiento { get; set; }
    public IEnumerable<PedidoFlag> ViasTransporte { get; set; }
    public IEnumerable<PedidoFlag> Canales { get; set; }
    public IEnumerable<PedidoFlag> ModalidadesEmbarque { get; set; }
    public IEnumerable<PedidoFlag> EstadosIngreso { get; set; } 
    public EmpresaEjercicioConsultaIntervaloFechaDto FechaIntervalo { get; set; }
    public List<OrdenDetalleCatalogoAtenderDto> CatalogoDetalles { get; set; }
    public IEnumerable<OrdenDetalleCatalogoAtenderDto> ItemsSelectedDetalle { get; set; }
    private TelerikGrid<OrdenDetalleCatalogoAtenderDto> GridCatalogoDetalleRef { get; set; }
    private bool IsLoadingCatalogoDetalles { get; set; }
    public bool EsVisibleCatalogoDetalles { get; set; }
    public GridSelectionMode SelectionModeCatalogoDetalle { get; set; }
    private bool EsSeleccionableCatalogoDetalle { get; set; } 
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    [Parameter] public Guid? Id { get; set; }
    public EmpresaConsultaPorCodigoWebDto Empresa { get; set; }
    private bool EsVisibleVolver { get; set; }
    [Parameter] public string CodigoWebEmpresa { get; set; }
    [Parameter][SupplyParameterFromQuery(Name = "returnpage")] public string ReturnPage { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    #endregion

    [Inject] public IImportacionPedido IPedido { get; set; }
    [Inject] public IImportacionOrden IOrden { get; set; }
    [Inject] public IPrincipalEmpresa IEmpresa { get; set; }
    [Inject] public IPrincipalMoneda IMoneda { get; set; }  
	[Inject] public IPrincipalPermiso IPermiso { get; set; }
	[Inject] public IPrincipalTipoCambioDia ITipoCambioDia { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public UtilService IUtil { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }
    [Inject] public IMapper IMapper { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Notify.ShowLoading(mensaje: "Obteniendo registro");

            PedidoEditar = new(); 

            Detalle = new();
            DetalleInsertar = new();
            DetalleEditar = new();
			GridDetalles = [];
			ItemGridDetalle = new();

            ViasTransporte = PedidoFlag.ViasTransporte();
            ModalidadesEmbarque = PedidoFlag.ModalidadesEmbarque();
            Canales = PedidoFlag.Canales();
            TiposFinanciamiento = PedidoFlag.TiposFinanciamiento();
            EstadosIngreso = PedidoFlag.EstadosIngreso();

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio);
            if (!IsAuthUser) return;

            Empresa = await IEmpresa.ConsultaPorCodigoWeb(CodigoWebEmpresa);
            
            EsVisibleVolver = !string.IsNullOrEmpty(ReturnPage) && ReturnPage is "index" or "view"; 
            rutaEmpresa = INavigation.Uri.Replace(INavigation.BaseUri, "").Split("?")[0].Replace(rutaServicio, "").Replace($"/{Id}", "").Replace(rutaAccion, "");

			if (!await IPermiso.ConsultaEsAsignadoPorSesion(PedidoAcceso.Editar, Empresa.Codigo))
			{
				INavigation.NavigateTo($"{rutaEmpresa}/inicio");
				Notify.Show("No tiene permiso para editar registros de pedidos de importación", "error");
				return;
			}

			FechaIntervalo = await IEmpresa.ConsultaEjercicioIntervaloFecha(Empresa.Codigo);

			Pedido = await IPedido.Obtener(Empresa.Codigo, (Guid) Id);
            if (Pedido is null || Pedido.CodigoEstado is not "ED")
            {
                INavigation.NavigateTo($"{rutaEmpresa}{rutaServicio}");
                Notify.Show($"El registro del pedido de importación consultado a editar no está disponible", "error");
                return;
            }

            MN = await IMoneda.ObtenerPorTipo("MN"); 
			IsEditAmountMN = Pedido.CodigoMoneda == MN.Codigo;

			ME = await IMoneda.ObtenerPorTipo("ME");
			IsEditAmountME = Pedido.CodigoMoneda == ME.Codigo;

			PedidoEditar = IMapper.Map<PedidoEditarDto>(Pedido);

            Validator = new();
            EditContext = new EditContext(PedidoEditar);

			GridDetalles = IMapper.Map<List<PedidoDetalleGrid>>(Pedido.Detalles); 
            IsInitPage = true;
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

    private async Task Editar()
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

            Notify.ShowLoading(mensaje: "Actualización en progreso");

            await IPedido.Editar(Empresa.Codigo, (Guid) Id, PedidoEditar);

            IsModified = false;
            Notify.Show($"El pedido de importación {Pedido.Codigo} ha sido editado con éxito", "success");
            INavigation.NavigateTo($"{rutaEmpresa}{rutaServicio}/{Id}");
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
     

    private async Task ObtenerTipoCambioDia()
    {
        DateTime? fechaTipoCambioDia = PedidoEditar.FechaCosto.HasValue ? PedidoEditar.FechaCosto : Pedido.FechaEmision;
        TipoCambioDiaObtenerPorFechaDto tipoCambioDia = fechaTipoCambioDia.HasValue ? await ITipoCambioDia.ObtenerPorFecha((DateTime)fechaTipoCambioDia, "V") : new();
        PedidoEditar.CodigoTipoCambioDia = tipoCambioDia.Codigo;
        PedidoEditar.MontoTipoCambioDia = tipoCambioDia.Monto;
    }

    private decimal? ConvertirMontoPorTipoCambioDia(decimal? montoConvertir, int cantidadDecimales = 6)
    {
        decimal? montoTipoCambioDia = PedidoEditar.MontoTipoCambioDia; 
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
        if (GridDetalles.Count > 0)
        {
            bool esItemNuevo;
            int indexEdit; 
            foreach ((PedidoDetalleGrid item, int indexGrid) in GridDetalles.Select((item, index) => (item, index)))
            {
                esItemNuevo = !item.Id.HasValue;
			    indexEdit = !esItemNuevo ? PedidoEditar.DetallesEditar.FindIndex(i => i.Id == item.Id) : PedidoEditar.DetallesInsertar.FindIndex(i => i.CodigoArticulo == item.CodigoArticulo);   
                if (Pedido.CodigoMoneda == MN.Codigo)
                {
                    GridDetalles[indexGrid].CostoEstimadoME = ConvertirMontoPorTipoCambioDia(item.CostoEstimadoMN);
					if (!esItemNuevo && indexEdit > -1)
						PedidoEditar.DetallesEditar[indexEdit].CostoEstimadoME = GridDetalles[indexGrid].CostoEstimadoME;
					else if (esItemNuevo)
						PedidoEditar.DetallesInsertar[indexEdit].CostoEstimadoME = GridDetalles[indexGrid].CostoEstimadoME;
                }
                else if (Pedido.CodigoMoneda == ME.Codigo)
                {
                    GridDetalles[indexGrid].CostoEstimadoMN = ConvertirMontoPorTipoCambioDia(item.CostoEstimadoME); 
				    if (!esItemNuevo && indexEdit > -1)
					    PedidoEditar.DetallesEditar[indexEdit].CostoEstimadoMN = GridDetalles[indexGrid].CostoEstimadoMN;
				    else if (esItemNuevo)
					    PedidoEditar.DetallesInsertar[indexEdit].CostoEstimadoMN = GridDetalles[indexGrid].CostoEstimadoMN;
                }
				if (!esItemNuevo && indexEdit == -1)
                    PedidoEditar.DetallesEditar.Add(IMapper.Map(IMapper.Map<PedidoDetalleObtenerDto>(GridDetalles[indexGrid]), new PedidoDetalleEditarDto()));
			} 
        } 
    } 

    private void IniciarAccionModal(string tipoAccion, string origenModal)
    {
        TipoAccionModal = tipoAccion;
        OrigenModal = origenModal;
        switch (tipoAccion)
        { 
            case "M" or "E":
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
                if (tipoAccion is "M" ) 
                    DetalleInsertarContext = new EditContext(DetalleInsertar);   
                else if (tipoAccion is "E") 
                    DetalleEditarContext = new EditContext(DetalleEditar); 
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
        VerboAccionModal = DescripcionAccionModal = TipoAccionModal = "";
        StateHasChanged();
    }

    private void Volver() => INavigation.NavigateTo($"{rutaEmpresa}{rutaServicio}{(ReturnPage == "view" ? $"/{Id}" : "")}");

    public void CerrarDialog()
    {
        if (OrigenDialog is "detalle")
            EsVisibleDialogDetalle = false;
        TipoAccionDialog = OrigenDialog = "";
        StateHasChanged();
    }

    private bool VerificarRegistroEsValido()
    {
        bool esValido = true;

        if (!PedidoEditar.MontoTipoCambioDia.HasValue)
        {
            string fechaTipoCambio = PedidoEditar.FechaCosto.HasValue ? PedidoEditar.FechaCosto?.ToString("dd/MM/yyyy") : Pedido.FechaEmision?.ToString("dd/MM/yyyy");
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

    private void NotifyChange(bool? isValueByVerify = null)
    {
        IsModified = isValueByVerify ?? EditContext.IsModified();
        if (!IsModified)
            IsModified = PedidoEditar.DetallesInsertar.Count > 0 || PedidoEditar.DetallesEditar.Count > 0 || PedidoEditar.DetallesEliminar.Count > 0;
    }

    #region CatalogoDetalles
    public async Task MostrarCatalogoDetalles()
    {
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
            if (!IsAuthUser)
            {
                EsVisibleCatalogoDetalles = false;
                return;
            }

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

            PedidoEditar.DetallesInsertar.Add(DetalleInsertar);
            GridDetalles.Add(ItemGridDetalle);
            detalleState.InsertedItem = ItemGridDetalle;

            await GridDetalleRef.SetStateAsync(detalleState);
        }
        ModificarImportesTotales();
        IsModified = true;
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

    public static void OnRowDetalleRenderHandler(GridRowRenderEventArgs args) => args.Class = !(args.Item as PedidoDetalleGrid).Id.HasValue ? "new-row" : "";

	public void MostrarModificarDetalle(PedidoDetalleGrid item = null)
	{
		if (TipoAccionModal is not "V")
			Detalle = IMapper.Map<PedidoDetalleObtenerDto>(item); 

		string tipoAccion = !Detalle.Id.HasValue ? "M" : "E";  
		if (tipoAccion is "M")
        {
            DetalleInsertar = IMapper.Map<PedidoDetalleInsertarDto>(Detalle);
            DetalleInsertarValidator = new()
            {
                FlagTipoMoneda = Pedido.CodigoMoneda == MN.Codigo ? "MN" : "ME", 
                UnidadConversion = Detalle.UnidadConversion
            }; 
        }
		else
        {
            DetalleEditar = IMapper.Map<PedidoDetalleEditarDto>(Detalle);
            DetalleEditarValidator = new()
            {
                FlagTipoMoneda = Pedido.CodigoMoneda == MN.Codigo ? "MN" : "ME",
                UnidadConversion = Detalle.UnidadConversion
            };
        } 
        IniciarAccionModal(tipoAccion, "detalle");
	}

	private void MostrarQuitarDetalle(PedidoDetalleGrid item = null)
	{
		if (TipoAccionModal is not "V")
			Detalle = IMapper.Map<PedidoDetalleObtenerDto>(item);

		IniciarAccionDialog(!Detalle.Id.HasValue ? "Q" : "X", "detalle");
	}

	public void VerItemDetalle(PedidoDetalleGrid item)
	{
		Detalle = IMapper.Map<PedidoDetalleObtenerDto>(item);
		IniciarAccionModal("V", "detalle");
	}

    private void OnChangeDetalleObservacionHandler(object value) => IsModifiedDetalle = Fnc.VerifyContextIsChanged((Detalle.Observacion ?? "") != ((string)value ?? ""), TipoAccionModal is "E" ? DetalleEditarContext : DetalleInsertarContext, "Observacion");

    private void OnChangeDetalleCantidadHandler(object value)
    {
        decimal? cantidad = (decimal?)value;
        IsModifiedDetalle = Fnc.VerifyContextIsChanged(Detalle.Cantidad != cantidad, TipoAccionModal is "E" ? DetalleEditarContext : DetalleInsertarContext, "Cantidad");
        ModificarImportesItem(cantidad, TipoAccionModal is "E" ? DetalleEditar.PrecioUnitario : DetalleInsertar.PrecioUnitario);
    }

    private void OnChangeDetallePrecioUnitarioHandler(object value)
    {
        decimal? precioUnitario = (decimal?)value;
        IsModifiedDetalle = Fnc.VerifyContextIsChanged(Detalle.PrecioUnitario != precioUnitario, TipoAccionModal is "E" ? DetalleEditarContext : DetalleInsertarContext, "PrecioUnitario");
        ModificarImportesItem(TipoAccionModal is "E" ? DetalleEditar.Cantidad : DetalleInsertar.Cantidad, precioUnitario);
    }

    private void OnChangeDetalleCostoEstimadoMEHandler(object value)
    {
        decimal? costoEstimadoME = (decimal?)value;
        IsModifiedDetalle = Fnc.VerifyContextIsChanged(Detalle.CostoEstimadoME != costoEstimadoME, TipoAccionModal is "E" ? DetalleEditarContext : DetalleInsertarContext, "CostoEstimadoME");
        decimal? montoConvertido = ConvertirMontoPorTipoCambioDia(costoEstimadoME);
        if (TipoAccionModal is "M")
            DetalleInsertar.CostoEstimadoMN = montoConvertido;
        else
            DetalleEditar.CostoEstimadoMN = montoConvertido;
    }

    private void OnChangeDetalleCostoEstimadoMNHandler(object value)
    {
        decimal? costoEstimadoMN = (decimal?)value;
        IsModifiedDetalle = Fnc.VerifyContextIsChanged(Detalle.CostoEstimadoMN != costoEstimadoMN, TipoAccionModal is "E" ? DetalleEditarContext : DetalleInsertarContext, "CostoEstimadoMN");
        decimal? montoConvertido = ConvertirMontoPorTipoCambioDia(costoEstimadoMN);
        if (TipoAccionModal is "M")
            DetalleInsertar.CostoEstimadoME = montoConvertido;
        else
            DetalleEditar.CostoEstimadoME = montoConvertido;
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

        int indexGrid = GridDetalles.FindIndex(i => i.CodigoArticulo == item.CodigoArticulo);
        bool esItemNuevo = !item.Id.HasValue;
        bool esActualizableImportes = false;
        int indexEdit = !esItemNuevo ? PedidoEditar.DetallesEditar.FindIndex(i => i.Id == item.Id) : PedidoEditar.DetallesInsertar.FindIndex(i => i.CodigoArticulo == item.CodigoArticulo);
         
        if (args.Field == "PrecioUnitario")
        {
            if (!esItemNuevo && indexEdit > -1)
                PedidoEditar.DetallesEditar[indexEdit].PrecioUnitario = item.PrecioUnitario;
            else if (esItemNuevo)
                PedidoEditar.DetallesInsertar[indexEdit].PrecioUnitario = item.PrecioUnitario; 
            GridDetalles[indexGrid].PrecioUnitario = item.PrecioUnitario;
			esActualizableImportes = true;
        }
        else if(args.Field == "Cantidad")
        {
            if (!esItemNuevo && indexEdit > -1)
                PedidoEditar.DetallesEditar[indexEdit].Cantidad = item.Cantidad;
            else if (esItemNuevo)
                PedidoEditar.DetallesInsertar[indexEdit].Cantidad = item.Cantidad; 
            GridDetalles[indexGrid].Cantidad = item.Cantidad;
			esActualizableImportes = true;
		}
        else if (args.Field == "CostoEstimadoMN")
        {
            decimal? montoConvertido = ConvertirMontoPorTipoCambioDia(item.CostoEstimadoMN);
			if (!esItemNuevo && indexEdit > -1)
            {
                PedidoEditar.DetallesEditar[indexEdit].CostoEstimadoMN = item.CostoEstimadoMN;
                PedidoEditar.DetallesEditar[indexEdit].CostoEstimadoME = montoConvertido;
            }
            else if (esItemNuevo)
            {
                PedidoEditar.DetallesInsertar[indexEdit].CostoEstimadoMN = item.CostoEstimadoMN;
                PedidoEditar.DetallesInsertar[indexEdit].CostoEstimadoME = montoConvertido;
            }
            GridDetalles[indexGrid].CostoEstimadoMN = item.CostoEstimadoMN;
			GridDetalles[indexGrid].CostoEstimadoME = montoConvertido;
		}
        else if (args.Field == "CostoEstimadoME")
        {
			decimal? montoConvertido = ConvertirMontoPorTipoCambioDia(item.CostoEstimadoME);
			if (!esItemNuevo && indexEdit > -1)
            {
                PedidoEditar.DetallesEditar[indexEdit].CostoEstimadoME = item.CostoEstimadoME;
				PedidoEditar.DetallesEditar[indexEdit].CostoEstimadoMN = montoConvertido;
			}
            else if (esItemNuevo)
            { 
                PedidoEditar.DetallesInsertar[indexEdit].CostoEstimadoME = item.CostoEstimadoME;
				PedidoEditar.DetallesInsertar[indexEdit].CostoEstimadoMN = montoConvertido;
			} 
            GridDetalles[indexGrid].CostoEstimadoME = item.CostoEstimadoME;
			GridDetalles[indexGrid].CostoEstimadoMN = montoConvertido;
		} 
        if (esActualizableImportes)
        {
			ModificarImportesItem(item.Cantidad, item.PrecioUnitario, indexGrid, indexEdit);
			ModificarImportesTotales();
		}  
		if (!esItemNuevo && indexEdit == -1)
        {
            PedidoEditar.DetallesEditar.Add(IMapper.Map(IMapper.Map<PedidoDetalleObtenerDto>(GridDetalles[indexGrid]), new PedidoDetalleEditarDto()));
            NotifyChange();
        } 
		IsEditingGridDetalle = false;
	}

    private void ModificarImportesItem(decimal? cantidad, decimal? precioUnitario, int indexGrid = -1, int indexEdit = -1)
    {
        decimal? importeBruto = null;
        if (cantidad.HasValue && precioUnitario.HasValue)
            importeBruto = Math.Round((decimal)(cantidad * precioUnitario), 2, MidpointRounding.AwayFromZero);
        if (indexGrid > -1)
        {
			GridDetalles[indexGrid].ImporteBruto = importeBruto; 
			if (indexEdit > -1)
            {
				if (GridDetalles[indexGrid].Id.HasValue)
					PedidoEditar.DetallesEditar[indexEdit].ImporteBruto = importeBruto;
				else
					PedidoEditar.DetallesInsertar[indexEdit].ImporteBruto = importeBruto;
			} 
        }
        else
        {
            if (TipoAccionModal is "E")
                DetalleEditar.ImporteBruto = importeBruto;
            else
                DetalleInsertar.ImporteBruto = importeBruto;
        }
    }

	private void ModificarImportesTotales()
	{
		PedidoEditar.TotalImporteBruto = GridDetalles.Count == 0 ? 0 : GridDetalles.Sum(x => x.ImporteBruto);
        PedidoEditar.TotalImporteNeto = PedidoEditar.TotalImporteBruto + Pedido.TotalImporteFlete + Pedido.TotalImporteSeguro;
	} 

    private async Task AccionarDetalle(string tipoAccion)
    {
        GridState<PedidoDetalleGrid> detalleState = GridDetalleRef.GetState();
        int indexGrid = GridDetalles.FindIndex(i => i.CodigoArticulo == Detalle.CodigoArticulo);
        int indexEdit = tipoAccion is "E" or "X" ? PedidoEditar.DetallesEditar.FindIndex(i => i.Id == Detalle.Id) : PedidoEditar.DetallesInsertar.FindIndex(i => i.CodigoArticulo == Detalle.CodigoArticulo);
        
        switch (tipoAccion)
        {
            case "M":
                PedidoEditar.DetallesInsertar[indexEdit] = DetalleInsertar;
                GridDetalles[indexGrid] = IMapper.Map<PedidoDetalleGrid>(IMapper.Map(DetalleInsertar, Detalle));
                break;
            case "E":
                if (indexEdit > -1) 
                    PedidoEditar.DetallesEditar[indexEdit] = DetalleEditar; 
                else 
                    PedidoEditar.DetallesEditar.Add(DetalleEditar);
                GridDetalles[indexGrid] = IMapper.Map<PedidoDetalleGrid>(IMapper.Map(DetalleEditar, Detalle));
                break;
            case "Q" or "X":
                if (tipoAccion == "Q")
                    PedidoEditar.DetallesInsertar.RemoveAt(indexEdit);
                else
                {
                    PedidoEditar.DetallesEliminar.Add(new() { Id = (Guid) Detalle.Id});
                    if (indexEdit > -1) 
                        PedidoEditar.DetallesEditar.RemoveAt(indexEdit); 
                }
                GridDetalles.RemoveAt(indexGrid);
                CerrarDialog();
                break;
        };
        CerrarModal();
		ModificarImportesTotales();
		NotifyChange();
		await GridDetalleRef.SetStateAsync(detalleState);
    }
    #endregion

    #region Catalogos
    private void CargarItemCatalogoSituacionImportacion(SituacionImportacionCatalogoDto item)
    {
        PedidoEditar.CodigoSituacionImportacion = Pedido.CodigoSituacionImportacion = item.CodigoSituacionImportacion;
        Pedido.NombreSituacionImportacion = item.NombreSituacionImportacion;
        IsChangedSituacionImportacion = true;
        Validator.MsgErrorSituacion = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoSituacionImportacion"));
        NotifyChange();
    }

    private void CargarItemCatalogoRegimenImportacion(RegimenImportacionCatalogoDto item)
    {
        PedidoEditar.CodigoRegimenImportacion = Pedido.CodigoRegimenImportacion = item.CodigoRegimenImportacion;
        Pedido.NombreRegimenImportacion = item.NombreRegimenImportacion;
        IsChangedRegimenImportacion = true;
        Validator.MsgErrorRegimen = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoRegimenImportacion"));
        NotifyChange();
    }

    private void CargarItemCatalogoEntidadAlmacen(EntidadProveedorCatalogoPorEmpresaDto item)
    {
        PedidoEditar.CodigoEntidadAlmacen = Pedido.CodigoEntidadAlmacen = item.CodigoEntidad.Trim();
        Pedido.NombreEntidadAlmacen = item.NombreEntidad;
        IsChangedEntidadAlmacen = true;
        Validator.MsgErrorEntidadAlmacen = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidadAlmacen"));
        NotifyChange();
    }

    private void CargarItemCatalogoEntidadAgenciaCarga(EntidadProveedorCatalogoPorEmpresaDto item)
    {
        PedidoEditar.CodigoEntidadAgenciaCarga = Pedido.CodigoEntidadAgenciaCarga = item.CodigoEntidad.Trim();
        Pedido.NombreEntidadAgenciaCarga = item.NombreEntidad;
        IsChangedEntidadAgenciaCarga = true;
        Validator.MsgErrorEntidadAgenciaCarga = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidadAgenciaCarga"));
        NotifyChange();
    }

    private void CargarItemCatalogoEntidadAgenteAduana(EntidadProveedorCatalogoPorEmpresaDto item)
    {
        PedidoEditar.CodigoEntidadAgenteAduana = Pedido.CodigoEntidadAgenteAduana = item.CodigoEntidad.Trim();
        Pedido.NombreEntidadAgenteAduana = item.NombreEntidad;
        IsChangedEntidadAgenteAduana = true;
        Validator.MsgErrorEntidadAgenteAduana = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidadAgenteAduana"));
        NotifyChange();
    }

    private void CargarItemCatalogoEntidadMuelle(EntidadProveedorCatalogoPorEmpresaDto item)
    {
        PedidoEditar.CodigoEntidadMuelle = Pedido.CodigoEntidadMuelle = item.CodigoEntidad.Trim();
        Pedido.NombreEntidadMuelle = item.NombreEntidad;
        IsChangedEntidadMuelle = true;
        Validator.MsgErrorEntidadMuelle = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidadMuelle"));
        NotifyChange();
    }

    private void CargarItemCatalogoEntidadTerminal(EntidadProveedorCatalogoPorEmpresaDto item)
    {
        PedidoEditar.CodigoEntidadTerminal = Pedido.CodigoEntidadTerminal = item.CodigoEntidad.Trim();
        Pedido.NombreEntidadTerminal = item.NombreEntidad;
        IsChangedEntidadTerminal = true;
        Validator.MsgErrorEntidadTerminal = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidadTerminal"));
        NotifyChange();
    }

    private void CargarItemCatalogoEntidadNaviera(EntidadProveedorCatalogoPorEmpresaDto item)
    {
        PedidoEditar.CodigoEntidadNaviera = Pedido.CodigoEntidadNaviera = item.CodigoEntidad.Trim();
        Pedido.NombreEntidadNaviera = item.NombreEntidad;
        IsChangedEntidadNaviera = true;
        Validator.MsgErrorEntidadNaviera = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidadNaviera"));
        NotifyChange();
    }

    private void CargarItemCatalogoPuertoEmbarque(PuertoCatalogoDto item)
    {
        PedidoEditar.CodigoPuertoEmbarque = Pedido.CodigoPuertoEmbarque = item.CodigoPuerto;
        Pedido.NombrePuertoEmbarque = item.NombrePuerto;
        Pedido.CodigoPaisEmbarque = item.CodigoPais;
        Pedido.NombrePaisEmbarque = item.NombrePais;
        IsChangedPuertoEmbarque = true;
        Validator.MsgErrorPuertoEmbarque = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoPuertoEmbarque"));
        NotifyChange();
    }

    private void CargarItemCatalogoPuertoLlegada(PuertoCatalogoDto item)
    {
        PedidoEditar.CodigoPuertoLlegada = Pedido.CodigoPuertoLlegada = item.CodigoPuerto;
        Pedido.NombrePuertoLlegada = item.NombrePuerto;
        Pedido.CodigoPaisLlegada = item.CodigoPais;
        Pedido.NombrePaisLlegada = item.NombrePais;
        IsChangedPuertoLlegada = true;
        Validator.MsgErrorPuertoLlegada = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoPuertoLlegada"));
        NotifyChange();
    }

    private void CargarItemCatalogoTransporteImportacion(TransporteImportacionCatalogoDto item)
    {
        PedidoEditar.CodigoTransporteImportacion = Pedido.CodigoTransporteImportacion = item.CodigoTransporteImportacion;
        Pedido.NombreTransporteImportacion = item.NombreTransporteImportacion;
        IsChangedTransporteImportacion = true;
        Validator.MsgErrorTransporte = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoTransporteImportacion"));
        NotifyChange();
    }
    #endregion

    #region EditContextHandlers
    private void ValueChangeEntidadAgenteAduanaHandler(string value)
    {
        PedidoEditar.CodigoEntidadAgenteAduana = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorEntidadAgenteAduana) && (Pedido.CodigoEntidadAgenteAduana ?? "").Trim() != (PedidoEditar.CodigoEntidadAgenteAduana ?? ""))
            Pedido.CodigoEntidadAgenteAduana = Validator.MsgErrorEntidadAgenteAduana = null;
    }

    private async Task OnChangeEntidadAgenteAduanaHandler(object value)
	{
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoEntidadAgenteAduana")))
		{ 
			PedidoEditar.CodigoEntidadAgenteAduana = string.IsNullOrEmpty(codigo) ? null : codigo;
			if ((Pedido.CodigoEntidadAgenteAduana ?? "").Trim() != codigo)
			{
				IsChangedEntidadAgenteAduana = true;
				if (!string.IsNullOrEmpty(codigo))
				{
					(EntidadProveedorObtenerPorCodigoEmpresaDto item, Validator.MsgErrorEntidadAgenteAduana) = await IUtil.ObtenerProveedorPorCodigoEmpresa(Alert, codigo, Empresa.Codigo, "A");
					if (item is not null)
					{
						Pedido.CodigoEntidadAgenteAduana = item.CodigoEntidad.Trim();
						Pedido.NombreEntidadAgenteAduana = item.NombreEntidad;
						goto exit;
					} 
				    EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidadAgenteAduana"));
				}
			}
			else
			{
				if (!IsChangedEntidadAgenteAduana) EditContext.MarkAsUnmodified(EditContext.Field("CodigoEntidadAgenteAduana"));
				goto exit;
			}
		}
        Pedido.CodigoEntidadAgenteAduana = codigo;
		Pedido.NombreEntidadAgenteAduana = null;
	exit:
		NotifyChange();
	}

    private void ValueChangeEntidadMuelleHandler(string value)
    {
        PedidoEditar.CodigoEntidadMuelle = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorEntidadMuelle) && (Pedido.CodigoEntidadMuelle ?? "").Trim() != (PedidoEditar.CodigoEntidadMuelle ?? ""))
            Pedido.CodigoEntidadMuelle = Validator.MsgErrorEntidadMuelle = null;
    }

    private async Task OnChangeEntidadMuelleHandler(object value)
	{
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoEntidadMuelle")))
		{ 
			PedidoEditar.CodigoEntidadMuelle = string.IsNullOrEmpty(codigo) ? null : codigo;
			if ((Pedido.CodigoEntidadMuelle ?? "").Trim() != codigo)
			{
				IsChangedEntidadMuelle = true;
				if (!string.IsNullOrEmpty(codigo))
				{
					(EntidadProveedorObtenerPorCodigoEmpresaDto item, Validator.MsgErrorEntidadMuelle) = await IUtil.ObtenerProveedorPorCodigoEmpresa(Alert, codigo, Empresa.Codigo, "A");
					if (item is not null)
					{
						Pedido.CodigoEntidadMuelle = item.CodigoEntidad.Trim();
						Pedido.NombreEntidadMuelle = item.NombreEntidad;
						goto exit;
					}
				    EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidadMuelle"));
				}
			}
			else
			{
				if (!IsChangedEntidadMuelle) EditContext.MarkAsUnmodified(EditContext.Field("CodigoEntidadMuelle"));
				goto exit;
			}
		}
        Pedido.CodigoEntidadMuelle = codigo;
		Pedido.NombreEntidadMuelle = null;
    exit:
		NotifyChange();
	}

    private void ValueChangeEntidadAgenciaCargaHandler(string value)
    {
        PedidoEditar.CodigoEntidadAgenciaCarga = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorEntidadAgenciaCarga) && (Pedido.CodigoEntidadAgenciaCarga ?? "").Trim() != (PedidoEditar.CodigoEntidadAgenciaCarga ?? ""))
            Pedido.CodigoEntidadAgenciaCarga = Validator.MsgErrorEntidadAgenciaCarga = null;
    }

    private async Task OnChangeEntidadAgenciaCargaHandler(object value)
	{
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoEntidadAgenciaCarga")))
		{ 
			PedidoEditar.CodigoEntidadAgenciaCarga = string.IsNullOrEmpty(codigo) ? null : codigo;
			if ((Pedido.CodigoEntidadAgenciaCarga ?? "").Trim() != codigo)
			{
				IsChangedEntidadAgenciaCarga = true;
				if (!string.IsNullOrEmpty(codigo))
				{
					(EntidadProveedorObtenerPorCodigoEmpresaDto item, Validator.MsgErrorEntidadAgenciaCarga) = await IUtil.ObtenerProveedorPorCodigoEmpresa(Alert, codigo, Empresa.Codigo, "A");
					if (item is not null)
					{
						Pedido.CodigoEntidadAgenciaCarga = item.CodigoEntidad.Trim();
						Pedido.NombreEntidadAgenciaCarga = item.NombreEntidad;
						goto exit;
					} 
				    EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidadAgenciaCarga"));
				}
			}
			else
			{
				if (!IsChangedEntidadAgenciaCarga) EditContext.MarkAsUnmodified(EditContext.Field("CodigoEntidadAgenciaCarga"));
				goto exit;
			}
		}
        Pedido.CodigoEntidadAgenciaCarga = codigo;
		Pedido.NombreEntidadAgenciaCarga = null;
	exit:
		NotifyChange();
	}

    private void ValueChangeEntidadTerminalHandler(string value)
    {
        PedidoEditar.CodigoEntidadTerminal = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorEntidadTerminal) && (Pedido.CodigoEntidadTerminal ?? "").Trim() != (PedidoEditar.CodigoEntidadTerminal ?? ""))
            Pedido.CodigoEntidadTerminal = Validator.MsgErrorEntidadTerminal = null;
    }

    private async Task OnChangeEntidadTerminalHandler(object value)
	{
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoEntidadTerminal")))
		{ 
			PedidoEditar.CodigoEntidadTerminal = string.IsNullOrEmpty(codigo) ? null : codigo;
			if ((Pedido.CodigoEntidadTerminal ?? "").Trim() != codigo)
			{
				IsChangedEntidadTerminal = true;
				if (!string.IsNullOrEmpty(codigo))
				{
					(EntidadProveedorObtenerPorCodigoEmpresaDto item, Validator.MsgErrorEntidadTerminal) = await IUtil.ObtenerProveedorPorCodigoEmpresa(Alert, codigo, Empresa.Codigo, "A");
					if (item is not null)
					{
						Pedido.CodigoEntidadTerminal = item.CodigoEntidad.Trim();
						Pedido.NombreEntidadTerminal = item.NombreEntidad;
						goto exit;
					} 
				    EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidadTerminal"));
				}
			}
			else
			{
				if (!IsChangedEntidadTerminal) EditContext.MarkAsUnmodified(EditContext.Field("CodigoEntidadTerminal"));
				goto exit;
			}
		}
        Pedido.CodigoEntidadTerminal = codigo;
		Pedido.NombreEntidadTerminal = null;
	exit:
		NotifyChange();
	}

    private void ValueChangeEntidadAlmacenHandler(string value)
    {
        PedidoEditar.CodigoEntidadAlmacen = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorEntidadAlmacen) && (Pedido.CodigoEntidadAlmacen ?? "").Trim() != (PedidoEditar.CodigoEntidadAlmacen ?? ""))
            Pedido.CodigoEntidadAlmacen = Validator.MsgErrorEntidadAlmacen = null;
    }

    private async Task OnChangeEntidadAlmacenHandler(object value)
	{
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoEntidadAlmacen")))
		{ 
			PedidoEditar.CodigoEntidadAlmacen = string.IsNullOrEmpty(codigo) ? null : codigo;
			if ((Pedido.CodigoEntidadAlmacen ?? "").Trim() != codigo)
			{
				IsChangedEntidadAlmacen = true;
				if (!string.IsNullOrEmpty(codigo))
				{
					(EntidadProveedorObtenerPorCodigoEmpresaDto item, Validator.MsgErrorEntidadAlmacen) = await IUtil.ObtenerProveedorPorCodigoEmpresa(Alert, codigo, Empresa.Codigo, "A");
					if (item is not null)
					{
						Pedido.CodigoEntidadAlmacen = item.CodigoEntidad.Trim();
						Pedido.NombreEntidadAlmacen = item.NombreEntidad;
						goto exit;
					} 
				    EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidadAlmacen"));
				}
			}
			else
			{
				if (!IsChangedEntidadAlmacen) EditContext.MarkAsUnmodified(EditContext.Field("CodigoEntidadAlmacen"));
				goto exit;
			}
		}
        Pedido.CodigoEntidadAlmacen = codigo;
		Pedido.NombreEntidadAlmacen = null;
	exit:
		NotifyChange();
	}

    private void ValueChangeEntidadNavieraHandler(string value)
    {
        PedidoEditar.CodigoEntidadNaviera = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorEntidadNaviera) && (Pedido.CodigoEntidadNaviera ?? "").Trim() != (PedidoEditar.CodigoEntidadNaviera ?? ""))
            Pedido.CodigoEntidadNaviera = Validator.MsgErrorEntidadNaviera = null;
    }

    private async Task OnChangeEntidadNavieraHandler(object value)
	{
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoEntidadNaviera")))
		{ 
			PedidoEditar.CodigoEntidadNaviera = string.IsNullOrEmpty(codigo) ? null : codigo;
			if ((Pedido.CodigoEntidadNaviera ?? "").Trim() != codigo)
			{
				IsChangedEntidadNaviera = true;
				if (!string.IsNullOrEmpty(codigo))
				{
					(EntidadProveedorObtenerPorCodigoEmpresaDto item, Validator.MsgErrorEntidadNaviera) = await IUtil.ObtenerProveedorPorCodigoEmpresa(Alert, codigo, Empresa.Codigo, "A");
					if (item is not null)
					{
						Pedido.CodigoEntidadNaviera = item.CodigoEntidad.Trim();
						Pedido.NombreEntidadNaviera = item.NombreEntidad;
						goto exit;
					} 
				    EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidadNaviera"));
				}
			}
			else
			{
				if (!IsChangedEntidadNaviera) EditContext.MarkAsUnmodified(EditContext.Field("CodigoEntidadNaviera"));
				goto exit;
			}
		}
        Pedido.CodigoEntidadNaviera = codigo;
		Pedido.NombreEntidadNaviera = null;
	exit:
		NotifyChange();
	}

    private void ValueChangeSituacionImportacionHandler(string value)
    {
        PedidoEditar.CodigoSituacionImportacion = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorSituacion) && (Pedido.CodigoSituacionImportacion ?? "").Trim() != (PedidoEditar.CodigoSituacionImportacion ?? ""))
            Pedido.CodigoSituacionImportacion = Validator.MsgErrorSituacion = null;
    }

    private async Task OnChangeSituacionImportacionHandler(object value)
	{
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoSituacionImportacion")))
		{ 
			PedidoEditar.CodigoSituacionImportacion = string.IsNullOrEmpty(codigo) ? null : codigo;
			if ((Pedido.CodigoSituacionImportacion ?? "") != codigo)
			{
				IsChangedSituacionImportacion = true;
                if (!string.IsNullOrEmpty(codigo))
                {
                    (SituacionImportacionObtenerPorCodigoDto item, Validator.MsgErrorSituacion) = await IUtil.ObtenerSituacionImportacionPorCodigo(Alert, codigo);
                    if (item is not null)
                    {
                        Pedido.CodigoSituacionImportacion = item.CodigoSituacionImportacion;
                        Pedido.NombreSituacionImportacion = item.NombreSituacionImportacion;
                        goto exit;
                    } 
                    EditContext.NotifyFieldChanged(EditContext.Field("CodigoSituacionImportacion"));
                }
			}
			else
			{
				if (!IsChangedSituacionImportacion) EditContext.MarkAsUnmodified(EditContext.Field("CodigoSituacionImportacion"));
				goto exit;
			}
		}
        Pedido.CodigoSituacionImportacion = codigo;
		Pedido.NombreSituacionImportacion = null;
	exit:
		NotifyChange();
	}

    private void ValueChangeTransporteImportacionHandler(string value)
    {
        PedidoEditar.CodigoTransporteImportacion = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorTransporte) && (Pedido.CodigoTransporteImportacion ?? "").Trim() != (PedidoEditar.CodigoTransporteImportacion ?? ""))
            Pedido.CodigoTransporteImportacion = Validator.MsgErrorTransporte = null;
    }

    private async Task OnChangeTransporteImportacionHandler(object value)
	{
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoTransporteImportacion")))
		{ 
			PedidoEditar.CodigoTransporteImportacion = string.IsNullOrEmpty(codigo) ? null : codigo;
			if ((Pedido.CodigoTransporteImportacion ?? "") != codigo)
			{
				IsChangedTransporteImportacion = true;
				if (!string.IsNullOrEmpty(codigo))
				{ 
					(TransporteImportacionObtenerPorCodigoDto item, Validator.MsgErrorTransporte) = await IUtil.ObtenerTransporteImportacionPorCodigo(Alert, codigo);
                    if (item is not null)
                    {
                        Pedido.CodigoTransporteImportacion = item.CodigoTransporteImportacion;
                        Pedido.NombreTransporteImportacion = item.NombreTransporteImportacion;
                        goto exit;
                    } 
                    EditContext.NotifyFieldChanged(EditContext.Field("CodigoTransporteImportacion"));
                }
			}
			else
			{
				if (!IsChangedTransporteImportacion) EditContext.MarkAsUnmodified(EditContext.Field("CodigoTransporteImportacion"));
				goto exit;
			}
		}
        Pedido.CodigoTransporteImportacion = codigo;
		Pedido.NombreTransporteImportacion = null;
	exit:
		NotifyChange();
	}

    private void ValueChangeRegimenImportacionHandler(string value)
    {
        PedidoEditar.CodigoRegimenImportacion = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorRegimen) && (Pedido.CodigoRegimenImportacion ?? "").Trim() != (PedidoEditar.CodigoRegimenImportacion ?? ""))
            Pedido.CodigoRegimenImportacion = Validator.MsgErrorRegimen = null;
    }

    private async Task OnChangeRegimenImportacionHandler(object value)
	{
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoRegimenImportacion")))
		{ 
			PedidoEditar.CodigoRegimenImportacion = string.IsNullOrEmpty(codigo) ? null : codigo;
			if ((Pedido.CodigoRegimenImportacion ?? "") != codigo)
			{
				IsChangedRegimenImportacion = true;
				if (!string.IsNullOrEmpty(codigo))
				{
                    (RegimenImportacionObtenerPorCodigoDto item, Validator.MsgErrorRegimen) = await IUtil.ObtenerRegimenImportacionPorCodigo(Alert, codigo);
                    if (item is not null)
                    {
                        Pedido.CodigoRegimenImportacion = item.CodigoRegimenImportacion;
                        Pedido.NombreRegimenImportacion = item.NombreRegimenImportacion;
                        goto exit;
                    } 
                    EditContext.NotifyFieldChanged(EditContext.Field("CodigoRegimenImportacion"));
                }
			}
			else
			{
				if (!IsChangedRegimenImportacion) EditContext.MarkAsUnmodified(EditContext.Field("CodigoRegimenImportacion"));
				goto exit;
			}
		}
        Pedido.CodigoRegimenImportacion = codigo;
		Pedido.NombreRegimenImportacion = null;
	exit:
		NotifyChange();
	}

    private void ValueChangePuertoEmbarqueHandler(string value)
    {
        PedidoEditar.CodigoPuertoEmbarque = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorPuertoEmbarque) && (Pedido.CodigoPuertoEmbarque ?? "").Trim() != (PedidoEditar.CodigoPuertoEmbarque ?? ""))
            Pedido.CodigoPuertoEmbarque = Validator.MsgErrorPuertoEmbarque = null;
    }

    private async Task OnChangePuertoEmbarqueHandler(object value)
	{
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoPuertoEmbarque")))
		{ 
			PedidoEditar.CodigoPuertoEmbarque = string.IsNullOrEmpty(codigo) ? null : codigo;
			if ((Pedido.CodigoPuertoEmbarque ?? "") != codigo)
			{
				IsChangedPuertoEmbarque = true;
				if (!string.IsNullOrEmpty(codigo))
				{ 
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
			}
			else
			{
				if (!IsChangedPuertoEmbarque) EditContext.MarkAsUnmodified(EditContext.Field("CodigoPuertoEmbarque"));
				goto exit;
			}
		}
        Pedido.CodigoPuertoEmbarque = codigo;
		Pedido.NombrePuertoEmbarque = Pedido.CodigoPaisEmbarque = Pedido.NombrePaisEmbarque = null;
	exit:
		NotifyChange();
	}

    private void ValueChangePuertoLlegadaHandler(string value)
    {
        PedidoEditar.CodigoPuertoLlegada = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorPuertoLlegada) && (Pedido.CodigoPuertoLlegada ?? "").Trim() != (PedidoEditar.CodigoPuertoLlegada ?? ""))
            Pedido.CodigoPuertoLlegada = Validator.MsgErrorPuertoLlegada = null;
    }

    private async Task OnChangePuertoLlegadaHandler(object value)
	{
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoPuertoLlegada")))
		{ 
			PedidoEditar.CodigoPuertoLlegada = string.IsNullOrEmpty(codigo) ? null : codigo;
			if ((Pedido.CodigoPuertoLlegada ?? "") != codigo)
			{
				IsChangedPuertoLlegada = true;
				if (!string.IsNullOrEmpty(codigo))
				{ 
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
			}
			else
			{
				if (!IsChangedPuertoLlegada) EditContext.MarkAsUnmodified(EditContext.Field("CodigoPuertoLlegada"));
				goto exit;
			}
		}
        Pedido.CodigoPuertoLlegada = codigo;
		Pedido.NombrePuertoLlegada = Pedido.CodigoPaisLlegada = Pedido.NombrePaisLlegada = null;
	exit:
		NotifyChange();
	}

	private void OnChangeObservacionHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged((Pedido.Observacion ?? "") != ((string)value ?? ""), EditContext, "Observacion"));

	private void OnChangeNumeroDuaHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged((Pedido.NumeroDUA ?? "") != ((string)value ?? ""), EditContext, "NumeroDUA"));

	private async Task OnChangeFechaCostoHandler(object value)
	{
        bool isChanged = Pedido.FechaCosto != (DateTime?)value;
		NotifyChange(Fnc.VerifyContextIsChanged(isChanged, EditContext, "FechaCosto"));
        if (isChanged)
        {
		    await ObtenerTipoCambioDia();
		    ActualizarMontosPorTipoCambioDia();
        }
	}

	private void OnChangeFlagCanalHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged((Pedido.FlagCanal ?? "") != ((string)value ?? ""), EditContext, "FlagCanal"));

	private void OnChangeEsVentaSucesivaHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Pedido.EsVentaSucesiva != (bool)value, EditContext, "EsVentaSucesiva"));

	public void OnChangeEsSujetoReclamoHandler(object value)
	{
		if (!(bool)value && Pedido.EsSujetoReclamo)
		{
			PedidoEditar.ComentarioReclamo = Pedido.ComentarioReclamo = null;
			EditContext.NotifyFieldChanged(EditContext.Field("ComentarioReclamo"));
		}
		NotifyChange(Fnc.VerifyContextIsChanged(Pedido.EsSujetoReclamo != (bool)value, EditContext, "EsSujetoReclamo"));
	}

	private void OnChangeComentarioReclamoHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged((Pedido.ComentarioReclamo ?? "") != ((string)value ?? ""), EditContext, "ComentarioReclamo"));

	private void OnChangeNumeroBLHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged((Pedido.NumeroBL ?? "") != ((string)value ?? ""), EditContext, "NumeroBL"));

	private void OnChangeNumeroInvoiceHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged((Pedido.NumeroInvoice ?? "") != ((string)value ?? ""), EditContext, "NumeroInvoice"));

	private void OnChangeFechaEmisionBLHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Pedido.FechaEmisionBL != (DateTime?)value, EditContext, "FechaEmisionBL"));

	private void OnChangeFechaEmisionInvoiceHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Pedido.FechaEmisionInvoice != (DateTime?)value, EditContext, "FechaEmisionInvoice"));

	private void OnChangeEsAfectoLiberaArancelHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Pedido.EsAfectoLiberaArancel != (bool)value, EditContext, "EsAfectoLiberaArancel"));

	private void OnChangeEsRequeridoInspeccionHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Pedido.EsRequeridoInspeccion != (bool)value, EditContext, "EsRequeridoInspeccion"));

	private void OnChangeEsPrevioAforoHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Pedido.EsPrevioAforo != (bool)value, EditContext, "EsPrevioAforo"));

	private void OnChangeComentarioHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged((Pedido.Comentario ?? "") != ((string)value ?? ""), EditContext, "Comentario"));
     
	private void OnChangeDescripcionEmpaqueHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged((Pedido.DescripcionEmpaque ?? "") != ((string)value ?? ""), EditContext, "DescripcionEmpaque"));

	private void OnChangeDescripcionFormaPagoHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged((Pedido.DescripcionFormaPago ?? "") != ((string)value ?? ""), EditContext, "DescripcionFormaPago"));

	private void OnChangeDescripcionMarcaHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged((Pedido.DescripcionMarca ?? "") != ((string)value ?? ""), EditContext, "DescripcionMarca"));

	private void OnChangeReferenciaHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged((Pedido.Referencia ?? "") != ((string)value ?? ""), EditContext, "Referencia"));

	private void OnChangeComentarioDocumentacionHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged((Pedido.ComentarioDocumentacion ?? "") != ((string)value ?? ""), EditContext, "ComentarioDocumentacion"));

	private void OnChangeCantidadDiasSobreestadiaHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Pedido.CantidadDiasSobreestadia != (int?)value, EditContext, "CantidadDiasSobreestadia"));

	private void OnChangeCantidadDiasAlmacenajeHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Pedido.CantidadDiasAlmacenaje != (int?)value, EditContext, "CantidadDiasAlmacenaje"));

	private void OnChangeFechaDireccionamientoHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Pedido.FechaDireccionamiento != (DateTime?)value, EditContext, "FechaDireccionamiento"));

	private void OnChangeFechaEntregaDocumentoHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Pedido.FechaEntregaDocumento != (DateTime?)value, EditContext, "FechaEntregaDocumento"));

	private void OnChangeFechaVistoBuenoHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Pedido.FechaVistoBueno != (DateTime?)value, EditContext, "FechaVistoBueno"));

	private void OnChangeDescripcionTransporteHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged((Pedido.DescripcionTransporte ?? "") != ((string)value ?? ""), EditContext, "DescripcionTransporte"));

	private void OnChangeNombreNaveHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged((Pedido.NombreNave ?? "") != ((string)value ?? ""), EditContext, "NombreNave"));

	private void OnChangeDescripcionLocalDestinoHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged((Pedido.DescripcionLocalDestino ?? "") != ((string)value ?? ""), EditContext, "DescripcionLocalDestino"));

	private void OnChangeNumeroManifiestoHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged((Pedido.NumeroManifiesto ?? "") != ((string)value ?? ""), EditContext, "NumeroManifiesto"));

	private void OnChangeFlagModalidadEmbarqueHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged((Pedido.FlagModalidadEmbarque ?? "") != ((string)value ?? ""), EditContext, "FlagModalidadEmbarque"));
     
	private void OnChangeFechaEmbarqueHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Pedido.FechaEmbarque != (DateTime?)value, EditContext, "FechaEmbarque"));

	private void OnChangeNombreTipoEmbarqueHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged((Pedido.NombreTipoEmbarque ?? "") != ((string)value ?? ""), EditContext, "NombreTipoEmbarque"));

	private void OnChangeFlagViaTransporteHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged((Pedido.FlagViaTransporte ?? "") != ((string)value ?? ""), EditContext, "FlagViaTransporte"));

	private void OnChangeFechaEstimadaArriboHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Pedido.FechaEstimadaArribo != (DateTime?)value, EditContext, "FechaEstimadaArribo"));

	private void OnChangeFechaLlegadaHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Pedido.FechaLlegada != (DateTime?)value, EditContext, "FechaLlegada"));

	private void OnChangeEsTransbordoHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Pedido.EsTransbordo != (bool)value, EditContext, "EsTransbordo"));

	private void OnChangeFechaDescargaHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Pedido.FechaDescarga != (DateTime?)value, EditContext, "FechaDescarga"));

	private void OnChangeFechaTarjaHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Pedido.FechaTarja != (DateTime?)value, EditContext, "FechaTarja"));

	private void OnChangeFechaDesgloseCargaHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Pedido.FechaDesgloseCarga != (DateTime?)value, EditContext, "FechaDesgloseCarga"));

	private void OnChangeNombreConsolidadorHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged((Pedido.NombreConsolidador ?? "") != ((string)value ?? ""), EditContext, "NombreConsolidador"));

	private void OnChangeDescripcionTramiteAduanaHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged((Pedido.DescripcionTramiteAduana ?? "") != ((string)value ?? ""), EditContext, "DescripcionTramiteAduana"));

	private void OnChangeDescripcionModalidadHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged((Pedido.DescripcionModalidad ?? "") != ((string)value ?? ""), EditContext, "DescripcionModalidad"));

	private void OnChangeFechaAforoCanalHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Pedido.FechaAforoCanal != (DateTime?)value, EditContext, "FechaAforoCanal"));

	private void OnChangeFechaSobreestadiaHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Pedido.FechaSobreestadia != (DateTime?)value, EditContext, "FechaSobreestadia"));

	private void OnChangeFechaLevanteCanalHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Pedido.FechaLevanteCanal != (DateTime?)value, EditContext, "FechaLevanteCanal"));

	private void OnChangeFechaLugarAlmacenajeHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Pedido.FechaLugarAlmacenaje != (DateTime?)value, EditContext, "FechaLugarAlmacenaje"));

	private void OnChangeFechaInspeccionHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Pedido.FechaInspeccion != (DateTime?)value, EditContext, "FechaInspeccion"));

	private void OnChangeFechaRecojoDocumentoAduanaHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Pedido.FechaRecojoDocumentoAduana != (DateTime?)value, EditContext, "FechaRecojoDocumentoAduana"));

	private void OnChangeFechaLevanteInspeccionHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Pedido.FechaLevanteInspeccion != (DateTime?)value, EditContext, "FechaLevanteInspeccion"));

	private void OnChangeNumeroPresupuestoHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged((Pedido.NumeroPresupuesto ?? "") != ((string)value ?? ""), EditContext, "NumeroPresupuesto"));

	private void OnChangeFlagTipoFinanciamientoHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged((Pedido.FlagTipoFinanciamiento ?? "") != ((string)value ?? ""), EditContext, "FlagTipoFinanciamiento"));

	private void OnChangeFechaPagoFinanciamientoHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Pedido.FechaPagoFinanciamiento != (DateTime?)value, EditContext, "FechaPagoFinanciamiento"));

	private void OnChangeCreditoAgenteAduanaHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged((Pedido.CreditoAgenteAduana ?? "") != ((string)value ?? ""), EditContext, "CreditoAgenteAduana"));

	private void OnChangeFechaPagoAgenteAduanaHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Pedido.FechaPagoAgenteAduana != (DateTime?)value, EditContext, "FechaPagoAgenteAduana"));

	private void OnChangeFechaEmisionDUAHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Pedido.FechaEmisionDUA != (DateTime?)value, EditContext, "FechaEmisionDUA"));

	private void OnChangeFechaCancelacionDUAHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Pedido.FechaCancelacionDUA != (DateTime?)value, EditContext, "FechaCancelacionDUA"));

	private void OnChangeFechaVencimientoInvoiceHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Pedido.FechaVencimientoInvoice != (DateTime?)value, EditContext, "FechaVencimientoInvoice"));

	private void OnChangeCantidadDiasVenceInvoiceHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Pedido.CantidadDiasVenceInvoice != (int?)value, EditContext, "CantidadDiasVenceInvoice"));

	private void OnChangeNumeroPackListHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged((Pedido.NumeroPackList ?? "") != ((string)value ?? ""), EditContext, "NumeroPackList"));

	private void OnChangeNumeroPolizaHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged((Pedido.NumeroPoliza ?? "") != ((string)value ?? ""), EditContext, "NumeroPoliza"));

	private void OnChangeFechaPackListHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Pedido.FechaPackList != (DateTime?)value, EditContext, "FechaPackList"));

	private void OnChangeFechaEmisionPolizaHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Pedido.FechaEmisionPoliza != (DateTime?)value, EditContext, "FechaEmisionPoliza"));
	#endregion

	public void Dispose() => GC.SuppressFinalize(this);
}
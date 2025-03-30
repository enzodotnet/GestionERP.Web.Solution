using AutoMapper;
using Blazored.FluentValidation;
using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Compra;
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

namespace GestionERP.Web.Pages.Empresa.Compra.Orden;

public partial class Edit : IDisposable
{
    #region Propiedades
    private const string codigoModulo = "01";
    private const string codigoServicio = "S103";
    private const string rutaAccion = "/editar";
    private const string rutaServicio = "/ordenes";
    private string rutaEmpresa = "";
    public FluentValidationValidator validator;

    public OrdenObtenerDto Orden { get; set; }
    public OrdenEditarDto OrdenEditar { get; set; }
	public List<OrdenDetalleGrid> GridDetalles { get; set; }
	public OrdenDetalleGrid ItemGridDetalle { get; set; }
	public OrdenDetalleObtenerDto Detalle { get; set; }
    public OrdenDetalleEditarDto DetalleEditar { get; set; }
    public OrdenDetalleInsertarDto DetalleInsertar { get; set; }
    private EditContext EditContext { get; set; }
    private EditContext DetalleInsertarContext { get; set; }
    private EditContext DetalleEditarContext { get; set; }
    public OrdenDetalleGridValidator GridDetalleValidator { get; set; }
    public OrdenEditarValidator Validator { get; set; }
    public OrdenDetalleEditarValidator DetalleEditarValidator { get; set; }
    public OrdenDetalleInsertarValidator DetalleInsertarValidator { get; set; }
    private SerieDocumentoConsultaPorCodigoEmpresaDto Numerador { get; set; }
    public bool IsEditingGridDetalle { get; set; }
    private TipoImpuestoConsultaEsPredeterminadoDto TipoImpuestoPredeterminado { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool IsAuthUser { get; set; }
    private bool IsModified { get; set; }
    private bool IsModifiedDetalle { get; set; } 
    public bool EsVisibleCatalogoSolicitudes { get; set; }
    public TelerikNotification AlertDetalle { get; set; }
    public TelerikNotification Alert { get; set; }
    public TelerikNotification AlertCatalogoSolicitudes { get; set; }
    public string VerboAccionModal { get; set; }
    public string DescripcionAccionModal { get; set; }
    public string TipoAccionModal { get; set; } 
    public string TipoAccionDialog { get; set; }
    public string OrigenDialog { get; set; }
    public string OrigenModal { get; set; }
    public bool EsVisibleDialogDetalle { get; set; }
    public bool EsVisibleModalDetalle { get; set; }
    private bool IsChangedEntidad { get; set; }
	private bool IsChangedLocal { get; set; }
    private bool IsChangedCentroCosto { get; set; }
	private bool IsChangedTipoProvision { get; set; }
	private bool IsChangedTipoImpuesto { get; set; }
	private bool IsLoadingCatalogoSolicitudes { get; set; }
    public ISvgIcon IconoAccionModal { get; set; }
    private TelerikGrid<OrdenDetalleGrid> GridDetalleRef { get; set; }
    private TelerikGrid<SolicitudCatalogoAtenderDto> GridCatalogoRef { get; set; }
    public GridSelectionMode SelectionModeCatalogoSolicitud { get; set; }
    private bool EsSeleccionableCatalogoSolicitud { get; set; }
    private IEnumerable<OrdenFlag> Origenes { get; set; }
    private IEnumerable<OrdenFlag> MediosPago { get; set; }
    public List<SolicitudCatalogoAtenderDto> CatalogoSolicitudes { get; set; }
    public IEnumerable<SolicitudCatalogoAtenderDto> ItemsSelectedSolicitud { get; set; }
    public string CodigoEjercicio { get; set; }
    private IEnumerable<EmpresaEjercicioCatalogoDto> CatalogoEjercicios { get; set; }
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    [Parameter] public Guid? Id { get; set; }
    public EmpresaConsultaPorCodigoWebDto Empresa { get; set; }
    private bool EsVisibleVolver { get; set; }
    private bool IsInitPage { get; set; }
    [Parameter] public string CodigoWebEmpresa { get; set; }
    [Parameter][SupplyParameterFromQuery(Name = "returnpage")] public string ReturnPage { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    #endregion

    [Inject] public ICompraOrden IOrden { get; set; }
    [Inject] public ICompraSolicitud ISolicitud { get; set; }
    [Inject] public IPrincipalEmpresa IEmpresa { get; set; } 
    [Inject] public IPrincipalSerieDocumento ISerieDocumento { get; set; }
    [Inject] public IPrincipalTipoImpuesto ITipoImpuesto { get; set; }
	[Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public UtilService IUtil { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }
    [Inject] public IMapper IMapper { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Notify.ShowLoading(mensaje: "Obteniendo registro");

            OrdenEditar = new(); 

            Detalle = new();
            DetalleInsertar = new();
            DetalleEditar = new();
			GridDetalles = [];
            ItemGridDetalle = new();

			Origenes = OrdenFlag.Origenes();
            MediosPago = OrdenFlag.MediosPago();

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio);
            if (!IsAuthUser) return;

            Empresa = await IEmpresa.ConsultaPorCodigoWeb(CodigoWebEmpresa);
            
            EsVisibleVolver = !string.IsNullOrEmpty(ReturnPage) && ReturnPage is "index" or "view";
            rutaEmpresa = INavigation.Uri.Replace(INavigation.BaseUri, "").Split("?")[0].Replace(rutaServicio, "").Replace($"/{Id}", "").Replace(rutaAccion, "");

			if (!await IPermiso.ConsultaEsAsignadoPorSesion(OrdenAcceso.Editar, Empresa.Codigo))
			{
				INavigation.NavigateTo($"{rutaEmpresa}/inicio");
				Notify.Show("No tiene permiso para editar registros de [Órdenes de Compra]", "error");
				return;
			}

			Orden = await IOrden.Obtener(Empresa.Codigo, (Guid) Id);
            if (Orden is null || Orden.CodigoEstado is not "ED")
            {
                INavigation.NavigateTo($"{rutaEmpresa}{rutaServicio}");
                Notify.Show($"El registro de la [Orden de Compra] consultado a editar no está disponible", "error");
                return;
            }

            OrdenEditar = IMapper.Map<OrdenEditarDto>(Orden);
			Validator = new()
			{
				EsAfectoImpuesto = Orden.EsAfectoImpuesto,
                FechaEmision = Orden.FechaEmision
            };
			EditContext = new EditContext(OrdenEditar);

			GridDetalles = IMapper.Map<List<OrdenDetalleGrid>>(Orden.Detalles);

            Numerador = await ISerieDocumento.ConsultaPorCodigoEmpresa(Orden.CodigoSerieDocumento, Orden.CodigoDocumento, Empresa.Codigo) ?? new();
 
            TipoImpuestoPredeterminado = await ITipoImpuesto.ConsultaEsPredeterminado() ?? new();
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

            await IOrden.Editar(Empresa.Codigo, (Guid) Id, OrdenEditar);

            IsModified = false;
            
            Notify.Show($"La orden de compra {Orden.Codigo} ha sido editada con éxito", "success");
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
                if (tipoAccion is "I" or "M")
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
        if (string.IsNullOrEmpty(OrdenEditar.CodigoLocalRecepcion) && string.IsNullOrEmpty(OrdenEditar.DescripcionLugarEntrega))
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
            else if (GridDetalles.Any(x => !x.PrecioUnitario.HasValue))
            {
                Fnc.MostrarAlerta(Alert, "Está pendiente completar alguna celda del [precio unitario] del detalle de la orden de compra", "error");
                esValido = false;
            }    
        }

        return esValido;
    }

    private void NotifyChange(bool? isFieldModified = null)
    {
        IsModified = isFieldModified ?? EditContext.IsModified();
        if (!IsModified)
            IsModified = OrdenEditar.DetallesInsertar.Count > 0 || OrdenEditar.DetallesEditar.Count > 0 || OrdenEditar.DetallesEliminar.Count > 0;
    }

    #region CatalogoSolicitudes
    public async Task MostrarCatalogoSolicitudes()
    {
        IsLoadingCatalogoSolicitudes = true;

        CatalogoEjercicios = await IEmpresa.CatalogoEjercicios(Empresa.Codigo) ?? [];
        CodigoEjercicio = await IEmpresa.ConsultaEjercicioCodigoPorAnio(Empresa.Codigo, DateTime.Now.Year);
        ItemsSelectedSolicitud = [];

        await CargarCatalogoSolicitudes();
        EsVisibleCatalogoSolicitudes = true;
    }

    private async Task VisibleCatalogoSolicitudesChangedHandler(bool esVisible)
    {
        if (!esVisible)
        {
            if (ItemsSelectedSolicitud.Any() && !CatalogoSolicitudes.Any(x => x.IsErrorSelected) && !await Dialog.ConfirmAsync("¿Está seguro de salir del catálogo y que los ítems seleccionados no se carguen?", "Saliendo de catálogo"))
                return;

            EsVisibleCatalogoSolicitudes = false;
            CatalogoSolicitudes = null;
        }
    }

    private async Task OnComboEjercicioValueChanged(string value)
    {
        CodigoEjercicio = value;
        if (!string.IsNullOrEmpty(CodigoEjercicio))
            await CargarCatalogoSolicitudes();
    }

    private async Task CargarCatalogoSolicitudes()
    {
        try
        {
            IsAuthUser = (await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio, User.FindFirst("code").Value)).esValido;
            if (!IsAuthUser) return;

            ItemsSelectedSolicitud = [];

            CatalogoSolicitudes = (List<SolicitudCatalogoAtenderDto>)await ISolicitud.CatalogoAtender(Empresa.Codigo, CodigoEjercicio, Numerador.CodigoProcesoDocumento, Numerador.CodigoLocal);

            SelectionModeCatalogoSolicitud = CatalogoSolicitudes is null ? GridSelectionMode.None : GridSelectionMode.Multiple;
            EsSeleccionableCatalogoSolicitud = CatalogoSolicitudes is not null;
            GridCatalogoRef?.Rebind();
        }
        catch (HttpRequestException)
        { 
            if (EsVisibleCatalogoSolicitudes)
                Fnc.MostrarAlerta(AlertCatalogoSolicitudes, Cnf.MsgErrorNotConnectApi, "error");
            else
                Notify.ShowError("NC");
        }
        catch (HttpResponseException ex)
        {
            if (EsVisibleCatalogoSolicitudes)
                Fnc.MostrarAlerta(AlertCatalogoSolicitudes, $"{(ex.Code == "NF" ? Cnf.MsgErrorNotFoundAPi : ex.Message)}", "error");
            else
                Notify.ShowError(ex.Code, ex);
        }
        catch (Exception ex)
        {
            if (EsVisibleCatalogoSolicitudes)
                Fnc.MostrarAlerta(AlertCatalogoSolicitudes, Cnf.MsgErrorFuncAppWeb, "error");
            else
                Notify.ShowError("FA", ex); 
        }
        finally
        {
            IsLoadingCatalogoSolicitudes = false;
            Notify.ShowLoading(false);
        }
    }

    public static void OnRowCatalogoSolicitudRenderHandler(GridRowRenderEventArgs args) => args.Class = (args.Item as SolicitudCatalogoAtenderDto).IsErrorSelected ? "row-error" : "";

    protected void OnSelectCatalogoSolicitud(IEnumerable<SolicitudCatalogoAtenderDto> itemsSeleccionado)
    {
        ItemsSelectedSolicitud = itemsSeleccionado;
        CatalogoSolicitudes?.ForEach(x => { x.IsErrorSelected = false; });
    }

    private async Task AgregarItemsSolicitud()
    {
        if (!ItemsSelectedSolicitud.Any())
        {
            Fnc.MostrarAlerta(AlertCatalogoSolicitudes, "Es necesario que seleccione al menos un ítem del catálogo", "warning");
            return;
        }

		bool isErrorSelectedCatalogo = false;
		foreach (SolicitudCatalogoAtenderDto item in ItemsSelectedSolicitud)
		{
			if (GridDetalles.Any(x => x.CodigoArticulo == item.CodigoArticulo))
			{
				Fnc.MostrarAlerta(AlertCatalogoSolicitudes, $"El ítem {item.CodigoArticulo} ya ha sido agregado en el detalle", "error");
				item.IsErrorSelected = isErrorSelectedCatalogo = true;
			}
		}
		List<IGrouping<string, SolicitudCatalogoAtenderDto>> elements = [.. ItemsSelectedSolicitud.GroupBy(x => x.CodigoArticulo).Where(x => x.Count() > 1)];
		if (elements.Count > 0)
		{
			Fnc.MostrarAlerta(AlertCatalogoSolicitudes, "Existen ítems seleccionados de igual código", "error");
			elements.ForEach(x => x.ToList().ForEach(x => x.IsErrorSelected = true));
			isErrorSelectedCatalogo = true;
		}

		if (isErrorSelectedCatalogo)
			return;

		GridState<OrdenDetalleGrid> detalleState = GridDetalleRef.GetState();
		foreach (SolicitudCatalogoAtenderDto itemCatalogo in ItemsSelectedSolicitud)
        {
			DetalleInsertar = new();
			Detalle = new();

			IMapper.Map(itemCatalogo, Detalle);
			IMapper.Map(Detalle, DetalleInsertar);

			ItemGridDetalle = IMapper.Map<OrdenDetalleGrid>(Detalle); 

			OrdenEditar.DetallesInsertar.Add(DetalleInsertar);
			GridDetalles.Add(ItemGridDetalle);
			detalleState.InsertedItem = ItemGridDetalle;

			await GridDetalleRef.SetStateAsync(detalleState);
		}

		EsVisibleCatalogoSolicitudes = false;
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

    public static void OnRowDetalleRenderHandler(GridRowRenderEventArgs args) => args.Class = !(args.Item as OrdenDetalleGrid).Id.HasValue ? "new-row" : "";
    
    public void MostrarAgregarDetalle()
    {
        DetalleInsertar = new();
        Detalle = new();
        DetalleInsertarValidator = new();
        IniciarAccionModal("I", "detalle");
    }

	public void MostrarModificarDetalle(OrdenDetalleGrid item = null)
	{
		if (TipoAccionModal is not "V")
			Detalle = IMapper.Map<OrdenDetalleObtenerDto>(item);

		string tipoAccion = !Detalle.Id.HasValue ? "M" : "E"; 
		if (tipoAccion is "M")
        {
			DetalleInsertar = IMapper.Map<OrdenDetalleInsertarDto>(Detalle);
            DetalleInsertarValidator = new()
            {
                UnidadConversion = Detalle.UnidadConversion
            };
        }
		else
        {
			DetalleEditar = IMapper.Map<OrdenDetalleEditarDto>(Detalle);
            DetalleEditarValidator = new()
            {
                UnidadConversion = Detalle.UnidadConversion
            };
        }
		IniciarAccionModal(tipoAccion, "detalle");
	}

	private void MostrarQuitarDetalle(OrdenDetalleGrid item = null)
	{
		if (TipoAccionModal is not "V")
			Detalle = IMapper.Map<OrdenDetalleObtenerDto>(item);

		IniciarAccionDialog(!Detalle.Id.HasValue ? "Q" : "X", "detalle");
	}

	public void VerItemDetalle(OrdenDetalleGrid item)
	{
		Detalle = IMapper.Map<OrdenDetalleObtenerDto>(item);
		IniciarAccionModal("V", "detalle");
	}

    private void OnChangeDetalleCantidadHandler(object value)
    {
        decimal? cantidad = (decimal?)value;
        IsModifiedDetalle = Fnc.VerifyContextIsChanged(Detalle.Cantidad != cantidad, TipoAccionModal is "E" ? DetalleEditarContext : DetalleInsertarContext, "Cantidad");
        ModificarImportesItem(cantidad, TipoAccionModal is "E" ? DetalleEditar.PrecioUnitario : DetalleInsertar.PrecioUnitario, TipoAccionModal is "E" ? Detalle.EsAfectoImpuesto : DetalleInsertar.EsAfectoImpuesto);
    }

    private void OnChangeDetallePrecioUnitarioHandler(object value)
    {
        decimal? precioUnitario = (decimal?)value;
        IsModifiedDetalle = Fnc.VerifyContextIsChanged(Detalle.PrecioUnitario != precioUnitario, TipoAccionModal is "E" ? DetalleEditarContext : DetalleInsertarContext, "PrecioUnitario");
        ModificarImportesItem(TipoAccionModal is "E" ? DetalleEditar.Cantidad : DetalleInsertar.Cantidad, precioUnitario, TipoAccionModal is "E" ? Detalle.EsAfectoImpuesto : DetalleInsertar.EsAfectoImpuesto);
    }

    public static void OnCellCantidadRenderHandler(GridCellRenderEventArgs args) => args.Class = !(args.Item as OrdenDetalleGrid).Cantidad.HasValue ? "cell-error" : "cell-editable";

    public static void OnCellPrecioUnitarioRenderHandler(GridCellRenderEventArgs args) => args.Class = !(args.Item as OrdenDetalleGrid).PrecioUnitario.HasValue ? "cell-error" : "cell-editable";

    public void EditDetalleHandler(GridCommandEventArgs args)
    {
        IsEditingGridDetalle = args.Field is "Cantidad" or "PrecioUnitario";
        if (IsEditingGridDetalle)
        { 
            GridDetalleValidator = new()
            {
                UnidadConversion = (args.Item as OrdenDetalleGrid).UnidadConversion
            };
        }
    }

    public void CancelDetalleHandler(GridCommandEventArgs args) => IsEditingGridDetalle = !(args.Field is "Cantidad" or "PrecioUnitario");

    public void UpdateItemDetalleHandler(GridCommandEventArgs args)
    {
		OrdenDetalleGrid item = (OrdenDetalleGrid) args.Item;
        int indexGrid = GridDetalles.FindIndex(i => i.CodigoArticulo == item.CodigoArticulo);
        bool esItemNuevo = !item.Id.HasValue;
        int indexEdit = !esItemNuevo ? OrdenEditar.DetallesEditar.FindIndex(i => i.Id == item.Id) : OrdenEditar.DetallesInsertar.FindIndex(i => i.CodigoArticulo == item.CodigoArticulo);

        if (args.Field == "PrecioUnitario")
        {
            if (!esItemNuevo && indexEdit > -1)
                OrdenEditar.DetallesEditar[indexEdit].PrecioUnitario = item.PrecioUnitario;
            else if (esItemNuevo)
                OrdenEditar.DetallesInsertar[indexEdit].PrecioUnitario = item.PrecioUnitario; 
            GridDetalles[indexGrid].PrecioUnitario = item.PrecioUnitario;
        }
        else if (args.Field == "Cantidad")
        {
            if (!esItemNuevo && indexEdit > -1)
                OrdenEditar.DetallesEditar[indexEdit].Cantidad = item.Cantidad;
            else if (esItemNuevo)
                OrdenEditar.DetallesInsertar[indexEdit].Cantidad = item.Cantidad; 
            GridDetalles[indexGrid].Cantidad = item.Cantidad;
        }

		ModificarImportesItem(item.Cantidad, item.PrecioUnitario, item.EsAfectoImpuesto, indexGrid, indexEdit);
		ModificarImportesTotales();

		if (!esItemNuevo && indexEdit == -1)
        {
            OrdenEditar.DetallesEditar.Add(IMapper.Map(IMapper.Map<OrdenDetalleObtenerDto>(GridDetalles[indexGrid]), new OrdenDetalleEditarDto()));
            NotifyChange();
        } 
        IsEditingGridDetalle = false;
    }

    private void ModificarImportesItem(decimal? cantidad, decimal? precioUnitario, bool esAfectoImpuesto, int indexGrid = -1, int indexEdit = -1)
    {
        decimal? importeBruto = null, importeImpuesto = null, importeNeto = null;
        if (cantidad.HasValue && precioUnitario.HasValue)
        {
            importeBruto = Math.Round((decimal)(cantidad * precioUnitario), 2, MidpointRounding.AwayFromZero);
            importeImpuesto = Orden.EsAfectoImpuesto ? Math.Round((decimal)(importeBruto * (esAfectoImpuesto ? OrdenEditar.PorcentajeImpuesto : 0)), 2, MidpointRounding.AwayFromZero) : 0;
            importeNeto = importeBruto + importeImpuesto;            
        }

        if (indexGrid > -1)
        {
			GridDetalles[indexGrid].ImporteBruto = (decimal)importeBruto;
			GridDetalles[indexGrid].ImporteImpuesto = (decimal)importeImpuesto;
			GridDetalles[indexGrid].ImporteNeto = (decimal)importeNeto;
            if (indexEdit > -1)
            {
				if (GridDetalles[indexGrid].Id.HasValue)
				{
					OrdenEditar.DetallesEditar[indexEdit].ImporteBruto = importeBruto;
					OrdenEditar.DetallesEditar[indexEdit].ImporteImpuesto = importeImpuesto;
					OrdenEditar.DetallesEditar[indexEdit].ImporteNeto = importeNeto;
				}
				else
				{
					OrdenEditar.DetallesInsertar[indexEdit].ImporteBruto = importeBruto;
					OrdenEditar.DetallesInsertar[indexEdit].ImporteImpuesto = importeImpuesto;
					OrdenEditar.DetallesInsertar[indexEdit].ImporteNeto = importeNeto;
				} 
			} 
        }
        else
        {
            if (TipoAccionModal is "E")
            {
                DetalleEditar.ImporteBruto = importeBruto;
                DetalleEditar.ImporteImpuesto = importeImpuesto;
                DetalleEditar.ImporteNeto = importeNeto;
            }
            else
            {
                DetalleInsertar.ImporteBruto = importeBruto;
                DetalleInsertar.ImporteImpuesto = importeImpuesto;
                DetalleInsertar.ImporteNeto = importeNeto;
            }
        }
    }

    private void RecalcularImportes()
    {
        if (GridDetalles.Count > 0)
        {
            int indexEdit;
            bool esItemNuevo;
            foreach ((OrdenDetalleGrid item, int indexGrid) in GridDetalles.Select((item, index) => (item, index)))
            {
				esItemNuevo = !item.Id.HasValue;
				indexEdit = !esItemNuevo ? OrdenEditar.DetallesEditar.FindIndex(i => i.Id == item.Id) : OrdenEditar.DetallesInsertar.FindIndex(i => i.CodigoArticulo == item.CodigoArticulo);
                ModificarImportesItem(item.Cantidad, item.PrecioUnitario, item.EsAfectoImpuesto, indexGrid, indexEdit); 
				if (!esItemNuevo && indexEdit == -1)
					OrdenEditar.DetallesEditar.Add(IMapper.Map(IMapper.Map<OrdenDetalleObtenerDto>(GridDetalles[indexGrid]), new OrdenDetalleEditarDto()));
            }
            ModificarImportesTotales();
        }
    }

    private void ModificarImportesTotales()
    {
        bool esGridVacio = GridDetalles.Count == 0;
        OrdenEditar.TotalImporteBruto = esGridVacio ? 0 : GridDetalles.Sum(x => x.ImporteBruto);
        OrdenEditar.TotalImporteImpuesto = esGridVacio ? 0 : GridDetalles.Sum(x => x.ImporteImpuesto);
        OrdenEditar.TotalImporteNeto = esGridVacio ? 0 : GridDetalles.Sum(x => x.ImporteNeto);
    }

    private void OnChangeDetalleObservacionHandler(object value) => IsModifiedDetalle = Fnc.VerifyContextIsChanged((Detalle.Observacion ?? "") != ((string)value ?? ""), TipoAccionModal is "E" ? DetalleEditarContext : DetalleInsertarContext, "Observacion");

    private void ValueChangeArticuloHandler(string value)
    {
        DetalleInsertar.CodigoArticulo = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(DetalleInsertarValidator.MsgErrorArticulo) && (Detalle.CodigoArticulo ?? "").Trim() != (DetalleInsertar.CodigoArticulo ?? ""))
	        Detalle.CodigoArticulo = DetalleInsertarValidator.MsgErrorArticulo = null;
    }

    private async Task OnChangeArticuloHandler(object value)
    {
		string codigo = (string)(value ?? "");
		if (DetalleInsertarContext.IsValid(DetalleInsertarContext.Field("CodigoArticulo")))
		{
			if ((Detalle.CodigoArticulo ?? "") == codigo) goto exit;
            if (GridDetalles.Any(x => x.CodigoArticulo.Trim() == codigo))
            {
                DetalleInsertarValidator.MsgErrorArticulo = "El código ya existe en el detalle del registro";
            }
            else
            {  
                (ArticuloObtenerPorCodigoEmpresaProcesoDocumentoDto item, DetalleInsertarValidator.MsgErrorArticulo) = await IUtil.ObtenerArticuloPorCodigoEmpresaProcesoDocumento(AlertDetalle, codigo, Empresa.Codigo, Numerador.CodigoProcesoDocumento);
			    if (item is not null)
			    {
			        Detalle.CodigoArticulo = item.CodigoArticulo.Trim();
			        Detalle.NombreArticulo = item.NombreArticulo;
			        Detalle.UnidadConversion = DetalleInsertarValidator.UnidadConversion = item.UnidadConversion; 
			        Detalle.CodigoUnidadMedida = item.CodigoUnidadMedida;
			        Detalle.NombreUnidadMedida = item.NombreUnidadMedida;
			        Detalle.Presentacion = item.Presentacion;
			        Detalle.CodigoTipoArticulo = item.CodigoTipoArticulo;
			        Detalle.NombreTipoArticulo = item.NombreTipoArticulo;
			        if (DetalleInsertar.Cantidad.HasValue)
				        DetalleInsertarContext.NotifyFieldChanged(DetalleInsertarContext.Field("Cantidad"));
                    if (Detalle.EsAfectoImpuesto != item.EsAfectoImpuesto)
                    {
                        DetalleInsertar.EsAfectoImpuesto = Detalle.EsAfectoImpuesto = item.EsAfectoImpuesto;
					    if (DetalleInsertar.Cantidad.HasValue && DetalleInsertarContext.IsValid(DetalleInsertarContext.Field("Cantidad")) && DetalleInsertar.PrecioUnitario.HasValue)
						    ModificarImportesItem(DetalleInsertar.Cantidad, DetalleInsertar.PrecioUnitario, DetalleInsertar.EsAfectoImpuesto);
                    }
				    goto exit;
			    }
            } 
            DetalleInsertarContext.NotifyFieldChanged(DetalleInsertarContext.Field("CodigoArticulo"));
		}
        Detalle.CodigoArticulo = codigo;
		Detalle.NombreArticulo = Detalle.CodigoUnidadMedida = Detalle.NombreUnidadMedida = Detalle.CodigoTipoArticulo = Detalle.NombreTipoArticulo = Detalle.Presentacion = null;
		Detalle.UnidadConversion = DetalleInsertarValidator.UnidadConversion = null;
		DetalleInsertar.EsAfectoImpuesto = Detalle.EsAfectoImpuesto = false; 
		if (string.IsNullOrEmpty(codigo)) 
            DetalleInsertarContext.MarkAsUnmodified(DetalleInsertarContext.Field("CodigoArticulo"));
		if (DetalleInsertar.Cantidad.HasValue)
			DetalleInsertarContext.NotifyFieldChanged(DetalleInsertarContext.Field("Cantidad"));
	exit:
		IsModifiedDetalle = DetalleInsertarContext.IsModified();
    } 

    private async Task AccionarDetalle(string tipoAccion)
    {
        GridState<OrdenDetalleGrid> detalleState = GridDetalleRef.GetState();
        int indexGrid = GridDetalles.FindIndex(i => i.CodigoArticulo == Detalle.CodigoArticulo);
        int indexEdit = tipoAccion is "E" or "X" ? OrdenEditar.DetallesEditar.FindIndex(i => i.Id == Detalle.Id) : OrdenEditar.DetallesInsertar.FindIndex(i => i.CodigoArticulo == Detalle.CodigoArticulo);
        
        switch (tipoAccion)
        {
            case "I" or "M":
				ItemGridDetalle = IMapper.Map<OrdenDetalleGrid>(IMapper.Map(DetalleInsertar, Detalle));
				if (tipoAccion == "I")
                {
                    OrdenEditar.DetallesInsertar.Add(DetalleInsertar);
                    GridDetalles.Add(ItemGridDetalle);
                    detalleState.InsertedItem = ItemGridDetalle;
                }
                else
                {
                    OrdenEditar.DetallesInsertar[indexEdit] = DetalleInsertar;
                    GridDetalles[indexGrid] = ItemGridDetalle; 
                } 
                break;
            case "E":
                if (indexEdit > -1) 
                    OrdenEditar.DetallesEditar[indexEdit] = DetalleEditar; 
                else 
                    OrdenEditar.DetallesEditar.Add(DetalleEditar);
                GridDetalles[indexGrid] = IMapper.Map<OrdenDetalleGrid>(IMapper.Map(DetalleEditar, Detalle));
                break;
            case "Q" or "X":
                if (tipoAccion == "Q")
                    OrdenEditar.DetallesInsertar.RemoveAt(indexEdit);
                else
                {
                    OrdenEditar.DetallesEliminar.Add(new() { Id = (Guid) Detalle.Id });
                    if (indexEdit > -1) 
                        OrdenEditar.DetallesEditar.RemoveAt(indexEdit); 
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
    private void CargarItemCatalogoArticuloPorProcesoDocumento(ArticuloCatalogoPorEmpresaProcesoDocumentoDto item)
    { 
        DetalleInsertar.CodigoArticulo = Detalle.CodigoArticulo = item.CodigoArticulo.Trim();
        Detalle.NombreArticulo = item.NombreArticulo; 
        Detalle.UnidadConversion = DetalleInsertarValidator.UnidadConversion = item.UnidadConversion; 
        Detalle.CodigoUnidadMedida = item.CodigoUnidadMedida;
        Detalle.NombreUnidadMedida = item.NombreUnidadMedida;
        Detalle.Presentacion = item.Presentacion;
        Detalle.CodigoTipoArticulo = item.CodigoTipoArticulo;
        Detalle.NombreTipoArticulo = item.NombreTipoArticulo;
        DetalleInsertar.EsAfectoImpuesto = item.EsAfectoImpuesto;
        DetalleInsertarValidator.MsgErrorArticulo = null;
        DetalleInsertarContext.NotifyFieldChanged(DetalleInsertarContext.Field("CodigoArticulo"));
        IsModifiedDetalle = DetalleInsertarContext.IsModified();
        if (DetalleInsertar.Cantidad.HasValue)
            DetalleInsertarContext.NotifyFieldChanged(DetalleInsertarContext.Field("Cantidad"));
        ModificarImportesItem(DetalleInsertar.Cantidad, DetalleInsertar.PrecioUnitario, DetalleInsertar.EsAfectoImpuesto);
    } 

    private void CargarItemCatalogoProveedor(EntidadProveedorCatalogoPorEmpresaDto item)
    {
        OrdenEditar.CodigoEntidad = Orden.CodigoEntidad = item.CodigoEntidad.Trim();
        Orden.NombreEntidad = item.NombreEntidad;
        OrdenEditar.CodigoModoPago = item.CodigoModoPago;
        Orden.NombreModoPago = item.NombreModoPago;
        OrdenEditar.CodigoPlazoCredito = item.CodigoPlazoCredito;
        Orden.NombrePlazoCredito = item.NombrePlazoCredito;
        Validator.MsgErrorEntidad = null;
        IsChangedEntidad = true;
		EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidad"));
		NotifyChange();
    }

    private void CargarItemCatalogoLocal(LocalCatalogoPorEmpresaDto item)
    {
        OrdenEditar.CodigoLocalRecepcion = Orden.CodigoLocalRecepcion = item.CodigoLocal;
        Orden.NombreLocalRecepcion = item.NombreLocal;
        IsChangedLocal = true;
        Validator.MsgErrorLocalRecepcion = null; 
		EditContext.NotifyFieldChanged(EditContext.Field("CodigoLocalRecepcion"));
		NotifyChange();
	}

    private void CargarItemCatalogoTipoProvision(TipoProvisionCatalogoDto item)
    {
        OrdenEditar.CodigoTipoProvision = Orden.CodigoTipoProvision = item.CodigoTipoProvision;
        Orden.NombreTipoProvision = item.NombreTipoProvision; 
        Orden.CodigoMoneda = item.CodigoMoneda;
        Orden.NombreMoneda = item.NombreMoneda;
        Orden.SimboloMoneda = item.SimboloMoneda;
        if (Orden.EsAfectoImpuesto != item.EsAfectoImpuesto)
        {
            Orden.EsAfectoImpuesto = Validator.EsAfectoImpuesto = item.EsAfectoImpuesto;
            if (Orden.EsAfectoImpuesto && string.IsNullOrEmpty(OrdenEditar.CodigoTipoImpuesto))
            {
                OrdenEditar.CodigoTipoImpuesto = TipoImpuestoPredeterminado.Codigo;
                Orden.NombreTipoImpuesto = TipoImpuestoPredeterminado.Nombre;
                OrdenEditar.PorcentajeImpuesto = TipoImpuestoPredeterminado.Porcentaje;
            }
            else if (!Orden.EsAfectoImpuesto && !string.IsNullOrEmpty(OrdenEditar.CodigoTipoImpuesto))
            {
                OrdenEditar.CodigoTipoImpuesto = Orden.NombreTipoImpuesto = Validator.MsgErrorTipoImpuesto = null;
                OrdenEditar.PorcentajeImpuesto = null;
            }
            EditContext.NotifyFieldChanged(EditContext.Field("CodigoTipoImpuesto"));
            RecalcularImportes();
        }
        Validator.MsgErrorTipoProvision = null;
        IsChangedTipoProvision = true;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoTipoProvision"));
        NotifyChange();
    }

    private void CargarItemCatalogoCentroCosto(CentroCostoCatalogoDto item)
    {
        OrdenEditar.CodigoCentroCosto = Orden.CodigoCentroCosto = item.CodigoCentroCosto.Trim();
        Orden.NombreCentroCosto = item.NombreCentroCosto;
        IsChangedCentroCosto = true;
        Validator.MsgErrorCentroCosto = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoCentroCosto"));
        NotifyChange();
    }

    private void CargarItemCatalogoTipoImpuesto(TipoImpuestoCatalogoDto item)
    {
        OrdenEditar.CodigoTipoImpuesto = Orden.CodigoTipoImpuesto = item.CodigoTipoImpuesto;
        Orden.NombreTipoImpuesto = item.NombreTipoImpuesto;
        if ((Orden.PorcentajeImpuesto ?? 0) != item.PorcentajeImpuesto)
        {
		    OrdenEditar.PorcentajeImpuesto = Orden.PorcentajeImpuesto = item.PorcentajeImpuesto;
		    RecalcularImportes();
        } 
        IsChangedTipoImpuesto = true;
        Validator.MsgErrorTipoImpuesto = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoTipoImpuesto"));
        NotifyChange();
    }
    #endregion

    #region EditContextHandlers
    private void ValueChangeEntidadHandler(string value)
    {
        OrdenEditar.CodigoEntidad = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorEntidad) && (Orden.CodigoEntidad ?? "").Trim() != (OrdenEditar.CodigoEntidad ?? ""))
            Orden.CodigoEntidad = Validator.MsgErrorEntidad = null;
    }

    private async Task OnChangeEntidadHandler(object value)
    {
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoEntidad")))
		{ 
			if ((Orden.CodigoEntidad ?? "").Trim() != codigo)
			{
                IsChangedEntidad = true;
				(EntidadProveedorObtenerPorCodigoEmpresaDto item, Validator.MsgErrorEntidad) = await IUtil.ObtenerProveedorPorCodigoEmpresa(Alert, codigo, Empresa.Codigo, "L");
                if (item is not null) 
                { 
                    Orden.CodigoEntidad = item.CodigoEntidad.Trim();
				    Orden.NombreEntidad = item.NombreEntidad;
				    OrdenEditar.CodigoModoPago = item.CodigoModoPago;
				    Orden.NombreModoPago = item.NombreModoPago;
                    OrdenEditar.CodigoPlazoCredito = item.CodigoPlazoCredito;
				    Orden.NombrePlazoCredito = item.NombrePlazoCredito;
                    goto exit;
                }
                EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidad"));
			}
            else
            {
                if (!IsChangedEntidad) EditContext.MarkAsUnmodified(EditContext.Field("CodigoEntidad"));
                goto exit;
            }
		}
        Orden.CodigoEntidad = codigo;
		Orden.NombreEntidad = OrdenEditar.CodigoModoPago = Orden.NombreModoPago = OrdenEditar.CodigoPlazoCredito = Orden.NombrePlazoCredito = null;
	exit:
        NotifyChange();
	}

    private void ValueChangeTipoProvisionHandler(string value)
    {
        OrdenEditar.CodigoTipoProvision = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorTipoProvision) && (Orden.CodigoTipoProvision ?? "").Trim() != (OrdenEditar.CodigoTipoProvision ?? ""))
            Orden.CodigoTipoProvision = Validator.MsgErrorTipoProvision = null;
    }

    private async Task OnChangeTipoProvisionHandler(object value)
    { 
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoTipoProvision")))
		{ 
			if ((Orden.CodigoTipoProvision ?? "") != codigo)
			{
                IsChangedTipoProvision = true;
				(TipoProvisionObtenerPorCodigoDto item, Validator.MsgErrorTipoProvision) = await IUtil.ObtenerTipoProvisionPorCodigo(Alert, codigo);
				if (item is not null)
				{
                    Orden.CodigoTipoProvision = item.CodigoTipoProvision;
					Orden.NombreTipoProvision = item.NombreTipoProvision; 
					Orden.CodigoMoneda = item.CodigoMoneda;
					Orden.NombreMoneda = item.NombreMoneda;
					Orden.SimboloMoneda = item.SimboloMoneda; 
                    if (Orden.EsAfectoImpuesto != item.EsAfectoImpuesto)
                    {
                        Orden.EsAfectoImpuesto = Validator.EsAfectoImpuesto = item.EsAfectoImpuesto;
					    if (item.EsAfectoImpuesto && string.IsNullOrEmpty(OrdenEditar.CodigoTipoImpuesto))
					    {
						    OrdenEditar.CodigoTipoImpuesto = TipoImpuestoPredeterminado.Codigo;
						    Orden.NombreTipoImpuesto = TipoImpuestoPredeterminado.Nombre;
						    OrdenEditar.PorcentajeImpuesto = Orden.PorcentajeImpuesto = TipoImpuestoPredeterminado.Porcentaje;
					    }
					    else if (!item.EsAfectoImpuesto && !string.IsNullOrEmpty(OrdenEditar.CodigoTipoImpuesto))
					    {
						    OrdenEditar.CodigoTipoImpuesto = Orden.NombreTipoImpuesto = Validator.MsgErrorTipoImpuesto = null;
						    OrdenEditar.PorcentajeImpuesto = Orden.PorcentajeImpuesto = null;
					    }
                        EditContext.NotifyFieldChanged(EditContext.Field("CodigoTipoImpuesto"));
					    RecalcularImportes();
                    }
					goto exit;
				} 
                EditContext.NotifyFieldChanged(EditContext.Field("CodigoTipoProvision")); 
			}
			else 
			{
				if (!IsChangedTipoProvision) EditContext.MarkAsUnmodified(EditContext.Field("CodigoTipoProvision"));
				goto exit;
			}
		}
        Orden.CodigoTipoProvision = codigo;
		Orden.NombreTipoProvision = Orden.CodigoMoneda = Orden.NombreMoneda = Orden.SimboloMoneda = OrdenEditar.CodigoTipoImpuesto = Orden.NombreTipoImpuesto = null;
		Orden.EsAfectoImpuesto = Validator.EsAfectoImpuesto = false;
		OrdenEditar.PorcentajeImpuesto = null;
	exit:
		NotifyChange();
	}

    private void ValueChangeLocalHandler(string value)
    {
        OrdenEditar.CodigoLocalRecepcion = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorLocalRecepcion) && (Orden.CodigoLocalRecepcion ?? "").Trim() != (OrdenEditar.CodigoLocalRecepcion ?? ""))
            Orden.CodigoLocalRecepcion = Validator.MsgErrorLocalRecepcion = null;
    }

    private async Task OnChangeLocalHandler(object value)
    {
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoLocalRecepcion")))
		{ 
			OrdenEditar.CodigoLocalRecepcion = string.IsNullOrEmpty(codigo) ? null : codigo; 
            if ((Orden.CodigoLocalRecepcion ?? "") != codigo)
			{
                IsChangedLocal = true;
                if (!string.IsNullOrEmpty(codigo))
                {
				    (LocalObtenerPorCodigoEmpresaDto item, Validator.MsgErrorLocalRecepcion) = await IUtil.ObtenerLocalPorCodigoEmpresa(Alert, codigo, Empresa.Codigo);
				    if (item is not null)
				    {
					    Orden.CodigoLocalRecepcion = item.CodigoLocal;
					    Orden.NombreLocalRecepcion = item.NombreLocal;
					    goto exit;
				    } 
                    EditContext.NotifyFieldChanged(EditContext.Field("CodigoLocalRecepcion")); 
                }
			}
			else 
            {
                if (!IsChangedLocal) EditContext.MarkAsUnmodified(EditContext.Field("CodigoLocalRecepcion"));
				goto exit;
			}
		}
        Orden.CodigoLocalRecepcion = codigo;
		Orden.NombreLocalRecepcion = null;
	exit:
		NotifyChange();
	}

	private void ValueChangeCentroCostoHandler(string value)
    {
        OrdenEditar.CodigoCentroCosto = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorCentroCosto) && (Orden.CodigoCentroCosto ?? "").Trim() != (OrdenEditar.CodigoCentroCosto ?? ""))
            Orden.CodigoCentroCosto = Validator.MsgErrorCentroCosto = null;
    }

	private async Task OnChangeCentroCostoHandler(object value)
	{  
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoCentroCosto")))
		{ 
			OrdenEditar.CodigoCentroCosto = string.IsNullOrEmpty(codigo) ? null : codigo; 
            if ((Orden.CodigoCentroCosto ?? "") != codigo)
			{
                IsChangedCentroCosto = true;
                if (!string.IsNullOrEmpty(codigo))
                {
				    (CentroCostoObtenerPorCodigoDto item, Validator.MsgErrorCentroCosto) = await IUtil.ObtenerCentroCostoPorCodigo(Alert, codigo, Empresa.Codigo);
				    if (item is not null)
				    {
					    Orden.CodigoCentroCosto = item.CodigoCentroCosto.Trim();
					    Orden.NombreCentroCosto = item.NombreCentroCosto;
					    goto exit;
				    } 
                    EditContext.NotifyFieldChanged(EditContext.Field("CodigoCentroCosto"));
                }
			}
			else
            {
                if (!IsChangedCentroCosto) EditContext.MarkAsUnmodified(EditContext.Field("CodigoCentroCosto"));
				goto exit;
			}
		}
        Orden.CodigoCentroCosto = codigo;
		Orden.NombreCentroCosto = null;
	exit:
		NotifyChange(); 
	}

    private void ValueChangeTipoImpuestoHandler(string value)
    {
        OrdenEditar.CodigoTipoImpuesto = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorTipoImpuesto) && (Orden.CodigoTipoImpuesto ?? "").Trim() != (OrdenEditar.CodigoTipoImpuesto ?? ""))
            Orden.CodigoTipoImpuesto = Validator.MsgErrorTipoImpuesto = null;
    }

    private async Task OnChangeTipoImpuestoHandler(object value)
	{
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoTipoImpuesto")))
		{ 
			if ((Orden.CodigoTipoImpuesto ?? "").Trim() != codigo)
			{
				(TipoImpuestoObtenerPorCodigoDto item, Validator.MsgErrorTipoImpuesto) = await IUtil.ObtenerTipoImpuestoPorCodigo(Alert, codigo, true);
				if (item is not null)
				{
                    Orden.CodigoTipoImpuesto = item.CodigoTipoImpuesto;
					Orden.NombreTipoImpuesto = item.NombreTipoImpuesto;
                    if ((Orden.PorcentajeImpuesto ?? 0) != item.PorcentajeImpuesto)
                    {
					    OrdenEditar.PorcentajeImpuesto = Orden.PorcentajeImpuesto = item.PorcentajeImpuesto;
					    RecalcularImportes();
                    }
					IsChangedTipoImpuesto = true;
					goto exit;
				} 
                EditContext.NotifyFieldChanged(EditContext.Field("CodigoTipoImpuesto"));
			}
			else
			{
				if (!IsChangedTipoImpuesto) EditContext.MarkAsUnmodified(EditContext.Field("CodigoTipoImpuesto"));
				goto exit;
			}
		}
        Orden.CodigoTipoImpuesto = codigo;
		Orden.NombreTipoImpuesto = null;
		OrdenEditar.PorcentajeImpuesto = Orden.PorcentajeImpuesto = null;
	exit:
		NotifyChange();
	}

	private void OnChangeObservacionHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged((Orden.Observacion ?? "") != ((string)value ?? ""), EditContext, "Observacion"));

    private void OnChangeDescripcionLugarEntregaHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged((Orden.DescripcionLugarEntrega ?? "") != ((string)value ?? ""), EditContext, "DescripcionLugarEntrega"));

    private void OnChangeMotivoHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged((Orden.Motivo ?? "") != ((string)value ?? ""), EditContext, "Motivo"));

    private void OnChangeEsPagoAnticipadoHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Orden.EsPagoAnticipado != (bool)value, EditContext, "EsPagoAnticipado"));

    private void OnChangeFlagMedioPagoHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Orden.FlagMedioPago != (string)value, EditContext, "FlagMedioPago"));

    private void OnChangeFechaEntregaHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Orden.FechaEntrega != (DateTime?)value, EditContext, "FechaEntrega"));
    #endregion

    public void Dispose() => GC.SuppressFinalize(this);
}
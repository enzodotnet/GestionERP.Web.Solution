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

namespace GestionERP.Web.Pages.Empresa.Importacion.Orden;

public partial class Edit : IDisposable
{
    #region Propiedades
    private const string codigoModulo = "02";
    private const string codigoServicio = "S105";
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
    public List<OrdenNotaGrid> GridNotas { get; set; }
    public OrdenNotaGrid ItemGridNota { get; set; }
    public OrdenNotaObtenerDto Nota { get; set; }
    public OrdenNotaEditarDto NotaEditar { get; set; }
    public OrdenNotaInsertarDto NotaInsertar { get; set; }
    private EditContext EditContext { get; set; }
    private EditContext DetalleInsertarContext { get; set; }
    private EditContext DetalleEditarContext { get; set; }
    private EditContext NotaInsertarContext { get; set; }
    private EditContext NotaEditarContext { get; set; }
    public OrdenDetalleGridValidator GridDetalleValidator { get; set; }
    public OrdenEditarValidator Validator { get; set; }
    public OrdenDetalleEditarValidator DetalleEditarValidator { get; set; }
    public OrdenDetalleInsertarValidator DetalleInsertarValidator { get; set; }
    public OrdenNotaInsertarValidator NotaInsertarValidator { get; set; }
    private SerieDocumentoConsultaPorCodigoEmpresaDto Numerador { get; set; }
    private bool IsInitPage { get; set; }
    public bool IsEditingGridDetalle { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool IsAuthUser { get; set; }
    private bool IsModified { get; set; }
    private bool IsModifiedDetalle { get; set; }
	private bool IsModifiedNota { get; set; } 
    public bool EsVisibleCatalogoSolicitudes { get; set; }
    public TelerikNotification AlertDetalle { get; set; }
    public TelerikNotification AlertNota { get; set; }
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
    public bool EsVisibleDialogNota { get; set; }
    public bool EsVisibleModalNota { get; set; }
    public bool EsVisibleListaNotas { get; set; }
    private bool IsLoadingCatalogoSolicitudes { get; set; }
    private bool IsChangedEntidad { get; set; }
    private bool IsChangedTipoImportacion { get; set; }
    private bool IsChangedAduana { get; set; }
    private bool IsChangedPaisOrigen { get; set; }
    private bool IsChangedPaisProcedencia { get; set; }
    public ISvgIcon IconoAccionModal { get; set; }
    private TelerikGrid<OrdenDetalleGrid> GridDetalleRef { get; set; }
    private TelerikGrid<OrdenNotaGrid> GridNotaRef { get; set; }
    private TelerikGrid<SolicitudCatalogoAtenderDto> GridCatalogoRef { get; set; }
    public GridSelectionMode SelectionModeCatalogoSolicitud { get; set; }
    private bool EsSeleccionableCatalogoSolicitud { get; set; }
    private IEnumerable<OrdenFlag> Origenes { get; set; }
    private IEnumerable<OrdenFlag> MediosPago { get; set; }
    private IEnumerable<NotaReporteOrdenFlag> Secciones { get; set; } 
    public List<SolicitudCatalogoAtenderDto> CatalogoSolicitudes { get; set; }
    public IEnumerable<SolicitudCatalogoAtenderDto> ItemsSelectedSolicitud { get; set; }
    public string CodigoEjercicio { get; set; }
    private IEnumerable<EmpresaEjercicioCatalogoDto> CatalogoEjercicios { get; set; }
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    [Parameter] public Guid? Id { get; set; }
    public EmpresaConsultaPorCodigoWebDto Empresa { get; set; }
    private bool EsVisibleVolver { get; set; }
    [Parameter] public string CodigoWebEmpresa { get; set; }
    [Parameter][SupplyParameterFromQuery(Name = "returnpage")] public string ReturnPage { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    #endregion

    [Inject] public IImportacionOrden IOrden { get; set; }
    [Inject] public IImportacionSolicitud ISolicitud { get; set; }
    [Inject] public IPrincipalEmpresa IEmpresa { get; set; }
    [Inject] public IPrincipalSerieDocumento ISerieDocumento { get; set; } 
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

            Nota = new();
            NotaInsertar = new();
            NotaEditar = new();
            GridNotas = [];
            ItemGridNota = new();

            Origenes = OrdenFlag.Origenes();
            MediosPago = OrdenFlag.MediosPago();
            Secciones = NotaReporteOrdenFlag.Secciones();

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio);
            if (!IsAuthUser) return;

            Empresa = await IEmpresa.ConsultaPorCodigoWeb(CodigoWebEmpresa);
                
            EsVisibleVolver = !string.IsNullOrEmpty(ReturnPage) && ReturnPage is "index" or "view"; 
            rutaEmpresa = INavigation.Uri.Replace(INavigation.BaseUri, "").Split("?")[0].Replace(rutaServicio, "").Replace($"/{Id}", "").Replace(rutaAccion, "");

			if (!await IPermiso.ConsultaEsAsignadoPorSesion(OrdenAcceso.Editar, Empresa.Codigo))
			{
				INavigation.NavigateTo($"{rutaEmpresa}/inicio");
				Notify.Show("No tiene permiso para editar registros de órdenes de importación", "error");
				return;
			}

			Orden = await IOrden.Obtener(Empresa.Codigo, (Guid) Id);
            if (Orden is null || Orden.CodigoEstado is not "ED")
            {
                INavigation.NavigateTo($"{rutaEmpresa}{rutaServicio}");
                Notify.Show($"El registro de la orden de importación consultado a editar no está disponible", "error");
                return;
            }

            OrdenEditar = IMapper.Map<OrdenEditarDto>(Orden);

            Validator = new();
            EditContext = new EditContext(OrdenEditar);

			GridDetalles = IMapper.Map<List<OrdenDetalleGrid>>(Orden.Detalles);
            GridNotas = IMapper.Map<List<OrdenNotaGrid>>(Orden.Notas);

            Numerador = await ISerieDocumento.ConsultaPorCodigoEmpresa(Orden.CodigoSerieDocumento, Orden.CodigoDocumento, Empresa.Codigo) ?? new();
 
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
            Notify.Show($"La orden de importación {Orden.Codigo} ha sido editada con éxito", "success");
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
            case "nota":
                if (tipoAccion is "I" or "M")
                    NotaInsertarContext = new EditContext(NotaInsertar);
                else if (tipoAccion is "E")
                    NotaEditarContext = new EditContext(NotaEditar);
                EsVisibleModalNota = true;
                break;
        }
    }

    private void IniciarAccionDialog(string tipoAccion, string origenDialog)
    {
        TipoAccionDialog = tipoAccion;
        OrigenDialog = origenDialog; 
        if(origenDialog is "detalle")
            EsVisibleDialogDetalle = true;
        else if(origenDialog is "nota")
            EsVisibleDialogNota = true;
    }

    public void CerrarModal()
    {
        if (OrigenModal is "detalle")
            IsModifiedDetalle = EsVisibleModalDetalle = false;
        else if (OrigenModal is "nota")
            IsModifiedNota = EsVisibleModalNota = false; 
        VerboAccionModal = DescripcionAccionModal = TipoAccionModal = "";
        StateHasChanged();
    }

	private void Volver() => INavigation.NavigateTo($"{rutaEmpresa}{rutaServicio}{(ReturnPage == "view" ? $"/{Id}" : "")}");

	public void CerrarDialog()
    { 
        if (OrigenDialog is "detalle")
            EsVisibleDialogDetalle = false;
        else if (OrigenDialog is "nota")
            EsVisibleDialogNota = false;
        TipoAccionDialog = OrigenDialog = "";
        StateHasChanged();
    }

    private bool VerificarRegistroEsValido()
    {
        bool esValido = true;

        if (OrdenEditar.FechaEstimadaETD?.Date > OrdenEditar.FechaEstimadaETA?.Date)
        {
            Fnc.MostrarAlerta(Alert, "La fecha estimada ETD debe ser menor o igual a la fecha estimada ETA", "error");
            esValido = false;
        }
        if (OrdenEditar.FechaEstimadaETA?.Date > OrdenEditar.FechaReposicionStock?.Date)
        {
            Fnc.MostrarAlerta(Alert, "La fecha estimada ETA debe ser menor o igual a la fecha reposición de stock", "error");
            esValido = false;
        }

        if (GridDetalles.Count == 0)
        {
            Fnc.MostrarAlerta(Alert, "Es obligatorio que agregue ítem(s) al detalle de la orden de importación", "error");
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
                    Fnc.MostrarAlerta(Alert, "Está pendiente completar alguna celda de [cantidad] del detalle de la orden de importación", "error");
                    esValido = false;
                } 
                if (GridDetalles.Any(x => !x.PrecioUnitario.HasValue))
                {
                    Fnc.MostrarAlerta(Alert, "Está pendiente completar alguna celda del [precio unitario] del detalle de la orden de importación", "error");
                    esValido = false;
                } 
            } 
        }

        return esValido;
    }

    private void NotifyChange(bool? isValueByVerify = null)
    {
        IsModified = isValueByVerify ?? EditContext.IsModified();
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
        CatalogoSolicitudes.ForEach(x => { x.IsErrorSelected = false; });
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
		List<IGrouping<string, SolicitudCatalogoAtenderDto>> elements = ItemsSelectedSolicitud.GroupBy(x => x.CodigoArticulo).Where(x => x.Count() > 1).ToList();
		if (elements.Count > 0)
		{
			Fnc.MostrarAlerta(AlertCatalogoSolicitudes, "Existen ítems seleccionados de igual código", "error");
			elements.ForEach(x => x.ToList().ForEach(x => x.IsErrorSelected = true));
			isErrorSelectedCatalogo = true;
		}

        if (ItemsSelectedSolicitud.Any(x => x.CodigoEntidad != OrdenEditar.CodigoEntidad))
		{
			Fnc.MostrarAlerta(AlertCatalogoSolicitudes, "Solo podrá elegir ítems del mismo proveedor ya seleccionado", "error");
			ItemsSelectedSolicitud.ToList().ForEach(x => x.IsErrorSelected = true);
			isErrorSelectedCatalogo = true;
		}

		if (isErrorSelectedCatalogo)
			return;

        string codigoPaisOrigen = ItemsSelectedSolicitud.Select(x => x.CodigoPaisOrigen).First();
        if (string.IsNullOrEmpty(OrdenEditar.CodigoPaisOrigen) && !ItemsSelectedSolicitud.Any(x => x.CodigoPaisOrigen != codigoPaisOrigen))
        {
            OrdenEditar.CodigoPaisOrigen = codigoPaisOrigen;
            Orden.NombrePaisOrigen = ItemsSelectedSolicitud.Select(x => x.NombrePaisOrigen).First();
            EditContext.NotifyFieldChanged(EditContext.Field("CodigoPaisOrigen"));
        }

        string codigoPaisProcedencia = ItemsSelectedSolicitud.Select(x => x.CodigoPaisProcedencia).First();
        if (string.IsNullOrEmpty(OrdenEditar.CodigoPaisProcedencia) && !ItemsSelectedSolicitud.Any(x => x.CodigoPaisProcedencia != codigoPaisProcedencia))
        {
            OrdenEditar.CodigoPaisProcedencia = codigoPaisProcedencia;
            Orden.NombrePaisProcedencia = ItemsSelectedSolicitud.Select(x => x.NombrePaisProcedencia).First();
            EditContext.NotifyFieldChanged(EditContext.Field("CodigoPaisProcedencia"));
        }

        DateTime fechaEstimadaETA = ItemsSelectedSolicitud.Select(x => x.FechaEstimadaETA).First();
        if (!OrdenEditar.FechaEstimadaETA.HasValue && !ItemsSelectedSolicitud.Any(x => x.FechaEstimadaETA != fechaEstimadaETA))
        {
            OrdenEditar.FechaEstimadaETA = fechaEstimadaETA;
            EditContext.NotifyFieldChanged(EditContext.Field("FechaEstimadaETA"));
        }

        DateTime fechaEstimadaETD = ItemsSelectedSolicitud.Select(x => x.FechaEstimadaETD).First();
        if (!OrdenEditar.FechaEstimadaETD.HasValue && !ItemsSelectedSolicitud.Any(x => x.FechaEstimadaETD != fechaEstimadaETD))
        {
            OrdenEditar.FechaEstimadaETD = fechaEstimadaETD;
            EditContext.NotifyFieldChanged(EditContext.Field("FechaEstimadaETD"));
        }

        DateTime fechaReposicionStock = ItemsSelectedSolicitud.Select(x => x.FechaReposicionStock).First();
        if (!OrdenEditar.FechaReposicionStock.HasValue && !ItemsSelectedSolicitud.Any(x => x.FechaReposicionStock != fechaReposicionStock))
        {
            OrdenEditar.FechaReposicionStock = fechaReposicionStock;
            EditContext.NotifyFieldChanged(EditContext.Field("FechaReposicionStock"));
        }

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
        ModificarImportesItem(cantidad, TipoAccionModal is "E" ? DetalleEditar.PrecioUnitario : DetalleInsertar.PrecioUnitario);
    }

    private void OnChangeDetalleObservacionHandler(object value) => IsModifiedDetalle = Fnc.VerifyContextIsChanged((Detalle.Observacion ?? "") != ((string)value ?? ""), TipoAccionModal is "E" ? DetalleEditarContext : DetalleInsertarContext, "Observacion");

    private void OnChangeDetallePrecioUnitarioHandler(object value)
    {
        decimal? precioUnitario = (decimal?)value;
        IsModifiedDetalle = Fnc.VerifyContextIsChanged(Detalle.PrecioUnitario != precioUnitario, TipoAccionModal is "E" ? DetalleEditarContext : DetalleInsertarContext, "PrecioUnitario");
        ModificarImportesItem(TipoAccionModal is "E" ? DetalleEditar.Cantidad : DetalleInsertar.Cantidad, precioUnitario);
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
        else if(args.Field == "Cantidad")
        {  
            if (!esItemNuevo && indexEdit > -1)
                OrdenEditar.DetallesEditar[indexEdit].Cantidad = item.Cantidad;
            else if (esItemNuevo)
                OrdenEditar.DetallesInsertar[indexEdit].Cantidad = item.Cantidad; 
            GridDetalles[indexGrid].Cantidad = item.Cantidad; 
        } 
		ModificarImportesItem(item.Cantidad, item.PrecioUnitario, indexGrid, indexEdit);
		ModificarImportesTotales();
        if (!esItemNuevo && indexEdit == -1)
        { 
            OrdenEditar.DetallesEditar.Add(IMapper.Map(IMapper.Map<OrdenDetalleObtenerDto>(GridDetalles[indexGrid]), new OrdenDetalleEditarDto()));
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
					OrdenEditar.DetallesEditar[indexEdit].ImporteBruto = importeBruto;
				else
					OrdenEditar.DetallesInsertar[indexEdit].ImporteBruto = importeBruto;
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

    private void ModificarImportesTotales() => OrdenEditar.TotalImporteBruto = GridDetalles.Count == 0 ? 0 : GridDetalles.Sum(x => x.ImporteBruto);

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
				    goto exit;
			    }
            } 
            DetalleInsertarContext.NotifyFieldChanged(DetalleInsertarContext.Field("CodigoArticulo"));
		}
        Detalle.CodigoArticulo = codigo;
		Detalle.NombreArticulo = Detalle.CodigoUnidadMedida = Detalle.NombreUnidadMedida = Detalle.CodigoTipoArticulo = Detalle.NombreTipoArticulo = Detalle.Presentacion = null;
		Detalle.UnidadConversion = DetalleInsertarValidator.UnidadConversion = null;
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

    #region Nota
    private void VisibleListaNotaChangedHandler(bool esVisible) => EsVisibleListaNotas = esVisible;

    public void InvalidarAccionNota(EditContext editContext) => Fnc.MostrarAlerta(AlertNota, Cnf.MsgErrorInvalidEditContext, "error");

    private async Task CerrarNota()
    {
        if (TipoAccionModal is not "V" && IsModifiedNota && !await Dialog.ConfirmAsync("¿Está seguro de cerrar el formulario y que los datos no se carguen?", "Saliendo del formulario"))
            return;
        CerrarModal();
    }

    public static void OnRowNotaRenderHandler(GridRowRenderEventArgs args) => args.Class = !(args.Item as OrdenNotaGrid).Id.HasValue ? "new-row" : "";
    
    public void MostrarAgregarNota()
    {
        NotaInsertar = new();
        Nota = new();
        NotaInsertarValidator = new();
        IniciarAccionModal("I", "nota");
    }

    public void MostrarModificarNota(OrdenNotaGrid item = null)
    {
        if (TipoAccionModal is not "V")
            Nota = IMapper.Map<OrdenNotaObtenerDto>(item);

        string tipoAccion = !Nota.Id.HasValue ? "M" : "E";
        if (tipoAccion is "M")
        {
			NotaInsertar = IMapper.Map<OrdenNotaInsertarDto>(Nota);
            NotaInsertarValidator = new();
		}
        else
        {
			NotaEditar = IMapper.Map<OrdenNotaEditarDto>(Nota);
		}
        IniciarAccionModal(tipoAccion, "nota");
    }

    private void MostrarQuitarNota(OrdenNotaGrid item = null)
    {
        if (TipoAccionModal is not "V")
            Nota = IMapper.Map<OrdenNotaObtenerDto>(item);

        IniciarAccionDialog(!Nota.Id.HasValue ? "Q" : "X", "nota");
    }

    public void VerItemNota(OrdenNotaGrid item)
    {
        Nota = IMapper.Map<OrdenNotaObtenerDto>(item);
        IniciarAccionModal("V", "nota");
    }

	private void OnChangeNotaContenidoHandler(object value) => IsModifiedNota = Fnc.VerifyContextIsChanged((Nota.Contenido ?? "") != ((string)value ?? ""), TipoAccionModal is "E" ? NotaEditarContext : NotaInsertarContext, "Contenido");

    private void ValueChangeNotaReporteOrdenHandler(string value)
    { 
        NotaInsertar.CodigoNotaReporteOrden = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(NotaInsertarValidator.MsgErrorNotaReporteOrden) && (Nota.CodigoNotaReporteOrden ?? "").Trim() != (NotaInsertar.CodigoNotaReporteOrden ?? ""))
	        Nota.CodigoNotaReporteOrden = NotaInsertarValidator.MsgErrorNotaReporteOrden = null;
    }

    private async Task OnChangeNotaReporteOrdenHandler(object value)
	{
		string codigo = (string)(value ?? "");
		if (NotaInsertarContext.IsValid(NotaInsertarContext.Field("CodigoNotaReporteOrden")))
		{
			if ((NotaInsertar.CodigoNotaReporteOrden ?? "") == codigo) goto exit;
			if (GridNotas.Any(x => x.CodigoNotaReporteOrden == codigo))
			{
				Fnc.MostrarAlerta(AlertNota, "El código ya existe en las notas", "error");
			}
			else
			{
                (NotaReporteOrdenObtenerPorCodigoDto item, NotaInsertarValidator.MsgErrorNotaReporteOrden) = await IUtil.ObtenerNotaReporteOrdenImportacionPorCodigo(AlertNota, Empresa.Codigo, codigo);
                if (item is not null)
                {
                    Nota.CodigoNotaReporteOrden = item.CodigoNotaReporteOrden;
                    Nota.NombreNotaReporteOrden = item.NombreNotaReporteOrden;
                    Nota.FlagSeccion = item.FlagSeccion;
                    Nota.Contenido = item.Contenido;
                    goto exit;
                }
            }
            NotaInsertarContext.NotifyFieldChanged(NotaInsertarContext.Field("CodigoNotaReporteOrden"));
		}
        Nota.CodigoNotaReporteOrden = codigo;
		Nota.NombreNotaReporteOrden = Nota.FlagSeccion = Nota.Contenido = null;
		if (string.IsNullOrEmpty(codigo))
			NotaInsertarContext.MarkAsUnmodified(NotaInsertarContext.Field("CodigoNotaReporteOrden")); 
	exit:
		IsModifiedNota = NotaInsertarContext.IsModified();
	}

	private async Task AccionarNota(string tipoAccion)
    {
        GridState<OrdenNotaGrid> notaState = GridNotaRef.GetState();
        int indexGrid = GridNotas.FindIndex(i => i.CodigoNotaReporteOrden == Nota.CodigoNotaReporteOrden);
        int indexEdit = tipoAccion is "E" or "X" ? OrdenEditar.NotasEditar.FindIndex(i => i.Id == Nota.Id) : OrdenEditar.NotasInsertar.FindIndex(i => i.CodigoNotaReporteOrden == Nota.CodigoNotaReporteOrden);
        
        switch (tipoAccion)
        {
            case "I" or "M": 
                ItemGridNota = IMapper.Map<OrdenNotaGrid>(IMapper.Map(NotaInsertar, Nota));
                if (tipoAccion == "I")
                {
                    OrdenEditar.NotasInsertar.Add(NotaInsertar);
                    GridNotas.Add(ItemGridNota);
                    notaState.InsertedItem = ItemGridNota;
                }
                else
                {
                    OrdenEditar.NotasInsertar[indexEdit] = NotaInsertar;
                    GridNotas[indexGrid] = ItemGridNota; 
                }
                break; 
            case "E":
                if (indexEdit > -1)
                    OrdenEditar.NotasEditar[indexEdit] = NotaEditar;
                else
                    OrdenEditar.NotasEditar.Add(NotaEditar);
                GridNotas[indexGrid] = IMapper.Map<OrdenNotaGrid>(IMapper.Map(NotaEditar, Nota));
                break; 
            case "Q" or "X":
                if (tipoAccion == "Q")
                    OrdenEditar.NotasInsertar.RemoveAt(indexEdit);
                else
                { 
                    OrdenEditar.NotasEliminar.Add(new() { Id = (Guid) Nota.Id });
                    if (indexEdit > -1)
                        OrdenEditar.NotasEditar.RemoveAt(indexEdit);
                }
                GridNotas.RemoveAt(indexGrid);
                CerrarDialog();
                break;
        };
        CerrarModal();
        NotifyChange();
        await GridNotaRef.SetStateAsync(notaState);
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
        DetalleInsertarValidator.MsgErrorArticulo = null;
        DetalleInsertarContext.NotifyFieldChanged(DetalleInsertarContext.Field("CodigoArticulo"));
        IsModifiedDetalle = DetalleInsertarContext.IsModified();
        if (DetalleInsertar.Cantidad.HasValue)
            DetalleInsertarContext.NotifyFieldChanged(DetalleInsertarContext.Field("Cantidad")); 
    }
    
    private void CargarItemCatalogoProveedor(EntidadProveedorCatalogoPorEmpresaDto item)
    {
        OrdenEditar.CodigoEntidad = Orden.CodigoEntidad = item.CodigoEntidad;
        Orden.NombreEntidad = item.NombreEntidad;
        OrdenEditar.CodigoModoPago = item.CodigoModoPago;
        Orden.NombreModoPago = item.NombreModoPago;
        OrdenEditar.CodigoPlazoCredito = item.CodigoPlazoCredito;
        Orden.NombrePlazoCredito = item.NombrePlazoCredito;
        IsChangedEntidad = true;
        Validator.MsgErrorEntidad = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidad"));
        NotifyChange();
    }

    private void CargarItemCatalogoPaisOrigen(PaisCatalogoDto item)
    {
        OrdenEditar.CodigoPaisOrigen = Orden.CodigoPaisOrigen = item.CodigoPais;
        Orden.NombrePaisOrigen = item.NombrePais;
        IsChangedPaisOrigen = true;
        Validator.MsgErrorPaisOrigen = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoPaisOrigen"));
        NotifyChange();
    }

    private void CargarItemCatalogoPaisProcedencia(PaisCatalogoDto item)
    {
        OrdenEditar.CodigoPaisProcedencia = Orden.CodigoPaisProcedencia = item.CodigoPais;
        Orden.NombrePaisProcedencia = item.NombrePais;
        IsChangedPaisProcedencia = true;
        Validator.MsgErrorPaisProcedencia = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoPaisProcedencia"));
        NotifyChange();
    }

    private void CargarItemCatalogoAduana(AduanaCatalogoDto item)
    {
        OrdenEditar.CodigoAduana = Orden.CodigoAduana = item.CodigoAduana;
        Orden.NombreAduana = item.NombreAduana;
        IsChangedAduana = true;
        Validator.MsgErrorAduana = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoAduana"));
        NotifyChange();
    }

    private void CargarItemCatalogoTipoImportacion(TipoImportacionCatalogoDto item)
    {
        OrdenEditar.CodigoTipoImportacion = Orden.CodigoTipoImportacion = item.CodigoTipoImportacion;
        Orden.NombreTipoImportacion = item.NombreTipoImportacion;
        IsChangedTipoImportacion = true;
        Validator.MsgErrorTipoImportacion = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoTipoImportacion"));
        NotifyChange();
    }

    private void CargarItemCatalogoNotaReporteOrden(NotaReporteOrdenCatalogoDto item)
    {
        NotaInsertar.CodigoNotaReporteOrden = Nota.CodigoNotaReporteOrden = item.CodigoNotaReporteOrden;
        Nota.NombreNotaReporteOrden = item.NombreNotaReporteOrden;
        Nota.FlagSeccion = item.FlagSeccion;
        Nota.Contenido = item.Contenido;
        NotaInsertarValidator.MsgErrorNotaReporteOrden = null;
		NotaInsertarContext.NotifyFieldChanged(NotaInsertarContext.Field("CodigoNotaReporteOrden"));
        IsModifiedNota = NotaInsertarContext.IsModified();
    }

    private void CargarItemCatalogoMoneda(MonedaCatalogoDto item)
    {
        OrdenEditar.CodigoMoneda = Orden.CodigoMoneda = item.CodigoMoneda;
        Orden.NombreMoneda = item.CodigoMoneda;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoMoneda"));
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
				(EntidadProveedorObtenerPorCodigoEmpresaDto item, Validator.MsgErrorEntidad) = await IUtil.ObtenerProveedorPorCodigoEmpresa(Alert, codigo, Empresa.Codigo, "I");
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

    private void ValueChangeAduanaHandler(string value)
    {
        OrdenEditar.CodigoAduana = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorAduana) && (Orden.CodigoAduana ?? "").Trim() != (OrdenEditar.CodigoAduana ?? ""))
            Orden.CodigoAduana = Validator.MsgErrorAduana = null;
    }

    private async Task OnChangeAduanaHandler(object value)
	{
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoAduana")))
		{ 
			if ((Orden.CodigoAduana ?? "").Trim() != codigo)
			{
				IsChangedAduana = true;
                (AduanaObtenerPorCodigoDto item, Validator.MsgErrorAduana) = await IUtil.ObtenerAduanaPorCodigo(Alert, codigo);
                if (item is not null)
                {
                    Orden.CodigoAduana = item.CodigoAduana;
                    Orden.NombreAduana = item.NombreAduana;
                    goto exit;
                } 
                EditContext.NotifyFieldChanged(EditContext.Field("CodigoAduana"));
            }
			else
			{
				if (!IsChangedAduana) EditContext.MarkAsUnmodified(EditContext.Field("CodigoAduana"));
				goto exit;
			}
		}
        Orden.CodigoAduana = codigo;
		Orden.NombreAduana = null;
	exit:
		NotifyChange();
	}

    private void ValueChangeTipoImportacionHandler(string value)
    {
        OrdenEditar.CodigoTipoImportacion = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorTipoImportacion) && (Orden.CodigoTipoImportacion ?? "").Trim() != (OrdenEditar.CodigoTipoImportacion ?? ""))
            Orden.CodigoTipoImportacion = Validator.MsgErrorTipoImportacion = null;
    }

    private async Task OnChangeTipoImportacionHandler(object value)
	{
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoTipoImportacion")))
		{ 
			if ((Orden.CodigoTipoImportacion ?? "").Trim() != codigo)
			{
				IsChangedTipoImportacion = true;
                (TipoImportacionObtenerPorCodigoDto item, Validator.MsgErrorTipoImportacion) = await IUtil.ObtenerTipoImportacionPorCodigo(Alert, codigo);
                if (item is not null)
                {
                    Orden.CodigoTipoImportacion = item.CodigoTipoImportacion;
                    Orden.NombreTipoImportacion = item.NombreTipoImportacion;
                    goto exit;
                } 
                EditContext.NotifyFieldChanged(EditContext.Field("CodigoTipoImportacion"));
            }
			else
			{
				if (!IsChangedTipoImportacion) EditContext.MarkAsUnmodified(EditContext.Field("CodigoTipoImportacion"));
				goto exit;
			}
		}
        Orden.CodigoTipoImportacion = codigo;
		Orden.NombreTipoImportacion = null;
	exit:
		NotifyChange();
	}

    private void ValueChangePaisOrigenHandler(string value)
    {
        OrdenEditar.CodigoPaisOrigen = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorPaisOrigen) && (Orden.CodigoPaisOrigen ?? "").Trim() != (OrdenEditar.CodigoPaisOrigen ?? ""))
            Orden.CodigoPaisOrigen = Validator.MsgErrorPaisOrigen = null;
    }

    private async Task OnChangePaisOrigenHandler(object value)
	{
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoPaisOrigen")))
		{ 
			if ((Orden.CodigoPaisOrigen ?? "").Trim() != codigo)
			{
				IsChangedPaisOrigen = true;
                (PaisObtenerPorCodigoDto item, Validator.MsgErrorPaisOrigen) = await IUtil.ObtenerPaisPorCodigo(Alert, codigo);
                if (item is not null)
                {
                    Orden.CodigoPaisOrigen = item.CodigoPais;
                    Orden.NombrePaisOrigen = item.NombrePais;
                    goto exit;
                } 
                EditContext.NotifyFieldChanged(EditContext.Field("CodigoPaisOrigen"));
            }
			else
			{
				if (!IsChangedPaisOrigen) EditContext.MarkAsUnmodified(EditContext.Field("CodigoPaisOrigen"));
				goto exit;
			}
		}
        Orden.CodigoPaisOrigen = codigo;
		Orden.NombrePaisOrigen = null;
	exit:
		NotifyChange();
	}

    private void ValueChangePaisProcedenciaHandler(string value)
    {
        OrdenEditar.CodigoPaisProcedencia = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorPaisProcedencia) && (Orden.CodigoPaisProcedencia ?? "").Trim() != (OrdenEditar.CodigoPaisProcedencia ?? ""))
            Orden.CodigoPaisProcedencia = Validator.MsgErrorPaisProcedencia = null;
    }

    private async Task OnChangePaisProcedenciaHandler(object value)
	{
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoPaisProcedencia")))
		{ 
			if ((Orden.CodigoPaisProcedencia ?? "").Trim() != codigo)
			{
				IsChangedPaisProcedencia = true;
                (PaisObtenerPorCodigoDto item, Validator.MsgErrorPaisOrigen) = await IUtil.ObtenerPaisPorCodigo(Alert, codigo);
                if (item is not null)
                {
                    Orden.CodigoPaisProcedencia = item.CodigoPais;
                    Orden.NombrePaisProcedencia = item.NombrePais;
                    goto exit;
                } 
                EditContext.NotifyFieldChanged(EditContext.Field("CodigoPaisProcedencia"));
            }
			else
			{
				if (!IsChangedPaisProcedencia) EditContext.MarkAsUnmodified(EditContext.Field("CodigoPaisProcedencia"));
				goto exit;
			}
		}
        Orden.CodigoPaisProcedencia = codigo;
		Orden.NombrePaisProcedencia = null;
	exit:
		NotifyChange();
	}

	private void OnChangeObservacionHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged((Orden.Observacion ?? "") != ((string)value ?? ""), EditContext, "Observacion"));

	private void OnChangeDescripcionLugarEntregaHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged((Orden.DescripcionLugarEntrega ?? "") != ((string)value ?? ""), EditContext, "DescripcionLugarEntrega"));

	private void OnChangeDescripcionFormaPagoHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged((Orden.DescripcionFormaPago ?? "") != ((string)value ?? ""), EditContext, "DescripcionFormaPago")); 

	private void OnChangeFlagMedioPagoHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Orden.FlagMedioPago != (string)value, EditContext, "FlagMedioPago"));

	private void OnChangeFechaReposicionStockHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Orden.FechaReposicionStock != (DateTime?)value, EditContext, "FechaReposicionStock"));

	private void OnChangeFechaEstimadaETAHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Orden.FechaEstimadaETA != (DateTime?)value, EditContext, "FechaEstimadaETA"));

	private void OnChangeFechaEstimadaETDHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Orden.FechaEstimadaETD != (DateTime?)value, EditContext, "FechaEstimadaETD"));
	#endregion

	public void Dispose() => GC.SuppressFinalize(this);
}
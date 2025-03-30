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

namespace GestionERP.Web.Pages.Empresa.Importacion.Orden;

public partial class Insert : IDisposable
{
    #region Propiedades
    private const string codigoModulo = "02";
    private const string codigoServicio = "S105";
    private const string rutaAccion = "/emitir";
    private string rutaEmpresa = "";
    private string pathBaseUri = "";
    private const string rutaServicio = "/ordenes";
    public FluentValidationValidator validator;
     
	public OrdenInsertarDto OrdenInsertar { get; set; }
    public OrdenObtenerDto Orden { get; set; }
    public OrdenDetalleInsertarDto DetalleInsertar { get; set; }
    public OrdenDetalleObtenerDto Detalle { get; set; }
	public OrdenNotaInsertarDto NotaInsertar { get; set; }
	public OrdenNotaObtenerDto Nota { get; set; }
	public List<OrdenDetalleGrid> GridDetalles { get; set; }
	public OrdenDetalleGrid ItemGridDetalle { get; set; }
	public List<OrdenNotaGrid> GridNotas { get; set; }
	public OrdenNotaGrid ItemGridNota { get; set; }
	private EditContext EditContext { get; set; }
    private EditContext DetalleContext { get; set; }
	private EditContext NotaContext{ get; set; }
    public OrdenDetalleGridValidator GridDetalleValidator { get; set; } 
    public OrdenInsertarValidator Validator { get; set; }
    public OrdenDetalleInsertarValidator DetalleValidator { get; set; }
    public OrdenNotaInsertarValidator NotaValidator { get; set; }
    public bool IsEditingGridDetalle { get; set; }
    public string CodigoLocalNumerador { get; set; }
    public string CodigoProcesoDocumento { get; set; }
    public List<DateTime> FechasCerradoOperacion { get; set; }
    public EmpresaEjercicioConsultaIntervaloFechaDto FechaIntervalo { get; set; }
	private IEnumerable<OrdenFlag> Origenes { get; set; }
    private IEnumerable<OrdenFlag> MediosPago { get; set; }
	private IEnumerable<NotaReporteOrdenFlag> Secciones { get; set; } 
    public List<SolicitudCatalogoAtenderDto> CatalogoSolicitudes { get; set; }
    public IEnumerable<SolicitudCatalogoAtenderDto> ItemsSelectedSolicitud { get; set; }
    public string CodigoEjercicio { get; set; }  
    private IEnumerable<EmpresaEjercicioCatalogoDto> CatalogoEjercicios { get; set; }
    public string CodigoTipoDocumento { get; set; }
	public bool EsVisibleBotonDocumento { get; set; }
    public bool EsVisibleCatalogoSolicitudes { get; set; } 
    private bool IsAuthUser { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool IsModified { get; set; }
    private bool IsModifiedDetalle { get; set; }
	private bool IsModifiedNota { get; set; }
	private bool IsLoadingCatalogoSolicitudes { get; set; }
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
    public bool IsInitPage { get; set; }
    public string CodigoItemAccion { get; set; }
    public ISvgIcon IconoAccionModal { get; set; }
    public string TituloTipoOrigen { get; set; }
    public string CodigoPermisoEmitir { get; set; }
    public TelerikNotification AlertDetalle { get; set; }
	public TelerikNotification AlertNota { get; set; }
	public TelerikNotification Alert { get; set; }
    public TelerikNotification AlertCatalogoSolicitudes { get; set; }
    private TelerikGrid<OrdenDetalleGrid> GridDetalleRef { get; set; }
	private TelerikGrid<OrdenNotaGrid> GridNotaRef { get; set; }
	private TelerikGrid<SolicitudCatalogoAtenderDto> GridCatalogoRef { get; set; }
    public GridSelectionMode SelectionModeCatalogoSolicitud { get; set; }
    private bool EsSeleccionableCatalogoSolicitud { get; set; }
    [Parameter][SupplyParameterFromQuery(Name = "returnpage")] public string ReturnPage { get; set; }
    public EmpresaConsultaPorCodigoWebDto Empresa { get; set; }
    private bool EsVisibleVolver { get; set; }
    [Parameter] public string CodigoWebEmpresa { get; set; }
    [Parameter] public string Origen { get; set; }
    public string OrigenTemp { get; set; }
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    #endregion

    [Inject] public IImportacionOrden IOrden { get; set; }
	[Inject] public IImportacionSolicitud ISolicitud { get; set; }
    [Inject] public IPrincipalEmpresa IEmpresa { get; set; } 
    [Inject] public IImportacionNotaReporteOrden INota { get; set; } 
	[Inject] public IPrincipalMoneda IMoneda { get; set; }
	[Inject] public IPrincipalPermiso IPermiso { get; set; } 
	[Inject] public UserService IUser { get; set; }
    [Inject] public UtilService IUtil { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }
    [Inject] public IMapper IMapper { get; set; }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);
        if (!string.IsNullOrEmpty(Origen))
        {
            if (!string.IsNullOrEmpty(OrigenTemp) && Origen != OrigenTemp)
                await IniciarVistaOrigen();

            OrigenTemp = Origen;
        }
    }

    protected override async Task OnInitializedAsync()
    {
        Origenes = OrdenFlag.Origenes();
        MediosPago = OrdenFlag.MediosPago();
        Secciones = NotaReporteOrdenFlag.Secciones();
        Validator = new(); 
        await IniciarVistaOrigen();
    }

    protected async Task IniciarVistaOrigen()
    {
        try
        {
			Notify.ShowLoading(mensaje: "Iniciando formulario");

			OrdenInsertar = new();
            Orden = new();
            GridDetalles = [];
            ItemGridDetalle = new();
            DetalleInsertar = new();
            Detalle = new();
            GridNotas = [];
            ItemGridNota = new();
            NotaInsertar = new();
            Nota = new();

            FechasCerradoOperacion = [];
            FechaIntervalo = new();
 
            CodigoTipoDocumento = "OR";
            EsVisibleBotonDocumento = true;
              
            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio);
            if (!IsAuthUser) return;

            Empresa = await IEmpresa.ConsultaPorCodigoWeb(CodigoWebEmpresa);
            EsVisibleVolver = !string.IsNullOrEmpty(ReturnPage) && ReturnPage is "index";

            pathBaseUri = $"/{INavigation.ToBaseRelativePath(INavigation.Uri).Split("?")[0]}";
            rutaEmpresa = INavigation.Uri.Replace(INavigation.BaseUri, "").Split("?")[0].Replace(rutaServicio, "").Replace(rutaAccion, "").Replace(string.Concat("/", Origen ?? ""), "");

            if (!VerificarTipoOrigenEsValido())
            {
                INavigation.NavigateTo($"{rutaEmpresa}/inicio");
                Notify.Show("La ruta al que intenta acceder para emitir [Orden de Compra] no tiene un tipo de origen válido", "error");
                return;
            }

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(CodigoPermisoEmitir, Empresa.Codigo))
            {
                INavigation.NavigateTo($"{rutaEmpresa}/inicio");
                Notify.Show("No tiene permiso para emitir registros de órdenes de importación", "error");
                return;
            }

            FechaIntervalo = await IEmpresa.ConsultaEjercicioIntervaloFecha(Empresa.Codigo);
            (await IEmpresa.ConsultaModuloPeriodoFechasEsCerradoOperacion(Empresa.Codigo, codigoModulo))?.ToList().ForEach(x => FechasCerradoOperacion.Add(x.Fecha));
            OrdenInsertar.FechaEmision = !FechasCerradoOperacion.Any(x => x == DateTime.Now.Date) ? DateTime.Now.Date : null;
            if (OrdenInsertar.FechaEmision.HasValue)
                Orden.FechaEmision = (DateTime)OrdenInsertar.FechaEmision;

            MonedaObtenerPorTipoDto ME = await IMoneda.ObtenerPorTipo("ME") ?? new();
            OrdenInsertar.CodigoMoneda = ME.Codigo;
            Orden.NombreMoneda = ME.Nombre;
            Orden.SimboloMoneda = ME.Simbolo;

            await CargarEmpresaNotasOrdenImportacion();
 
            EditContext = new EditContext(OrdenInsertar);
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
            IsModified = false;
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

            Guid id = await IOrden.Insertar(Empresa.Codigo, OrdenInsertar);

            IsModified = false;
            Notify.Show("La orden de importación ha sido emitida con éxito en la empresa", "success");
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
 
    private bool VerificarTipoOrigenEsValido()
    {
        bool esValido = false;
        if (!string.IsNullOrEmpty(Origen) && Origen is "directa" or "solicitud")
        { 
            if (Origen is "directa")
            {
                CodigoPermisoEmitir = OrdenAcceso.EmitirDirecta;
                OrdenInsertar.FlagOrigen = "D";
                TituloTipoOrigen = "directa";
            }
            else 
            {
                CodigoPermisoEmitir = OrdenAcceso.EmitirPorSolicitud;
                OrdenInsertar.FlagOrigen = "S";
                TituloTipoOrigen = "por solicitud";
            }
            esValido = true;
        }
        return esValido;
    }

    private bool VerificarRegistroEsValido()
    {
        bool esValido  = true;

        if (OrdenInsertar.FechaEstimadaETD?.Date > OrdenInsertar.FechaEstimadaETA?.Date)
        {
            Fnc.MostrarAlerta(Alert, "La fecha estimada ETD debe ser menor o igual a la fecha estimada ETA", "error");
            esValido = false;
        }
        if (OrdenInsertar.FechaEstimadaETA?.Date > OrdenInsertar.FechaReposicionStock?.Date)
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

    private async Task Cerrar(LocationChangingContext context)
    {
        if (IsAuthUser && IsModified && pathBaseUri != context.TargetLocation && !await Dialog.ConfirmAsync("¿Está seguro de salir del formulario de emitir sin haber guardado?", "Saliendo del formulario"))
            context.PreventNavigation();
    }

    private void LimpiarCamposPorOrigenSolicitud()
    {
        OrdenInsertar.CodigoEntidad = null;
        Orden.NombreEntidad = "";
        Validator.MsgErrorEntidad = null;
        OrdenInsertar.CodigoPaisOrigen = null;
        Orden.NombrePaisOrigen = "";
        Validator.MsgErrorPaisOrigen = null;
        OrdenInsertar.CodigoPaisProcedencia = null;
        Orden.NombrePaisProcedencia = "";
        Validator.MsgErrorPaisProcedencia = null;
        OrdenInsertar.FechaEstimadaETA = null;
        OrdenInsertar.FechaEstimadaETD = null;
        OrdenInsertar.FechaReposicionStock = null;
		OrdenInsertar.Observacion = null;
		OrdenInsertar.CodigoModoPago = null;
		Orden.NombreModoPago = "";
        OrdenInsertar.CodigoPlazoCredito = null;
		Orden.NombrePlazoCredito = "";
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
            case "nota":
				if (tipoAccion is "I" or "M") 
					NotaContext = new EditContext(NotaInsertar);  
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
        VerboAccionModal = DescripcionAccionModal = TipoAccionModal = CodigoItemAccion = "";
        StateHasChanged();
    }

    private void Volver() => INavigation.NavigateTo($"{rutaEmpresa}{rutaServicio}");
 
    public void CerrarDialog()
    {
        if (OrigenDialog is "detalle")
            EsVisibleDialogDetalle = false;
        else if (OrigenDialog is "nota")
            EsVisibleDialogNota = false;
        TipoAccionDialog = OrigenDialog = "";
        StateHasChanged();
    }


	#region CatalogoSolicitudes
	public async Task MostrarCatalogoSolicitudes()
	{
        if (AgregarItemDetalleEsValido())
        {
            IsLoadingCatalogoSolicitudes = true;

			CatalogoEjercicios = await IEmpresa.CatalogoEjercicios(Empresa.Codigo) ?? [];
			CodigoEjercicio = await IEmpresa.ConsultaEjercicioCodigoPorAnio(Empresa.Codigo, DateTime.Now.Year);
			ItemsSelectedSolicitud = [];

			await CargarCatalogoSolicitudes(); 
			EsVisibleCatalogoSolicitudes = true;
		}
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

			CatalogoSolicitudes = (List<SolicitudCatalogoAtenderDto>) await ISolicitud.CatalogoAtender(Empresa.Codigo, CodigoEjercicio, CodigoProcesoDocumento, CodigoLocalNumerador);
			 
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

        elements = [.. ItemsSelectedSolicitud.GroupBy(x => x.CodigoEntidad)];
        if (elements.Count > 1)
        {
            Fnc.MostrarAlerta(AlertCatalogoSolicitudes, "No puede elegir ítems de distintos proveedores", "error");
            elements.ForEach(x => x.ToList().ForEach(x => x.IsErrorSelected = true));
            isErrorSelectedCatalogo = true;
        }
        else if (!string.IsNullOrEmpty(OrdenInsertar.CodigoEntidad) && OrdenInsertar.CodigoEntidad != ItemsSelectedSolicitud.Select(x => x.CodigoEntidad).First())
        {
            Fnc.MostrarAlerta(AlertCatalogoSolicitudes, "Solo podrá elegir ítems del mismo proveedor ya seleccionado", "error");
            ItemsSelectedSolicitud.ToList().ForEach(x => x.IsErrorSelected = true);
            isErrorSelectedCatalogo = true;
        }
        
        if (isErrorSelectedCatalogo)
            return;

        string codigoPaisOrigen = ItemsSelectedSolicitud.Select(x => x.CodigoPaisOrigen).First();
        if (string.IsNullOrEmpty(OrdenInsertar.CodigoPaisOrigen) && !ItemsSelectedSolicitud.Any(x => x.CodigoPaisOrigen != codigoPaisOrigen))
        {
            OrdenInsertar.CodigoPaisOrigen = codigoPaisOrigen;
            Orden.NombrePaisOrigen = ItemsSelectedSolicitud.Select(x => x.NombrePaisOrigen).First();
            EditContext.NotifyFieldChanged(EditContext.Field("CodigoPaisOrigen"));
		}

		string codigoPaisProcedencia = ItemsSelectedSolicitud.Select(x => x.CodigoPaisProcedencia).First();
        if (string.IsNullOrEmpty(OrdenInsertar.CodigoPaisProcedencia) && !ItemsSelectedSolicitud.Any(x => x.CodigoPaisProcedencia != codigoPaisProcedencia))
        {
            OrdenInsertar.CodigoPaisProcedencia = codigoPaisProcedencia;
            Orden.NombrePaisProcedencia = ItemsSelectedSolicitud.Select(x => x.NombrePaisProcedencia).First();
            EditContext.NotifyFieldChanged(EditContext.Field("CodigoPaisProcedencia"));
        }

        DateTime fechaEstimadaETA = ItemsSelectedSolicitud.Select(x => x.FechaEstimadaETA).First();
        if (!OrdenInsertar.FechaEstimadaETA.HasValue && !ItemsSelectedSolicitud.Any(x => x.FechaEstimadaETA != fechaEstimadaETA))
        {
            OrdenInsertar.FechaEstimadaETA = fechaEstimadaETA; 
            EditContext.NotifyFieldChanged(EditContext.Field("FechaEstimadaETA"));
        }

        DateTime fechaEstimadaETD = ItemsSelectedSolicitud.Select(x => x.FechaEstimadaETD).First();
        if (!OrdenInsertar.FechaEstimadaETD.HasValue && !ItemsSelectedSolicitud.Any(x => x.FechaEstimadaETD != fechaEstimadaETD))
        {
            OrdenInsertar.FechaEstimadaETD = fechaEstimadaETD;
            EditContext.NotifyFieldChanged(EditContext.Field("FechaEstimadaETD"));
        }

        DateTime fechaReposicionStock = ItemsSelectedSolicitud.Select(x => x.FechaReposicionStock).First();
        if (!OrdenInsertar.FechaReposicionStock.HasValue && !ItemsSelectedSolicitud.Any(x => x.FechaReposicionStock != fechaReposicionStock))
        {
            OrdenInsertar.FechaReposicionStock = fechaReposicionStock;
            EditContext.NotifyFieldChanged(EditContext.Field("FechaReposicionStock"));
        }
        
		if (string.IsNullOrEmpty(OrdenInsertar.CodigoEntidad))
		{
            SolicitudCatalogoAtenderDto itemCatalogo = ItemsSelectedSolicitud.Where(x => x.CodigoEntidad == ItemsSelectedSolicitud.Select(x => x.CodigoEntidad).First()).First();
            OrdenInsertar.CodigoEntidad = Orden.CodigoEntidad = itemCatalogo.CodigoEntidad;
            Orden.NombreEntidad = itemCatalogo.NombreEntidad;
            OrdenInsertar.CodigoModoPago = itemCatalogo.CodigoModoPago;
            Orden.NombreModoPago = itemCatalogo.NombreModoPago;
            OrdenInsertar.CodigoPlazoCredito = itemCatalogo.CodigoPlazoCredito;
            Orden.NombrePlazoCredito = itemCatalogo.NombrePlazoCredito;
            Validator.MsgErrorEntidad = null;
            EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidad"));
        }

		string observacion = ItemsSelectedSolicitud.Select(x => x.Observacion).First();
        if (string.IsNullOrEmpty(OrdenInsertar.Observacion) && !string.IsNullOrEmpty(observacion) && !ItemsSelectedSolicitud.Any(x => x.Observacion != observacion))
        {
            OrdenInsertar.Observacion = observacion;
            EditContext.NotifyFieldChanged(EditContext.Field("Observacion"));
        }

		GridState<OrdenDetalleGrid> detalleState = GridDetalleRef.GetState();
        foreach (SolicitudCatalogoAtenderDto itemCatalogo in ItemsSelectedSolicitud)
        {
			DetalleInsertar = new();
			Detalle = new();

			IMapper.Map(itemCatalogo, Detalle);
			IMapper.Map(Detalle, DetalleInsertar);

			ItemGridDetalle = IMapper.Map<OrdenDetalleGrid>(Detalle); 

			OrdenInsertar.Detalles.Add(DetalleInsertar);
			GridDetalles.Add(ItemGridDetalle);
			detalleState.InsertedItem = ItemGridDetalle;

			await GridDetalleRef.SetStateAsync(detalleState);
		}

        EsVisibleBotonDocumento = GridDetalles.Count == 0; 
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

    private bool AgregarItemDetalleEsValido()
    {
		bool esValido = true;
		if (string.IsNullOrEmpty(OrdenInsertar.CodigoDocumento))
		{
			Fnc.MostrarAlerta(Alert, "Es necesario que seleccione el numerador de [Documento]", "error");
			esValido = false;
		}
		if (string.IsNullOrEmpty(OrdenInsertar.CodigoMoneda))
		{
			Fnc.MostrarAlerta(Alert, "Es necesario que seleccione el código de moneda", "error");
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

    public void MostrarModificarDetalle(OrdenDetalleGrid item = null)
    {
		if (TipoAccionModal is not "V")
			Detalle = IMapper.Map<OrdenDetalleObtenerDto>(item);

		DetalleInsertar = IMapper.Map<OrdenDetalleInsertarDto>(Detalle);
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

	public void VerItemDetalle(OrdenDetalleGrid item)
	{
		Detalle = IMapper.Map<OrdenDetalleObtenerDto>(item);
		CodigoItemAccion = item.CodigoArticulo;
		IniciarAccionModal("V", "detalle");
	}

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
        int index = GridDetalles.FindIndex(i => i.CodigoArticulo == item.CodigoArticulo);
        
        if (args.Field == "PrecioUnitario")
            OrdenInsertar.Detalles[index].PrecioUnitario = GridDetalles[index].PrecioUnitario = item.PrecioUnitario; 
        else if (args.Field == "Cantidad")
            OrdenInsertar.Detalles[index].Cantidad = GridDetalles[index].Cantidad = item.Cantidad;

        ModificarImportesItem(item.Cantidad, item.PrecioUnitario, index);
		ModificarImportesTotales();
		IsEditingGridDetalle = false;
	}

    private void ModificarImportesItem(decimal? cantidad, decimal? precioUnitario, int indexGrid = -1)
    {
        decimal? importeBruto = null;
        if (cantidad.HasValue && precioUnitario.HasValue)
            importeBruto = Math.Round((decimal)(cantidad * precioUnitario), 2, MidpointRounding.AwayFromZero);
        if (indexGrid > -1) 
            OrdenInsertar.Detalles[indexGrid].ImporteBruto = GridDetalles[indexGrid].ImporteBruto = importeBruto;  
        else 
            DetalleInsertar.ImporteBruto = importeBruto; 
    }

    private void ModificarImportesTotales() => OrdenInsertar.TotalImporteBruto = GridDetalles.Count == 0 ? 0 : GridDetalles.Sum(x => x.ImporteBruto);

    private void OnChangeDetalleObservacionHandler(object value) => IsModifiedDetalle = Fnc.VerifyContextIsChanged((Detalle.Observacion ?? "") != ((string)value ?? ""), DetalleContext, "Observacion");

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
                (ArticuloObtenerPorCodigoEmpresaProcesoDocumentoDto item, DetalleValidator.MsgErrorArticulo) = await IUtil.ObtenerArticuloPorCodigoEmpresaProcesoDocumento(AlertDetalle, codigo, Empresa.Codigo, CodigoProcesoDocumento);
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
        GridState<OrdenDetalleGrid> detalleState = GridDetalleRef.GetState();
        int index = GridDetalles.FindIndex(i => i.CodigoArticulo == CodigoItemAccion);

        switch (tipoAccion)
        {
            case "I" or "M":
				ItemGridDetalle = IMapper.Map<OrdenDetalleGrid>(IMapper.Map(DetalleInsertar, Detalle));
				if (tipoAccion == "I")
                {
                    OrdenInsertar.Detalles.Add(DetalleInsertar);
                    GridDetalles.Add(ItemGridDetalle);
                    detalleState.InsertedItem = ItemGridDetalle;
                }
                else
                {
                    OrdenInsertar.Detalles[index] = DetalleInsertar;
                    GridDetalles[index] = ItemGridDetalle; 
				} 
                break;
            case "Q":
                OrdenInsertar.Detalles.RemoveAt(index);
                GridDetalles.RemoveAt(index);
                CerrarDialog();
                break;
        };
        CerrarModal();
        ModificarImportesTotales();
        
        EsVisibleBotonDocumento = GridDetalles.Count == 0;
        if (EsVisibleBotonDocumento && OrdenInsertar.FlagOrigen is "S")
            LimpiarCamposPorOrigenSolicitud();

        EsVisibleBotonDocumento = GridDetalles.Count == 0;
        await GridDetalleRef.SetStateAsync(detalleState);
    }
    #endregion

    #region Nota
    private async Task CargarEmpresaNotasOrdenImportacion()
    {
		IEnumerable<NotaReporteOrdenCatalogoDto> notas = await INota.Catalogo(Empresa.Codigo);
		if (notas is not null)
		{
			foreach (NotaReporteOrdenCatalogoDto item in notas)
			{
				Nota = new()
                {
                    FlagSeccion = item.FlagSeccion,
                    NombreNotaReporteOrden = item.NombreNotaReporteOrden
                };
				NotaInsertar = new()
                { 
                    CodigoNotaReporteOrden = item.CodigoNotaReporteOrden,
                    Contenido = item.Contenido
                };
				IMapper.Map(NotaInsertar, Nota);
				ItemGridNota = IMapper.Map<OrdenNotaGrid>(Nota);
				OrdenInsertar.Notas.Add(NotaInsertar);
				GridNotas.Add(ItemGridNota);
			}
		}
	}
    
    private void VisibleListaNotaChangedHandler(bool esVisible) => EsVisibleListaNotas = esVisible;

	public void InvalidarAccionNota(EditContext editContext) => Fnc.MostrarAlerta(AlertNota, Cnf.MsgErrorInvalidEditContext, "error");

    private async Task CerrarNota()
    {
        if (TipoAccionModal is not "V" && IsModifiedNota && !await Dialog.ConfirmAsync("¿Está seguro de cerrar el formulario y que los datos no se carguen?", "Saliendo del formulario"))
            return;
        CerrarModal();
    }

    public void MostrarAgregarNota()
	{
		NotaInsertar = new();
		CodigoItemAccion = "";
		Nota = new();
        NotaValidator = new();
		IniciarAccionModal("I", "nota");
	}

	public void MostrarModificarNota(OrdenNotaGrid item = null)
	{
		if (TipoAccionModal is not "V")
			Nota = IMapper.Map<OrdenNotaObtenerDto>(item);
		NotaInsertar = IMapper.Map<OrdenNotaInsertarDto>(Nota);
        NotaValidator = new();
		CodigoItemAccion = Nota.CodigoNotaReporteOrden;
		IniciarAccionModal("M", "nota");
	}

	private void MostrarQuitarNota(string codigoItem)
	{
		CodigoItemAccion = codigoItem;
		IniciarAccionDialog("Q", "nota");
	}

	private void OnChangeNotaContenidoHandler(object value) => IsModifiedNota = Fnc.VerifyContextIsChanged((Nota.Contenido ?? "") != ((string)value ?? ""), NotaContext, "Contenido");

    private void ValueChangeNotaReporteOrdenHandler(string value)
    {
        NotaInsertar.CodigoNotaReporteOrden = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(NotaValidator.MsgErrorNotaReporteOrden) && (Nota.CodigoNotaReporteOrden ?? "").Trim() != (NotaInsertar.CodigoNotaReporteOrden ?? ""))
            Nota.CodigoNotaReporteOrden = NotaValidator.MsgErrorNotaReporteOrden = null;
    }

    private async Task OnChangeNotaReporteOrdenHandler(object value)
	{
		string codigo = (string)(value ?? "");
		if (NotaContext.IsValid(NotaContext.Field("CodigoNotaReporteOrden")))
		{
			if ((NotaInsertar.CodigoNotaReporteOrden ?? "") == codigo) goto exit;
			if (GridNotas.Any(x => x.CodigoNotaReporteOrden == codigo))
			{
				Fnc.MostrarAlerta(AlertNota, "El código ya existe en las notas", "error");
			}
			else
			{ 
				(NotaReporteOrdenObtenerPorCodigoDto item, NotaValidator.MsgErrorNotaReporteOrden) = await IUtil.ObtenerNotaReporteOrdenImportacionPorCodigo(AlertNota, Empresa.Codigo, codigo);
                if (item is not null)
                {
                    Nota.CodigoNotaReporteOrden = item.CodigoNotaReporteOrden;
					Nota.NombreNotaReporteOrden = item.NombreNotaReporteOrden;
					Nota.FlagSeccion = item.FlagSeccion;
					Nota.Contenido = item.Contenido;
					goto exit; 
                }
			}
            NotaContext.NotifyFieldChanged(NotaContext.Field("CodigoNotaReporteOrden"));
		}
        Nota.CodigoNotaReporteOrden = codigo;
		Nota.NombreNotaReporteOrden = Nota.FlagSeccion = Nota.Contenido = null;
		if (string.IsNullOrEmpty(codigo))
			NotaContext.MarkAsUnmodified(NotaContext.Field("CodigoNotaReporteOrden"));
	exit:
		IsModifiedNota = NotaContext.IsModified();
	}

	private async Task AccionarNota(string tipoAccion)
	{
		GridState<OrdenNotaGrid> notaState = GridNotaRef.GetState();
		int index = GridNotas.FindIndex(i => i.CodigoNotaReporteOrden.Trim() == CodigoItemAccion);

		switch (tipoAccion)
		{
			case "I" or "M": 
				ItemGridNota = IMapper.Map<OrdenNotaGrid>(IMapper.Map(NotaInsertar, Nota));
				if (tipoAccion == "I")
				{
					OrdenInsertar.Notas.Add(NotaInsertar);
					GridNotas.Add(ItemGridNota);
					notaState.InsertedItem = ItemGridNota;
				}
				else
				{
					OrdenInsertar.Notas[index] = NotaInsertar;
					GridNotas[index] = ItemGridNota;
				}
				break;
			case "Q":
				OrdenInsertar.Notas.RemoveAt(index);
				GridNotas.RemoveAt(index);
				CerrarDialog();
				break;
		};
        CerrarModal();
        IsModified = true;
		await GridNotaRef.SetStateAsync(notaState);
	}
	#endregion

	#region Catalogos
	private void CargarItemCatalogoSerieDocumento(SerieDocumentoCatalogoPorEmpresaSesionDto item)
    {
        OrdenInsertar.CodigoDocumento = item.CodigoDocumento;
        OrdenInsertar.CodigoSerieDocumento = item.CodigoSerieDocumento;
        Orden.NombreSerieDocumento = item.NombreSerieDocumento;

		if (OrdenInsertar.FlagOrigen is "S")
			LimpiarCamposPorOrigenSolicitud();

        CodigoProcesoDocumento = item.CodigoProcesoDocumento; 

        EditContext.NotifyFieldChanged(EditContext.Field("CodigoDocumento"));
        IsModified = EditContext.IsModified();
    }
     
    private void CargarItemCatalogoProveedor(EntidadProveedorCatalogoPorEmpresaDto item)
    {
        OrdenInsertar.CodigoEntidad = Orden.CodigoEntidad = item.CodigoEntidad;
        Orden.NombreEntidad = item.NombreEntidad;
        OrdenInsertar.CodigoModoPago = item.CodigoModoPago;
        Orden.NombreModoPago = item.NombreModoPago;
        OrdenInsertar.CodigoPlazoCredito = item.CodigoPlazoCredito;
        Orden.NombrePlazoCredito = item.NombrePlazoCredito;
        Validator.MsgErrorEntidad = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidad"));
        IsModified = EditContext.IsModified();
    }

    private void CargarItemCatalogoArticuloPorProcesoDocumento(ArticuloCatalogoPorEmpresaProcesoDocumentoDto item)
    { 
        DetalleInsertar.CodigoArticulo = Detalle.CodigoArticulo = item.CodigoArticulo.Trim();
        Detalle.NombreArticulo = item.NombreArticulo; 
        Detalle.UnidadConversion = DetalleValidator.UnidadConversion = item.UnidadConversion; 
        Detalle.CodigoUnidadMedida = item.CodigoUnidadMedida;
        Detalle.NombreUnidadMedida = item.NombreUnidadMedida;
        Detalle.Presentacion = item.Presentacion;
        Detalle.CodigoTipoArticulo = item.CodigoTipoArticulo;
        Detalle.NombreTipoArticulo = item.NombreTipoArticulo;
        DetalleValidator.MsgErrorArticulo = null;
        DetalleContext.NotifyFieldChanged(DetalleContext.Field("CodigoArticulo"));
        IsModifiedDetalle = DetalleContext.IsModified();
        if (DetalleInsertar.Cantidad.HasValue)
            DetalleContext.NotifyFieldChanged(DetalleContext.Field("Cantidad")); 
    }  

    private void CargarItemCatalogoPaisOrigen(PaisCatalogoDto item)
    {
        OrdenInsertar.CodigoPaisOrigen = item.CodigoPais;
        Orden.NombrePaisOrigen = item.NombrePais;
        Validator.MsgErrorPaisOrigen = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoPaisOrigen"));
        IsModified = EditContext.IsModified();
    }

    private void CargarItemCatalogoPaisProcedencia(PaisCatalogoDto item)
    {
        OrdenInsertar.CodigoPaisProcedencia = item.CodigoPais;
        Orden.NombrePaisProcedencia = item.NombrePais;
        Validator.MsgErrorPaisProcedencia = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoPaisProcedencia"));
        IsModified = EditContext.IsModified();
    }

	private void CargarItemCatalogoAduana(AduanaCatalogoDto item)
	{
		OrdenInsertar.CodigoAduana = item.CodigoAduana;
		Orden.NombreAduana = item.NombreAduana;
        Validator.MsgErrorAduana = null;
		EditContext.NotifyFieldChanged(EditContext.Field("CodigoAduana"));
		IsModified = EditContext.IsModified();
	}

	private void CargarItemCatalogoTipoImportacion(TipoImportacionCatalogoDto item)
	{
		OrdenInsertar.CodigoTipoImportacion = item.CodigoTipoImportacion;
		Orden.NombreTipoImportacion = item.NombreTipoImportacion;
        Validator.MsgErrorTipoImportacion = null;
		EditContext.NotifyFieldChanged(EditContext.Field("CodigoTipoImportacion"));
		IsModified = EditContext.IsModified();
	}

	private void CargarItemCatalogoNotaReporteOrden(NotaReporteOrdenCatalogoDto item)
	{
        NotaInsertar.CodigoNotaReporteOrden = Nota.CodigoNotaReporteOrden = item.CodigoNotaReporteOrden;
		Nota.NombreNotaReporteOrden = item.NombreNotaReporteOrden;
        Nota.FlagSeccion = item.FlagSeccion;
        Nota.Contenido = item.Contenido;
        NotaValidator.MsgErrorNotaReporteOrden = null;
		NotaContext.NotifyFieldChanged(NotaContext.Field("CodigoNotaReporteOrden"));
		IsModifiedNota = NotaContext.IsModified();
	}

	private void CargarItemCatalogoMoneda(MonedaCatalogoDto item)
	{
		OrdenInsertar.CodigoMoneda = Orden.CodigoMoneda = item.CodigoMoneda;
		Orden.NombreMoneda = item.CodigoMoneda;
		EditContext.NotifyFieldChanged(EditContext.Field("CodigoMoneda"));
		IsModified = EditContext.IsModified();
	}
	#endregion

	#region EditContextHandlers
	private void ValueChangeEntidadHandler(string value)
    {
        OrdenInsertar.CodigoEntidad = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorEntidad) && (Orden.CodigoEntidad ?? "").Trim() != (OrdenInsertar.CodigoEntidad ?? ""))
            Orden.CodigoEntidad = Validator.MsgErrorEntidad = null;
    }

	private async Task OnChangeEntidadHandler(object value)
	{
		string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoEntidad")))
		{ 
			if ((Orden.CodigoEntidad ?? "").Trim() == codigo) goto exit; 
			(EntidadProveedorObtenerPorCodigoEmpresaDto item, Validator.MsgErrorEntidad) = await IUtil.ObtenerProveedorPorCodigoEmpresa(Alert, codigo, Empresa.Codigo, "I");
			if (item is not null)
			{
				Orden.CodigoEntidad = item.CodigoEntidad.Trim();
				Orden.NombreEntidad = item.NombreEntidad;
				OrdenInsertar.CodigoModoPago = item.CodigoModoPago;
				Orden.NombreModoPago = item.NombreModoPago;
                OrdenInsertar.CodigoPlazoCredito = item.CodigoPlazoCredito;
				Orden.NombrePlazoCredito = item.NombrePlazoCredito;
				goto exit;
			}
            EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidad"));
		}
        Orden.CodigoEntidad = codigo;
		Orden.NombreEntidad = OrdenInsertar.CodigoModoPago = Orden.NombreModoPago = OrdenInsertar.CodigoPlazoCredito = Orden.NombrePlazoCredito = null;
		if (string.IsNullOrEmpty(codigo)) EditContext.MarkAsUnmodified(EditContext.Field("CodigoEntidad"));
	exit:
		IsModified = EditContext.IsModified();
	}

    private void ValueChangeAduanaHandler(string value)
    {
        OrdenInsertar.CodigoAduana = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorAduana) && (Orden.CodigoAduana ?? "").Trim() != (OrdenInsertar.CodigoAduana ?? ""))
            Orden.CodigoAduana = Validator.MsgErrorAduana = null;
    }

    private async Task OnChangeAduanaHandler(object value)
	{
		string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoAduana")))
		{
			if ((Orden.CodigoAduana ?? "") == codigo) goto exit; 
			(AduanaObtenerPorCodigoDto item, Validator.MsgErrorAduana) = await IUtil.ObtenerAduanaPorCodigo(Alert, codigo);
            if(item is not null)
            {
                Orden.CodigoAduana = item.CodigoAduana;
                Orden.NombreAduana = item.NombreAduana;
                goto exit;
            } 
            EditContext.NotifyFieldChanged(EditContext.Field("CodigoAduana"));
        }
        Orden.CodigoAduana = codigo;
		Orden.NombreAduana = null;
		if (string.IsNullOrEmpty(codigo)) EditContext.MarkAsUnmodified(EditContext.Field("CodigoAduana"));
	exit:
		IsModified = EditContext.IsModified();
	}

    private void ValueChangeTipoImportacionHandler(string value)
    {
        OrdenInsertar.CodigoTipoImportacion = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorTipoImportacion) && (Orden.CodigoTipoImportacion ?? "").Trim() != (OrdenInsertar.CodigoTipoImportacion ?? ""))
            Orden.CodigoTipoImportacion = Validator.MsgErrorTipoImportacion = null;
    }

    private async Task OnChangeTipoImportacionHandler(object value)
	{
		string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoTipoImportacion")))
		{
			if ((Orden.CodigoTipoImportacion ?? "") == codigo) goto exit; 
			(TipoImportacionObtenerPorCodigoDto item, Validator.MsgErrorTipoImportacion) = await IUtil.ObtenerTipoImportacionPorCodigo(Alert, codigo);
            if(item is not null)
            {
                Orden.CodigoTipoImportacion = item.CodigoTipoImportacion;
                Orden.NombreTipoImportacion = item.NombreTipoImportacion;
                goto exit;
            } 
            EditContext.NotifyFieldChanged(EditContext.Field("CodigoTipoImportacion"));
        }
        Orden.CodigoTipoImportacion = codigo;
		Orden.NombreTipoImportacion = null;
		if (string.IsNullOrEmpty(codigo)) EditContext.MarkAsUnmodified(EditContext.Field("CodigoTipoImportacion"));
	exit:
		IsModified = EditContext.IsModified();
	}

    private void ValueChangePaisOrigenHandler(string value)
    {
        OrdenInsertar.CodigoPaisOrigen = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorPaisOrigen) && (Orden.CodigoPaisOrigen ?? "").Trim() != (OrdenInsertar.CodigoPaisOrigen ?? ""))
            Orden.CodigoPaisOrigen = Validator.MsgErrorPaisOrigen = null;
    }

    private async Task OnChangePaisOrigenHandler(object value)
	{
		string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoPaisOrigen")))
		{
			if ((Orden.CodigoPaisOrigen ?? "") == codigo) goto exit;  
			(PaisObtenerPorCodigoDto item, Validator.MsgErrorPaisOrigen) = await IUtil.ObtenerPaisPorCodigo(Alert, codigo);
            if(item is not null)
            {
                Orden.CodigoPaisOrigen = item.CodigoPais;
                Orden.NombrePaisOrigen = item.NombrePais;
                goto exit;
            } 
            EditContext.NotifyFieldChanged(EditContext.Field("CodigoPaisOrigen"));
        }
        Orden.CodigoPaisOrigen = codigo;
		Orden.NombrePaisOrigen = null;
		if (string.IsNullOrEmpty(codigo)) EditContext.MarkAsUnmodified(EditContext.Field("CodigoPaisOrigen"));
	exit:
		IsModified = EditContext.IsModified();
	}

    private void ValueChangePaisProcedenciaHandler(string value)
    {
        OrdenInsertar.CodigoPaisProcedencia = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorPaisProcedencia) && (Orden.CodigoPaisProcedencia ?? "").Trim() != (OrdenInsertar.CodigoPaisProcedencia ?? ""))
            Orden.CodigoPaisProcedencia = Validator.MsgErrorPaisProcedencia = null;
    }

    private async Task OnChangePaisProcedenciaHandler(object value)
	{
		string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoPaisProcedencia")))
		{
            if ((Orden.CodigoPaisProcedencia ?? "") == codigo) goto exit;
            (PaisObtenerPorCodigoDto item, Validator.MsgErrorPaisOrigen) = await IUtil.ObtenerPaisPorCodigo(Alert, codigo);
            if (item is not null)
            { 
				Orden.CodigoPaisProcedencia = item.CodigoPais;
				Orden.NombrePaisProcedencia = item.NombrePais;
				goto exit;
			} 
            EditContext.NotifyFieldChanged(EditContext.Field("CodigoPaisProcedencia"));
        }
        Orden.CodigoPaisProcedencia = codigo;
		Orden.NombrePaisProcedencia = null;
		if (string.IsNullOrEmpty(codigo)) EditContext.MarkAsUnmodified(EditContext.Field("CodigoPaisProcedencia"));
	exit:
	    IsModified = EditContext.IsModified();
    }

	private void OnChangeObservacionHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "Observacion");

	private void OnChangeDescripcionLugarEntregaHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "DescripcionLugarEntrega");

	private void OnChangeDescripcionFormaPagoHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "DescripcionFormaPago");

	private void OnChangeFlagMedioPagoHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "FlagMedioPago");

	private void OnChangeFechaReposicionStockHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(value is not null, EditContext, "FechaReposicionStock");

	private void OnChangeFechaEstimadaETAHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(value is not null, EditContext, "FechaEstimadaETA");

	private void OnChangeFechaEstimadaETDHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(value is not null, EditContext, "FechaEstimadaETD");

	private void OnChangeFechaEmisionHandler(object value) => IsModified = Fnc.VerifyContextIsChanged((value is null && Orden.FechaEmision.HasValue) || (!Orden.FechaEmision.HasValue && value is not null) || (Orden.FechaEmision.HasValue && value is not null && Orden.FechaEmision != (DateTime?)value), EditContext, "FechaEmision");
	#endregion

	public void Dispose() => GC.SuppressFinalize(this);
}
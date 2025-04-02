using Blazored.FluentValidation;
using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Compra;
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

namespace GestionERP.Web.Pages.Empresa.Compra.Orden;

public partial class Insert : IDisposable
{
    #region Propiedades
    private const string codigoModulo = "01";
    private const string codigoServicio = "S103";
    private const string rutaAccion = "/emitir";
    private string rutaEmpresa = "";
    private string pathBaseUri = "";
    private const string rutaServicio = "/ordenes";
     
	public OrdenInsertarDto OrdenInsertar { get; set; }
    public OrdenObtenerDto Orden { get; set; }
    public OrdenDetalleInsertarDto DetalleInsertar { get; set; }
    public OrdenDetalleObtenerDto Detalle { get; set; }
    public List<OrdenDetalleGrid> GridDetalles { get; set; }
	public OrdenDetalleGrid ItemGridDetalle { get; set; }
	private EditContext EditContext { get; set; }
    private EditContext DetalleContext { get; set; }
    public OrdenDetalleGridValidator GridDetalleValidator { get; set; }
    public OrdenInsertarValidator Validator { get; set; }
    public OrdenDetalleInsertarValidator DetalleValidator { get; set; }
    public bool IsInitPage { get; set; }
    public bool IsEditingGridDetalle { get; set; }
    public string CodigoLocalNumerador { get; set; }
    public string CodigoProcesoDocumento { get; set; }
    public List<DateTime> FechasCerradoOperacion { get; set; }
    public EmpresaEjercicioConsultaIntervaloFechaDto FechaIntervalo { get; set; }
	private IEnumerable<OrdenFlag> Origenes { get; set; }
    private IEnumerable<OrdenFlag> MediosPago { get; set; } 
    public bool IsChangedArea { get; set; }
    public List<SolicitudCatalogoAtenderDto> CatalogoSolicitudes { get; set; }
    public IEnumerable<SolicitudCatalogoAtenderDto> ItemsSelectedSolicitud { get; set; }
    public string CodigoEjercicio { get; set; }  
    private IEnumerable<EmpresaEjercicioCatalogoDto> CatalogoEjercicios { get; set; }
    private TipoImpuestoConsultaEsPredeterminadoDto TipoImpuestoPredeterminado { get; set; }
    public string CodigoTipoDocumento { get; set; }
	public bool EsVisibleBotonDocumento { get; set; }
    public bool EsVisibleCatalogoSolicitudes { get; set; }
    private bool IsAuthUser { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool IsModified { get; set; }
    private bool IsModifiedDetalle { get; set; }
    private bool IsLoadingCatalogoSolicitudes { get; set; }
    public string VerboAccionModal { get; set; }
    public string DescripcionAccionModal { get; set; }
    public string TipoAccionModal { get; set; }
    public string TipoAccionDialog { get; set; }
    public string OrigenDialog { get; set; }
    public string OrigenModal { get; set; }
    public bool EsVisibleDialogDetalle { get; set; }
    public bool EsVisibleModalDetalle { get; set; }
    public string CodigoItemAccion { get; set; }
    public string TituloTipoOrigen { get; set; }
    public string CodigoPermisoEmitir { get; set; }
    public ISvgIcon IconoAccionModal { get; set; } 
    public TelerikNotification AlertDetalle { get; set; }
    public TelerikNotification Alert { get; set; }
    public TelerikNotification AlertCatalogoSolicitudes { get; set; }
    private TelerikGrid<OrdenDetalleGrid> GridDetalleRef { get; set; }
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

    [Inject] public ICompraOrden IOrden { get; set; }
	[Inject] public ICompraSolicitud ISolicitud { get; set; } 
    [Inject] public IPrincipalEmpresa IEmpresa { get; set; }
    [Inject] public IPrincipalUsuario IUsuario { get; set; }
    [Inject] public IPrincipalTipoImpuesto ITipoImpuesto { get; set; }
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
        Validator = new(); 
        await IniciarVistaOrigen();
    }

    private async Task IniciarVistaOrigen()
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
                Notify.Show($"No tiene permiso para emitir registros de [Órdenes de Compra] {TituloTipoOrigen}", "error");
                return;
            }

            await CargarConsultaUsuarioEmpresa();
            FechaIntervalo = await IEmpresa.ConsultaEjercicioIntervaloFecha(Empresa.Codigo);
            (await IEmpresa.ConsultaModuloPeriodoFechasEsCerradoOperacion(Empresa.Codigo, codigoModulo))?.ToList().ForEach(x => FechasCerradoOperacion.Add(x.Fecha));
            OrdenInsertar.FechaEmision = !FechasCerradoOperacion.Any(x => x == DateTime.Now.Date) ? DateTime.Now.Date : null;
            if (OrdenInsertar.FechaEmision.HasValue)
                Orden.FechaEmision = (DateTime)OrdenInsertar.FechaEmision;

            TipoImpuestoPredeterminado = await ITipoImpuesto.ConsultaEsPredeterminado() ?? new();

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
            Notify.Show("La orden de compra ha sido emitida con éxito en la empresa", "success");
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
        if (string.IsNullOrEmpty(OrdenInsertar.CodigoLocalRecepcion) && string.IsNullOrEmpty(OrdenInsertar.DescripcionLugarEntrega))
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
                if (GridDetalles.Any(x => !x.PrecioUnitario.HasValue))
                {
                    Fnc.MostrarAlerta(Alert, "Está pendiente completar alguna celda del [precio unitario] del detalle de la orden de compra", "error");
                    esValido = false;
                } 
            }  
        }
		
		return esValido;
    }

    private async Task CargarConsultaUsuarioEmpresa()
    {
        UsuarioConsultaPorEmpresaSesionDto usuarioEmpresa = await IUsuario.ConsultaPorEmpresaSesion(Empresa.Codigo);
        OrdenInsertar.CodigoArea = Orden.CodigoArea = usuarioEmpresa.CodigoArea;
        Orden.NombreArea = usuarioEmpresa.NombreArea; 
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
		OrdenInsertar.CodigoArea = null;
		Orden.NombreArea = "";
        Validator.MsgErrorArea = null;
		OrdenInsertar.Motivo = null;
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
		List<IGrouping<string, SolicitudCatalogoAtenderDto>> elements = ItemsSelectedSolicitud.GroupBy(x => x.CodigoArticulo).Where(x => x.Count() > 1).ToList();
		if (elements.Count > 0)
		{
			Fnc.MostrarAlerta(AlertCatalogoSolicitudes, "Existen ítems seleccionados de igual código", "error");
			elements.ForEach(x => x.ToList().ForEach(x => x.IsErrorSelected = true));
			isErrorSelectedCatalogo = true;
		}

		if (isErrorSelectedCatalogo)
			return;

		string codigoArea = ItemsSelectedSolicitud.Select(x => x.CodigoArea).First();
        if (string.IsNullOrEmpty(OrdenInsertar.CodigoArea) && !string.IsNullOrEmpty(codigoArea) && !ItemsSelectedSolicitud.Any(x => x.CodigoArea != codigoArea))
        {
            OrdenInsertar.CodigoArea = Orden.CodigoArea = codigoArea;
            Orden.NombreArea = ItemsSelectedSolicitud.Select(x => x.NombreArea).First();
            Validator.MsgErrorArea = null;
            EditContext.NotifyFieldChanged(EditContext.Field("CodigoArea"));
        }

        if (string.IsNullOrEmpty(CodigoLocalNumerador))
        {
            string codigoLocalRecepcion = ItemsSelectedSolicitud.Select(x => x.CodigoLocalRecepcion).First();
            if (string.IsNullOrEmpty(OrdenInsertar.CodigoLocalRecepcion) && !string.IsNullOrEmpty(codigoLocalRecepcion) && !ItemsSelectedSolicitud.Any(x => x.CodigoLocalRecepcion != codigoLocalRecepcion))
            {
                OrdenInsertar.CodigoLocalRecepcion = Orden.CodigoLocalRecepcion = codigoLocalRecepcion;
                Orden.NombreLocalRecepcion = ItemsSelectedSolicitud.Select(x => x.NombreLocalRecepcion).First();
                Validator.MsgErrorLocalRecepcion = null;
                EditContext.NotifyFieldChanged(EditContext.Field("CodigoLocalRecepcion"));
            }
        }

        string codigoEntidadProveedor = ItemsSelectedSolicitud.Where(x => !string.IsNullOrEmpty(x.CodigoEntidad)).Select(x => x.CodigoEntidad).FirstOrDefault();
        if (string.IsNullOrEmpty(OrdenInsertar.CodigoEntidad) && !string.IsNullOrEmpty(codigoEntidadProveedor) && !ItemsSelectedSolicitud.Where(x => !string.IsNullOrEmpty(x.CodigoEntidad)).Any(x => x.CodigoEntidad != codigoEntidadProveedor))
		{
            SolicitudCatalogoAtenderDto itemCatalogo = ItemsSelectedSolicitud.Where(x => x.CodigoEntidad == codigoEntidadProveedor).First();
            OrdenInsertar.CodigoEntidad = Orden.CodigoEntidad = itemCatalogo.CodigoEntidad;
            Orden.NombreEntidad = itemCatalogo.NombreEntidad;
            OrdenInsertar.CodigoModoPago = itemCatalogo.CodigoModoPago;
            Orden.NombreModoPago = itemCatalogo.NombreModoPago;
            OrdenInsertar.CodigoPlazoCredito = itemCatalogo.CodigoPlazoCredito;
            Orden.NombrePlazoCredito = itemCatalogo.NombrePlazoCredito;
            Validator.MsgErrorEntidad = null;
            EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidad"));
        }

		string motivo = ItemsSelectedSolicitud.Select(x => x.Motivo).First();
        if (string.IsNullOrEmpty(OrdenInsertar.Motivo) && !string.IsNullOrEmpty(motivo) && !ItemsSelectedSolicitud.Any(x => x.Motivo != motivo))
        {
            OrdenInsertar.Motivo = motivo;
            EditContext.NotifyFieldChanged(EditContext.Field("Motivo"));
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
		if (string.IsNullOrEmpty(OrdenInsertar.CodigoTipoProvision))
		{
			Fnc.MostrarAlerta(Alert, "Es necesario que seleccione el código de [Tipo de Provisión]", "error");
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

    private void OnChangeDetalleObservacionHandler(object value) => IsModifiedDetalle = Fnc.VerifyContextIsChanged((Detalle.Observacion ?? "") != ((string)value ?? ""), DetalleContext, "Observacion");

    private void OnChangeDetalleCantidadHandler(object value)
    {
        decimal? cantidad = (decimal?)value;
        IsModifiedDetalle = Fnc.VerifyContextIsChanged(Detalle.Cantidad != cantidad, DetalleContext, "Cantidad");
        ModificarImportesItem(cantidad, DetalleInsertar.PrecioUnitario, DetalleInsertar.EsAfectoImpuesto);
    }

    private void OnChangeDetallePrecioUnitarioHandler(object value)
    {
        decimal? precioUnitario = (decimal?)value;
        IsModifiedDetalle = Fnc.VerifyContextIsChanged(Detalle.PrecioUnitario != precioUnitario, DetalleContext, "PrecioUnitario");
        ModificarImportesItem(DetalleInsertar.Cantidad, precioUnitario, DetalleInsertar.EsAfectoImpuesto);
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
          
        ModificarImportesItem(item.Cantidad, item.PrecioUnitario, item.EsAfectoImpuesto, index);
		ModificarImportesTotales();
        IsEditingGridDetalle = false;
    }

    private void ModificarImportesItem(decimal? cantidad, decimal? precioUnitario, bool esAfectoImpuesto, int indexGrid = -1)
    {
        decimal? importeBruto = null, importeImpuesto = null, importeNeto = null;
        if (cantidad.HasValue && precioUnitario.HasValue)
        {
            importeBruto = Math.Round((decimal)(cantidad * precioUnitario), 2, MidpointRounding.AwayFromZero);
            importeImpuesto = Orden.EsAfectoImpuesto ? Math.Round((decimal)(importeBruto * (esAfectoImpuesto ? OrdenInsertar.PorcentajeImpuesto: 0)), 2, MidpointRounding.AwayFromZero) : 0;
            importeNeto = importeBruto + importeImpuesto;
        }
        if (indexGrid > -1)
        {
            OrdenInsertar.Detalles[indexGrid].ImporteBruto = GridDetalles[indexGrid].ImporteBruto = importeBruto;
            OrdenInsertar.Detalles[indexGrid].ImporteImpuesto = GridDetalles[indexGrid].ImporteImpuesto = importeImpuesto;
            OrdenInsertar.Detalles[indexGrid].ImporteNeto = GridDetalles[indexGrid].ImporteNeto = importeNeto;
        }
        else
        {
            DetalleInsertar.ImporteBruto = importeBruto;
            DetalleInsertar.ImporteImpuesto = importeImpuesto;
            DetalleInsertar.ImporteNeto = importeNeto;
        }
    }

    private void RecalcularImportes()
    {
        if (GridDetalles.Count > 0) 
        {
            foreach ((OrdenDetalleGrid item, int index) in GridDetalles.Select((item, index) => (item, index))) 
                ModificarImportesItem(item.Cantidad, item.PrecioUnitario, item.EsAfectoImpuesto, index); 

            ModificarImportesTotales(); 
        }
    }

    private void ModificarImportesTotales()
    {
        bool esGridVacio = GridDetalles.Count == 0;
        OrdenInsertar.TotalImporteBruto = esGridVacio ? 0 : GridDetalles.Sum(x => x.ImporteBruto);
        OrdenInsertar.TotalImporteImpuesto = esGridVacio ? 0 : GridDetalles.Sum(x => x.ImporteImpuesto);
        OrdenInsertar.TotalImporteNeto = esGridVacio ? 0 : GridDetalles.Sum(x => x.ImporteNeto);
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
                    if (Detalle.EsAfectoImpuesto != item.EsAfectoImpuesto)
                    {
                        DetalleInsertar.EsAfectoImpuesto = Detalle.EsAfectoImpuesto = item.EsAfectoImpuesto;
					    if (DetalleInsertar.Cantidad.HasValue && DetalleContext.IsValid(DetalleContext.Field("Cantidad")) && DetalleInsertar.PrecioUnitario.HasValue)
						    ModificarImportesItem(DetalleInsertar.Cantidad, DetalleInsertar.PrecioUnitario, DetalleInsertar.EsAfectoImpuesto);
                    }
				    goto exit;
			    }
            } 
            DetalleContext.NotifyFieldChanged(DetalleContext.Field("CodigoArticulo"));
		}
        Detalle.CodigoArticulo = codigo;
		Detalle.NombreArticulo = Detalle.CodigoUnidadMedida = Detalle.NombreUnidadMedida = Detalle.CodigoTipoArticulo = Detalle.NombreTipoArticulo = Detalle.Presentacion = null;
		Detalle.UnidadConversion = DetalleValidator.UnidadConversion = null;
		DetalleInsertar.EsAfectoImpuesto = Detalle.EsAfectoImpuesto = false; 
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

		await GridDetalleRef.SetStateAsync(detalleState);
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
		OrdenInsertar.CodigoLocalRecepcion = CodigoLocalNumerador = item.CodigoLocal;
        Orden.NombreLocalRecepcion = item.NombreLocal;
        CodigoProcesoDocumento = item.CodigoProcesoDocumento; 

		EditContext.NotifyFieldChanged(EditContext.Field("CodigoDocumento"));
        IsModified = EditContext.IsModified();
    }

    private void CargarItemCatalogoLocal(LocalCatalogoPorEmpresaDto item)
    {
        OrdenInsertar.CodigoLocalRecepcion = Orden.CodigoLocalRecepcion = item.CodigoLocal;
        Orden.NombreLocalRecepcion = item.NombreLocal; 
        Validator.MsgErrorLocalRecepcion = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoLocalRecepcion"));
        IsModified = EditContext.IsModified();
    }

    private void CargarItemCatalogoProveedor(EntidadProveedorCatalogoPorEmpresaDto item)
    {
        OrdenInsertar.CodigoEntidad = Orden.CodigoEntidad = item.CodigoEntidad.Trim();
        Orden.NombreEntidad = item.NombreEntidad;
        OrdenInsertar.CodigoModoPago = item.CodigoModoPago;
        Orden.NombreModoPago = item.NombreModoPago;
        OrdenInsertar.CodigoPlazoCredito = item.CodigoPlazoCredito;
        Orden.NombrePlazoCredito = item.NombrePlazoCredito;
        Validator.MsgErrorEntidad = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidad"));
        IsModified = EditContext.IsModified();
    }

    private void CargarItemCatalogoTipoProvision(TipoProvisionCatalogoDto item)
    {
        OrdenInsertar.CodigoTipoProvision = Orden.CodigoTipoProvision = item.CodigoTipoProvision;
		Orden.NombreTipoProvision = item.NombreTipoProvision;
		Orden.CodigoMoneda = item.CodigoMoneda;
		Orden.NombreMoneda = item.NombreMoneda;
		Orden.SimboloMoneda = item.SimboloMoneda;
        if (Orden.EsAfectoImpuesto != item.EsAfectoImpuesto)
        {
            Orden.EsAfectoImpuesto = Validator.EsAfectoImpuesto = item.EsAfectoImpuesto;
            if (Orden.EsAfectoImpuesto && string.IsNullOrEmpty(OrdenInsertar.CodigoTipoImpuesto))
            {
                OrdenInsertar.CodigoTipoImpuesto = TipoImpuestoPredeterminado.Codigo;
                Orden.NombreTipoImpuesto = TipoImpuestoPredeterminado.Nombre;
                OrdenInsertar.PorcentajeImpuesto = TipoImpuestoPredeterminado.Porcentaje;
                EditContext.NotifyFieldChanged(EditContext.Field("CodigoTipoImpuesto"));
            }
            else if (!Orden.EsAfectoImpuesto && !string.IsNullOrEmpty(OrdenInsertar.CodigoTipoImpuesto))
            {
                OrdenInsertar.CodigoTipoImpuesto = Orden.NombreTipoImpuesto = Validator.MsgErrorTipoImpuesto = null;
                OrdenInsertar.PorcentajeImpuesto = null;
                EditContext.MarkAsUnmodified(EditContext.Field("CodigoTipoImpuesto"));
            }
            RecalcularImportes();
        }
        Validator.MsgErrorTipoProvision = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoTipoProvision"));
        IsModified = EditContext.IsModified();
    }

    private void CargarItemCatalogoCentroCosto(CentroCostoCatalogoDto item)
    {
        OrdenInsertar.CodigoCentroCosto = Orden.CodigoCentroCosto = item.CodigoCentroCosto.Trim();
        Orden.NombreCentroCosto = item.NombreCentroCosto;
        Validator.MsgErrorCentroCosto = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoCentroCosto"));
        IsModified = EditContext.IsModified();
    }

    private void CargarItemCatalogoArea(AreaCatalogoDto item)
    {
        OrdenInsertar.CodigoArea = Orden.CodigoArea = item.CodigoArea;
        Orden.NombreArea = item.NombreArea;
        Validator.MsgErrorArea = null;
        IsChangedArea = true;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoArea"));
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
        DetalleInsertar.EsAfectoImpuesto = item.EsAfectoImpuesto;
        DetalleValidator.MsgErrorArticulo = null;
        DetalleContext.NotifyFieldChanged(DetalleContext.Field("CodigoArticulo"));
        IsModifiedDetalle = DetalleContext.IsModified();
        if (DetalleInsertar.Cantidad.HasValue)
            DetalleContext.NotifyFieldChanged(DetalleContext.Field("Cantidad"));
        ModificarImportesItem(DetalleInsertar.Cantidad, DetalleInsertar.PrecioUnitario, DetalleInsertar.EsAfectoImpuesto);
    }  

    private void CargarItemCatalogoTipoImpuesto(TipoImpuestoCatalogoDto item)
    {
        OrdenInsertar.CodigoTipoImpuesto = Orden.CodigoTipoImpuesto = item.CodigoTipoImpuesto;
        Orden.NombreTipoImpuesto = item.NombreTipoImpuesto;
        if ((Orden.PorcentajeImpuesto ?? 0) != item.PorcentajeImpuesto)
        {
            OrdenInsertar.PorcentajeImpuesto = Orden.PorcentajeImpuesto = item.PorcentajeImpuesto;
            RecalcularImportes();
        }
        Validator.MsgErrorTipoImpuesto = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoTipoImpuesto"));
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
            if ((Orden.CodigoEntidad ?? "") == codigo) goto exit;
			(EntidadProveedorObtenerPorCodigoEmpresaDto item, Validator.MsgErrorEntidad) = await IUtil.ObtenerProveedorPorCodigoEmpresa(Alert, codigo, Empresa.Codigo, "L");
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

    private void ValueChangeTipoProvisionHandler(string value)
    {
        OrdenInsertar.CodigoTipoProvision = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorTipoProvision) && (Orden.CodigoTipoProvision ?? "").Trim() != (OrdenInsertar.CodigoTipoProvision ?? ""))
            Orden.CodigoTipoProvision = Validator.MsgErrorTipoProvision = null;
    }

    private async Task OnChangeTipoProvisionHandler(object value)
    {
		string codigo = (string)(value ?? "");
        if (EditContext.IsValid(EditContext.Field("CodigoTipoProvision")))
		{
			if ((Orden.CodigoTipoProvision ?? "") == codigo) goto exit;
			(TipoProvisionObtenerPorCodigoDto item, Validator.MsgErrorTipoProvision) = await IUtil.ObtenerTipoProvisionPorCodigo(Alert, OrdenInsertar.CodigoTipoProvision);
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
                    if (Orden.EsAfectoImpuesto && string.IsNullOrEmpty(OrdenInsertar.CodigoTipoImpuesto))
                    {
                        OrdenInsertar.CodigoTipoImpuesto = TipoImpuestoPredeterminado.Codigo;
                        Orden.NombreTipoImpuesto = TipoImpuestoPredeterminado.Nombre;
                        OrdenInsertar.PorcentajeImpuesto = TipoImpuestoPredeterminado.Porcentaje;
                        EditContext.NotifyFieldChanged(EditContext.Field("CodigoTipoImpuesto"));
                    }
                    else if (!Orden.EsAfectoImpuesto && !string.IsNullOrEmpty(OrdenInsertar.CodigoTipoImpuesto))
                    {
                        OrdenInsertar.CodigoTipoImpuesto = Orden.NombreTipoImpuesto = Validator.MsgErrorTipoImpuesto = null;
                        OrdenInsertar.PorcentajeImpuesto = null;
                        EditContext.MarkAsUnmodified(EditContext.Field("CodigoTipoImpuesto"));
                    }
                    RecalcularImportes();                    
                }
				goto exit;
			} 
            EditContext.NotifyFieldChanged(EditContext.Field("CodigoTipoProvision")); 
		}
        Orden.CodigoTipoProvision = codigo;
		Orden.NombreTipoProvision = Orden.CodigoMoneda = Orden.NombreMoneda = Orden.SimboloMoneda = OrdenInsertar.CodigoTipoImpuesto = Orden.NombreTipoImpuesto = null;
		Orden.EsAfectoImpuesto = Validator.EsAfectoImpuesto = false;
		OrdenInsertar.PorcentajeImpuesto = null;
		if (string.IsNullOrEmpty(codigo))
			EditContext.MarkAsUnmodified(EditContext.Field("CodigoTipoProvision"));
	exit:
		IsModified = EditContext.IsModified();
	}

    private void ValueChangeLocalHandler(string value)
    {
        OrdenInsertar.CodigoLocalRecepcion = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorLocalRecepcion) && (Orden.CodigoLocalRecepcion ?? "").Trim() != (OrdenInsertar.CodigoLocalRecepcion ?? ""))
            Orden.CodigoLocalRecepcion = Validator.MsgErrorLocalRecepcion = null;
    }

    private async Task OnChangeLocalHandler(object value)
    { 
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoLocalRecepcion")))
		{ 
			if (!string.IsNullOrEmpty(codigo))
			{ 
                if ((Orden.CodigoLocalRecepcion ?? "") == codigo) goto exit;
                (LocalObtenerPorCodigoEmpresaDto item, Validator.MsgErrorLocalRecepcion) = await IUtil.ObtenerLocalPorCodigoEmpresa(Alert, codigo, Empresa.Codigo);
                if (item is not null)
                {
                    Orden.CodigoLocalRecepcion = item.CodigoLocal;
                    Orden.NombreLocalRecepcion = item.NombreLocal; 
                    goto exit;
                } 
                EditContext.NotifyFieldChanged(EditContext.Field("CodigoLocalRecepcion"));    
			}
			else
            {
                OrdenInsertar.CodigoLocalRecepcion = null;
				EditContext.MarkAsUnmodified(EditContext.Field("CodigoLocalRecepcion"));
            }
		}
        Orden.CodigoLocalRecepcion = codigo;
		Orden.NombreLocalRecepcion = null;
	exit:
		IsModified = EditContext.IsModified();
	}

    private void ValueChangeAreaHandler(string value)
    {
        OrdenInsertar.CodigoArea = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorArea) && (Orden.CodigoArea ?? "").Trim() != (OrdenInsertar.CodigoArea ?? ""))
            Orden.CodigoArea = Validator.MsgErrorArea = null;
    }

    private async Task OnChangeAreaHandler(object value)
    {
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoArea")))
		{ 
			if ((Orden.CodigoArea ?? "") != codigo)
			{
                IsChangedArea = true;
				(AreaObtenerPorCodigoDto item, Validator.MsgErrorArea) = await IUtil.ObtenerAreaPorCodigo(Alert, codigo);
				if (item is not null)
				{
					Orden.CodigoArea = item.CodigoArea;
					Orden.NombreArea = item.NombreArea;
					goto exit;
				} 
                EditContext.NotifyFieldChanged(EditContext.Field("CodigoArea")); 
			}
            else
            {
                if (!IsChangedArea) EditContext.MarkAsUnmodified(EditContext.Field("CodigoArea"));
                goto exit;
            }
		}
        Orden.CodigoArea = codigo;
		Orden.NombreArea = null; 
	exit:
		IsModified = EditContext.IsModified();
	}

    private void ValueChangeCentroCostoHandler(string value)
    {
        OrdenInsertar.CodigoCentroCosto = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorCentroCosto) && (Orden.CodigoCentroCosto ?? "").Trim() != (OrdenInsertar.CodigoCentroCosto ?? ""))
            Orden.CodigoCentroCosto = Validator.MsgErrorCentroCosto = null;
    }

    private async Task OnChangeCentroCostoHandler(object value)
    { 
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoCentroCosto")))
		{ 
			if (!string.IsNullOrEmpty(codigo))
			{ 
                if ((Orden.CodigoCentroCosto ?? "") == codigo) goto exit;
                (CentroCostoObtenerPorCodigoDto item, Validator.MsgErrorCentroCosto) = await IUtil.ObtenerCentroCostoPorCodigo(Alert, codigo, Empresa.Codigo);
                if (item is not null)
                {
                    Orden.CodigoCentroCosto = item.CodigoCentroCosto.Trim();
                    Orden.NombreCentroCosto = item.NombreCentroCosto; 
                    goto exit;
                } 
                EditContext.NotifyFieldChanged(EditContext.Field("CodigoCentroCosto"));   
			}
			else
            {
                OrdenInsertar.CodigoCentroCosto = null;
				EditContext.MarkAsUnmodified(EditContext.Field("CodigoCentroCosto"));
            }
		}
        Orden.CodigoCentroCosto = codigo;
		Orden.NombreCentroCosto = null;
	exit:
		IsModified = EditContext.IsModified();
    }

    private void ValueChangeTipoImpuestoHandler(string value)
    {
        OrdenInsertar.CodigoTipoImpuesto = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorTipoImpuesto) && (Orden.CodigoTipoImpuesto ?? "").Trim() != (OrdenInsertar.CodigoTipoImpuesto ?? ""))
            Orden.CodigoTipoImpuesto = Validator.MsgErrorTipoImpuesto = null;
    }

    private async Task OnChangeTipoImpuestoHandler(object value)
	{ 
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoTipoImpuesto")))
		{ 
			if ((Orden.CodigoTipoImpuesto ?? "") == codigo) goto exit;
			(TipoImpuestoObtenerPorCodigoDto item, Validator.MsgErrorTipoImpuesto) = await IUtil.ObtenerTipoImpuestoPorCodigo(Alert, codigo, true);
			if (item is not null)
			{
				Orden.CodigoTipoImpuesto = item.CodigoTipoImpuesto;
				Orden.NombreTipoImpuesto = item.NombreTipoImpuesto; 
                if ((Orden.PorcentajeImpuesto ?? 0) != item.PorcentajeImpuesto)
                {
                    OrdenInsertar.PorcentajeImpuesto = Orden.PorcentajeImpuesto = item.PorcentajeImpuesto;
                    RecalcularImportes();
                }
				goto exit;
			}
            EditContext.NotifyFieldChanged(EditContext.Field("CodigoTipoImpuesto"));
		}
        Orden.CodigoTipoImpuesto = codigo;
		Orden.NombreTipoImpuesto = null;
        OrdenInsertar.PorcentajeImpuesto = null; 
	exit:
		IsModified = EditContext.IsModified();
	}

	private void OnChangeObservacionHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "Observacion");

    private void OnChangeDescripcionLugarEntregaHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "DescripcionLugarEntrega");

    private void OnChangeMotivoHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "Motivo");

    private void OnChangeEsPagoAnticipadoHandler(object value) => IsModified = Fnc.VerifyContextIsChanged((bool)value, EditContext, "EsPagoAnticipado");

    private void OnChangeFlagMedioPagoHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "FlagMedioPago");

    private void OnChangeFechaEntregaHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(value is not null, EditContext, "FechaEntrega");

    private void OnChangeFechaEmisionHandler(object value)
    {
        IsModified = Fnc.VerifyContextIsChanged((value is null && Orden.FechaEmision.HasValue) || (!Orden.FechaEmision.HasValue && value is not null) || (Orden.FechaEmision.HasValue && value is not null && Orden.FechaEmision != (DateTime?)value), EditContext, "FechaEmision");
        if (OrdenInsertar.FechaEntrega.HasValue)
            EditContext.NotifyFieldChanged(EditContext.Field("FechaEntrega"));
    }
    #endregion

    public void Dispose() => GC.SuppressFinalize(this);
}
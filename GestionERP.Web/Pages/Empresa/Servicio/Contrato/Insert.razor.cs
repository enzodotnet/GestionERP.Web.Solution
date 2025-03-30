using Blazored.FluentValidation;
using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Servicio;
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

namespace GestionERP.Web.Pages.Empresa.Servicio.Contrato;

public partial class Insert : IDisposable
{
    #region Propiedades
    private const string codigoModulo = "03";
    private const string codigoServicio = "S110";
    private const string rutaAccion = "/emitir";
    private string rutaEmpresa = "";
    private string pathBaseUri = "";
    private const string rutaServicio = "/contratos";
    public FluentValidationValidator validator; 

	public ContratoInsertarDto ContratoInsertar { get; set; }
    public ContratoObtenerDto Contrato { get; set; }
    public ContratoDetalleInsertarDto DetalleInsertar { get; set; }
    public ContratoDetalleObtenerDto Detalle { get; set; }
    public List<ContratoDetalleGrid> GridDetalles { get; set; }
    public ContratoDetalleGrid ItemGridDetalle { get; set; }
    public ContratoCuotaInsertarDto CuotaInsertar { get; set; }
	public ContratoCuotaObtenerDto Cuota { get; set; }
	public List<ContratoCuotaGrid> GridCuotas { get; set; }
    public ContratoCuotaGrid ItemGridCuota { get; set; }
    public ContratoTerminoInsertarDto TerminoInsertar { get; set; }
	public ContratoTerminoObtenerDto Termino { get; set; }
	public List<ContratoTerminoGrid> GridTerminos { get; set; }
    public ContratoTerminoGrid ItemGridTermino { get; set; }
    private EditContext EditContext { get; set; }
    private EditContext DetalleContext { get; set; }
	private EditContext CuotaContext{ get; set; }
	private EditContext TerminoContext { get; set; }
    public ContratoInsertarValidator Validator { get; set; }
    public ContratoTerminoInsertarValidator TerminoValidator { get; set; }
    public ContratoDetalleInsertarValidator DetalleValidator { get; set; }
    public bool IsInitPage { get; set; }
	public bool IsEditingGridDetalle { get; set; }
	public bool IsEditingGridCuota { get; set; }
    public string CodigoLocalNumerador { get; set; }
    public string CodigoProcesoDocumento { get; set; }
    public List<DateTime> FechasCerradoOperacion { get; set; }
    public EmpresaEjercicioConsultaIntervaloFechaDto FechaIntervalo { get; set; }
    private IEnumerable<ContratoFlag> TiposRegistro { get; set; } 
    private string NombreTipoRegistro { get; set; }
	private IEnumerable<ContratoFlag> IntervalosInforme { get; set; }
	private IEnumerable<ContratoFlag> MediosPago { get; set; }
    public bool IsChangedArea { get; set; } 
    private TipoImpuestoConsultaEsPredeterminadoDto TipoImpuestoPredeterminado { get; set; }
    public bool EsVisibleBotonDocumento { get; set; }
	public string CodigoTipoDocumento { get; set; } 
    private bool IsAuthUser { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool IsModified { get; set; }
    private bool IsModifiedDetalle { get; set; }
	private bool IsModifiedTermino { get; set; }
	private bool IsModifiedCuota { get; set; }
	public string VerboAccionModal { get; set; }
    public string DescripcionAccionModal { get; set; }
    public string TipoAccionModal { get; set; }
    public string TipoAccionDialog { get; set; }
    public string OrigenDialog { get; set; }
    public string OrigenModal { get; set; }
    public bool EsVisibleDialogDetalle { get; set; }
    public bool EsVisibleModalDetalle { get; set; }
	public bool EsVisibleListaTerminos { get; set; }
	public bool EsVisibleDialogTermino { get; set; }
	public bool EsVisibleModalTermino { get; set; }
	public bool EsVisibleListaCuotas { get; set; }
	public bool EsVisibleDialogCuota { get; set; }
	public bool EsVisibleDialogQuitarCuotas { get; set; }
	public bool EsVisibleDialogCalcularCuotas { get; set; }
	public bool EsVisibleModalCuota { get; set; }
    public decimal TotalImporteBrutoCuotas { get; set; }
	public string CodigoItemAccion { get; set; }
    public ISvgIcon IconoAccionModal { get; set; }
    public TelerikNotification AlertDetalle { get; set; }
	public TelerikNotification AlertTermino { get; set; }
	public TelerikNotification AlertListaTermino { get; set; }
	public TelerikNotification AlertCuota { get; set; }
	public TelerikNotification AlertListaCuota { get; set; }
	public TelerikNotification Alert { get; set; }
    private TelerikGrid<ContratoDetalleGrid> GridDetalleRef { get; set; }
    private TelerikGrid<ContratoTerminoGrid> GridTerminoRef { get; set; }
	private TelerikGrid<ContratoCuotaGrid> GridCuotaRef { get; set; }
    [Parameter][SupplyParameterFromQuery(Name = "returnpage")] public string ReturnPage { get; set; }
    public EmpresaConsultaPorCodigoWebDto Empresa { get; set; }
    private bool EsVisibleVolver { get; set; }
    [Parameter] public string CodigoWebEmpresa { get; set; }
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    #endregion

    [Inject] public IServicioContrato IContrato { get; set; }  
    [Inject] public IPrincipalEmpresa IEmpresa { get; set; }
    [Inject] public IPrincipalUsuario IUsuario { get; set; } 
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
			Notify.ShowLoading(mensaje: "Iniciando formulario");

			ContratoInsertar = new();
            Contrato = new();
            GridDetalles = [];
            ItemGridDetalle = new();
            DetalleInsertar = new();
            Detalle = new();
            GridCuotas = [];
            ItemGridCuota = new();
            CuotaInsertar = new();
            Cuota = new();
            GridTerminos = [];
            ItemGridTermino = new();
            TerminoInsertar = new();
            Termino = new();

            FechasCerradoOperacion = [];
            FechaIntervalo = new();

            TiposRegistro = ContratoFlag.TiposRegistro();
            MediosPago = ContratoFlag.MediosPago();
            IntervalosInforme = ContratoFlag.IntervalosInforme();
            CodigoTipoDocumento = "CO";
            EsVisibleBotonDocumento = true;
             
            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio);
            if (!IsAuthUser) return;

            Empresa = await IEmpresa.ConsultaPorCodigoWeb(CodigoWebEmpresa);
            
            EsVisibleVolver = !string.IsNullOrEmpty(ReturnPage) && ReturnPage is "index";

            pathBaseUri = $"/{INavigation.ToBaseRelativePath(INavigation.Uri).Split("?")[0]}";
            rutaEmpresa = INavigation.Uri.Replace(INavigation.BaseUri, "").Split("?")[0].Replace(rutaServicio, "").Replace(rutaAccion, "");

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(ContratoAcceso.Emitir, Empresa.Codigo))
            {
                INavigation.NavigateTo($"{rutaEmpresa}/inicio");
                Notify.Show("No tiene permiso para emitir registros de contratos", "error");
                return;
            }

            await CargarConsultaUsuarioEmpresa();
            FechaIntervalo = await IEmpresa.ConsultaEjercicioIntervaloFecha(Empresa.Codigo);
            (await IEmpresa.ConsultaModuloPeriodoFechasEsCerradoOperacion(Empresa.Codigo, codigoModulo))?.ToList().ForEach(x => FechasCerradoOperacion.Add(x.Fecha));
            ContratoInsertar.FechaEmision = !FechasCerradoOperacion.Any(x => x == DateTime.Now.Date) ? DateTime.Now.Date : null;
            if (ContratoInsertar.FechaEmision.HasValue)
                Contrato.FechaEmision = (DateTime)ContratoInsertar.FechaEmision;

            TipoImpuestoPredeterminado = await ITipoImpuesto.ConsultaEsPredeterminado() ?? new();
            Validator = new();
            EditContext = new EditContext(ContratoInsertar);
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

            Guid id = await IContrato.Insertar(Empresa.Codigo, ContratoInsertar);

            IsModified = false;
            Notify.Show($"El contrato {NombreTipoRegistro} ha sido insertado con éxito en la empresa", "success");
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

        if (ContratoInsertar.FechaFin?.Date < ContratoInsertar.FechaInicio?.Date)
        {
            Fnc.MostrarAlerta(Alert, "La fecha de fin debe ser mayor o igual a la fecha de inicio", "error");
            esValido = false;
        }

        if (GridDetalles.Count == 0)
        {
            Fnc.MostrarAlerta(Alert, "Es obligatorio que agregue ítem(s) al detalle del contrato", "error");
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
                if (GridDetalles.Any(x => !x.ImporteBruto.HasValue))
                {
                    Fnc.MostrarAlerta(Alert, "Está pendiente completar alguna celda de [importe bruto] del detalle del contrato", "error");
                    esValido = false;
                }
            }
        } 

        if (GridCuotas.Count == 0)
        {
            Fnc.MostrarAlerta(Alert, "Es obligatorio que agregue ítem(s) a las cuotas del contrato", "error");
            esValido = false;
        }
        else
        {
            if (GridCuotas.Any(x => !x.ImporteBruto.HasValue))
            {
                Fnc.MostrarAlerta(Alert, "Está pendiente completar alguna celda de [importe bruto] de las cuotas del contrato", "error");
                esValido = false;
            }
            else
            {
                if (ContratoInsertar.TotalImporteBruto != GridCuotas.Sum(x => x.ImporteBruto ?? 0))
                {
                    Fnc.MostrarAlerta(Alert, "El importe bruto total difiere con lo agregado en la(s) cuota(s)", "error");
                    esValido = false;
                } 
                if (ContratoInsertar.CantidadCuotas != GridCuotas.Count)
                {
                    Fnc.MostrarAlerta(AlertListaCuota, $"La cantidad total de cuotas difiere con lo agregado en la(s) cuota(s)", "error");
                    esValido = false;
                } 
            } 
		} 

		return esValido;
    }

    private async Task CargarConsultaUsuarioEmpresa()
    {
        UsuarioConsultaPorEmpresaSesionDto usuarioEmpresa = await IUsuario.ConsultaPorEmpresaSesion(Empresa.Codigo);
        ContratoInsertar.CodigoArea = Contrato.CodigoArea = usuarioEmpresa.CodigoArea;
        Contrato.NombreArea = usuarioEmpresa.NombreArea;
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
            case "cuota":
				if (tipoAccion is "I" or "M")
					CuotaContext = new EditContext(CuotaInsertar);
				EsVisibleModalCuota = true;
				break;
			case "termino":
				if (tipoAccion is "I" or "M")
					TerminoContext = new EditContext(TerminoInsertar);
				EsVisibleModalTermino = true;
				break;
        }
    }

    private void IniciarAccionDialog(string tipoAccion, string origenDialog)
    {
        TipoAccionDialog = tipoAccion;
        OrigenDialog = origenDialog;
        if(origenDialog is "detalle")
            EsVisibleDialogDetalle = true;
        else if(origenDialog is "cuota")
            EsVisibleDialogCuota = true;
        else if(origenDialog is "termino")
            EsVisibleDialogTermino = true;
    }

    public void CerrarModal()
    {
        if (OrigenModal is "detalle")
            IsModifiedDetalle = EsVisibleModalDetalle = false;
        else if (OrigenModal is "cuota")
            IsModifiedCuota = EsVisibleModalCuota = false;
        else if (OrigenModal is "termino")
            IsModifiedTermino = EsVisibleModalTermino = false;
        VerboAccionModal = DescripcionAccionModal = TipoAccionModal = CodigoItemAccion = "";
        StateHasChanged();
    }

    private void Volver() => INavigation.NavigateTo($"{rutaEmpresa}{rutaServicio}");
 
    public void CerrarDialog()
    {
        if (OrigenDialog is "detalle")
            EsVisibleDialogDetalle = false;
        else if (OrigenDialog is "cuota")
            EsVisibleDialogCuota = false;
        else if (OrigenDialog is "termino")
            EsVisibleDialogTermino = false;  
        TipoAccionDialog = OrigenDialog = "";
        StateHasChanged();
    }
     
    private void CalcularTiempoDuracion()
    {
        DateTime? fechaInicio = ContratoInsertar.FechaInicio;
        DateTime? fechaFin = ContratoInsertar.FechaFin;
        int cantidadDias = 0, cantidadMeses = 0, cantidadAnios = 0;

        if (fechaInicio.HasValue && fechaFin.HasValue && fechaInicio is DateTime && fechaFin is DateTime && fechaInicio <= fechaFin)
        {
            TimeSpan diff = (TimeSpan)(fechaFin - fechaInicio);
            cantidadDias = diff.Days;

            cantidadAnios = cantidadDias / 365;
            if (cantidadAnios > 0)
                cantidadDias %= 365;

            cantidadMeses = cantidadDias / 30;
            if (cantidadMeses > 0)
                cantidadDias %= 30;
        }

        Contrato.CantidadDias = cantidadDias;
        Contrato.CantidadMeses = cantidadMeses;
        Contrato.CantidadAnios = cantidadAnios;
    }

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
		if (string.IsNullOrEmpty(ContratoInsertar.CodigoDocumento))
		{
			Fnc.MostrarAlerta(Alert, "Es necesario que seleccione el numerador de [Documento]", "error");
			esValido = false;
		}
		if (string.IsNullOrEmpty(ContratoInsertar.CodigoTipoProvision))
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

    public void MostrarModificarDetalle(ContratoDetalleGrid item = null)
    {
        if (TipoAccionModal is not "V")
            Detalle = IMapper.Map<ContratoDetalleObtenerDto>(item);

        DetalleInsertar = IMapper.Map<ContratoDetalleInsertarDto>(Detalle);
        CodigoItemAccion = Detalle.CodigoArticulo;
        DetalleValidator = new();
        IniciarAccionModal("M", "detalle");
    }

    private void MostrarQuitarDetalle(string codigoItem)
	{
		CodigoItemAccion = codigoItem;
		IniciarAccionDialog("Q", "detalle");
	}

    public void VerItemDetalle(ContratoDetalleGrid item)
    {
        Detalle = IMapper.Map<ContratoDetalleObtenerDto>(item);
        CodigoItemAccion = item.CodigoArticulo;
        IniciarAccionModal("V", "detalle");
    }

    private void OnChangeDetalleObservacionHandler(object value) => IsModifiedDetalle = Fnc.VerifyContextIsChanged((Detalle.Observacion ?? "") != ((string)value ?? ""), DetalleContext, "Observacion");

    private void OnChangeDetalleImporteBrutoHandler(object value)
    {
        decimal? importeBruto = (decimal?)value;
		IsModifiedDetalle = Fnc.VerifyContextIsChanged(Detalle.ImporteBruto != importeBruto, DetalleContext, "ImporteBruto");
        if (importeBruto.HasValue)
            ModificarImportesItem(importeBruto, DetalleInsertar.EsAfectoImpuesto);
    } 

    public static void OnCellImporteBrutoRenderHandler(GridCellRenderEventArgs args) => args.Class = !(args.Item as ContratoDetalleGrid).ImporteBruto.HasValue ? "cell-error" : "cell-editable";

	public void EditDetalleHandler(GridCommandEventArgs args) => IsEditingGridDetalle = args.Field is "ImporteBruto";

	public void CancelDetalleHandler(GridCommandEventArgs args) => IsEditingGridDetalle = args.Field is not "ImporteBruto";

	public void UpdateItemDetalleHandler(GridCommandEventArgs args)
	{
        ContratoDetalleGrid item = (ContratoDetalleGrid) args.Item; 
		int index = GridDetalles.FindIndex(i => i.CodigoArticulo == item.CodigoArticulo); 

        if (args.Field == "ImporteBruto") 
            ContratoInsertar.Detalles[index].ImporteBruto = GridDetalles[index].ImporteBruto = item.ImporteBruto; 
        
        ModificarImportesItem(item.ImporteBruto, item.EsAfectoImpuesto, index);
		ModificarImportesTotales();
		IsEditingGridDetalle = false;
	}

	private void ModificarImportesItem(decimal? importeBruto, bool esAfectoImpuesto, int indexGrid = -1)
	{
        decimal? importeImpuesto = null, importeNeto = null;
        if (importeBruto.HasValue)
        {
		    importeImpuesto = Contrato.EsAfectoImpuesto ? Math.Round((decimal)(importeBruto * (esAfectoImpuesto ? ContratoInsertar.PorcentajeImpuesto : 0)), 2, MidpointRounding.AwayFromZero) : 0;
            importeNeto = importeBruto ?? 0 + importeImpuesto;             
        }
		if (indexGrid > -1)
		{
			ContratoInsertar.Detalles[indexGrid].ImporteImpuesto = GridDetalles[indexGrid].ImporteImpuesto = importeImpuesto;
			ContratoInsertar.Detalles[indexGrid].ImporteNeto = GridDetalles[indexGrid].ImporteNeto = importeNeto; 
		}
		else
		{
			DetalleInsertar.ImporteImpuesto = importeImpuesto;
			DetalleInsertar.ImporteNeto = importeNeto;
		}
	}

	private void RecalcularImportes()
	{
		if (GridDetalles.Count > 0)
		{
			foreach ((ContratoDetalleGrid item, int index) in GridDetalles.Select((item, index) => (item, index))) 
				ModificarImportesItem(item.ImporteBruto, item.EsAfectoImpuesto, index); 
			ModificarImportesTotales(); 
		}
	}

	private void ModificarImportesTotales()
	{
		bool esGridVacio = GridDetalles.Count == 0;
		ContratoInsertar.TotalImporteBruto = esGridVacio ? 0 : GridDetalles.Sum(x => x.ImporteBruto ?? 0);
		ContratoInsertar.TotalImporteImpuesto = esGridVacio ? 0 : GridDetalles.Sum(x => x.ImporteImpuesto);
		ContratoInsertar.TotalImporteNeto = esGridVacio ? 0 : GridDetalles.Sum(x => x.ImporteNeto);
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
			if ((Detalle.CodigoArticulo ?? "") == codigo)
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
                    if (Detalle.EsAfectoImpuesto != item.EsAfectoImpuesto)
                    {
                        DetalleInsertar.EsAfectoImpuesto = Detalle.EsAfectoImpuesto = item.EsAfectoImpuesto;
					    if (DetalleInsertar.ImporteBruto.HasValue)
						    ModificarImportesItem(DetalleInsertar.ImporteBruto, DetalleInsertar.EsAfectoImpuesto);
                    }
				    goto exit;
			    }
            } 
            DetalleContext.NotifyFieldChanged(DetalleContext.Field("CodigoArticulo"));
		}
        Detalle.CodigoArticulo = codigo;
		Detalle.NombreArticulo = null;
		DetalleInsertar.EsAfectoImpuesto = Detalle.EsAfectoImpuesto = false; 
		if (string.IsNullOrEmpty(codigo)) 
            DetalleContext.MarkAsUnmodified(DetalleContext.Field("CodigoArticulo"));
	exit:
		IsModifiedDetalle = DetalleContext.IsModified();
    }

	private async Task AccionarDetalle(string tipoAccion)
	{
		GridState<ContratoDetalleGrid> detalleState = GridDetalleRef.GetState();
		int index = GridDetalles.FindIndex(i => i.CodigoArticulo == CodigoItemAccion);

		switch (tipoAccion)
		{
			case "I" or "M":
                ItemGridDetalle = IMapper.Map<ContratoDetalleGrid>(IMapper.Map(DetalleInsertar, Detalle));
                if (tipoAccion == "I")
				{
					ContratoInsertar.Detalles.Add(DetalleInsertar);
					GridDetalles.Add(ItemGridDetalle);
					detalleState.InsertedItem = ItemGridDetalle;
				}
				else
				{
					ContratoInsertar.Detalles[index] = DetalleInsertar;
					GridDetalles[index] = ItemGridDetalle; 
				} 
				break;
			case "Q":
				ContratoInsertar.Detalles.RemoveAt(index);
				GridDetalles.RemoveAt(index);
				CerrarDialog();
				break;
		};
        CerrarModal();
        ModificarImportesTotales();
        EsVisibleBotonDocumento = GridDetalles.Count == 0 && GridCuotas.Count == 0 && GridTerminos.Count == 0;
		await GridDetalleRef.SetStateAsync(detalleState);
	}
	#endregion

	#region Cuotas
	private void VisibleListaCuotaChangedHandler(bool esVisible) => EsVisibleListaCuotas = esVisible;

	public void InvalidarAccionCuota(EditContext editContext) => Fnc.MostrarAlerta(AlertCuota, Cnf.MsgErrorInvalidEditContext, "error");

    private async Task CerrarCuota()
    {
        if (TipoAccionModal is not "V" && IsModifiedCuota && !await Dialog.ConfirmAsync("¿Está seguro de cerrar el formulario y que los datos no se carguen?", "Saliendo del formulario"))
            return;
        CerrarModal();
    }

    private bool AgregarItemCuotaEsValido()
	{
		bool esValido = true;
        if ((ContratoInsertar.CantidadCuotas ?? 0) == 0)
        {
            Fnc.MostrarAlerta(AlertListaCuota, "Es necesario que la cantidad de cuotas se mayor a cero", "error");
            EditContext.NotifyFieldChanged(EditContext.Field("CantidadCuotas"));
            esValido = false;
        }
        else if (ContratoInsertar.CantidadCuotas == GridCuotas.Count)
        {
			Fnc.MostrarAlerta(AlertListaCuota, $"La cantidad total de la(s) cuota(s) ({ContratoInsertar.CantidadCuotas}) ya se han terminado de agregar", "error");
			esValido = false;
		}

        if (ContratoInsertar.TotalImporteBruto == 0)
        {
            Fnc.MostrarAlerta(AlertListaCuota, "Es necesario que el [Total Importe Bruto] sea mayor a cero", "error");
            esValido = false;
        }
        else if (GridCuotas.Count > 0 && ContratoInsertar.TotalImporteBruto == GridCuotas.Sum(x => x.ImporteBruto ?? 0))
        {
			Fnc.MostrarAlerta(AlertListaCuota, "El importe bruto total ya se han terminado de distribuir en la(s) cuota(s)", "error");
			esValido = false;
		}

		return esValido;
	}

	public void MostrarAgregarCuota()
	{
		if (AgregarItemCuotaEsValido())
		{ 
			CuotaInsertar = new();
			CodigoItemAccion = "";
			Cuota = new()
			{
				NumeroCuota = GridCuotas.Count > 0 ? GridCuotas.Select(x => x.NumeroCuota).Max() + 1 : 1
			};
			IniciarAccionModal("I", "cuota");
		}
	}

	public void MostrarModificarCuota(ContratoCuotaGrid item = null)
	{
        if (TipoAccionModal is not "V")
            Cuota = IMapper.Map<ContratoCuotaObtenerDto>(item);

        CuotaInsertar = IMapper.Map<ContratoCuotaInsertarDto>(Cuota);
        CodigoItemAccion = Cuota.NumeroCuota.ToString();
        IniciarAccionModal("M", "cuota");
    }

	private void MostrarQuitarCuota(string codigoItem)
	{
		CodigoItemAccion = codigoItem;
		IniciarAccionDialog("Q", "cuota");
	}

    private void MostrarQuitarCuotas(bool esVisible = true) => EsVisibleDialogQuitarCuotas = esVisible;

	private void MostrarCalcularCuotas(bool esVisible = true) => EsVisibleDialogCalcularCuotas = esVisible;

	public void VerItemCuota(ContratoCuotaGrid item)
	{
		Cuota = IMapper.Map<ContratoCuotaObtenerDto>(item);
		CodigoItemAccion = item.NumeroCuota.ToString();
		IniciarAccionModal("V", "cuota");
	}

	public static void OnCellFechaVencimientoCuotaRenderHandler(GridCellRenderEventArgs args) => args.Class = !(args.Item as ContratoCuotaGrid).FechaVencimiento.HasValue ? "cell-error" : "cell-editable";

	public static void OnCellImporteBrutoCuotaRenderHandler(GridCellRenderEventArgs args) => args.Class = !(args.Item as ContratoCuotaGrid).ImporteBruto.HasValue ? "cell-error" : "cell-editable";

	public void EditCuotaHandler(GridCommandEventArgs args) => IsEditingGridCuota = args.Field is "ImporteBruto" or "FechaVencimiento";

	public void CancelCuotaHandler(GridCommandEventArgs args) => IsEditingGridCuota = !(args.Field is "ImporteBruto" or "FechaVencimiento");

	private void OnChangeCuotaObservacionHandler(object value) => IsModifiedCuota = Fnc.VerifyContextIsChanged((Cuota.Observacion ?? "") != ((string)value ?? ""), CuotaContext, "Observacion");

	private void OnChangeCuotaFechaVencimientoHandler(object value) => IsModifiedCuota = Fnc.VerifyContextIsChanged(Cuota.FechaVencimiento != (DateTime?)value, CuotaContext, "FechaVencimiento");

	private void OnChangeCuotaImporteBrutoHandler(object value) => IsModifiedCuota = Fnc.VerifyContextIsChanged(Cuota.ImporteBruto != (decimal?)value, CuotaContext, "ImporteBruto");

	private void UpdateItemCuotaHandler(GridCommandEventArgs args)
	{
        ContratoCuotaGrid item = (ContratoCuotaGrid)args.Item; 
		int indexGrid = GridCuotas.FindIndex(i => i.NumeroCuota == item.NumeroCuota);
		if (args.Field == "ImporteBruto")
		{ 
			ContratoInsertar.Cuotas[indexGrid].ImporteBruto = GridCuotas[indexGrid].ImporteBruto = item.ImporteBruto; 
            CalcularTotalCuotas();
		}
        else if (args.Field == "FechaVencimiento")
        { 
            ContratoInsertar.Cuotas[indexGrid].FechaVencimiento = GridCuotas[indexGrid].FechaVencimiento = item.FechaVencimiento; 
        }
		IsEditingGridCuota = false;
	}
    
	private bool ValidarItemCuotaEsConforme()
	{
        decimal totalImporteBrutoCuotas = GridCuotas.Count == 0 ? 0 : GridCuotas.Where(x => x.NumeroCuota != Cuota.NumeroCuota).Sum(x => x.ImporteBruto ?? 0);
		if (ContratoInsertar.TotalImporteBruto < (totalImporteBrutoCuotas + CuotaInsertar.ImporteBruto))
		{
			Fnc.MostrarAlerta(AlertCuota, "La sumatoria de montos de cuota excede al del total importe bruto", "warning");
			return false;
		} 
		return true;
	}

	private void CalcularTotalCuotas() => TotalImporteBrutoCuotas = GridCuotas.Count == 0 ? 0 : GridCuotas.Sum(x => x.ImporteBruto ?? 0);

	private async Task AccionarCuota(string tipoAccion)
	{
		GridState<ContratoCuotaGrid> cuotaState = GridCuotaRef.GetState();
		int index = GridCuotas.FindIndex(i => i.NumeroCuota.ToString().Trim() == CodigoItemAccion);

		switch (tipoAccion)
		{
			case "I" or "M":
				if (!ValidarItemCuotaEsConforme())
                    return;
                ItemGridCuota = IMapper.Map<ContratoCuotaGrid>(IMapper.Map(CuotaInsertar, Cuota));
                if (tipoAccion == "I")
				{
					ContratoInsertar.Cuotas.Add(CuotaInsertar);
					GridCuotas.Add(ItemGridCuota);
					cuotaState.InsertedItem = ItemGridCuota;
				}
				else
				{
					ContratoInsertar.Cuotas[index] = CuotaInsertar;
					GridCuotas[index] = ItemGridCuota; 
				}
				break;
			case "Q":
				ContratoInsertar.Cuotas.RemoveAt(index);
				GridCuotas.RemoveAt(index); 
				CerrarDialog();
				break;
		};
        CerrarModal();
        CalcularTotalCuotas();
        EsVisibleBotonDocumento = GridDetalles.Count == 0 && GridCuotas.Count == 0 && GridTerminos.Count == 0;
		await GridCuotaRef.SetStateAsync(cuotaState);
	}

    private async Task LimpiarCuotas()
    {
		GridState<ContratoCuotaGrid> cuotaState = GridCuotaRef.GetState();
		GridCuotas.Clear();
        ContratoInsertar.Cuotas.Clear();
		TotalImporteBrutoCuotas = 0;
		MostrarQuitarCuotas(false);
		await GridCuotaRef.SetStateAsync(cuotaState);
	}

	private async Task GenerarCuotas()
	{
        await LimpiarCuotas();

		GridState<ContratoCuotaGrid> cuotaState = GridCuotaRef.GetState();
		decimal importeBrutoCuota = Math.Round((decimal)(ContratoInsertar.TotalImporteBruto / (int)ContratoInsertar.CantidadCuotas), 2, MidpointRounding.AwayFromZero);

        for (int i = 0; i < ContratoInsertar.CantidadCuotas; i++)
        {
            Cuota = new();
            CuotaInsertar = new()
            {
                NumeroCuota = i + 1,
                FechaVencimiento = DateTime.Now.Date,
                ImporteBruto = importeBrutoCuota
            };
			IMapper.Map(CuotaInsertar, Cuota);
            ItemGridCuota = IMapper.Map<ContratoCuotaGrid>(Cuota); 
            ContratoInsertar.Cuotas.Add(CuotaInsertar);
			GridCuotas.Add(ItemGridCuota);
			cuotaState.InsertedItem = ItemGridCuota;
		}

        decimal diferenciaCalculo = (decimal)(ContratoInsertar.TotalImporteBruto - GridCuotas.Sum(x => (decimal) x.ImporteBruto));
        if (diferenciaCalculo != 0) 
            GridCuotas[^1].ImporteBruto = (decimal) GridCuotas.Last().ImporteBruto + diferenciaCalculo;

        CalcularTotalCuotas();
		MostrarCalcularCuotas(false);
		await GridCuotaRef.SetStateAsync(cuotaState);
	}
	#endregion

	#region Termino
	private void VisibleListaTerminoChangedHandler(bool esVisible) => EsVisibleListaTerminos = esVisible;

    public void InvalidarAccionTermino(EditContext editContext) => Fnc.MostrarAlerta(AlertTermino, Cnf.MsgErrorInvalidEditContext, "error");

    private async Task CerrarTermino()
    {
        if (TipoAccionModal is not "V" && IsModifiedTermino && !await Dialog.ConfirmAsync("¿Está seguro de cerrar el formulario y que los datos no se carguen?", "Saliendo del formulario"))
            return;
        CerrarModal();
    }

    private bool AgregarItemTerminoEsValido()
	{
		bool esValido = true;
		if (string.IsNullOrEmpty(ContratoInsertar.FlagTipoRegistro))
		{
			Fnc.MostrarAlerta(AlertListaTermino, "Es necesario que seleccione el numerador de [Documento]", "error");
			esValido = false;
		}
		return esValido;
	}

	public void MostrarAgregarTermino()
    {
        if (AgregarItemTerminoEsValido())
        {
            TerminoInsertar = new();
            Termino = new();
            TerminoValidator = new();
            IniciarAccionModal("I", "termino");
        }
    }

    public void MostrarModificarTermino(ContratoTerminoGrid item = null)
    {
        if (TipoAccionModal is not "V")
            Termino = IMapper.Map<ContratoTerminoObtenerDto>(item);

        TerminoInsertar = IMapper.Map<ContratoTerminoInsertarDto>(Termino);
        CodigoItemAccion = string.Concat(Termino.CodigoTipoTermino.Trim(), Termino.NumeroTermino);
        TerminoValidator = new();
        IniciarAccionModal("M", "termino");
    }

    private void MostrarQuitarTermino(string codigoItem)
    {
        CodigoItemAccion = codigoItem;
        IniciarAccionDialog("Q", "termino");
    }

    public void VerItemTermino(ContratoTerminoGrid item)
    {
        Termino = IMapper.Map<ContratoTerminoObtenerDto>(item);
        CodigoItemAccion = string.Concat(item.CodigoTipoTermino.Trim(), item.NumeroTermino);
		IniciarAccionModal("V", "termino");
    }

    private void ValueChangeTipoTerminoHandler(string value)
    {
        TerminoInsertar.CodigoTipoTermino = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(TerminoValidator.MsgErrorTipoTermino) && (TerminoInsertar.CodigoTipoTermino ?? "").Trim() != (TerminoInsertar.CodigoTipoTermino ?? ""))
            TerminoInsertar.CodigoTipoTermino = TerminoValidator.MsgErrorTipoTermino = null;
    }

    private async Task OnChangeTipoTerminoHandler(object value)
	{
		string codigo = (string)(value ?? "");
		if (TerminoContext.IsValid(TerminoContext.Field("CodigoTipoTermino")))
		{
			if ((Termino.CodigoTipoTermino ?? "") == codigo) goto exit; 
			(TipoTerminoObtenerPorCodigoDto item, TerminoValidator.MsgErrorTipoTermino) = await IUtil.ObtenerTipoTerminoPorCodigo(Alert, codigo, Contrato.FlagTipoRegistro);
            if(item is not null)
            {
                Termino.CodigoTipoTermino = item.CodigoTipoTermino;
                Termino.NombreTipoTermino = item.NombreTipoTermino;
                TerminoInsertar.NumeroTermino = GridTerminos.Any(x => x.CodigoTipoTermino == codigo) ? GridTerminos.Where(x => x.CodigoTipoTermino == codigo).Select(x => x.NumeroTermino).Max() + 1 : 1;
                goto exit;
            } 
            TerminoContext.NotifyFieldChanged(TerminoContext.Field("CodigoTipoTermino"));
        }
        Termino.CodigoTipoTermino = codigo;
		Termino.NombreTipoTermino = null;
		TerminoInsertar.NumeroTermino = null;
		if (string.IsNullOrEmpty(codigo))
			TerminoContext.MarkAsUnmodified(TerminoContext.Field("CodigoTipoTermino"));
	exit:
        IsModifiedTermino = TerminoContext.IsModified();
	}

    private void OnChangeTerminoDescripcionHandler(object value) => IsModifiedTermino = Fnc.VerifyContextIsChanged((Termino.Descripcion ?? "") != ((string)value ?? ""), TerminoContext, "Descripcion");

	private async Task AccionarTermino(string tipoAccion)
    {
        GridState<ContratoTerminoGrid> terminoState = GridTerminoRef.GetState();
        int index = GridTerminos.FindIndex(i => string.Concat(i.CodigoTipoTermino.Trim(), i.NumeroTermino) == CodigoItemAccion);

        switch (tipoAccion)
        {
            case "I" or "M":
				ItemGridTermino = IMapper.Map<ContratoTerminoGrid>(IMapper.Map(TerminoInsertar, Termino));
				if (tipoAccion == "I")
                {
					ContratoInsertar.Terminos.Add(TerminoInsertar);
                    GridTerminos.Add(ItemGridTermino);
                    terminoState.InsertedItem = ItemGridTermino;
                }
                else
                {
                    ContratoInsertar.Terminos[index] = TerminoInsertar;
                    GridTerminos[index] = ItemGridTermino;
                }
                break;
            case "Q":
                ContratoInsertar.Terminos.RemoveAt(index);
                GridTerminos.RemoveAt(index);
                CerrarDialog();
                break;
        };
        CerrarModal();
        EsVisibleBotonDocumento = GridDetalles.Count == 0 && GridCuotas.Count == 0 && GridTerminos.Count == 0; 
		await GridTerminoRef.SetStateAsync(terminoState);
    }
    #endregion

    #region Catalogos
    private void CargarItemCatalogoSerieDocumento(SerieDocumentoCatalogoPorEmpresaSesionDto item)
    { 
        ContratoInsertar.CodigoDocumento = item.CodigoDocumento;
        ContratoInsertar.CodigoSerieDocumento = item.CodigoSerieDocumento;
        Contrato.NombreSerieDocumento = item.NombreSerieDocumento; 
        CodigoProcesoDocumento = item.CodigoProcesoDocumento;

        //ContratoInsertar.FlagTipoRegistro = TiposRegistro.Where(x => x.CodigosProcesoDocumento.Any(y => y == CodigoProcesoDocumento)).Select(x => x.Codigo).Single(); 
        ContratoInsertar.FlagIntervaloInforme = ContratoInsertar.FlagTipoRegistro == "SH" ? null : "N";
        NombreTipoRegistro = TiposRegistro.Where(x => x.Codigo == ContratoInsertar.FlagTipoRegistro).Select(x => x.Nombre).Single().ToLower(); 

        EditContext.NotifyFieldChanged(EditContext.Field("CodigoDocumento")); 
        IsModified = EditContext.IsModified();
    } 
     
    private void CargarItemCatalogoProveedor(EntidadProveedorCatalogoPorEmpresaDto item)
    { 
        ContratoInsertar.CodigoEntidad = Contrato.CodigoEntidad = item.CodigoEntidad.Trim();
        Contrato.NombreEntidad = item.NombreEntidad;
        ContratoInsertar.CodigoModoPago = item.CodigoModoPago;
        Contrato.NombreModoPago = item.NombreModoPago;
        ContratoInsertar.CodigoPlazoCredito = item.CodigoPlazoCredito;
        Contrato.NombrePlazoCredito = item.NombrePlazoCredito;
        Validator.MsgErrorEntidad = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidad"));
        IsModified = EditContext.IsModified();
    }

    private void CargarItemCatalogoTipoProvision(TipoProvisionCatalogoDto item)
    {
        ContratoInsertar.CodigoTipoProvision = Contrato.CodigoTipoProvision = item.CodigoTipoProvision;
		Contrato.NombreTipoProvision = item.NombreTipoProvision; 
        Contrato.CodigoMoneda = item.CodigoMoneda;
		Contrato.NombreMoneda = item.NombreMoneda;
		Contrato.SimboloMoneda = item.SimboloMoneda;
        if (Contrato.EsAfectoImpuesto != item.EsAfectoImpuesto)
        {
            Contrato.EsAfectoImpuesto = Validator.EsAfectoImpuesto = item.EsAfectoImpuesto;
            if (Contrato.EsAfectoImpuesto && string.IsNullOrEmpty(ContratoInsertar.CodigoTipoImpuesto))
            {
                ContratoInsertar.CodigoTipoImpuesto = TipoImpuestoPredeterminado.Codigo;
                Contrato.NombreTipoImpuesto = TipoImpuestoPredeterminado.Nombre;
                ContratoInsertar.PorcentajeImpuesto = TipoImpuestoPredeterminado.Porcentaje;
                EditContext.NotifyFieldChanged(EditContext.Field("CodigoTipoImpuesto"));
            }
            else if (!Contrato.EsAfectoImpuesto && !string.IsNullOrEmpty(ContratoInsertar.CodigoTipoImpuesto))
            {
                ContratoInsertar.CodigoTipoImpuesto = Contrato.NombreTipoImpuesto = Validator.MsgErrorTipoImpuesto = null;
                ContratoInsertar.PorcentajeImpuesto = null;
                EditContext.MarkAsUnmodified(EditContext.Field("CodigoTipoImpuesto"));
            }
            RecalcularImportes();
        }
        Validator.MsgErrorTipoProvision = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoTipoProvision"));
        IsModified = EditContext.IsModified();
	}

	private void CargarItemCatalogoTipoServicio(TipoServicioCatalogoDto item)
	{ 
		ContratoInsertar.CodigoTipoServicio = Contrato.CodigoTipoServicio = item.CodigoTipoServicio;
		Contrato.NombreTipoServicio = item.NombreTipoServicio;  
        Validator.MsgErrorTipoServicio = null;
		EditContext.NotifyFieldChanged(EditContext.Field("CodigoTipoServicio"));
		IsModified = EditContext.IsModified();
	}

	private void CargarItemCatalogoTipoTermino(TipoTerminoCatalogoDto item)
	{
		TerminoInsertar.CodigoTipoTermino = Termino.CodigoTipoTermino = item.CodigoTipoTermino;
		Termino.NombreTipoTermino = item.NombreTipoTermino;
		TerminoInsertar.NumeroTermino = GridTerminos.Any(x => x.CodigoTipoTermino == item.CodigoTipoTermino) ? GridTerminos.Where(x => x.CodigoTipoTermino == item.CodigoTipoTermino).Select(x => x.NumeroTermino).Max() + 1 : 1;
		EditContext.NotifyFieldChanged(EditContext.Field("CodigoTipoTermino"));
		IsModifiedTermino = TerminoContext.IsModified();
	}

	private void CargarItemCatalogoCentroCosto(CentroCostoCatalogoDto item)
    {
        ContratoInsertar.CodigoCentroCosto = Contrato.CodigoCentroCosto = item.CodigoCentroCosto.Trim();
        Contrato.NombreCentroCosto = item.NombreCentroCosto;
        Validator.MsgErrorCentroCosto = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoCentroCosto"));
        IsModified = EditContext.IsModified();
    }

    private void CargarItemCatalogoArea(AreaCatalogoDto item)
    {
        ContratoInsertar.CodigoArea = Contrato.CodigoArea = item.CodigoArea;
        Contrato.NombreArea = item.NombreArea;
        Validator.MsgErrorArea = null;
        IsChangedArea = true;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoArea"));
        IsModified = EditContext.IsModified();
    }

	private void CargarItemCatalogoUsuarioAutoriza(UsuarioCatalogoPorEmpresaDto item)
	{
		ContratoInsertar.CodigoUsuarioAutoriza = Contrato.CodigoUsuarioAutoriza = item.CodigoUsuario;
		Contrato.NombreUsuarioAutoriza = item.NombreUsuario; 
        Validator.MsgErrorUsuarioAutoriza = null;
		EditContext.NotifyFieldChanged(EditContext.Field("CodigoUsuarioAutoriza"));
		IsModified = EditContext.IsModified();
	}

	private void CargarItemCatalogoUsuarioVerifica(UsuarioCatalogoPorEmpresaDto item)
	{
		ContratoInsertar.CodigoUsuarioVerifica = Contrato.CodigoUsuarioVerifica = item.CodigoUsuario;
		Contrato.NombreUsuarioVerifica = item.NombreUsuario; 
        Validator.MsgErrorUsuarioVerifica = null;
		EditContext.NotifyFieldChanged(EditContext.Field("CodigoUsuarioVerifica"));
		IsModified = EditContext.IsModified();
	}

	private void CargarItemCatalogoTipoDevengo(TipoDevengoCatalogoDto item)
	{
		ContratoInsertar.CodigoTipoDevengo = Contrato.CodigoTipoDevengo = item.CodigoTipoDevengo;
		Contrato.NombreTipoDevengo = item.NombreTipoDevengo; 
        Validator.MsgErrorTipoDevengo = null;
		EditContext.NotifyFieldChanged(EditContext.Field("CodigoTipoDevengo"));
        IsModified = EditContext.IsModified();
	}

	private void CargarItemCatalogoArticuloPorProcesoDocumento(ArticuloCatalogoPorEmpresaProcesoDocumentoDto item)
    {  
        DetalleInsertar.CodigoArticulo = Detalle.CodigoArticulo = item.CodigoArticulo.Trim();
        Detalle.NombreArticulo = item.NombreArticulo;
        DetalleInsertar.EsAfectoImpuesto = item.EsAfectoImpuesto; 
        DetalleValidator.MsgErrorArticulo = null;
        DetalleContext.NotifyFieldChanged(DetalleContext.Field("CodigoArticulo"));
        IsModifiedDetalle = DetalleContext.IsModified(); 
        ModificarImportesItem(DetalleInsertar.ImporteBruto, DetalleInsertar.EsAfectoImpuesto);
    } 

    private void CargarItemCatalogoTipoImpuesto(TipoImpuestoCatalogoDto item)
    {
        ContratoInsertar.CodigoTipoImpuesto = Contrato.CodigoTipoImpuesto = item.CodigoTipoImpuesto;
        Contrato.NombreTipoImpuesto = item.NombreTipoImpuesto;
        if ((Contrato.PorcentajeImpuesto ?? 0) != item.PorcentajeImpuesto)
        {
            ContratoInsertar.PorcentajeImpuesto = Contrato.PorcentajeImpuesto = item.PorcentajeImpuesto;
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
        ContratoInsertar.CodigoEntidad = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorEntidad) && (Contrato.CodigoEntidad ?? "").Trim() != (ContratoInsertar.CodigoEntidad ?? ""))
            Contrato.CodigoEntidad = Validator.MsgErrorEntidad = null;
    }

    private async Task OnChangeEntidadHandler(object value)
    {
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoEntidad")))
		{ 
            if ((Contrato.CodigoEntidad ?? "") == codigo) goto exit;
			(EntidadProveedorObtenerPorCodigoEmpresaDto item, Validator.MsgErrorEntidad) = await IUtil.ObtenerProveedorPorCodigoEmpresa(Alert, codigo, Empresa.Codigo, "L");
            if (item is not null)
            {				
                Contrato.CodigoEntidad = item.CodigoEntidad.Trim();
			    Contrato.NombreEntidad = item.NombreEntidad;
			    ContratoInsertar.CodigoModoPago = item.CodigoModoPago;
			    Contrato.NombreModoPago = item.NombreModoPago;
                ContratoInsertar.CodigoPlazoCredito = item.CodigoPlazoCredito;
			    Contrato.NombrePlazoCredito = item.NombrePlazoCredito;
                goto exit;
            } 
            EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidad"));
		}
        Contrato.CodigoEntidad = codigo;
        Contrato.NombreEntidad = ContratoInsertar.CodigoModoPago = Contrato.NombreModoPago = ContratoInsertar.CodigoPlazoCredito = Contrato.NombrePlazoCredito = null;
        if (string.IsNullOrEmpty(codigo)) EditContext.MarkAsUnmodified(EditContext.Field("CodigoEntidad"));
	exit:
        IsModified = EditContext.IsModified();
    }

    private void ValueChangeTipoProvisionHandler(string value)
    {
        ContratoInsertar.CodigoTipoProvision = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorTipoProvision) && (Contrato.CodigoTipoProvision ?? "").Trim() != (ContratoInsertar.CodigoTipoProvision ?? ""))
            Contrato.CodigoTipoProvision = Validator.MsgErrorTipoProvision = null;
    }

    private async Task OnChangeTipoProvisionHandler(object value)
    {
		string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoTipoProvision")))
		{
			if ((Contrato.CodigoTipoProvision ?? "") == codigo) goto exit;
			(TipoProvisionObtenerPorCodigoDto item, Validator.MsgErrorTipoProvision) = await IUtil.ObtenerTipoProvisionPorCodigo(Alert, ContratoInsertar.CodigoTipoProvision);
			if (item is not null)
			{
				Contrato.CodigoTipoProvision = item.CodigoTipoProvision;
				Contrato.NombreTipoProvision = item.NombreTipoProvision; 
				Contrato.CodigoMoneda = item.CodigoMoneda;
				Contrato.NombreMoneda = item.NombreMoneda;
				Contrato.SimboloMoneda = item.SimboloMoneda;
                if (Contrato.EsAfectoImpuesto != item.EsAfectoImpuesto)
                {
                    Contrato.EsAfectoImpuesto = Validator.EsAfectoImpuesto = item.EsAfectoImpuesto;
                    if (Contrato.EsAfectoImpuesto && string.IsNullOrEmpty(ContratoInsertar.CodigoTipoImpuesto))
                    {
                        ContratoInsertar.CodigoTipoImpuesto = TipoImpuestoPredeterminado.Codigo;
                        Contrato.NombreTipoImpuesto = TipoImpuestoPredeterminado.Nombre;
                        ContratoInsertar.PorcentajeImpuesto = TipoImpuestoPredeterminado.Porcentaje;
                        EditContext.NotifyFieldChanged(EditContext.Field("CodigoTipoImpuesto"));
                    }
                    else if (!Contrato.EsAfectoImpuesto && !string.IsNullOrEmpty(ContratoInsertar.CodigoTipoImpuesto))
                    {
                        ContratoInsertar.CodigoTipoImpuesto = Contrato.NombreTipoImpuesto = Validator.MsgErrorTipoImpuesto = null;
                        ContratoInsertar.PorcentajeImpuesto = null;
                        EditContext.MarkAsUnmodified(EditContext.Field("CodigoTipoImpuesto"));
                    }
                    RecalcularImportes();                    
                }
				goto exit;
			} 
            EditContext.NotifyFieldChanged(EditContext.Field("CodigoTipoProvision")); 
		}
        Contrato.CodigoTipoProvision = codigo;
		Contrato.NombreTipoProvision = Contrato.CodigoMoneda = Contrato.NombreMoneda = Contrato.SimboloMoneda = ContratoInsertar.CodigoTipoImpuesto = Contrato.NombreTipoImpuesto = null;
		Contrato.EsAfectoImpuesto = Validator.EsAfectoImpuesto = false;
		ContratoInsertar.PorcentajeImpuesto = null;
		if (string.IsNullOrEmpty(codigo)) EditContext.MarkAsUnmodified(EditContext.Field("CodigoTipoProvision"));
	exit:
		IsModified = EditContext.IsModified();
    }

    private void ValueChangeCentroCostoHandler(string value)
    {
        ContratoInsertar.CodigoCentroCosto = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorCentroCosto) && (Contrato.CodigoCentroCosto ?? "").Trim() != (ContratoInsertar.CodigoCentroCosto ?? ""))
            Contrato.CodigoCentroCosto = Validator.MsgErrorCentroCosto = null;
    }

    private async Task OnChangeCentroCostoHandler(object value)
	{
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoCentroCosto")))
		{ 
			if (!string.IsNullOrEmpty(codigo))
			{ 
                if ((Contrato.CodigoCentroCosto ?? "") == codigo) goto exit;
                (CentroCostoObtenerPorCodigoDto item, Validator.MsgErrorCentroCosto) = await IUtil.ObtenerCentroCostoPorCodigo(Alert, codigo, Empresa.Codigo);
                if (item is not null)
                {
                    Contrato.CodigoCentroCosto = item.CodigoCentroCosto.Trim();
                    Contrato.NombreCentroCosto = item.NombreCentroCosto; 
                    goto exit;
                } 
                EditContext.NotifyFieldChanged(EditContext.Field("CodigoCentroCosto"));   
			}
			else
            {
                ContratoInsertar.CodigoCentroCosto = null;
				EditContext.MarkAsUnmodified(EditContext.Field("CodigoCentroCosto"));
            }
		}
        Contrato.CodigoCentroCosto = codigo;
		Contrato.NombreCentroCosto = null;
	exit:
		IsModified = EditContext.IsModified();
	}

    private void ValueChangeAreaHandler(string value)
    {
        ContratoInsertar.CodigoArea = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorArea) && (Contrato.CodigoArea ?? "").Trim() != (ContratoInsertar.CodigoArea ?? ""))
            Contrato.CodigoArea = Validator.MsgErrorArea = null;
    }

	private async Task OnChangeAreaHandler(object value)
	{
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoArea")))
		{ 
			if ((Contrato.CodigoArea ?? "") != codigo)
			{
                IsChangedArea = true;
				(AreaObtenerPorCodigoDto item, Validator.MsgErrorArea) = await IUtil.ObtenerAreaPorCodigo(Alert, codigo);
				if (item is not null)
				{
					Contrato.CodigoArea = item.CodigoArea;
					Contrato.NombreArea = item.NombreArea;
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
        Contrato.CodigoArea = codigo;
		Contrato.NombreArea = null; 
	exit:
		IsModified = EditContext.IsModified();
	}

    private void ValueChangeTipoImpuestoHandler(string value)
    {
        ContratoInsertar.CodigoTipoImpuesto = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorTipoImpuesto) && (Contrato.CodigoTipoImpuesto ?? "").Trim() != (ContratoInsertar.CodigoTipoImpuesto ?? ""))
            Contrato.CodigoTipoImpuesto = Validator.MsgErrorTipoImpuesto = null;
    }

    private async Task OnChangeTipoImpuestoHandler(object value)
	{
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoTipoImpuesto")))
		{ 
			if ((Contrato.CodigoTipoImpuesto ?? "") == codigo) goto exit;
			(TipoImpuestoObtenerPorCodigoDto item, Validator.MsgErrorTipoImpuesto) = await IUtil.ObtenerTipoImpuestoPorCodigo(Alert, codigo, true);
			if (item is not null)
			{
				Contrato.CodigoTipoImpuesto = item.CodigoTipoImpuesto;
				Contrato.NombreTipoImpuesto = item.NombreTipoImpuesto;
				if ((Contrato.PorcentajeImpuesto ?? 0) != item.PorcentajeImpuesto)
                {
                    ContratoInsertar.PorcentajeImpuesto = Contrato.PorcentajeImpuesto = item.PorcentajeImpuesto;
                    RecalcularImportes();
                }
				goto exit;
			} 
            EditContext.NotifyFieldChanged(EditContext.Field("CodigoTipoImpuesto"));
		}
        Contrato.CodigoTipoImpuesto = codigo;
		Contrato.NombreTipoImpuesto = null;
        ContratoInsertar.PorcentajeImpuesto = null; 
	exit:
		IsModified = EditContext.IsModified();
	}

    private void ValueChangeTipoServicioHandler(string value)
    {
        ContratoInsertar.CodigoTipoServicio = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorTipoServicio) && (Contrato.CodigoTipoServicio ?? "").Trim() != (ContratoInsertar.CodigoTipoServicio ?? ""))
            Contrato.CodigoTipoServicio = Validator.MsgErrorTipoServicio = null;
    }

    private async Task OnChangeTipoServicioHandler(object value)
	{
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoTipoServicio")))
		{ 
            if ((Contrato.CodigoTipoServicio ?? "") == codigo) goto exit;
			(TipoServicioObtenerPorCodigoDto item, Validator.MsgErrorTipoServicio) = await IUtil.ObtenerTipoServicioPorCodigo(Alert, codigo, Contrato.FlagTipoRegistro);
            if(item is not null)
            {
                Contrato.CodigoTipoServicio = item.CodigoTipoServicio;
                Contrato.NombreTipoServicio = item.NombreTipoServicio;
                goto exit;
            }  
            EditContext.NotifyFieldChanged(EditContext.Field("CodigoTipoServicio"));
        }
        Contrato.CodigoTipoServicio = codigo;
        Contrato.NombreTipoServicio = null;
        if (string.IsNullOrEmpty(codigo)) EditContext.MarkAsUnmodified(EditContext.Field("CodigoTipoServicio"));
	exit:
        IsModified = EditContext.IsModified();
	}

	private void ValueChangeTipoDevengoHandler(string value) => ContratoInsertar.CodigoTipoDevengo = value?.Trim().ToUpper();

	private async Task OnChangeTipoDevengoHandler(object value)
	{ 
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoTipoDevengo")))
		{
            if ((Contrato.CodigoTipoDevengo ?? "") == codigo) goto exit;
			(TipoDevengoObtenerPorCodigoDto item, Validator.MsgErrorTipoDevengo) = await IUtil.ObtenerTipoDevengoPorCodigo(Alert, codigo);
            if(item is not null)
            {
                Contrato.CodigoTipoDevengo = item.CodigoTipoDevengo;
                Contrato.NombreTipoDevengo = item.NombreTipoDevengo;
                goto exit;
            }
            EditContext.NotifyFieldChanged(EditContext.Field("CodigoTipoDevengo"));
		}
        Contrato.CodigoTipoDevengo = codigo;
        Contrato.NombreTipoDevengo = null;
        if (string.IsNullOrEmpty(codigo)) EditContext.MarkAsUnmodified(EditContext.Field("CodigoTipoDevengo"));
	exit:
        IsModified = EditContext.IsModified();
	}

    private void ValueChangeUsuarioVerificaHandler(string value)
    {
        ContratoInsertar.CodigoUsuarioVerifica = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorUsuarioVerifica) && (Contrato.CodigoUsuarioVerifica ?? "").Trim() != (ContratoInsertar.CodigoUsuarioVerifica ?? ""))
            Contrato.CodigoUsuarioVerifica = Validator.MsgErrorUsuarioVerifica = null;
    }

    private async Task OnChangeUsuarioVerificaHandler(object value)
	{ 
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoUsuarioVerifica")))
		{ 
            if ((Contrato.CodigoUsuarioVerifica ?? "") == codigo) goto exit;
			(UsuarioObtenerPorCodigoEmpresaDto item, Validator.MsgErrorUsuarioVerifica) = await IUtil.ObtenerUsuarioPorCodigoEmpresa(Alert, codigo, Empresa.Codigo);
            if (item is not null)
            {				
                Contrato.CodigoUsuarioVerifica = item.CodigoUsuario;
                Contrato.NombreUsuarioVerifica = item.NombreUsuario;
                goto exit;
            } 
            EditContext.NotifyFieldChanged(EditContext.Field("CodigoUsuarioVerifica"));
        }
        Contrato.CodigoUsuarioVerifica = codigo;
        Contrato.NombreUsuarioVerifica = null;
        if (string.IsNullOrEmpty(codigo)) EditContext.MarkAsUnmodified(EditContext.Field("CodigoUsuarioVerifica"));
	exit:
        IsModified = EditContext.IsModified();
	}

    private void ValueChangeUsuarioAutorizaHandler(string value)
    {
        ContratoInsertar.CodigoUsuarioAutoriza = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorUsuarioAutoriza) && (Contrato.CodigoUsuarioAutoriza ?? "").Trim() != (ContratoInsertar.CodigoUsuarioAutoriza ?? ""))
            Contrato.CodigoUsuarioAutoriza = Validator.MsgErrorUsuarioAutoriza = null;
    }

    private async Task OnChangeUsuarioAutorizaHandler(object value)
	{
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoUsuarioAutoriza")))
		{ 
            if ((Contrato.CodigoUsuarioAutoriza ?? "") == codigo) goto exit;
            (UsuarioObtenerPorCodigoEmpresaDto item, Validator.MsgErrorUsuarioVerifica) = await IUtil.ObtenerUsuarioPorCodigoEmpresa(Alert, codigo, Empresa.Codigo);
            if (item is not null)
            {				
                Contrato.CodigoUsuarioAutoriza = item.CodigoUsuario;
                Contrato.NombreUsuarioAutoriza = item.NombreUsuario;
                goto exit;
            } 
            EditContext.NotifyFieldChanged(EditContext.Field("CodigoUsuarioAutoriza"));
        }
        Contrato.CodigoUsuarioAutoriza = codigo;
        Contrato.NombreUsuarioAutoriza = null;
        if (string.IsNullOrEmpty(codigo)) EditContext.MarkAsUnmodified(EditContext.Field("CodigoUsuarioAutoriza"));
	exit:
        IsModified = EditContext.IsModified();
	}

	private void OnChangeCantidadCuotasHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(value is not null, EditContext, "CantidadCuotas");

	private void OnChangeObservacionHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "Observacion");

	private void OnChangeReferenciaHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "Referencia");

	private void OnChangeFlagMedioPagoHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "FlagMedioPago");

	private void OnChangeFlagIntervaloInformeHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "FlagIntervaloInforme");

	private void OnChangeFechaEmisionHandler(object value) => IsModified = Fnc.VerifyContextIsChanged((value is null && Contrato.FechaEmision.HasValue) || (!Contrato.FechaEmision.HasValue && value is not null) || (Contrato.FechaEmision.HasValue && value is not null && Contrato.FechaEmision != (DateTime?)value), EditContext, "FechaEmision");

	private void OnChangeFechaInicioHandler(object value)
	{
        IsModified = Fnc.VerifyContextIsChanged(value is not null, EditContext, "FechaInicio");
		CalcularTiempoDuracion();
	}

	private void OnChangeFechaFinHandler(object value)
	{
		IsModified = Fnc.VerifyContextIsChanged(value is not null, EditContext, "FechaFin");
		CalcularTiempoDuracion();
	}

    private void OnChangeEsGenerableDevengoHandler(object value)
    {
        if (!(bool)value)
        {
            ContratoInsertar.CodigoTipoDevengo = Contrato.CodigoTipoDevengo = Contrato.NombreTipoDevengo = null;
            EditContext.MarkAsUnmodified(EditContext.Field("CodigoTipoDevengo"));
        }
        IsModified = Fnc.VerifyContextIsChanged((bool)value, EditContext, "EsGenerableDevengo");
    }
    #endregion

    public void Dispose() => GC.SuppressFinalize(this);
}
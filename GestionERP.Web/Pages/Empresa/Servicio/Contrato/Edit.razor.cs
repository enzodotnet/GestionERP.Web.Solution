using AutoMapper;
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
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Global; 

namespace GestionERP.Web.Pages.Empresa.Servicio.Contrato;

public partial class Edit : IDisposable
{
    #region Propiedades
    private const string codigoModulo = "03";
    private const string codigoServicio = "S110";
    private const string rutaAccion = "/editar";
    private const string rutaServicio = "/contratos";
    private string rutaEmpresa = "";
    public FluentValidationValidator validator;

    public ContratoObtenerDto Contrato { get; set; }
    public ContratoEditarDto ContratoEditar { get; set; }
	public List<ContratoDetalleGrid> GridDetalles { get; set; }
	public ContratoDetalleGrid ItemGridDetalle { get; set; }
	public List<ContratoCuotaGrid> GridCuotas { get; set; }
	public ContratoCuotaGrid ItemGridCuota { get; set; }
	public List<ContratoTerminoGrid> GridTerminos { get; set; }
	public ContratoTerminoGrid ItemGridTermino { get; set; }
	public ContratoDetalleObtenerDto Detalle { get; set; }
    public ContratoDetalleEditarDto DetalleEditar { get; set; }
    public ContratoDetalleInsertarDto DetalleInsertar { get; set; }
	public ContratoCuotaObtenerDto Cuota { get; set; }
	public ContratoCuotaEditarDto CuotaEditar { get; set; }
	public ContratoCuotaInsertarDto CuotaInsertar { get; set; }
	public ContratoTerminoObtenerDto Termino { get; set; }
	public ContratoTerminoEditarDto TerminoEditar { get; set; }
	public ContratoTerminoInsertarDto TerminoInsertar { get; set; }
	private EditContext EditContext { get; set; }
    private EditContext DetalleInsertarContext { get; set; }
    private EditContext DetalleEditarContext { get; set; }
	private EditContext CuotaInsertarContext { get; set; }
	private EditContext CuotaEditarContext { get; set; }
	private EditContext TerminoInsertarContext { get; set; }
	private EditContext TerminoEditarContext { get; set; }
    public ContratoEditarValidator Validator { get; set; }
	public ContratoDetalleInsertarValidator DetalleInsertarValidator { get; set; }
    public ContratoTerminoInsertarValidator TerminoInsertarValidator { get; set; }
    private SerieDocumentoConsultaPorCodigoEmpresaDto Numerador { get; set; }
    private bool IsInitPage { get; set; }
    public bool IsEditingGridDetalle { get; set; }
	public bool IsEditingGridCuota { get; set; } 
    private TipoImpuestoConsultaEsPredeterminadoDto TipoImpuestoPredeterminado { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool IsAuthUser { get; set; }
    private bool IsModified { get; set; }
	private bool IsModifiedDetalle { get; set; }
	private bool IsModifiedTermino { get; set; }
	private bool IsModifiedCuota { get; set; }
	public TelerikNotification AlertDetalle { get; set; }
	public TelerikNotification AlertTermino { get; set; }
	public TelerikNotification AlertListaTermino { get; set; }
	public TelerikNotification AlertCuota { get; set; }
	public TelerikNotification AlertListaCuota { get; set; }
	public TelerikNotification Alert { get; set; }
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
	private bool IsChangedCentroCosto { get; set; }
	private bool IsChangedEntidad { get; set; }
	private bool IsChangedTipoProvision { get; set; }
	private bool IsChangedTipoServicio { get; set; }
	private bool IsChangedTipoDevengo { get; set; }
	private bool IsChangedUsuarioAutoriza { get; set; }
	private bool IsChangedUsuarioVerifica { get; set; }
	private bool IsChangedTipoImpuesto { get; set; }
	public decimal TotalImporteBrutoCuotas { get; set; }
	public ISvgIcon IconoAccionModal { get; set; }
    private TelerikGrid<ContratoDetalleGrid> GridDetalleRef { get; set; }
	private TelerikGrid<ContratoTerminoGrid> GridTerminoRef { get; set; }
	private TelerikGrid<ContratoCuotaGrid> GridCuotaRef { get; set; }
	private IEnumerable<ContratoFlag> TiposRegistro { get; set; }
	private string NombreTipoRegistro { get; set; }
	private IEnumerable<ContratoFlag> IntervalosInforme { get; set; }
	private IEnumerable<ContratoFlag> MediosPago { get; set; }  
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    [Parameter] public Guid? Id { get; set; }
    public EmpresaConsultaPorCodigoWebDto Empresa { get; set; }
    private bool EsVisibleVolver { get; set; }
    [Parameter] public string CodigoWebEmpresa { get; set; }
    [Parameter][SupplyParameterFromQuery(Name = "returnpage")] public string ReturnPage { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    #endregion

    [Inject] public IServicioContrato IContrato { get; set; } 
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

            ContratoEditar = new(); 

            Detalle = new();
			GridDetalles = [];
			ItemGridDetalle = new();
			DetalleInsertar = new();
            DetalleEditar = new();

			Cuota = new();
			GridCuotas = [];
			ItemGridCuota = new();
			CuotaInsertar = new();
			CuotaEditar = new();

			Termino = new();
			GridTerminos = [];
			ItemGridTermino = new();
			TerminoInsertar = new();
			TerminoEditar = new();

			TiposRegistro = ContratoFlag.TiposRegistro();
			MediosPago = ContratoFlag.MediosPago();
			IntervalosInforme = ContratoFlag.IntervalosInforme();
             
            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio);
            if (!IsAuthUser) return;

            Empresa = await IEmpresa.ConsultaPorCodigoWeb(CodigoWebEmpresa);
            
            EsVisibleVolver = !string.IsNullOrEmpty(ReturnPage) && ReturnPage is "index" or "view"; 
            rutaEmpresa = INavigation.Uri.Replace(INavigation.BaseUri, "").Split("?")[0].Replace(rutaServicio, "").Replace($"/{Id}", "").Replace(rutaAccion, "");

			if (!await IPermiso.ConsultaEsAsignadoPorSesion(ContratoAcceso.Editar, Empresa.Codigo))
			{
				INavigation.NavigateTo($"{rutaEmpresa}/inicio");
				Notify.Show("No tiene permiso para editar registros de contratos", "error");
				return;
			}

			Contrato = await IContrato.Obtener(Empresa.Codigo, (Guid) Id);
            if (Contrato is null || Contrato.CodigoEstado is not "ED")
            {
                INavigation.NavigateTo($"{rutaEmpresa}{rutaServicio}");
                Notify.Show("El registro del contrato consultado a editar no está disponible", "error");
                return;
            }

            ContratoEditar = IMapper.Map<ContratoEditarDto>(Contrato);
            Validator = new()
			{
				EsAfectoImpuesto = Contrato.EsAfectoImpuesto
			};
            EditContext = new EditContext(ContratoEditar);

			GridDetalles = IMapper.Map<List<ContratoDetalleGrid>>(Contrato.Detalles);
			GridTerminos = IMapper.Map<List<ContratoTerminoGrid>>(Contrato.Terminos) ?? [];
			GridCuotas = IMapper.Map<List<ContratoCuotaGrid>>(Contrato.Cuotas);

            Numerador = await ISerieDocumento.ConsultaPorCodigoEmpresa(Contrato.CodigoSerieDocumento, Contrato.CodigoDocumento, Empresa.Codigo) ?? new();
            TotalImporteBrutoCuotas = Contrato.TotalImporteBruto;

            NombreTipoRegistro = TiposRegistro.Where(x => x.Codigo == Contrato.FlagTipoRegistro).Select(x => x.Nombre).Single().ToLower();
             
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

            await IContrato.Editar(Empresa.Codigo, (Guid) Id, ContratoEditar);

            IsModified = false;
            Notify.Show($"El contrato {NombreTipoRegistro} {Contrato.Codigo} ha sido editado con éxito", "success");
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
            case "cuota":
                if (tipoAccion is "I" or "M") 
                    CuotaInsertarContext = new EditContext(CuotaInsertar); 
                else if (tipoAccion is "E") 
                    CuotaEditarContext = new EditContext(CuotaEditar);  
                EsVisibleModalCuota = true;
                break;
            case "termino":
                if (tipoAccion is "I" or "M") 
                    TerminoInsertarContext = new EditContext(TerminoInsertar);
                else if (tipoAccion is "E") 
                    TerminoEditarContext = new EditContext(TerminoEditar);  
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
        VerboAccionModal = DescripcionAccionModal = TipoAccionModal = "";
        StateHasChanged();
    }

    private void Volver() => INavigation.NavigateTo($"{rutaEmpresa}{rutaServicio}{(ReturnPage == "view" ? $"/{Id}" : "")}");

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

    private bool VerificarRegistroEsValido()
    {
        bool esValido = true;

		if (ContratoEditar.FechaFin?.Date < ContratoEditar.FechaInicio?.Date)
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
				if (ContratoEditar.TotalImporteBruto != GridCuotas.Sum(x => x.ImporteBruto ?? 0))
				{
					Fnc.MostrarAlerta(Alert, "El importe bruto total difiere con lo agregado en la(s) cuota(s)", "error");
					esValido = false;
				}
				if (ContratoEditar.CantidadCuotas != GridCuotas.Count)
				{
					Fnc.MostrarAlerta(AlertListaCuota, $"La cantidad total de cuotas difiere con lo agregado en la(s) cuota(s)", "error");
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
        {
            IsModified = ContratoEditar.DetallesInsertar.Count > 0 || ContratoEditar.DetallesEditar.Count > 0 || ContratoEditar.DetallesEliminar.Count > 0;
            if (!IsModified)
                IsModified = ContratoEditar.CuotasInsertar.Count > 0 || ContratoEditar.CuotasEditar.Count > 0 || ContratoEditar.CuotasEliminar.Count > 0;
        }  
    }

    private void CalcularTiempoDuracion()
    {
        DateTime? fechaInicio = ContratoEditar.FechaInicio;
        DateTime? fechaFin = ContratoEditar.FechaFin;
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

    public static void OnRowDetalleRenderHandler(GridRowRenderEventArgs args) => args.Class = !(args.Item as ContratoDetalleGrid).Id.HasValue ? "new-row" : "";
    
    public void MostrarAgregarDetalle()
    {
        DetalleInsertar = new();
        Detalle = new();
		DetalleInsertarValidator = new();
        IniciarAccionModal("I", "detalle");
    }

    public void MostrarModificarDetalle(ContratoDetalleGrid item = null)
    {
		if (TipoAccionModal is not "V")
			Detalle = IMapper.Map<ContratoDetalleObtenerDto>(item);
 
		string tipoAccion = !Detalle.Id.HasValue ? "M" : "E";
		if (tipoAccion is "M")
        {
            DetalleInsertar = IMapper.Map<ContratoDetalleInsertarDto>(Detalle);
			DetalleInsertarValidator = new();
        } 
		else
			DetalleEditar = IMapper.Map<ContratoDetalleEditarDto>(Detalle); 
		IniciarAccionModal(tipoAccion, "detalle");
    }

    private void MostrarQuitarDetalle(ContratoDetalleGrid item = null)
    {
		if (TipoAccionModal is not "V")
			Detalle = IMapper.Map<ContratoDetalleObtenerDto>(item);
		IniciarAccionDialog(!Detalle.Id.HasValue ? "Q" : "X", "detalle");
	}

    public void VerItemDetalle(ContratoDetalleGrid item)
    {
		Detalle = IMapper.Map<ContratoDetalleObtenerDto>(item);
		IniciarAccionModal("V", "detalle");
    }

    private void OnChangeDetalleObservacionHandler(object value) => IsModifiedDetalle = Fnc.VerifyContextIsChanged((Detalle.Observacion ?? "") != ((string)value ?? ""), TipoAccionModal is "E" ? DetalleEditarContext : DetalleInsertarContext, "Observacion");

	private void OnChangeDetalleImporteBrutoHandler(object value)
    {
		decimal? importeBruto = (decimal?)value;
		IsModifiedDetalle = Fnc.VerifyContextIsChanged(Detalle.ImporteBruto != importeBruto, TipoAccionModal is "E" ? DetalleEditarContext : DetalleInsertarContext, "ImporteBruto");
		ModificarImportesItem(importeBruto, TipoAccionModal is "E" ? Detalle.EsAfectoImpuesto : DetalleInsertar.EsAfectoImpuesto);
	}

	public static void OnCellImporteBrutoRenderHandler(GridCellRenderEventArgs args) => args.Class = !(args.Item as ContratoDetalleGrid).ImporteBruto.HasValue ? "cell-error" : "cell-editable";

	public void EditDetalleHandler(GridCommandEventArgs args) => IsEditingGridDetalle = args.Field is "ImporteBruto";

	public void CancelDetalleHandler(GridCommandEventArgs args) => IsEditingGridDetalle = args.Field is not "ImporteBruto";

	public void UpdateItemDetalleHandler(GridCommandEventArgs args)
    {
        ContratoDetalleGrid item = (ContratoDetalleGrid)args.Item;
        int indexGrid = GridDetalles.FindIndex(i => i.CodigoArticulo == item.CodigoArticulo);
        bool esItemNuevo = !item.Id.HasValue;
        int indexEdit = !esItemNuevo ? ContratoEditar.DetallesEditar.FindIndex(i => i.Id == item.Id) : ContratoEditar.DetallesInsertar.FindIndex(i => i.CodigoArticulo == item.CodigoArticulo);

        if (args.Field == "ImporteBruto")
        {
            if (!esItemNuevo && indexEdit > -1)
                ContratoEditar.DetallesEditar[indexEdit].ImporteBruto = item.ImporteBruto;
            else if (esItemNuevo)
                ContratoEditar.DetallesInsertar[indexEdit].ImporteBruto = item.ImporteBruto; 
            GridDetalles[indexGrid].ImporteBruto = item.ImporteBruto;
        } 
		ModificarImportesItem(item.ImporteBruto, item.EsAfectoImpuesto, indexGrid, indexEdit);
		ModificarImportesTotales(); 
		if (!esItemNuevo && indexEdit == -1)
        {
            ContratoEditar.DetallesEditar.Add(IMapper.Map(IMapper.Map<ContratoDetalleObtenerDto>(GridDetalles[indexGrid]), new ContratoDetalleEditarDto())); 
            NotifyChange();
        }  
        IsEditingGridDetalle = false;
    }

    private void ModificarImportesItem(decimal? importeBruto, bool esAfectoImpuesto, int indexGrid = -1, int indexEdit = -1)
    {
        decimal? importeImpuesto = null, importeNeto = null;
        if (importeBruto.HasValue)
        {
		    importeImpuesto = Contrato.EsAfectoImpuesto ? Math.Round((decimal)(importeBruto * (esAfectoImpuesto ? ContratoEditar.PorcentajeImpuesto : 0)), 2, MidpointRounding.AwayFromZero) : 0;
            importeNeto = importeBruto ?? 0 + importeImpuesto;             
        }

        if (indexGrid > -1)
        {
			GridDetalles[indexGrid].ImporteImpuesto = importeImpuesto;
			GridDetalles[indexGrid].ImporteNeto = importeNeto; 
            if (indexEdit > -1)
            {
				if (GridDetalles[indexGrid].Id.HasValue)
				{
					ContratoEditar.DetallesEditar[indexEdit].ImporteImpuesto = importeImpuesto;
					ContratoEditar.DetallesEditar[indexEdit].ImporteNeto = importeNeto;
				}
				else
				{
					ContratoEditar.DetallesInsertar[indexEdit].ImporteImpuesto = importeImpuesto;
					ContratoEditar.DetallesInsertar[indexEdit].ImporteNeto = importeNeto;
				}
			}
        }
        else
        {
            if (TipoAccionModal is "E")
            {
                DetalleEditar.ImporteImpuesto = importeImpuesto;
                DetalleEditar.ImporteNeto = importeNeto;
            }
            else
            {
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
            foreach ((ContratoDetalleGrid item, int indexGrid) in GridDetalles.Select((item, index) => (item, index)))
            {
				esItemNuevo = !item.Id.HasValue;
				indexEdit = !esItemNuevo ? ContratoEditar.DetallesEditar.FindIndex(i => i.Id == item.Id) : ContratoEditar.DetallesInsertar.FindIndex(i => i.CodigoArticulo == item.CodigoArticulo);
                ModificarImportesItem(item.ImporteBruto, item.EsAfectoImpuesto, indexGrid, indexEdit); 
				if (!esItemNuevo && indexEdit == -1)
					ContratoEditar.DetallesEditar.Add(IMapper.Map(IMapper.Map<ContratoDetalleObtenerDto>(GridDetalles[indexGrid]), new ContratoDetalleEditarDto()));
            }
            ModificarImportesTotales();
        }
    }

    private void ModificarImportesTotales()
    {
        bool esGridVacio = GridDetalles.Count == 0;
        ContratoEditar.TotalImporteBruto = esGridVacio ? 0 : GridDetalles.Sum(x => x.ImporteBruto ?? 0);
        ContratoEditar.TotalImporteImpuesto = esGridVacio ? 0 : GridDetalles.Sum(x => x.ImporteImpuesto);
        ContratoEditar.TotalImporteNeto = esGridVacio ? 0 : GridDetalles.Sum(x => x.ImporteNeto);
    }

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
                    if (Detalle.EsAfectoImpuesto != item.EsAfectoImpuesto)
                    {
                        DetalleInsertar.EsAfectoImpuesto = Detalle.EsAfectoImpuesto = item.EsAfectoImpuesto;
					    if (DetalleInsertar.ImporteBruto.HasValue)
						    ModificarImportesItem(DetalleInsertar.ImporteBruto, DetalleInsertar.EsAfectoImpuesto);
                    }
				    goto exit;
			    }
            } 
            DetalleInsertarContext.NotifyFieldChanged(DetalleInsertarContext.Field("CodigoArticulo"));
		}
        Detalle.CodigoArticulo = codigo;
		Detalle.NombreArticulo = null;
		DetalleInsertar.EsAfectoImpuesto = Detalle.EsAfectoImpuesto = false; 
		if (string.IsNullOrEmpty(codigo)) 
            DetalleInsertarContext.MarkAsUnmodified(DetalleInsertarContext.Field("CodigoArticulo")); 
	exit:
		IsModifiedDetalle = DetalleInsertarContext.IsModified();
    }

    private async Task AccionarDetalle(string tipoAccion)
    {
        GridState<ContratoDetalleGrid> detalleState = GridDetalleRef.GetState();
        int indexGrid = GridDetalles.FindIndex(i => i.CodigoArticulo == Detalle.CodigoArticulo);
        int indexEdit = tipoAccion is "E" or "X" ? ContratoEditar.DetallesEditar.FindIndex(i => i.Id == Detalle.Id) : ContratoEditar.DetallesInsertar.FindIndex(i => i.CodigoArticulo == Detalle.CodigoArticulo);
        
        switch (tipoAccion)
        {
            case "I" or "M":
				ItemGridDetalle = IMapper.Map<ContratoDetalleGrid>(IMapper.Map(DetalleInsertar, Detalle));
				if (tipoAccion == "I")
                {
                    ContratoEditar.DetallesInsertar.Add(DetalleInsertar);
                    GridDetalles.Add(ItemGridDetalle);
                    detalleState.InsertedItem = ItemGridDetalle;
                }
                else
                {
                    ContratoEditar.DetallesInsertar[indexEdit] = DetalleInsertar;
                    GridDetalles[indexGrid] = ItemGridDetalle; 
                }
                break;
            case "E":
                if (indexEdit > -1) 
                    ContratoEditar.DetallesEditar[indexEdit] = DetalleEditar; 
                else 
                    ContratoEditar.DetallesEditar.Add(DetalleEditar);
                GridDetalles[indexGrid] = IMapper.Map<ContratoDetalleGrid>(IMapper.Map(DetalleEditar, Detalle));
                break;
            case "Q" or "X":
                if (tipoAccion == "Q")
                    ContratoEditar.DetallesInsertar.RemoveAt(indexEdit);
                else
                {
                    ContratoEditar.DetallesEliminar.Add(new() { Id = (Guid) Detalle.Id });
                    if (indexEdit > -1) 
                        ContratoEditar.DetallesEditar.RemoveAt(indexEdit); 
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

	#region Cuotas
	private void VisibleListaCuotaChangedHandler(bool esVisible) => EsVisibleListaCuotas = esVisible;

	public void InvalidarAccionCuota(EditContext editContext) => Fnc.MostrarAlerta(AlertCuota, Cnf.MsgErrorInvalidEditContext, "error");

    private async Task CerrarCuota()
    {
        if (TipoAccionModal is not "V" && IsModifiedCuota && !await Dialog.ConfirmAsync("¿Está seguro de cerrar el formulario y que los datos no se carguen?", "Saliendo del formulario"))
            return;
        CerrarModal();
    }

    public static void OnRowCuotaRenderHandler(GridRowRenderEventArgs args) => args.Class = !(args.Item as ContratoCuotaGrid).Id.HasValue ? "new-row" : "";
	
	private bool AgregarItemCuotaEsValido()
	{
		bool esValido = true;
		if ((ContratoEditar.CantidadCuotas ?? 0) == 0)
		{
			Fnc.MostrarAlerta(AlertListaCuota, "Es necesario que la cantidad de cuotas se mayor a cero", "error");
			EditContext.NotifyFieldChanged(EditContext.Field("CantidadCuotas"));
			esValido = false;
		}
		else if (ContratoEditar.CantidadCuotas == GridCuotas.Count)
		{
			Fnc.MostrarAlerta(AlertListaCuota, $"La cantidad total de la(s) cuota(s) ({ContratoEditar.CantidadCuotas}) ya se han terminado de agregar", "error");
			esValido = false;
		}
		if (ContratoEditar.TotalImporteBruto == 0)
		{
			Fnc.MostrarAlerta(AlertListaCuota, "Es necesario que el [Total Importe Bruto] sea mayor a cero", "error");
			esValido = false;
		}
		else if (GridCuotas.Count > 0 && ContratoEditar.TotalImporteBruto == GridCuotas.Sum(x => x.ImporteBruto ?? 0))
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

		string tipoAccion = !Cuota.Id.HasValue ? "M" : "E";
		if (tipoAccion is "M")
			CuotaInsertar = IMapper.Map<ContratoCuotaInsertarDto>(Cuota);
		else
			CuotaEditar = IMapper.Map<ContratoCuotaEditarDto>(Cuota);

		IniciarAccionModal(tipoAccion, "cuota");
	}

	private void MostrarQuitarCuota(ContratoCuotaGrid item = null)
	{
		if (TipoAccionModal is not "V")
			Cuota = IMapper.Map<ContratoCuotaObtenerDto>(item);

		IniciarAccionDialog(!Detalle.Id.HasValue ? "Q" : "X", "cuota");
	}

	private void MostrarQuitarCuotas(bool esVisible = true) => EsVisibleDialogQuitarCuotas = esVisible;

	private void MostrarCalcularCuotas(bool esVisible = true) => EsVisibleDialogCalcularCuotas = esVisible;

	public void VerItemCuota(ContratoCuotaGrid item)
	{
		Cuota = IMapper.Map<ContratoCuotaObtenerDto>(item);
		IniciarAccionModal("V", "cuota");
	}

	public static void OnCellFechaVencimientoCuotaRenderHandler(GridCellRenderEventArgs args) => args.Class = !(args.Item as ContratoCuotaGrid).FechaVencimiento.HasValue ? "cell-error" : "cell-editable";

	public static void OnCellImporteBrutoCuotaRenderHandler(GridCellRenderEventArgs args) => args.Class = !(args.Item as ContratoCuotaGrid).ImporteBruto.HasValue ? "cell-error" : "cell-editable";

	public void EditCuotaHandler(GridCommandEventArgs args) => IsEditingGridCuota = args.Field is "ImporteBruto" or "FechaVencimiento";

	public void CancelCuotaHandler(GridCommandEventArgs args) => IsEditingGridCuota = !(args.Field is "ImporteBruto" or "FechaVencimiento");

	private void OnChangeCuotaObservacionHandler(object value) => IsModifiedCuota = Fnc.VerifyContextIsChanged((Cuota.Observacion ?? "") != ((string)value ?? ""), TipoAccionModal is "E" ? CuotaEditarContext : CuotaInsertarContext, "Observacion");

	private void OnChangeCuotaFechaVencimientoHandler(object value) => IsModifiedCuota = Fnc.VerifyContextIsChanged(Cuota.FechaVencimiento != (DateTime?)value, TipoAccionModal is "E" ? CuotaEditarContext : CuotaInsertarContext, "FechaVencimiento");

	private void OnChangeCuotaImporteBrutoHandler(object value) => IsModifiedCuota = Fnc.VerifyContextIsChanged(Cuota.ImporteBruto != (decimal?)value, TipoAccionModal is "E" ? CuotaEditarContext : CuotaInsertarContext, "ImporteBruto");

	private void UpdateItemCuotaHandler(GridCommandEventArgs args)
	{
		ContratoCuotaGrid item = (ContratoCuotaGrid)args.Item;

		int indexGrid = GridCuotas.FindIndex(i => i.NumeroCuota == item.NumeroCuota);
		bool esItemNuevo = !item.Id.HasValue;
        int indexEdit = !esItemNuevo ? ContratoEditar.CuotasEditar.FindIndex(i => i.Id == item.Id) : ContratoEditar.CuotasInsertar.FindIndex(i => i.NumeroCuota == item.NumeroCuota);
        
        if (args.Field == "ImporteBruto")
		{ 
            if (!esItemNuevo && indexEdit > -1)
                ContratoEditar.CuotasEditar[indexEdit].ImporteBruto = item.ImporteBruto;
            else
                ContratoEditar.CuotasInsertar[indexEdit].ImporteBruto = item.ImporteBruto;
            GridCuotas[indexGrid].ImporteBruto = item.ImporteBruto; 
            CalcularTotalCuotas();
        }
		else if (args.Field == "FechaVencimiento")
		{ 
            if (!esItemNuevo && indexEdit > -1)
                ContratoEditar.CuotasEditar[indexEdit].FechaVencimiento = item.FechaVencimiento;
            else
                ContratoEditar.CuotasInsertar[indexEdit].FechaVencimiento = item.FechaVencimiento;
            GridCuotas[indexGrid].FechaVencimiento = item.FechaVencimiento; 
        } 
        if (!esItemNuevo && indexEdit == -1)
        {
            ContratoEditar.CuotasEditar.Add(IMapper.Map(IMapper.Map<ContratoCuotaObtenerDto>(GridCuotas[indexGrid]), new ContratoCuotaEditarDto()));
            NotifyChange();
        } 
		IsEditingGridCuota = false;
	}

	private bool VerificarItemCuotaEsValido()
	{
        decimal importeBrutoCuota = Cuota.Id.HasValue ? CuotaEditar.ImporteBruto ?? 0 : CuotaInsertar.ImporteBruto ?? 0;
		decimal totalImporteBrutoCuotas = GridCuotas.Count == 0 ? 0 : GridCuotas.Where(x => x.NumeroCuota != Cuota.NumeroCuota).Sum(x => x.ImporteBruto ?? 0);
		if (ContratoEditar.TotalImporteBruto < (totalImporteBrutoCuotas + importeBrutoCuota))
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
		int indexGrid = GridCuotas.FindIndex(i => i.NumeroCuota == Cuota.NumeroCuota);
		int indexEdit = tipoAccion is "E" or "X" ? ContratoEditar.CuotasEditar.FindIndex(i => i.Id == Cuota.Id) : ContratoEditar.CuotasInsertar.FindIndex(i => i.NumeroCuota == Cuota.NumeroCuota);
		
		switch (tipoAccion)
		{
			case "I" or "M":
				if (!VerificarItemCuotaEsValido())
                    return;
                ItemGridCuota = IMapper.Map<ContratoCuotaGrid>(IMapper.Map(CuotaInsertar, Cuota));
                if (tipoAccion == "I")
				{
					ContratoEditar.CuotasInsertar.Add(CuotaInsertar);
					GridCuotas.Add(ItemGridCuota);
					cuotaState.InsertedItem = ItemGridCuota;
				}
				else
				{
					ContratoEditar.CuotasInsertar[indexGrid] = CuotaInsertar;
					GridCuotas[indexGrid] = ItemGridCuota; 
				} 
				break;
			case "E":
                if (indexEdit > -1)
                    ContratoEditar.CuotasEditar[indexEdit] = CuotaEditar;
                else
                    ContratoEditar.CuotasEditar.Add(CuotaEditar);
                GridCuotas[indexGrid] = IMapper.Map<ContratoCuotaGrid>(IMapper.Map(CuotaEditar, Cuota));
				break;
			case "Q" or "X":
				if (tipoAccion == "Q")
					ContratoEditar.CuotasInsertar.RemoveAt(indexEdit);
				else
				{
					ContratoEditar.CuotasEliminar.Add(new() { Id = (Guid) Cuota.Id });
					if (indexEdit > -1)
						ContratoEditar.CuotasEditar.RemoveAt(indexEdit);
				}
				GridCuotas.RemoveAt(indexGrid);
				CerrarDialog();
				break;
		};
        CerrarModal();
        CalcularTotalCuotas();
        NotifyChange();
        await GridCuotaRef.SetStateAsync(cuotaState);
	}

	private async Task LimpiarCuotas()
	{
		GridState<ContratoCuotaGrid> cuotaState = GridCuotaRef.GetState();

        ContratoEditar.CuotasEliminar.Clear();

		if (GridCuotas.Count > 0)
        {
			foreach (ContratoCuotaGrid item in GridCuotas.Where(x => x.Id.HasValue))
			{
				ContratoEditar.CuotasEliminar.Add(new() { Id = (Guid)item.Id });
			}
		} 

		GridCuotas.Clear();
		ContratoEditar.CuotasInsertar.Clear();
		ContratoEditar.CuotasEditar.Clear();

		TotalImporteBrutoCuotas = 0;
		MostrarQuitarCuotas(false);
		await GridCuotaRef.SetStateAsync(cuotaState);
	}

	private async Task GenerarCuotas()
	{
		await LimpiarCuotas();

		GridState<ContratoCuotaGrid> cuotaState = GridCuotaRef.GetState();

		decimal importeBrutoCuota = Math.Round((decimal)(ContratoEditar.TotalImporteBruto / (int) ContratoEditar.CantidadCuotas), 2, MidpointRounding.AwayFromZero);

		for (int i = 0; i < ContratoEditar.CantidadCuotas; i++)
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
            ContratoEditar.CuotasInsertar.Add(CuotaInsertar);
			GridCuotas.Add(ItemGridCuota);
			cuotaState.InsertedItem = ItemGridCuota;
		}

		decimal diferenciaCalculo = (decimal)(ContratoEditar.TotalImporteBruto - GridCuotas.Sum(x => (decimal)x.ImporteBruto)); 
		if (diferenciaCalculo != 0)
			GridCuotas[^1].ImporteBruto = (decimal) GridCuotas.Last().ImporteBruto + diferenciaCalculo;

		CalcularTotalCuotas();
		MostrarCalcularCuotas(false);
        NotifyChange();
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

    public static void OnRowTerminoRenderHandler(GridRowRenderEventArgs args) => args.Class = !(args.Item as ContratoTerminoGrid).Id.HasValue ? "new-row" : "";
	
	public void MostrarAgregarTermino()
	{ 
		TerminoInsertar = new(); 
		Termino = new();
        TerminoInsertarValidator = new();
        IniciarAccionModal("I", "termino"); 
	}

	public void MostrarModificarTermino(ContratoTerminoGrid item = null)
	{
        if (TipoAccionModal is not "V")
            Termino = IMapper.Map<ContratoTerminoObtenerDto>(item);

        string tipoAccion = !Termino.Id.HasValue ? "M" : "E";
        if (tipoAccion is "M")
        {
            TerminoInsertar = IMapper.Map<ContratoTerminoInsertarDto>(Termino);
            TerminoInsertarValidator = new();
        }
        else
        {
            TerminoEditar = IMapper.Map<ContratoTerminoEditarDto>(Termino);
        }
        IniciarAccionModal(tipoAccion, "termino");
    }

	private void MostrarQuitarTermino(ContratoTerminoGrid item = null)
	{
        if (TipoAccionModal is not "V")
            Termino = IMapper.Map<ContratoTerminoObtenerDto>(item);
        IniciarAccionDialog(!Termino.Id.HasValue ? "Q" : "X", "termino");
    }

	public void VerItemTermino(ContratoTerminoGrid item)
	{
        Termino = IMapper.Map<ContratoTerminoObtenerDto>(item);
        IniciarAccionModal("V", "termino");
	}

    private void ValueChangeTipoTerminoHandler(string value)
    {
        TerminoInsertar.CodigoTipoTermino = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(TerminoInsertarValidator.MsgErrorTipoTermino) && (TerminoInsertar.CodigoTipoTermino ?? "").Trim() != (TerminoInsertar.CodigoTipoTermino ?? ""))
            TerminoInsertar.CodigoTipoTermino = TerminoInsertarValidator.MsgErrorTipoTermino = null;
    }

    private async Task OnChangeTipoTerminoHandler(object value)
	{
		string codigo = (string)(value ?? "");
		if (TerminoInsertarContext.IsValid(TerminoInsertarContext.Field("CodigoTipoTermino")))
		{
			if ((Termino.CodigoTipoTermino ?? "") == codigo) goto exit;
            (TipoTerminoObtenerPorCodigoDto item, TerminoInsertarValidator.MsgErrorTipoTermino) = await IUtil.ObtenerTipoTerminoPorCodigo(Alert, codigo, Contrato.FlagTipoRegistro);
            if (item is not null)
            {
                Termino.CodigoTipoTermino = item.CodigoTipoTermino;
                Termino.NombreTipoTermino = item.NombreTipoTermino;
                TerminoInsertar.NumeroTermino = GridTerminos.Any(x => x.CodigoTipoTermino == codigo) ? GridTerminos.Where(x => x.CodigoTipoTermino == codigo).Select(x => x.NumeroTermino).Max() + 1 : 1;
                goto exit;
            } 
            TerminoInsertarContext.NotifyFieldChanged(TerminoInsertarContext.Field("CodigoTipoTermino"));
        }
        Termino.CodigoTipoTermino = codigo;
		Termino.NombreTipoTermino = null;
		TerminoInsertar.NumeroTermino = null;
		if (string.IsNullOrEmpty(codigo))
			TerminoInsertarContext.MarkAsUnmodified(TerminoInsertarContext.Field("CodigoTipoTermino"));
	exit:
		IsModifiedTermino = TerminoInsertarContext.IsModified();
	}

	private void OnChangeTerminoDescripcionHandler(object value) => IsModifiedTermino = Fnc.VerifyContextIsChanged((Termino.Descripcion ?? "") != ((string)value ?? ""), TipoAccionModal is "E" ? TerminoEditarContext : TerminoInsertarContext, "Descripcion");

	private async Task AccionarTermino(string tipoAccion)
	{
		GridState<ContratoTerminoGrid> terminoState = GridTerminoRef.GetState();
		int indexGrid = GridTerminos.FindIndex(i => string.Concat(i.CodigoTipoTermino.Trim(), i.NumeroTermino) == string.Concat((Termino.CodigoTipoTermino ?? "").Trim(), Termino.NumeroTermino));
        int indexEdit = tipoAccion is "E" or "X" ? ContratoEditar.TerminosEditar.FindIndex(i => i.Id == Termino.Id) : ContratoEditar.TerminosInsertar.FindIndex(i => string.Concat(i.CodigoTipoTermino.Trim(), i.NumeroTermino) == string.Concat(Termino.CodigoTipoTermino, Termino.NumeroTermino));
        
        switch (tipoAccion)
		{
			case "I" or "M":
				ItemGridTermino = IMapper.Map<ContratoTerminoGrid>(IMapper.Map(TerminoInsertar, Termino));
				if (tipoAccion == "I")
				{
					ContratoEditar.TerminosInsertar.Add(TerminoInsertar);
                    GridTerminos.Add(ItemGridTermino);
					terminoState.InsertedItem = ItemGridTermino;
				}
				else
				{ 
                    ContratoEditar.TerminosInsertar[indexGrid] = TerminoInsertar;
					GridTerminos[indexGrid] = ItemGridTermino;
				}
				break;
            case "E":
                if (indexEdit > -1)
                    ContratoEditar.TerminosEditar[indexEdit] = TerminoEditar;
                else
                    ContratoEditar.TerminosEditar.Add(TerminoEditar);
                GridTerminos[indexGrid] = IMapper.Map<ContratoTerminoGrid>(IMapper.Map(TerminoEditar, Termino));
                break;
            case "Q" or "X":
                if (tipoAccion == "Q")
                    ContratoEditar.TerminosInsertar.RemoveAt(indexEdit);
                else
                { 
                    ContratoEditar.TerminosEliminar.Add(new() { Id = (Guid) Termino.Id });
                    if (indexEdit > -1)
                        ContratoEditar.TerminosEditar.RemoveAt(indexEdit);
                }
				GridTerminos.RemoveAt(indexGrid);
                CerrarDialog();
                break;
		};
        CerrarModal();
        NotifyChange();
		await GridTerminoRef.SetStateAsync(terminoState);
	}
	#endregion

	#region Catalogos

	private void CargarItemCatalogoArticuloPorProcesoDocumento(ArticuloCatalogoPorEmpresaProcesoDocumentoDto item)
    { 
        DetalleInsertar.CodigoArticulo = Detalle.CodigoArticulo = item.CodigoArticulo.Trim();
        Detalle.NombreArticulo = item.NombreArticulo;
        DetalleInsertar.EsAfectoImpuesto = item.EsAfectoImpuesto;
		DetalleInsertarValidator.MsgErrorArticulo = null;
        DetalleInsertarContext.NotifyFieldChanged(DetalleInsertarContext.Field("CodigoArticulo"));
        IsModifiedDetalle = DetalleInsertarContext.IsModified();
		ModificarImportesItem(DetalleInsertar.ImporteBruto, TipoAccionModal is "E" ? Detalle.EsAfectoImpuesto : DetalleInsertar.EsAfectoImpuesto); 
    }   

    private void CargarItemCatalogoProveedor(EntidadProveedorCatalogoPorEmpresaDto item)
    { 
        ContratoEditar.CodigoEntidad = Contrato.CodigoEntidad = item.CodigoEntidad.Trim();
        Contrato.NombreEntidad = item.NombreEntidad;
        ContratoEditar.CodigoModoPago = item.CodigoModoPago;
        Contrato.NombreModoPago = item.NombreModoPago;
        ContratoEditar.CodigoPlazoCredito = item.CodigoPlazoCredito;
        Contrato.NombrePlazoCredito = item.NombrePlazoCredito;
		IsChangedEntidad = true;
		Validator.MsgErrorEntidad = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidad"));
        NotifyChange();
    }

    private void CargarItemCatalogoTipoProvision(TipoProvisionCatalogoDto item)
    {
        ContratoEditar.CodigoTipoProvision = Contrato.CodigoTipoProvision = item.CodigoTipoProvision;
		Contrato.NombreTipoProvision = item.NombreTipoProvision;
        Contrato.CodigoMoneda = item.CodigoMoneda;
        Contrato.NombreMoneda = item.NombreMoneda;
        Contrato.SimboloMoneda = item.SimboloMoneda;
        if (Contrato.EsAfectoImpuesto != item.EsAfectoImpuesto)
        {
            Contrato.EsAfectoImpuesto = Validator.EsAfectoImpuesto = item.EsAfectoImpuesto;
            if (Contrato.EsAfectoImpuesto && string.IsNullOrEmpty(ContratoEditar.CodigoTipoImpuesto))
            {
                ContratoEditar.CodigoTipoImpuesto = TipoImpuestoPredeterminado.Codigo;
                Contrato.NombreTipoImpuesto = TipoImpuestoPredeterminado.Nombre;
                ContratoEditar.PorcentajeImpuesto = TipoImpuestoPredeterminado.Porcentaje;
            }
            else if (!Contrato.EsAfectoImpuesto && !string.IsNullOrEmpty(ContratoEditar.CodigoTipoImpuesto))
            {
                ContratoEditar.CodigoTipoImpuesto = Contrato.NombreTipoImpuesto = Validator.MsgErrorTipoImpuesto = null;
                ContratoEditar.PorcentajeImpuesto = null;
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
        ContratoEditar.CodigoCentroCosto = Contrato.CodigoCentroCosto = item.CodigoCentroCosto.Trim();
        Contrato.NombreCentroCosto = item.NombreCentroCosto;
		IsChangedCentroCosto = true;
		Validator.MsgErrorCentroCosto = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoCentroCosto"));
        NotifyChange();
    }

	private void CargarItemCatalogoTipoServicio(TipoServicioCatalogoDto item)
	{
        ContratoEditar.CodigoTipoServicio = Contrato.CodigoTipoServicio = item.CodigoTipoServicio;
		Contrato.NombreTipoServicio = item.NombreTipoServicio;
		IsChangedTipoServicio = true;
		Validator.MsgErrorTipoServicio = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoTipoServicio"));
        NotifyChange();
	}

	private void CargarItemCatalogoTipoTermino(TipoTerminoCatalogoDto item)
	{
		TerminoInsertar.CodigoTipoTermino = Termino.CodigoTipoTermino = item.CodigoTipoTermino;
		Termino.NombreTipoTermino = item.NombreTipoTermino;
		TerminoInsertar.NumeroTermino = GridTerminos.Any(x => x.CodigoTipoTermino == item.CodigoTipoTermino) ? GridTerminos.Where(x => x.CodigoTipoTermino == item.CodigoTipoTermino).Select(x => x.NumeroTermino).Max() + 1 : 1;
		EditContext.NotifyFieldChanged(EditContext.Field("CodigoTipoTermino"));
		IsModifiedTermino = TerminoInsertarContext.IsModified();
	}

	private void CargarItemCatalogoUsuarioAutoriza(UsuarioCatalogoPorEmpresaDto item)
	{
        ContratoEditar.CodigoUsuarioAutoriza = Contrato.CodigoUsuarioAutoriza = item.CodigoUsuario;
        Contrato.NombreUsuarioAutoriza = item.NombreUsuario;
		IsChangedUsuarioAutoriza = true;
		Validator.MsgErrorUsuarioAutoriza = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoUsuarioAutoriza"));
        NotifyChange();
	}

	private void CargarItemCatalogoUsuarioVerifica(UsuarioCatalogoPorEmpresaDto item)
	{ 
        ContratoEditar.CodigoUsuarioVerifica = Contrato.CodigoUsuarioVerifica = item.CodigoUsuario;
        Contrato.NombreUsuarioVerifica = item.NombreUsuario;
		IsChangedUsuarioVerifica = true;
		Validator.MsgErrorUsuarioVerifica = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoUsuarioVerifica"));
        NotifyChange();
	}

	private void CargarItemCatalogoTipoDevengo(TipoDevengoCatalogoDto item)
	{
        ContratoEditar.CodigoTipoDevengo = Contrato.CodigoTipoDevengo = item.CodigoTipoDevengo;
        Contrato.NombreTipoDevengo = item.NombreTipoDevengo;
		IsChangedTipoDevengo = true;
		Validator.MsgErrorTipoDevengo = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoTipoDevengo"));
        NotifyChange();
	}

	private void CargarItemCatalogoTipoImpuesto(TipoImpuestoCatalogoDto item)
	{
		ContratoEditar.CodigoTipoImpuesto = Contrato.CodigoTipoImpuesto = item.CodigoTipoImpuesto;
		Contrato.NombreTipoImpuesto = item.NombreTipoImpuesto;
		if ((Contrato.PorcentajeImpuesto ?? 0) != item.PorcentajeImpuesto)
        {
		    ContratoEditar.PorcentajeImpuesto = Contrato.PorcentajeImpuesto = item.PorcentajeImpuesto;
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
        ContratoEditar.CodigoEntidad = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorEntidad) && (Contrato.CodigoEntidad ?? "").Trim() != (ContratoEditar.CodigoEntidad ?? ""))
            Contrato.CodigoEntidad = Validator.MsgErrorEntidad = null;
    }

    private async Task OnChangeEntidadHandler(object value)
    {
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoEntidad")))
		{ 
			if ((Contrato.CodigoEntidad ?? "").Trim() != codigo)
			{
                IsChangedEntidad = true;
				(EntidadProveedorObtenerPorCodigoEmpresaDto item, Validator.MsgErrorEntidad) = await IUtil.ObtenerProveedorPorCodigoEmpresa(Alert, codigo, Empresa.Codigo, "L");
                if (item is not null) 
                { 
                    Contrato.CodigoEntidad = item.CodigoEntidad.Trim();
				    Contrato.NombreEntidad = item.NombreEntidad;
				    ContratoEditar.CodigoModoPago = item.CodigoModoPago;
				    Contrato.NombreModoPago = item.NombreModoPago;
                    ContratoEditar.CodigoPlazoCredito = item.CodigoPlazoCredito;
				    Contrato.NombrePlazoCredito = item.NombrePlazoCredito;
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
        Contrato.CodigoEntidad = codigo;
		Contrato.NombreEntidad = ContratoEditar.CodigoModoPago = Contrato.NombreModoPago = ContratoEditar.CodigoPlazoCredito = Contrato.NombrePlazoCredito = null;
	exit:
        NotifyChange();
    }

    private void ValueChangeTipoProvisionHandler(string value)
    {
        ContratoEditar.CodigoTipoProvision = value?.Trim().ToUpper();
		if (!string.IsNullOrEmpty(Validator.MsgErrorTipoProvision) && (Contrato.CodigoTipoProvision ?? "").Trim() != (ContratoEditar.CodigoTipoProvision ?? ""))
            Contrato.CodigoTipoProvision = Validator.MsgErrorTipoProvision = null;
    }

    private async Task OnChangeTipoProvisionHandler(object value)
    {
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoTipoProvision")))
		{ 
			if ((Contrato.CodigoTipoProvision ?? "") != codigo)
			{
                IsChangedTipoProvision = true;
				(TipoProvisionObtenerPorCodigoDto item, Validator.MsgErrorTipoProvision) = await IUtil.ObtenerTipoProvisionPorCodigo(Alert, codigo);
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
					    if (item.EsAfectoImpuesto && string.IsNullOrEmpty(ContratoEditar.CodigoTipoImpuesto))
					    {
						    ContratoEditar.CodigoTipoImpuesto = TipoImpuestoPredeterminado.Codigo;
						    Contrato.NombreTipoImpuesto = TipoImpuestoPredeterminado.Nombre;
						    ContratoEditar.PorcentajeImpuesto = Contrato.PorcentajeImpuesto = TipoImpuestoPredeterminado.Porcentaje;
					    }
					    else if (!item.EsAfectoImpuesto && !string.IsNullOrEmpty(ContratoEditar.CodigoTipoImpuesto))
					    {
						    ContratoEditar.CodigoTipoImpuesto = Contrato.NombreTipoImpuesto = Validator.MsgErrorTipoImpuesto = null;
						    ContratoEditar.PorcentajeImpuesto = Contrato.PorcentajeImpuesto = null;
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
        Contrato.CodigoTipoProvision = codigo;
		Contrato.NombreTipoProvision = Contrato.CodigoMoneda = Contrato.NombreMoneda = Contrato.SimboloMoneda = ContratoEditar.CodigoTipoImpuesto = Contrato.NombreTipoImpuesto = null;
		Contrato.EsAfectoImpuesto = Validator.EsAfectoImpuesto = false;
		ContratoEditar.PorcentajeImpuesto = null;
	exit:
		NotifyChange();
    }

    private void ValueChangeCentroCostoHandler(string value)
    {
        ContratoEditar.CodigoCentroCosto = value?.Trim().ToUpper();
		if (!string.IsNullOrEmpty(Validator.MsgErrorCentroCosto) && (Contrato.CodigoCentroCosto ?? "").Trim() != (ContratoEditar.CodigoCentroCosto ?? ""))
            Contrato.CodigoCentroCosto = Validator.MsgErrorCentroCosto = null;
    }

    private async Task OnChangeCentroCostoHandler(object value)
	{
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoCentroCosto")))
		{ 
			ContratoEditar.CodigoCentroCosto = string.IsNullOrEmpty(codigo) ? null : codigo; 
            if ((Contrato.CodigoCentroCosto ?? "") != codigo)
			{
                IsChangedCentroCosto = true;
                if (!string.IsNullOrEmpty(codigo))
                {
				    (CentroCostoObtenerPorCodigoDto item, Validator.MsgErrorCentroCosto) = await IUtil.ObtenerCentroCostoPorCodigo(Alert, codigo, Empresa.Codigo);
				    if (item is not null)
				    {
					    Contrato.CodigoCentroCosto = item.CodigoCentroCosto.Trim();
					    Contrato.NombreCentroCosto = item.NombreCentroCosto;
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
        Contrato.CodigoCentroCosto = codigo;
		Contrato.NombreCentroCosto = null;
	exit:
		NotifyChange(); 
	}

    private void ValueChangeTipoImpuestoHandler(string value)
    {
        ContratoEditar.CodigoTipoImpuesto = value?.Trim().ToUpper();
		if (!string.IsNullOrEmpty(Validator.MsgErrorTipoImpuesto) && (Contrato.CodigoTipoImpuesto ?? "").Trim() != (ContratoEditar.CodigoTipoImpuesto ?? ""))
            Contrato.CodigoTipoImpuesto = Validator.MsgErrorTipoImpuesto = null;
    }

    private async Task OnChangeTipoImpuestoHandler(object value)
	{
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoTipoImpuesto")))
		{ 
			if ((Contrato.CodigoTipoImpuesto ?? "").Trim() != codigo)
			{
                IsChangedTipoImpuesto = true;
				(TipoImpuestoObtenerPorCodigoDto item, Validator.MsgErrorTipoImpuesto) = await IUtil.ObtenerTipoImpuestoPorCodigo(Alert, codigo, true);
				if (item is not null)
				{
                    Contrato.CodigoTipoImpuesto = item.CodigoTipoImpuesto;
					Contrato.NombreTipoImpuesto = item.NombreTipoImpuesto;
                    if ((Contrato.PorcentajeImpuesto ?? 0) != item.PorcentajeImpuesto)
                    {
					    ContratoEditar.PorcentajeImpuesto = Contrato.PorcentajeImpuesto = item.PorcentajeImpuesto;
					    RecalcularImportes();
                    }
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
        Contrato.CodigoTipoImpuesto = codigo;
		Contrato.NombreTipoImpuesto = null;
		ContratoEditar.PorcentajeImpuesto = Contrato.PorcentajeImpuesto = null;
	exit:
		NotifyChange();
	}

    private void ValueChangeTipoServicioHandler(string value)
    {
        ContratoEditar.CodigoTipoServicio = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorTipoServicio) && (Contrato.CodigoTipoServicio ?? "").Trim() != (ContratoEditar.CodigoTipoServicio ?? ""))
            Contrato.CodigoTipoServicio = Validator.MsgErrorTipoServicio = null;
    }

    private async Task OnChangeTipoServicioHandler(object value)
	{
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoTipoServicio")))
		{ 
			if ((Contrato.CodigoTipoServicio ?? "").Trim() != codigo)
			{
                IsChangedTipoServicio = true;
				(TipoServicioObtenerPorCodigoDto item, Validator.MsgErrorTipoServicio) = await IUtil.ObtenerTipoServicioPorCodigo(Alert, codigo, Contrato.FlagTipoRegistro);
                if(item is not null)
                {
                    Contrato.CodigoTipoServicio = item.CodigoTipoServicio;
                    Contrato.NombreTipoServicio = item.NombreTipoServicio;
                    goto exit;
                }
                EditContext.NotifyFieldChanged(EditContext.Field("CodigoTipoServicio"));
			}
            else
            {
                if (!IsChangedTipoServicio) EditContext.MarkAsUnmodified(EditContext.Field("CodigoTipoServicio"));
                goto exit;
            }
		}
        Contrato.CodigoTipoServicio = codigo;
		Contrato.NombreTipoServicio = null;
	exit:
        NotifyChange();
	}

	private void ValueChangeTipoDevengoHandler(string value) => ContratoEditar.CodigoTipoDevengo = value?.Trim().ToUpper();

	private async Task OnChangeTipoDevengoHandler(object value)
	{
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoTipoDevengo")))
		{ 
			if ((Contrato.CodigoTipoDevengo ?? "").Trim() != codigo)
			{
                IsChangedTipoDevengo = true;
				(TipoDevengoObtenerPorCodigoDto item, Validator.MsgErrorTipoDevengo) = await IUtil.ObtenerTipoDevengoPorCodigo(Alert, codigo);
				if (item is not null)
				{
					Contrato.CodigoTipoDevengo = item.CodigoTipoDevengo;
					Contrato.NombreTipoDevengo = item.NombreTipoDevengo;
					goto exit;
				} 
				EditContext.NotifyFieldChanged(EditContext.Field("CodigoTipoDevengo"));
			}
            else
            {
                if (!IsChangedTipoDevengo) EditContext.MarkAsUnmodified(EditContext.Field("CodigoTipoDevengo"));
                goto exit;
            }
		}
        Contrato.CodigoTipoDevengo = codigo;
		Contrato.NombreTipoDevengo = null;
	exit:
        NotifyChange();
	}

    private void ValueChangeUsuarioVerificaHandler(string value)
    {
        ContratoEditar.CodigoUsuarioVerifica = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorUsuarioVerifica) && (Contrato.CodigoUsuarioVerifica ?? "").Trim() != (ContratoEditar.CodigoUsuarioVerifica ?? ""))
            Contrato.CodigoUsuarioVerifica = Validator.MsgErrorUsuarioVerifica = null;
    }

    private async Task OnChangeUsuarioVerificaHandler(object value)
	{
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoUsuarioVerifica")))
		{ 
			if ((Contrato.CodigoUsuarioVerifica ?? "").Trim() != codigo)
			{
                IsChangedUsuarioVerifica = true;
                (UsuarioObtenerPorCodigoEmpresaDto item, Validator.MsgErrorUsuarioVerifica) = await IUtil.ObtenerUsuarioPorCodigoEmpresa(Alert, codigo, Empresa.Codigo);
                if (item is not null)
                {
                    Contrato.CodigoUsuarioVerifica = item.CodigoUsuario;
                    Contrato.NombreUsuarioVerifica = item.NombreUsuario;
                    goto exit;
                } 
                EditContext.NotifyFieldChanged(EditContext.Field("CodigoUsuarioVerifica"));
            }
            else
            {
                if (!IsChangedUsuarioVerifica) EditContext.MarkAsUnmodified(EditContext.Field("CodigoUsuarioVerifica"));
                goto exit;
            }
		}
        Contrato.CodigoUsuarioVerifica = codigo;
		Contrato.NombreUsuarioVerifica = null;
	exit:
        NotifyChange();
	}

    private void ValueChangeUsuarioAutorizaHandler(string value)
    {
        ContratoEditar.CodigoUsuarioAutoriza = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorUsuarioAutoriza) && (Contrato.CodigoUsuarioAutoriza ?? "").Trim() != (ContratoEditar.CodigoUsuarioAutoriza ?? ""))
            Contrato.CodigoUsuarioAutoriza = Validator.MsgErrorUsuarioAutoriza = null;
    }

    private async Task OnChangeUsuarioAutorizaHandler(object value)
	{
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoUsuarioAutoriza")))
		{ 
			if ((Contrato.CodigoUsuarioAutoriza ?? "").Trim() != codigo)
			{
                IsChangedUsuarioAutoriza = true;
                (UsuarioObtenerPorCodigoEmpresaDto item, Validator.MsgErrorUsuarioVerifica) = await IUtil.ObtenerUsuarioPorCodigoEmpresa(Alert, codigo, Empresa.Codigo);
                if (item is not null)
                {
                    Contrato.CodigoUsuarioAutoriza = item.CodigoUsuario;
                    Contrato.NombreUsuarioAutoriza = item.NombreUsuario;
                    goto exit;
                } 
                EditContext.NotifyFieldChanged(EditContext.Field("CodigoUsuarioAutoriza"));
            }
            else
            {
                if (!IsChangedUsuarioAutoriza) EditContext.MarkAsUnmodified(EditContext.Field("CodigoUsuarioAutoriza"));
                goto exit;
            }
		}
        Contrato.CodigoUsuarioAutoriza = codigo;
		Contrato.NombreUsuarioAutoriza = null;
	exit:
        NotifyChange();
	}

	private void OnChangeCantidadCuotasHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Contrato.CantidadCuotas != (int?)value, EditContext, "CantidadCuotas"));

	private void OnChangeObservacionHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged((Contrato.Observacion ?? "") != ((string)value ?? ""), EditContext, "Observacion"));

	private void OnChangeReferenciaHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged((Contrato.Referencia ?? "") != ((string)value ?? ""), EditContext, "Referencia"));

	private void OnChangeFlagMedioPagoHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Contrato.FlagMedioPago != (string)value, EditContext, "FlagMedioPago"));

	private void OnChangeFlagIntervaloInformeHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Contrato.FlagIntervaloInforme != (string)value, EditContext, "FlagIntervaloInforme"));

	private void OnChangeFechaInicioHandler(object value)
	{
		NotifyChange(Fnc.VerifyContextIsChanged(Contrato.FechaInicio != (DateTime?)value, EditContext, "FechaInicio"));
		CalcularTiempoDuracion();
	}

	private void OnChangeFechaFinHandler(object value)
	{
		NotifyChange(Fnc.VerifyContextIsChanged(Contrato.FechaInicio != (DateTime?)value, EditContext, "FechaFin"));
		CalcularTiempoDuracion();
	}

    private void OnChangeEsGenerableDevengoHandler(object value)
    {
		if (!(bool)value && Contrato.EsGenerableDevengo)
        {
            ContratoEditar.CodigoTipoDevengo = Contrato.CodigoTipoDevengo = Contrato.NombreTipoDevengo = null;
            EditContext.NotifyFieldChanged(EditContext.Field("CodigoTipoDevengo"));
        }
        NotifyChange(Fnc.VerifyContextIsChanged(Contrato.EsGenerableDevengo != (bool)value, EditContext, "EsGenerableDevengo"));
    }
    #endregion

    public void Dispose() => GC.SuppressFinalize(this);
}
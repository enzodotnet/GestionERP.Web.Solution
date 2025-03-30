using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Requests;
using GestionERP.Web.Models.Dtos.Servicio;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components;
using GestionERP.Web.Services;
using System.Security.Claims; 
using GestionERP.Web.Models.Dtos.Principal;
using Telerik.Blazor;
using Telerik.Blazor.Components;
using Telerik.SvgIcons;
using GestionERP.Web.Models.Dtos.Report;
using Microsoft.AspNetCore.Components.Routing;
using GestionERP.Web.Global; 

namespace GestionERP.Web.Pages.Empresa.Servicio.Contrato;

public partial class View : IDisposable
{
    private const string codigoModulo = "03";
    private const string codigoServicio = "S110";
    private const string rutaServicio = "/contratos";
    private string rutaEmpresa = "";
    private const string nombreReporte = "ContratoServicioReport.trdp";

    public ContratoObtenerDto Contrato { get; set; }
    public ContratoDetalleObtenerDto Detalle { get; set; }
	public ContratoCuotaObtenerDto Cuota { get; set; }
	public ContratoTerminoObtenerDto Termino { get; set; }
	public EstadoActualizarRequest EstadoActualizar { get; set; }
    public ReportPrintDto ReportPrint { get; set; }
    public SerieDocumentoConsultaPorCodigoEmpresaDto Numerador { get; set; }
    public TelerikNotification AlertAccionDialog { get; set; }
    public TelerikNotification AlertPrintDialog { get; set; }
    public bool EsVisibleModalDetalle { get; set; }
    public bool EsVisibleModalCuota { get; set; }
    public bool EsVisibleModalTermino { get; set; }
    public bool EsVisibleListaCuotas { get; set; }
	public bool EsVisibleListaTerminos { get; set; } 
    public bool EsVisibleModalProvisiones { get; set; }
    public ISvgIcon IconoAccionModal { get; set; }
    public string VerboAccionDialog { get; set; }
    public string ObservacionAceptacion { get; set; }
    public string TituloAccionDialog { get; set; }
    public string TituloAccionLoading { get; set; }
    public string ResultadoAccion { get; set; }
    private bool EsVisibleAccionDialog { get; set; }
    private bool IsAuthUser { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool EsEstadoActualizar { get; set; }
    private bool EsVisiblePrintDialog { get; set; }
    private bool EsAsignadoImprimirDirecto { get; set; }
    private bool EsVisibleImprimirDirecto { get; set; }
    private bool EsAsignadoEliminar { get; set; }
    private bool EsVisibleEliminar { get; set; }
    private bool EsAsignadoExcluir { get; set; }
    private bool EsVisibleExcluir { get; set; }
    private bool EsAsignadoAnular { get; set; } 
    private bool EsAsignadoEditar { get; set; }
    private bool EsVisibleEditar { get; set; }
    private bool EsAsignadoEnviarAprobacion { get; set; }
    private bool EsAsignadoRevertirEnvioAprobacion { get; set; }
    private bool EsVisibleRevertirEnvioAprobacion { get; set; }
	private bool EsAsignadoVerReporte { get; set; } 
    private bool EsAsignadoVerAuditoria { get; set; }
    private string CodigoEstadoAccion { get; set; }
    private IEnumerable<ContratoFlag> TiposRegistro { get; set; }
    private IEnumerable<ContratoFlag> MediosPago { get; set; }
    private IEnumerable<ContratoFlag> EstadosDevengo { get; set; }
    private IEnumerable<ContratoFlag> IntervalosInforme { get; set; } 
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    public EmpresaConsultaPorCodigoWebDto Empresa { get; set; }
    [Parameter] public string CodigoWebEmpresa { get; set; } 
    [Parameter] public Guid? Id { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }

    [Inject] public IServicioContrato IContrato { get; set; }
    [Inject] public IReport IReport { get; set; }
    [Inject] public IPrincipalSerieDocumento ISerieDocumento { get; set; }
    [Inject] public IPrincipalEmpresa IEmpresa { get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Notify.ShowLoading(mensaje: "Obteniendo registro");

            EstadoActualizar = new();
			TiposRegistro = ContratoFlag.TiposRegistro();
			MediosPago = ContratoFlag.MediosPago();
			IntervalosInforme = ContratoFlag.IntervalosInforme();
            EstadosDevengo = ContratoFlag.EstadosDevengo();

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio);
            if (!IsAuthUser) return;

            Empresa = await IEmpresa.ConsultaPorCodigoWeb(CodigoWebEmpresa);
            rutaEmpresa = INavigation.Uri.Replace(INavigation.BaseUri, "").Split("?")[0].Replace(rutaServicio, "").Replace($"/{Id}", "");
            
            await ObtenerContrato();  
            if (Contrato is null)
            {
                INavigation.NavigateTo($"{rutaEmpresa}{rutaServicio}");
                Notify.Show($"El registro de la [Contrato de Servicio] a visualizar no está disponible", "error");
                return;
            }
            await CargarConsultaSerieDocumento();
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

    private async Task ObtenerContrato() 
    {  
        Contrato = await IContrato.Obtener(Empresa.Codigo, (Guid)Id);
        if (Contrato is not null)
        {
            ValidarEstadoOptionButtons(Contrato.CodigoEstado);
            await ValidarPermisoOptionButtons();
        }
    }
    
    private async Task Accionar()
    {
        try
        {
            if (EsEstadoActualizar && (string.IsNullOrEmpty(EstadoActualizar.Motivo) || EstadoActualizar.Motivo.Trim() == ""))
            {
                Fnc.MostrarAlerta(AlertAccionDialog, $"Es necesario que ingrese el motivo por el cual va a {VerboAccionDialog} el registro", "error");
                return;
            }

            IsLoadingAction = true;
            EsVisibleAccionDialog = false;

            IsAuthUser = (await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio, User.FindFirst("code").Value)).esValido;
			if (!IsAuthUser)
			{
				CerrarDialog();
				return;
			}
            
            Notify.ShowLoading(mensaje: $"{TituloAccionLoading} en progreso");
              
            if (!EsEstadoActualizar)
            {
                await IContrato.Eliminar(Empresa.Codigo, (Guid) Id);
                IsLoadingAction = false;
                INavigation.NavigateTo($"{rutaEmpresa}{rutaServicio}");
            }
            else
            { 
                await IContrato.ActualizarEstado(Empresa.Codigo, EstadoActualizar);
                await ObtenerContrato();
                CerrarDialog();
            }

            Notify.Show($"La solicitud de servicio {Contrato.Codigo} ha sido {ResultadoAccion} con éxito", "success");
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
            IsLoadingAction = false;
            Notify.ShowLoading(false);
        }
    }

    public async Task Cerrar(LocationChangingContext context)
    {
        if (IsAuthUser && IsLoadingAction && !await Dialog.ConfirmAsync("¿Está seguro de salir del formulario sin haber culminado la acción?", "Saliendo del formulario"))
            context.PreventNavigation();
    }

    private async Task ImprimirDirecto()
    {
        try
        {
            IsLoadingAction = true;

			IsAuthUser = (await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio, User.FindFirst("code").Value)).esValido;
			if (!IsAuthUser)
			{
				MostrarPrintDialog(false);
				return;
			}

            await CargarConsultaSerieDocumento();
            if (!EsVisibleImprimirDirecto)
            {
                Fnc.MostrarAlerta(AlertPrintDialog, Cnf.MsgErrorNoConfigPrinterDoc, "error");
                return;
            }

			ReportPrint = new()
            {
                ReportName = nombreReporte,
                CopieNumbers = 1,
                PrinterHost = Numerador.RutaHostImpresora,
                Parameters =
                {
					["Id"] = Id.ToString(),
                    ["Uid"] = User.FindFirst("uid").Value
                }
            };

            MostrarPrintDialog(false);
            StateHasChanged();
            Notify.ShowLoading(mensaje: "Impresión en progreso");
             
            if (!await IPermiso.ConsultaEsAsignadoPorSesion(ContratoAcceso.ImprimirDirecto, Empresa.Codigo))
            {
                Notify.Show("No tiene permiso para imprimir de forma directa registros de contratos", "error");
                return;
            }

            await IReport.Print(ReportPrint);
            Notify.Show($"El contrato {Contrato.Codigo} ha sido impreso con éxito", "success");
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

    private async Task CargarConsultaSerieDocumento()
    {
        Numerador = await ISerieDocumento.ConsultaPorCodigoEmpresa(Contrato.CodigoSerieDocumento, Contrato.CodigoDocumento, Empresa.Codigo);
        EsVisibleImprimirDirecto = !string.IsNullOrEmpty(Numerador.RutaHostImpresora) && !string.IsNullOrEmpty(Numerador.NombreImpresora);
    }

    private void MostrarPrintDialog(bool visible) => EsVisiblePrintDialog = visible;

    private async Task ValidarPermisoOptionButtons()
    {
        EsAsignadoEditar = await IPermiso.ConsultaEsAsignadoPorSesion(ContratoAcceso.Editar, Empresa.Codigo);
        EsAsignadoEliminar = await IPermiso.ConsultaEsAsignadoPorSesion(ContratoAcceso.Eliminar, Empresa.Codigo);
        EsAsignadoExcluir = await IPermiso.ConsultaEsAsignadoPorSesion(ContratoAcceso.Excluir, Empresa.Codigo);
        EsAsignadoAnular = await IPermiso.ConsultaEsAsignadoPorSesion(ContratoAcceso.Anular, Empresa.Codigo);
        EsAsignadoVerReporte = await IPermiso.ConsultaEsAsignadoPorSesion(ContratoAcceso.VerReporte, Empresa.Codigo);
		EsAsignadoImprimirDirecto = await IPermiso.ConsultaEsAsignadoPorSesion(ContratoAcceso.ImprimirDirecto, Empresa.Codigo);
        EsAsignadoVerAuditoria = await IPermiso.ConsultaEsAsignadoPorSesion(ContratoAcceso.VerAuditoria, Empresa.Codigo);
        EsAsignadoEnviarAprobacion = await IPermiso.ConsultaEsAsignadoPorSesion(ContratoAcceso.EnviarAprobacion, Empresa.Codigo);
        EsAsignadoRevertirEnvioAprobacion = await IPermiso.ConsultaEsAsignadoPorSesion(ContratoAcceso.RevertirEnvioAprobacion, Empresa.Codigo);
    }

    private void ValidarEstadoOptionButtons(string codigoEstado)
    {
        EsVisibleEditar = codigoEstado is "ED";
        EsVisibleExcluir = codigoEstado is "AX" or "DX" or "CX";
        EsVisibleEliminar = EsVisibleExcluir || codigoEstado is "EX";
        EsVisibleRevertirEnvioAprobacion = codigoEstado is "EA";
    }
     
    private void VerItemDetalle(ContratoDetalleObtenerDto item)
    {
        Detalle = item;
        VisibleDetalleChangedHandler(true);
    }

    private void VisibleDetalleChangedHandler(bool esVisible) => EsVisibleModalDetalle = esVisible;

    private void VerItemCuota(ContratoCuotaObtenerDto item)
    {
        Cuota = item;
        VisibleCuotaChangedHandler(true);
    }

    private void VisibleListaCuotaChangedHandler(bool esVisible) => EsVisibleListaCuotas = esVisible;

    private void VisibleCuotaChangedHandler(bool esVisible) => EsVisibleModalCuota = esVisible;

    private void VerItemTermino(ContratoTerminoObtenerDto item)
    {
        Termino = item;
        VisibleTerminoChangedHandler(true);
    }

    private void VisibleListaTerminoChangedHandler(bool esVisible) => EsVisibleListaTerminos = esVisible;

    private void VisibleTerminoChangedHandler(bool esVisible) => EsVisibleModalTermino = esVisible;

    private void Volver() => INavigation.NavigateTo($"{rutaEmpresa}{rutaServicio}");

    private void MostrarAccionDialog(string codigoEstado = "")
    {
        CodigoEstadoAccion = codigoEstado;
        EsEstadoActualizar = !string.IsNullOrEmpty(codigoEstado);

		IconoAccionModal = CodigoEstadoAccion switch
		{
			"AX" => SvgIcon.XOutline,
			"EX" => SvgIcon.MinusOutline,
            "EA" => SvgIcon.RotateRight,
            "RC" => SvgIcon.RotateLeft,
			_ => SvgIcon.Trash,
		};

		TituloAccionDialog = Fnc.ObtenerEstado(CodigoEstadoAccion, "title");
        TituloAccionLoading = Fnc.ObtenerEstado(CodigoEstadoAccion, "loading");
        ResultadoAccion = Fnc.ObtenerEstado(CodigoEstadoAccion, "result");
        VerboAccionDialog = Fnc.ObtenerEstado(CodigoEstadoAccion, "action").ToLower();

        if (EsEstadoActualizar)
            EstadoActualizar = new() { RegistroId = (Guid) Id, CodigoEstado = CodigoEstadoAccion };

        EsVisibleAccionDialog = true;
    }

    private void CerrarDialog()
    {
        EsEstadoActualizar = false; 
        EstadoActualizar = new();
        EsVisibleAccionDialog = false;
    }

    private void MostrarProvisiones(ContratoCuotaObtenerDto item)
    {
        Cuota = item;
        VisibleProvisionesChangedHandler(true);
    }
    public void VisibleProvisionesChangedHandler(bool esVisible) => EsVisibleModalProvisiones = esVisible;

    private void IrEditar() => INavigation.NavigateTo(INavigation.GetUriWithQueryParameters($"{rutaEmpresa}{rutaServicio}/{Id}/editar", new Dictionary<string, object> { ["returnpage"] = "view" }));

    private void IrReporte() => INavigation.NavigateTo($"{rutaEmpresa}{rutaServicio}/{Id}/reporte");

    public void Dispose() => GC.SuppressFinalize(this);
}
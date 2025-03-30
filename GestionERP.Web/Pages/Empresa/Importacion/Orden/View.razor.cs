using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Requests;
using GestionERP.Web.Models.Dtos.Importacion;
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

namespace GestionERP.Web.Pages.Empresa.Importacion.Orden;

public partial class View : IDisposable
{
    private const string codigoModulo = "02";
    private const string codigoServicio = "S105";
    private const string rutaServicio = "/ordenes";
    private string rutaEmpresa = "";
    private const string nombreReporte = "OrdenImportacionReport.trdp";

    public OrdenObtenerDto Orden { get; set; }
    public OrdenDetalleObtenerDto Detalle { get; set; }
	public OrdenNotaObtenerDto Nota { get; set; }
	public EstadoActualizarRequest EstadoActualizar { get; set; }
    public ReportPrintDto ReportPrint { get; set; }
    public SerieDocumentoConsultaPorCodigoEmpresaDto Numerador { get; set; }
    public TelerikNotification AlertAccionDialog { get; set; }
    public TelerikNotification AlertPrintDialog { get; set; }
    public bool EsVisibleModalDetalle { get; set; }
    public ISvgIcon IconoAccionModal { get; set; }
    public string VerboAccionDialog { get; set; }
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
	public bool EsVisibleListaNotas { get; set; }
    private string CodigoEstadoAccion { get; set; }
    private IEnumerable<OrdenFlag> Origenes { get; set; }
    private IEnumerable<OrdenFlag> MediosPago { get; set; }
	private IEnumerable<NotaReporteOrdenFlag> Secciones { get; set; }
	[CascadingParameter] public DialogFactory Dialog { get; set; }
    public EmpresaConsultaPorCodigoWebDto Empresa { get; set; }
    [Parameter] public string CodigoWebEmpresa { get; set; } 
    [Parameter] public Guid? Id { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }

    [Inject] public IImportacionOrden IOrden { get; set; }
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
            Origenes = OrdenFlag.Origenes();
            MediosPago = OrdenFlag.MediosPago();
			Secciones = NotaReporteOrdenFlag.Secciones();

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio);
            if (!IsAuthUser) return;

            Empresa = await IEmpresa.ConsultaPorCodigoWeb(CodigoWebEmpresa);
            rutaEmpresa = INavigation.Uri.Replace(INavigation.BaseUri, "").Split("?")[0].Replace(rutaServicio, "").Replace($"/{Id}", "");
            
            await ObtenerOrden();
            if (Orden is null)
            {
                INavigation.NavigateTo($"{rutaEmpresa}{rutaServicio}");
                Notify.Show($"El registro de la [Orden de Importación] a visualizar no está disponible", "error");
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

    private async Task ObtenerOrden() 
    { 
        Orden = await IOrden.Obtener(Empresa.Codigo, (Guid)Id);
        if (Orden is not null)
        {
            ValidarEstadoOptionButtons(Orden.CodigoEstado);
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
                await IOrden.Eliminar(Empresa.Codigo, (Guid) Id);
                IsLoadingAction = false;
                INavigation.NavigateTo($"{rutaEmpresa}{rutaServicio}");
            }
            else
            { 
                await IOrden.ActualizarEstado(Empresa.Codigo, EstadoActualizar);
                await ObtenerOrden();
                CerrarDialog();
            }
            
            Notify.Show($"La orden de importación {Orden.Codigo} ha sido {ResultadoAccion} con éxito", "success");
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
             
            if (!await IPermiso.ConsultaEsAsignadoPorSesion(OrdenAcceso.ImprimirDirecto, Empresa.Codigo))
            {
                Notify.Show("No tiene permiso para imprimir de forma directa registros de [Ordenes]", "error");
                return;
            }

            await IReport.Print(ReportPrint);
            Notify.Show($"La orden de importación {Orden.Codigo} ha sido impresa con éxito", "success");
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
        Numerador = await ISerieDocumento.ConsultaPorCodigoEmpresa(Orden.CodigoSerieDocumento, Orden.CodigoDocumento, Empresa.Codigo);
        EsVisibleImprimirDirecto = !string.IsNullOrEmpty(Numerador.RutaHostImpresora) && !string.IsNullOrEmpty(Numerador.NombreImpresora);
    }
     
    private void MostrarPrintDialog(bool visible) => EsVisiblePrintDialog = visible;

    private async Task ValidarPermisoOptionButtons()
    {
        EsAsignadoEditar = await IPermiso.ConsultaEsAsignadoPorSesion(OrdenAcceso.Editar, Empresa.Codigo);
        EsAsignadoEliminar = await IPermiso.ConsultaEsAsignadoPorSesion(OrdenAcceso.Eliminar, Empresa.Codigo);
        EsAsignadoExcluir = await IPermiso.ConsultaEsAsignadoPorSesion(OrdenAcceso.Excluir, Empresa.Codigo);
        EsAsignadoAnular = await IPermiso.ConsultaEsAsignadoPorSesion(OrdenAcceso.Anular, Empresa.Codigo);
        EsAsignadoVerReporte = await IPermiso.ConsultaEsAsignadoPorSesion(OrdenAcceso.VerReporte, Empresa.Codigo); 
		EsAsignadoImprimirDirecto = await IPermiso.ConsultaEsAsignadoPorSesion(OrdenAcceso.ImprimirDirecto, Empresa.Codigo);
        EsAsignadoVerAuditoria = await IPermiso.ConsultaEsAsignadoPorSesion(OrdenAcceso.VerAuditoria, Empresa.Codigo);
        EsAsignadoEnviarAprobacion = await IPermiso.ConsultaEsAsignadoPorSesion(OrdenAcceso.EnviarAprobacion, Empresa.Codigo);
        EsAsignadoRevertirEnvioAprobacion = await IPermiso.ConsultaEsAsignadoPorSesion(OrdenAcceso.RevertirEnvioAprobacion, Empresa.Codigo);
    }

    private void ValidarEstadoOptionButtons(string codigoEstado)
    {
        EsVisibleEditar = codigoEstado is "ED";
        EsVisibleExcluir = codigoEstado is "AX" or "DX" or "CX";
        EsVisibleEliminar = EsVisibleExcluir || codigoEstado is "EX";
        EsVisibleRevertirEnvioAprobacion = codigoEstado is "EA";
    }
     
    private void VerItemDetalle(OrdenDetalleObtenerDto item)
    {
        Detalle = item;
        VisibleDetalleChangedHandler(true);
    }

	private void VisibleListaNotaChangedHandler(bool esVisible) => EsVisibleListaNotas = esVisible;

	private void VisibleDetalleChangedHandler(bool esVisible) => EsVisibleModalDetalle = esVisible;

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
     
    private void IrEditar() => INavigation.NavigateTo(INavigation.GetUriWithQueryParameters($"{rutaEmpresa}{rutaServicio}/{Id}/editar", new Dictionary<string, object> { ["returnpage"] = "view" }));

    private void IrReporte() => INavigation.NavigateTo($"{rutaEmpresa}{rutaServicio}/{Id}/reporte");

    public void Dispose() => GC.SuppressFinalize(this);
}
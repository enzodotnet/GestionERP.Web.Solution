using Microsoft.AspNetCore.Components;
using GestionERP.Web.Models.Dtos.Servicio;
using GestionERP.Web.Components;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Services;
using GestionERP.Web.Handlers;
using System.Security.Claims;  
using GestionERP.Web.Models.Dtos.Principal;
using Telerik.ReportViewer.BlazorNative; 
using Telerik.Blazor.Components;
using Telerik.ReportViewer.BlazorNative.Tools;
using GestionERP.Web.Models.Dtos.Report;
using Microsoft.AspNetCore.Components.Routing;
using Telerik.Blazor;
using GestionERP.Web.Global;

namespace GestionERP.Web.Pages.Empresa.Servicio.Orden;

public partial class Report : IDisposable
{
    #region Propiedades
    private const string codigoModulo = "03";
    private const string codigoServicio = "S109";
    private const string rutaAccion = "/reporte";
    private const string rutaServicio = "/ordenes";
    private string rutaEmpresa = "";
    private const string nombreReporte = "OrdenServicioReport.trdp";

    public OrdenObtenerDto Orden { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool IsAuthUser { get; set; }
    private bool EsVisiblePrintDialog { get; set; }
    private bool EsVisibleImprimirDirecto { get; set; }
    private bool EsAsignadoImprimirDirecto { get; set; }
    public ReportPrintDto ReportPrint { get; set; }
    public SerieDocumentoConsultaPorCodigoEmpresaDto Numerador { get; set; }
    public List<IReportViewerTool> ToolsReport { get; set; }
    public ReportSourceOptions ReportSource { get; set; }
    public TelerikNotification AlertPrintDialog { get; set; }
    [Parameter] public Guid? Id { get; set; }
    public EmpresaConsultaPorCodigoWebDto Empresa { get; set; }
    private bool EsVisibleVolver { get; set; }
    [Parameter] public string CodigoWebEmpresa { get; set; }
    [Parameter][SupplyParameterFromQuery(Name = "returnpage")] public string ReturnPage { get; set; }
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    #endregion

    [Inject] public IServicioOrden IOrden { get; set; }
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
            Notify.ShowLoading(mensaje: "Obteniendo reporte");
             
            ReportPrint = new();
            CargarToolsReport();

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio);
            if (!IsAuthUser) return;

            Empresa = await IEmpresa.ConsultaPorCodigoWeb(CodigoWebEmpresa);
            EsVisibleVolver = !string.IsNullOrEmpty(ReturnPage) && ReturnPage is "index" or "view";
            rutaEmpresa = INavigation.Uri.Replace(INavigation.BaseUri, "").Split("?")[0].Replace(rutaServicio, "").Replace($"/{Id}", "").Replace(rutaAccion, "");

			if (!await IPermiso.ConsultaEsAsignadoPorSesion(OrdenAcceso.VerReporte, Empresa.Codigo))
			{
				INavigation.NavigateTo($"{rutaEmpresa}/inicio");
				Notify.Show("No tiene permiso para ver reporte de [Órdenes de Servicio]", "error");
				return;
			}

			EsAsignadoImprimirDirecto = await IPermiso.ConsultaEsAsignadoPorSesion(OrdenAcceso.ImprimirDirecto, Empresa.Codigo);

            if (await IPermiso.ConsultaEsAsignadoPorSesion(OrdenAcceso.ExportarReportePDF, Empresa.Codigo))
                ToolsReport.Add(new Export());

            if (await IPermiso.ConsultaEsAsignadoPorSesion(OrdenAcceso.Imprimir, Empresa.Codigo))
                ToolsReport.Add(new Print());

            Orden = await IOrden.Obtener(Empresa.Codigo, (Guid)Id);
            if (Orden is null)
            {
                INavigation.NavigateTo($"{rutaEmpresa}{rutaServicio}");
                Notify.Show($"El registro de la [Orden de Servicio] consultado a ver reporte no está disponible", "error");
                return;
            }
            ReportSource = new
            (
                nombreReporte, 
                new Dictionary<string, object> 
                {
					["Id"] = Id.ToString(),
                    ["Uid"] = User.FindFirst("uid").Value
                }
            );

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
     
    private async Task ImprimirDirecto()
    { 
        try
        {
            IsLoadingAction = true;

            IsAuthUser = (await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio, User.FindFirst("code").Value)).esValido;
            if (!IsAuthUser) return;

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
                Notify.Show("No tiene permiso para imprimir de forma directa registros de [Órdenes]", "error");
                return;
            }

            await IReport.Print(ReportPrint);
            Notify.Show($"La orden de servicio {Orden.Codigo} ha sido impresa con éxito", "success");
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

    public async Task Cerrar(LocationChangingContext context)
    {
        if (IsAuthUser && IsLoadingAction && !await Dialog.ConfirmAsync("¿Está seguro de salir del formulario sin haber culminado la acción?", "Saliendo del formulario"))
            context.PreventNavigation();
    }

    private void CargarToolsReport()
    {
        ToolsReport =
        [
            new Refresh(),
            new NavigateBackward(),
            new NavigateForward(),
            new FirstPage(),
            new PreviousPage(),
            new PageNumber(),
            new NextPage(),
            new LastPage(),
            new ToggleViewMode(), 
            new ZoomIn(),
            new ZoomOut(),
            new ToggleScaleMode(),
            new Search()
        ]; 
    }

    private async Task CargarConsultaSerieDocumento()
    {
        Numerador = await ISerieDocumento.ConsultaPorCodigoEmpresa(Orden.CodigoSerieDocumento, Orden.CodigoDocumento, Empresa.Codigo);
        EsVisibleImprimirDirecto = !string.IsNullOrEmpty(Numerador.RutaHostImpresora) && !string.IsNullOrEmpty(Numerador.NombreImpresora);
    }

    private void Volver() => INavigation.NavigateTo($"{rutaEmpresa}{rutaServicio}{(ReturnPage == "view" ? $"/{Id}" : "")}");
     
    private void MostrarPrintDialog(bool visible) => EsVisiblePrintDialog = visible;
    
    public void Dispose() => GC.SuppressFinalize(this);
}
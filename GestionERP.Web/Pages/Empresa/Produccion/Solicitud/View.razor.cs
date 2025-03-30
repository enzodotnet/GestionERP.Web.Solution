using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Requests;
using GestionERP.Web.Models.Dtos.Produccion;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components;
using GestionERP.Web.Services;
using System.Security.Claims;  
using GestionERP.Web.Models.Dtos.Principal;
using Telerik.Blazor;
using Telerik.Blazor.Components;
using Telerik.SvgIcons;
using Microsoft.AspNetCore.Components.Routing;
using GestionERP.Web.Global;

namespace GestionERP.Web.Pages.Empresa.Produccion.Solicitud;

public partial class View : IDisposable
{
    private const string codigoModulo = "05";
    private const string codigoServicio = "S111";
    private const string rutaServicio = "/solicitudes";
    private string rutaEmpresa = "";

    public SolicitudObtenerDto Solicitud { get; set; }
    public EstadoActualizarRequest EstadoActualizar { get; set; }
    public SerieDocumentoConsultaPorCodigoEmpresaDto Numerador { get; set; }
    public TelerikNotification AlertAccionDialog { get; set; }
    public TelerikNotification AlertPrintDialog { get; set; }
    public ISvgIcon IconoAccionModal { get; set; }
    public string VerboAccionDialog { get; set; }
    public string TituloAccionDialog { get; set; }
    public string TituloAccionLoading { get; set; }
    public string ResultadoAccion { get; set; } 
    public string TipoAccionDialog { get; set; }
    private bool EsVisibleAccionDialog { get; set; } 
    private bool IsAuthUser { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool EsEstadoActualizar { get; set; }
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
    private IEnumerable<SolicitudFlag> NivelesPrioridad { get; set; }
    private IEnumerable<SolicitudFlag> TiposRegistro { get; set; }
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    public EmpresaConsultaPorCodigoWebDto Empresa { get; set; }
    [Parameter] public string CodigoWebEmpresa { get; set; } 
    [Parameter] public Guid? Id { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }

    [Inject] public IProduccionSolicitud ISolicitud { get; set; }
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
            NivelesPrioridad = SolicitudFlag.NivelesPrioridad();
            TiposRegistro = SolicitudFlag.TiposRegistro();

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio);
            if (!IsAuthUser) return;

            Empresa = await IEmpresa.ConsultaPorCodigoWeb(CodigoWebEmpresa);
            rutaEmpresa = INavigation.Uri.Replace(INavigation.BaseUri, "").Split("?")[0].Replace(rutaServicio, "").Replace($"/{Id}", "");
            
            await ObtenerSolicitud();
            if (Solicitud is null)
            {
                INavigation.NavigateTo($"{rutaEmpresa}{rutaServicio}");
                Notify.Show($"El registro de la [Solicitud de Produccion] a visualizar no está disponible", "error");
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

    private async Task ObtenerSolicitud() 
    { 
        Solicitud = await ISolicitud.Obtener(Empresa.Codigo, (Guid)Id);
        if (Solicitud is not null)
        {
            ValidarEstadoOptionButtons(Solicitud.CodigoEstado);
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
                await ISolicitud.Eliminar(Empresa.Codigo, (Guid) Id);
                IsLoadingAction = false;
                INavigation.NavigateTo($"{rutaEmpresa}{rutaServicio}");
            }
            else
            { 
                await ISolicitud.ActualizarEstado(Empresa.Codigo, EstadoActualizar);
                await ObtenerSolicitud();
                CerrarDialog();
            }
            
            Notify.Show($"La solicitud de producción {Solicitud.Codigo} ha sido {ResultadoAccion} con éxito", "success");
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

    private async Task CargarConsultaSerieDocumento()
    {
        Numerador = await ISerieDocumento.ConsultaPorCodigoEmpresa(Solicitud.CodigoSerieDocumento, Solicitud.CodigoDocumento, Empresa.Codigo);
    }

    private async Task ValidarPermisoOptionButtons()
    {
        EsAsignadoEditar = await IPermiso.ConsultaEsAsignadoPorSesion(SolicitudAcceso.Editar, Empresa.Codigo);
        EsAsignadoEliminar = await IPermiso.ConsultaEsAsignadoPorSesion(SolicitudAcceso.Eliminar, Empresa.Codigo);
        EsAsignadoExcluir = await IPermiso.ConsultaEsAsignadoPorSesion(SolicitudAcceso.Excluir, Empresa.Codigo);
        EsAsignadoAnular = await IPermiso.ConsultaEsAsignadoPorSesion(SolicitudAcceso.Anular, Empresa.Codigo);
        EsAsignadoVerReporte = await IPermiso.ConsultaEsAsignadoPorSesion(SolicitudAcceso.VerReporte, Empresa.Codigo);
        EsAsignadoVerAuditoria = await IPermiso.ConsultaEsAsignadoPorSesion(SolicitudAcceso.VerAuditoria, Empresa.Codigo);
        EsAsignadoEnviarAprobacion = await IPermiso.ConsultaEsAsignadoPorSesion(SolicitudAcceso.EnviarAprobacion, Empresa.Codigo);
        EsAsignadoRevertirEnvioAprobacion = await IPermiso.ConsultaEsAsignadoPorSesion(SolicitudAcceso.RevertirEnvioAprobacion, Empresa.Codigo);
    }

    private void ValidarEstadoOptionButtons(string codigoEstado)
    {
        EsVisibleEditar = codigoEstado is "ED";
        EsVisibleExcluir = codigoEstado is "AX" or "DX" or "CX";
        EsVisibleEliminar = EsVisibleExcluir || codigoEstado is "EX";
        EsVisibleRevertirEnvioAprobacion = codigoEstado is "EA";
    }

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
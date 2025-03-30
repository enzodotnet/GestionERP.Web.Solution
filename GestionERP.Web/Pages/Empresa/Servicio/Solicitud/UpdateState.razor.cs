using Microsoft.AspNetCore.Components;
using GestionERP.Web.Models.Dtos.Servicio;
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Components;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Services;
using GestionERP.Web.Handlers;
using System.Security.Claims;
using Telerik.SvgIcons;
using Microsoft.AspNetCore.Components.Web;
using Telerik.Blazor.Components;
using GestionERP.Web.Models.Requests;
using Microsoft.AspNetCore.Components.Routing;
using Telerik.Blazor;
using GestionERP.Web.Global; 

namespace GestionERP.Web.Pages.Empresa.Servicio.Solicitud;

public partial class UpdateState : IDisposable
{
    #region Propiedades
    private const string codigoModulo = "03";
    private const string codigoServicio = "S108";
    private const string rutaServicio = "/solicitudes";
    private const string rutaAccion = "/estado";
    private string rutaEmpresa = "";

    private IEnumerable<SolicitudCatalogoActualizarEstadoDto> CatalogoSolicitudes { get; set; }
    private SolicitudCatalogoActualizarEstadoDto SolicitudCatalogo { get; set; }
    private EstadoActualizarRequest EstadoActualizar { get; set; }
    public TelerikNotification AlertAccionDialog { get; set; }
    private bool EsVisibleAccionDialog { get; set; }
    private string CodigoRegistro { get; set; }  
    private bool EsAsignadoDesestimar { get; set; }
    private bool IsInitGrid { get; set; }
    private bool IsAuthUser { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool IsSelectedBotonFiltro { get; set; }
    private bool EsVisibleModalCatalogo { get; set; }
    private string TituloModalCatalogoAccion { get; set; }
    private string CodigoEstadoCatalogo { get; set; }
    private string CodigoEstadoAccion { get; set; }
    private string NombreBotonFiltro { get; set; }
    private ISvgIcon IconoBotonFiltro { get; set; }
    private ISvgIcon IconoAccion { get; set; }
    private string ClassAccionBoton { get; set; }
    private string VerboAccionBoton { get; set; }
    private string TitleAccionBoton { get; set; }
    private bool EsVisibleBotonFiltro { get; set; } 
    private bool EsVisibleDesestimar { get; set; }
    private bool EsVisibleNivelPrioridad { get; set; }
    private bool EsVisibleCantidadAtendida { get; set; }
    private bool EsAccionDesestimar { get; set; }
    private string TituloVistaAccion { get; set; }
    private string TituloCardAccion { get; set; } 
    private string VerboAccionDialog { get; set; }
    private string TituloAccionDialog { get; set; }
    private string TituloAccionLoading { get; set; }
    private string ResultadoAccion { get; set; }
    private string PermisoAccion { get; set; }
    private bool EsAsignadoAccion { get; set; }
    private string VistaAccionTemp { get; set; }
    private IEnumerable<SolicitudFlag> NivelesPrioridad { get; set; }
    public EmpresaConsultaPorCodigoWebDto Empresa { get; set; }
    [Parameter] public string CodigoWebEmpresa { get; set; }
    [Parameter] public string VistaAccion { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    #endregion

    [Inject] public IServicioSolicitud ISolicitud { get; set; }
    [Inject] public IPrincipalEmpresa IEmpresa { get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);
        if (!string.IsNullOrEmpty(VistaAccion))
        {
            if (!string.IsNullOrEmpty(VistaAccionTemp) && VistaAccion != VistaAccionTemp) 
                await IniciarVistaAccion(); 
            
            VistaAccionTemp = VistaAccion;
        }
    }

    protected override async Task OnInitializedAsync()
    {
		await IniciarVistaAccion();
	}

    private async Task Accionar()
    {
        try
        {
            if (string.IsNullOrEmpty(EstadoActualizar.Motivo) || EstadoActualizar.Motivo.Trim() == "")
            {
                Fnc.MostrarAlerta(AlertAccionDialog, $"Es necesario que ingrese el motivo por el cual va a {VerboAccionDialog}", "error");
                return;
            }
            
            IsLoadingAction = true;
            EsVisibleAccionDialog = EsVisibleModalCatalogo = false;
            IsAuthUser = (await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio, User.FindFirst("code").Value)).esValido;
            if (!IsAuthUser)
            {
                CerrarDialog();
                return;
            }
            
            Notify.ShowLoading(mensaje: $"{TituloAccionLoading} en progreso");

            await ISolicitud.ActualizarEstado(Empresa.Codigo, EstadoActualizar);
            await CargarCatalogoSolicitudes();
            CerrarDialog();

            Notify.Show($"La solicitud de servicio {CodigoRegistro} ha sido {ResultadoAccion} con éxito", "success");
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

    private async Task IniciarVistaAccion()
    {
		try
		{
			Notify.ShowLoading(mensaje: "Listando catálogo");

			EsVisibleAccionDialog = IsInitGrid = false;
			NivelesPrioridad = SolicitudFlag.NivelesPrioridad();
            
            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio);
            if (!IsAuthUser) return;

            Empresa = await IEmpresa.ConsultaPorCodigoWeb(CodigoWebEmpresa);
			rutaEmpresa = INavigation.Uri.Replace(INavigation.BaseUri, "").Split("?")[0].Replace(rutaServicio, "").Replace(rutaAccion, "").Replace(string.Concat("/", VistaAccion ?? ""), "");

			if (!VerificarTipoOpcionEsValido())
			{
				INavigation.NavigateTo($"{rutaEmpresa}/inicio");
				Notify.Show("El tipo de acción al que intenta acceder para actualizar estado en [Solicitudes de Servicios] no es válido", "error");
				return;
			}
             
			EsAsignadoDesestimar = await IPermiso.ConsultaEsAsignadoPorSesion(SolicitudAcceso.Desestimar, Empresa.Codigo);
			EsAsignadoAccion = await IPermiso.ConsultaEsAsignadoPorSesion(PermisoAccion, Empresa.Codigo);

			await CargarCatalogoSolicitudes();
            StateHasChanged();
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
    
    private async Task RefrescarCatalogoSolicitudes()
    {
        try
        {
            IsAuthUser = (await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio, User.FindFirst("code").Value)).esValido;
            if (!IsAuthUser) return;

            Notify.ShowLoading(mensaje: "Refrescando catálogo");

            CatalogoSolicitudes = await ISolicitud.CatalogoActualizarEstado(Empresa.Codigo, CodigoEstadoCatalogo);
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

    public async Task Cerrar(LocationChangingContext context)
    { 
        if (IsAuthUser && IsLoadingAction && !await Dialog.ConfirmAsync($"¿Está seguro de salir sin haber culminado la acción de {VerboAccionDialog}?", "Saliendo del formulario"))
            context.PreventNavigation(); 
    }

    private bool VerificarTipoOpcionEsValido() 
    {
        bool esValido = false; 
        if (!string.IsNullOrEmpty(VistaAccion))
        { 
            switch (VistaAccion)
            {
                case "revisar":
                    esValido = true;
                    CodigoEstadoCatalogo = "RV"; 
                    NombreBotonFiltro = "Filtrar registros revisados";
                    IconoBotonFiltro = SvgIcon.Filter;
                    EsVisibleBotonFiltro = true;
                    IconoAccion = SvgIcon.CheckCircle;
                    ClassAccionBoton = "icon-check";
                    PermisoAccion = SolicitudAcceso.Revisar;
                    EsVisibleDesestimar = EsVisibleNivelPrioridad = true;
					IsSelectedBotonFiltro = false;
					break;
                case "aprobar":
                    esValido = true;
                    CodigoEstadoCatalogo = "AP"; 
                    NombreBotonFiltro = "Filtrar registros aprobados";
                    IconoBotonFiltro = SvgIcon.Filter;
                    EsVisibleBotonFiltro = true;
                    IconoAccion = SvgIcon.CheckCircle;
                    ClassAccionBoton = "icon-check";
                    PermisoAccion = SolicitudAcceso.Aprobar;
                    EsVisibleDesestimar = EsVisibleNivelPrioridad = true;
					IsSelectedBotonFiltro = false;
					break;
                case "rescindir":
                    esValido = true;
                    CodigoEstadoCatalogo = "RS"; 
                    IconoAccion = SvgIcon.MinusCircle;
                    ClassAccionBoton = "icon-minus";
                    PermisoAccion = SolicitudAcceso.Rescindir;
                    EsVisibleCantidadAtendida = true;
                    EsVisibleNivelPrioridad = false;
                    IsSelectedBotonFiltro = false;
                    EsVisibleBotonFiltro = false;
                    EsVisibleDesestimar = false;
                    break;
                case "cancelar":
                    esValido = true;
                    CodigoEstadoCatalogo = "CX"; 
                    IconoAccion = SvgIcon.XCircle;
                    ClassAccionBoton = "icon-x";
                    PermisoAccion = SolicitudAcceso.Cancelar;
					EsVisibleNivelPrioridad = false;
                    IsSelectedBotonFiltro = false;
                    EsVisibleBotonFiltro = false;
                    EsVisibleDesestimar = false;
                    break;
                default: 
                    esValido = false; 
                    break;
            }

            if (esValido)
            {
                TituloVistaAccion = Fnc.ObtenerEstado(CodigoEstadoCatalogo, "view"); 
                TituloCardAccion = $"{TituloVistaAccion} de solicitudes";
                VerboAccionBoton = Fnc.ObtenerEstado(CodigoEstadoCatalogo, "action");
                TitleAccionBoton = $"{VerboAccionBoton} registro";
                TituloModalCatalogoAccion = Fnc.ObtenerEstado(CodigoEstadoCatalogo, "action").ToLower();
            }
        }
        return esValido;
    }

    public static void OnCellNivelPrioridadRenderHandler(GridCellRenderEventArgs args) => args.Class = args.Value.ToString() switch
    {
        "1" => "cell-red",
        "2" => "cell-orange",
        "3" => "cell-yellow",
        "4" => "cell-green",
        "5" => "cell-blue",
        _ => ""
    };

    private async Task OnToggleButtonFiltroClick(MouseEventArgs args)
    {
        CatalogoSolicitudes = [];
        IsInitGrid = false;
 
        if (!IsSelectedBotonFiltro)
        {
            IconoBotonFiltro = SvgIcon.FilterClear;
            NombreBotonFiltro = string.Concat("Limpiar filtro de ", VistaAccion == "revisar" ? "revisados" : "aprobados");
            CodigoEstadoCatalogo = VistaAccion == "revisar" ? "RR" : "DP";
            IconoAccion = SvgIcon.XCircle;
            ClassAccionBoton = "icon-x";
            PermisoAccion = VistaAccion == "revisar" ? SolicitudAcceso.RetirarRevision : SolicitudAcceso.Desaprobar;
            EsVisibleDesestimar = false;
        }
        else
        {
            IconoBotonFiltro = SvgIcon.Filter;
            NombreBotonFiltro = string.Concat("Filtrar registros ", VistaAccion == "revisar" ? "revisados" : "aprobados");
            CodigoEstadoCatalogo = VistaAccion == "revisar" ? "RV" : "AP";
            IconoAccion = SvgIcon.CheckCircle;
            ClassAccionBoton = "icon-check";
            PermisoAccion = VistaAccion == "revisar" ? SolicitudAcceso.Revisar : SolicitudAcceso.Aprobar;
            EsVisibleDesestimar = true;
        }

        VerboAccionBoton = Fnc.ObtenerEstado(CodigoEstadoCatalogo, "action");
        TitleAccionBoton = $"{Fnc.ObtenerEstado(CodigoEstadoCatalogo, "action")} registro";
        TituloModalCatalogoAccion = Fnc.ObtenerEstado(CodigoEstadoCatalogo, "action").ToLower();
        EsAsignadoAccion = await IPermiso.ConsultaEsAsignadoPorSesion(PermisoAccion, Empresa.Codigo);

        await RefrescarCatalogoSolicitudes();
    }

    private void MostrarAccionDialog(Guid id, bool esDesestimar = false)
    {
        EsAccionDesestimar = esDesestimar; 
        CodigoEstadoAccion = EsAccionDesestimar ? "DX" : CodigoEstadoCatalogo;  
            
        TituloAccionDialog = Fnc.ObtenerEstado(CodigoEstadoAccion, "title");
        TituloAccionLoading = Fnc.ObtenerEstado(CodigoEstadoAccion, "loading");
        ResultadoAccion = Fnc.ObtenerEstado(CodigoEstadoAccion, "result");
        VerboAccionDialog = Fnc.ObtenerEstado(CodigoEstadoAccion, "action").ToLower();

        EstadoActualizar = new() 
        { 
            RegistroId = id, 
            CodigoEstado = CodigoEstadoAccion
        };
         
        CodigoRegistro = CatalogoSolicitudes.Where(x => x.SolicitudId == id).Select(x => x.CodigoSolicitud).FirstOrDefault().Trim();
        EsVisibleAccionDialog = true;
    }

    private void CerrarDialog()
    {
        EstadoActualizar = new();
        EsAccionDesestimar = false;
        EsVisibleAccionDialog = false;
        SolicitudCatalogo = new();
        EsVisibleModalCatalogo = false;
    }

    private void VerItemCatalogo(SolicitudCatalogoActualizarEstadoDto item)
    {
        SolicitudCatalogo = item;
        EsVisibleModalCatalogo = true;
    }

    private void VisibleModalChangedHandler(bool esVisible) => EsVisibleModalCatalogo = esVisible;

    private async Task CargarCatalogoSolicitudes()
    {
        CatalogoSolicitudes = await ISolicitud.CatalogoActualizarEstado(Empresa.Codigo, CodigoEstadoCatalogo);
        IsInitGrid = true; 
    }

    public void Dispose() => GC.SuppressFinalize(this); 
}
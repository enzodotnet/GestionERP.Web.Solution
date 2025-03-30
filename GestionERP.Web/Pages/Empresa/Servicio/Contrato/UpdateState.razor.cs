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

namespace GestionERP.Web.Pages.Empresa.Servicio.Contrato;

public partial class UpdateState : IDisposable
{
    #region Propiedades
    private const string codigoModulo = "03";
    private const string codigoServicio = "S110";
    private const string rutaServicio = "/contratos";
    private const string rutaAccion = "/estado";
    private string rutaEmpresa = "";

    private IEnumerable<ContratoCatalogoActualizarEstadoDto> CatalogoContratos { get; set; }
    private ContratoCatalogoActualizarEstadoDto ContratoCatalogo { get; set; }
    private EstadoActualizarRequest EstadoActualizar { get; set; }
    public TelerikNotification AlertAccionDialog { get; set; } 
    private IEnumerable<ContratoFlag> MediosPago { get; set; } 
	private IEnumerable<ContratoFlag> TiposRegistro { get; set; }  
	private IEnumerable<ContratoFlag> IntervalosInforme { get; set; }
	private bool EsVisibleAccionDialog { get; set; }
    private string CodigoRegistro { get; set; }  
    private bool EsAsignadoDesestimar { get; set; }
    private bool IsInitGrid { get; set; }
    private bool IsAuthUser { get; set; }
    private bool IsLoadingAction { get; set; }
    public bool EsVisibleDescripcionTermino { get; set; }
    public bool EsVisibleListaCuotas { get; set; }
    public bool EsVisibleListaTerminos { get; set; }
    public string DescripcionTerminoDialog { get; set; }
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
    public EmpresaConsultaPorCodigoWebDto Empresa { get; set; }
    [Parameter] public string CodigoWebEmpresa { get; set; }
    [Parameter] public string VistaAccion { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    #endregion

    [Inject] public IServicioContrato IContrato { get; set; }
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
        TiposRegistro = ContratoFlag.TiposRegistro();
        MediosPago = ContratoFlag.MediosPago();
        IntervalosInforme = ContratoFlag.IntervalosInforme();
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
             
            await IContrato.ActualizarEstado(Empresa.Codigo, EstadoActualizar);
            await CargarCatalogoContratos();
            CerrarDialog();

            Notify.Show($"El contrato {CodigoRegistro} ha sido {ResultadoAccion} con éxito", "success");
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
            
            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio);
            if (!IsAuthUser) return;

            Empresa = await IEmpresa.ConsultaPorCodigoWeb(CodigoWebEmpresa);
			rutaEmpresa = INavigation.Uri.Replace(INavigation.BaseUri, "").Split("?")[0].Replace(rutaServicio, "").Replace(rutaAccion, "").Replace(string.Concat("/", VistaAccion ?? ""), "");

			if (!VerificarTipoOpcionEsValido())
			{
				INavigation.NavigateTo($"{rutaEmpresa}/inicio");
				Notify.Show("El tipo de acción al que intenta acceder para actualizar estado en contratos no es válido", "error");
				return;
			} 

			EsAsignadoDesestimar = await IPermiso.ConsultaEsAsignadoPorSesion(ContratoAcceso.Desestimar, Empresa.Codigo);
			EsAsignadoAccion = await IPermiso.ConsultaEsAsignadoPorSesion(PermisoAccion, Empresa.Codigo);

			await CargarCatalogoContratos();
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
    
    private async Task RefrescarCatalogoContratos()
    {
        try
        {
            IsAuthUser = (await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio, User.FindFirst("code").Value)).esValido;
            if (!IsAuthUser) return;

            Notify.ShowLoading(mensaje: "Refrescando catálogo");

            CatalogoContratos = await IContrato.CatalogoActualizarEstado(Empresa.Codigo, CodigoEstadoCatalogo);
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
                case "aprobar":
                    esValido = true;
                    CodigoEstadoCatalogo = "AP"; 
                    NombreBotonFiltro = "Filtrar registros aprobados";
                    IconoBotonFiltro = SvgIcon.Filter;
                    EsVisibleBotonFiltro = true;
                    IconoAccion = SvgIcon.CheckCircle;
                    ClassAccionBoton = "icon-check";
                    PermisoAccion = ContratoAcceso.Aprobar;
                    EsVisibleDesestimar = true;
					IsSelectedBotonFiltro = false;
					break;
                case "rescindir":
                    esValido = true;
                    CodigoEstadoCatalogo = "RS"; 
                    IconoAccion = SvgIcon.MinusCircle;
                    ClassAccionBoton = "icon-minus";
                    PermisoAccion = ContratoAcceso.Rescindir;
                    EsVisibleCantidadAtendida = true;
                    IsSelectedBotonFiltro = false;
                    EsVisibleBotonFiltro = false;
                    EsVisibleDesestimar = false;
                    break;
                case "cancelar":
                    esValido = true;
                    CodigoEstadoCatalogo = "CX"; 
                    IconoAccion = SvgIcon.XCircle;
                    ClassAccionBoton = "icon-x";
                    PermisoAccion = ContratoAcceso.Cancelar;
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
                TituloCardAccion = $"{TituloVistaAccion} de contratos"; 
                VerboAccionBoton = Fnc.ObtenerEstado(CodigoEstadoCatalogo, "action");
                TitleAccionBoton = $"{VerboAccionBoton} registro";
                TituloModalCatalogoAccion = Fnc.ObtenerEstado(CodigoEstadoCatalogo, "action").ToLower();
            }
        }
        return esValido;
    }

    private async Task OnToggleButtonFiltroClick(MouseEventArgs args)
    {
        CatalogoContratos = [];
        IsInitGrid = false;
 
        if (!IsSelectedBotonFiltro)
        {
            IconoBotonFiltro = SvgIcon.FilterClear;
            NombreBotonFiltro = "Limpiar filtro de aprobados";
            CodigoEstadoCatalogo = "DP";
            IconoAccion = SvgIcon.XCircle;
            ClassAccionBoton = "icon-x";
            PermisoAccion = ContratoAcceso.Desaprobar;
            EsVisibleDesestimar = false;
        }
        else
        {
            IconoBotonFiltro = SvgIcon.Filter;
            NombreBotonFiltro = "Filtrar registros aprobados";
            CodigoEstadoCatalogo = "AP";
            IconoAccion = SvgIcon.CheckCircle;
            ClassAccionBoton = "icon-check";
            PermisoAccion = ContratoAcceso.Aprobar;
            EsVisibleDesestimar = true;
        }

        VerboAccionBoton = Fnc.ObtenerEstado(CodigoEstadoCatalogo, "action");
        TitleAccionBoton = $"{Fnc.ObtenerEstado(CodigoEstadoCatalogo, "action")} registro";
        TituloModalCatalogoAccion = Fnc.ObtenerEstado(CodigoEstadoCatalogo, "action").ToLower();
        EsAsignadoAccion = await IPermiso.ConsultaEsAsignadoPorSesion(PermisoAccion, Empresa.Codigo);

        await RefrescarCatalogoContratos();
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
         
        CodigoRegistro = CatalogoContratos.Where(x => x.ContratoId == id).Select(x => x.CodigoContrato).FirstOrDefault().Trim();
        EsVisibleAccionDialog = true;
    }

    private void CerrarDialog()
    {
        EstadoActualizar = new();
        EsAccionDesestimar = false;
        EsVisibleAccionDialog = false;
        ContratoCatalogo = new();
        EsVisibleModalCatalogo = false;
    }

    private void VerItemCatalogo(ContratoCatalogoActualizarEstadoDto item)
    {
        ContratoCatalogo = item;
        EsVisibleModalCatalogo = true;
    }

    private void VisibleModalChangedHandler(bool esVisible) => EsVisibleModalCatalogo = esVisible;

    private async Task CargarCatalogoContratos()
    {
        CatalogoContratos = await IContrato.CatalogoActualizarEstado(Empresa.Codigo, CodigoEstadoCatalogo);
        IsInitGrid = true; 
    }

    private void VisibleListaCuotaChangedHandler(bool esVisible) => EsVisibleListaCuotas = esVisible;

    private void VisibleListaTerminoChangedHandler(bool esVisible) => EsVisibleListaTerminos = esVisible;

    private void VerDescripcionTermino(string descripcion)
    {
        DescripcionTerminoDialog = descripcion;
        VisibleDescripcionTerminoDialogChangedHandler(true);
    }

    private void VisibleDescripcionTerminoDialogChangedHandler(bool esVisible) => EsVisibleDescripcionTermino = esVisible;

    public void Dispose() => GC.SuppressFinalize(this); 
}
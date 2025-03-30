using Blazored.FluentValidation;
using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Produccion;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components;
using GestionERP.Web.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Telerik.Blazor;
using Telerik.Blazor.Components; 
using AutoMapper; 
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Global; 

namespace GestionERP.Web.Pages.Empresa.Produccion.Solicitud;

public partial class Insert : IDisposable
{
    #region Propiedades
    private const string codigoModulo = "05";
    private const string codigoServicio = "S111";
    private const string rutaAccion = "/emitir";
    private string rutaEmpresa = "";
    private string pathBaseUri = "";
    private const string rutaServicio = "/solicitudes";
    public FluentValidationValidator validator;

    public SolicitudInsertarDto SolicitudInsertar { get; set; }
    public SolicitudObtenerDto Solicitud { get; set; }
    public SolicitudInsertarValidator Validator { get; set; }
    private EditContext EditContext { get; set; }
    public bool IsInitPage { get; set; }
    public string CodigoLocalNumerador { get; set; }
    public string CodigoProcesoDocumento { get; set; }  
    public List<DateTime> FechasCerradoOperacion { get; set; }
    public TipoProduccionConsultaPorCodigoDto TipoProduccion { get; set; }
    public EntidadConsultaPorEmpresaEsGestionadaDto EntidadGestionada { get; set; }
    public EmpresaEjercicioConsultaIntervaloFechaDto FechaIntervalo { get; set; }
	public string CodigoTipoDocumento { get; set; }
	public bool EsVisibleBotonDocumento { get; set; }
    private bool IsAuthUser { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool IsModified { get; set; }
    public TelerikNotification Alert { get; set; }
    private IEnumerable<SolicitudFlag> NivelesPrioridad { get; set; }
    private IEnumerable<SolicitudFlag> TiposRegistro { get; set; }
    [Parameter][SupplyParameterFromQuery(Name = "returnpage")] public string ReturnPage { get; set; }
    public EmpresaConsultaPorCodigoWebDto Empresa { get; set; }
    private bool EsVisibleVolver { get; set; }
    [Parameter] public string CodigoWebEmpresa { get; set; }
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    #endregion

    [Inject] public IProduccionSolicitud ISolicitud { get; set; }
    [Inject] public IPrincipalEmpresa IEmpresa { get; set; }
    [Inject] public IPrincipalTipoProduccion ITipoProduccion { get; set; }
    [Inject] public IPrincipalUsuario IUsuario { get; set; } 
    [Inject] public IPrincipalEntidad IEntidad { get; set; }
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

			SolicitudInsertar = new();
            Solicitud = new();
            TipoProduccion = new();
             
            FechasCerradoOperacion = [];
            FechaIntervalo = new();

            NivelesPrioridad = SolicitudFlag.NivelesPrioridad();
            TiposRegistro = SolicitudFlag.TiposRegistro();

			CodigoTipoDocumento = "SO";
			EsVisibleBotonDocumento = true;
             
            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio);
            if (!IsAuthUser) return;

            Empresa = await IEmpresa.ConsultaPorCodigoWeb(CodigoWebEmpresa);
            
            EsVisibleVolver = !string.IsNullOrEmpty(ReturnPage) && ReturnPage is "index";

            pathBaseUri = $"/{INavigation.ToBaseRelativePath(INavigation.Uri).Split("?")[0]}";
            rutaEmpresa = INavigation.Uri.Replace(INavigation.BaseUri, "").Split("?")[0].Replace(rutaServicio, "").Replace(rutaAccion, "");

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(SolicitudAcceso.Emitir, Empresa.Codigo))
            {
                INavigation.NavigateTo($"{rutaEmpresa}/inicio");
                Notify.Show("No tiene permiso para emitir registros de [Solicitudes de Produccion]", "error");
                return;
            }

            await CargarConsultaUsuarioEmpresa();
            EntidadGestionada = await IEntidad.ConsultaPorEmpresaEsGestionada(Empresa.Codigo);

            FechaIntervalo = await IEmpresa.ConsultaEjercicioIntervaloFecha(Empresa.Codigo);
            (await IEmpresa.ConsultaModuloPeriodoFechasEsCerradoOperacion(Empresa.Codigo, codigoModulo))?.ToList().ForEach(x => FechasCerradoOperacion.Add(x.Fecha));
            SolicitudInsertar.FechaEmision = !FechasCerradoOperacion.Any(x => x == DateTime.Now.Date) ? DateTime.Now.Date : null;
            if (SolicitudInsertar.FechaEmision.HasValue)
                Solicitud.FechaEmision = (DateTime)SolicitudInsertar.FechaEmision;

            Validator = new();
            EditContext = new EditContext(SolicitudInsertar); 
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

            Notify.ShowLoading(mensaje: "Emisión en progreso");

            Guid id = await ISolicitud.Insertar(Empresa.Codigo, SolicitudInsertar);

            IsModified = false;
            Notify.Show("La solicitud de producción ha sido emitida con éxito en la empresa", "success");
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

    private async Task CargarConsultaUsuarioEmpresa()
    {
        UsuarioConsultaPorEmpresaSesionDto usuarioEmpresa = await IUsuario.ConsultaPorEmpresaSesion(Empresa.Codigo);
        SolicitudInsertar.CodigoArea = usuarioEmpresa.CodigoArea;
        Solicitud.NombreArea = usuarioEmpresa.NombreArea;
        SolicitudInsertar.CodigoUsuarioSolicitante = usuarioEmpresa.CodigoUsuario;
        Solicitud.NombreUsuarioSolicitante = usuarioEmpresa.NombreUsuario;
    }

    private async Task Cerrar(LocationChangingContext context)
    {
        if (IsAuthUser && IsModified && pathBaseUri != context.TargetLocation && !await Dialog.ConfirmAsync("¿Está seguro de salir del formulario de emitir sin haber guardado?", "Saliendo del formulario"))
            context.PreventNavigation();
    }

    private void Volver() => INavigation.NavigateTo($"{rutaEmpresa}{rutaServicio}");

    #region Catalogos
    private void CargarItemCatalogoSerieDocumento(SerieDocumentoCatalogoPorEmpresaSesionDto item)
    {
        SolicitudInsertar.CodigoDocumento = item.CodigoDocumento;
        SolicitudInsertar.CodigoSerieDocumento = item.CodigoSerieDocumento;
        Solicitud.NombreSerieDocumento = item.NombreSerieDocumento;
        CodigoProcesoDocumento = item.CodigoProcesoDocumento;

        string flagTipoRegistro = TiposRegistro.Where(x => x.CodigoProcesoDocumento == CodigoProcesoDocumento).Select(x => x.Codigo).Single();
        if (SolicitudInsertar.FlagTipoRegistro != flagTipoRegistro)
        {
            SolicitudInsertar.FlagTipoRegistro = flagTipoRegistro;
            if (!string.IsNullOrEmpty(SolicitudInsertar.CodigoPlan))
            {
                Validator.MsgErrorPlan = null;
                TipoProduccion = new();
                SolicitudInsertar.CodigoPlan = Solicitud.CodigoTipoProduccion = Solicitud.NombreTipoProduccion = null;
                EditContext.NotifyFieldChanged(EditContext.Field("CodigoPlan"));
                EditContext.MarkAsUnmodified(EditContext.Field("CodigoPlan"));
                if (!string.IsNullOrEmpty(SolicitudInsertar.CodigoEntidad))
                {
                    Validator.MsgErrorEntidad = null;
                    SolicitudInsertar.CodigoEntidad = Solicitud.CodigoEntidad = Solicitud.NombreEntidad = null;
                    EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidad"));
                    EditContext.MarkAsUnmodified(EditContext.Field("CodigoEntidad"));
                }
            }
        } 
        if (!string.IsNullOrEmpty(item.CodigoLocal))
        {
            Validator.MsgErrorLocalRecepcion = null;
            SolicitudInsertar.CodigoLocalRecepcion = Solicitud.CodigoLocalRecepcion = CodigoLocalNumerador = item.CodigoLocal;
            Solicitud.NombreLocalRecepcion = item.NombreLocal;
            EditContext.NotifyFieldChanged(EditContext.Field("CodigoLocalRecepcion"));
        }

        EditContext.NotifyFieldChanged(EditContext.Field("CodigoDocumento"));
        IsModified = EditContext.IsModified();
    }

    private async Task CargarItemCatalogoPlan(PlanCatalogoDto item)
    {
        SolicitudInsertar.CodigoPlan = Solicitud.CodigoPlan = item.CodigoPlan;
        Solicitud.NombrePlan = item.NombrePlan;
        Solicitud.CodigoTipoProduccion = item.CodigoTipoProduccion;
        Solicitud.NombreTipoProduccion = item.NombreTipoProduccion;

        Validator.MsgErrorEntidad = null;
        if (TipoProduccion.FlagTipoEncargo != item.FlagTipoEncargo)
        {
            TipoProduccion = await ITipoProduccion.ConsultaPorCodigo(item.CodigoTipoProduccion);
            if (item.FlagTipoEncargo is "G")
            {
                SolicitudInsertar.CodigoEntidad = Solicitud.CodigoEntidad = EntidadGestionada.CodigoEntidad;
                Solicitud.NombreEntidad = EntidadGestionada.NombreEntidad;
                EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidad"));
            }
            else
            {
                SolicitudInsertar.CodigoEntidad = Solicitud.CodigoEntidad = Solicitud.NombreEntidad = null;
                EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidad"));
                EditContext.MarkAsUnmodified(EditContext.Field("CodigoEntidad"));
            }
        }

        Solicitud.CodigoArticuloTerminado = item.CodigoArticuloTerminado.Trim();
        Solicitud.NombreArticuloTerminado = item.NombreArticuloTerminado;
        Solicitud.UnidadConversionArticulo = Validator.UnidadConversionArticulo = item.UnidadConversionArticulo;
        Solicitud.CodigoUnidadMedida = item.CodigoUnidadMedida;
        Solicitud.NombreUnidadMedida = item.NombreUnidadMedida;
        Solicitud.PresentacionArticulo = item.PresentacionArticulo; 
        if (SolicitudInsertar.Cantidad.HasValue)
            EditContext.NotifyFieldChanged(EditContext.Field("Cantidad"));

        Validator.MsgErrorPlan = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoPlan"));
    }

    private void CargarItemCatalogoEntidad(EntidadCatalogoPorEmpresaDto item)
    {
        SolicitudInsertar.CodigoEntidad = Solicitud.CodigoEntidad = item.CodigoEntidad.Trim();
        Solicitud.NombreEntidad = item.NombreEntidad;
        Validator.MsgErrorEntidad = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidad"));
    }

    private void CargarItemCatalogoLocal(LocalCatalogoPorEmpresaDto item)
    {
        SolicitudInsertar.CodigoLocalRecepcion = Solicitud.CodigoLocalRecepcion = item.CodigoLocal;
        Solicitud.NombreLocalRecepcion = item.NombreLocal;
        Validator.MsgErrorLocalRecepcion = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoLocalRecepcion"));
        IsModified = EditContext.IsModified();
    }
    #endregion

    #region EditContextHandlers
    private void ValueChangePlanHandler(string value)
    {
        SolicitudInsertar.CodigoPlan = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorPlan) && (Solicitud.CodigoPlan ?? "") != (SolicitudInsertar.CodigoPlan ?? ""))
			Solicitud.CodigoPlan = Validator.MsgErrorPlan = null;
    }

    private async Task OnChangePlanHandler(object value)
    {  
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoPlan")))
		{ 
            if ((Solicitud.CodigoPlan ?? "") == (codigo ?? "")) goto exit;
			(PlanObtenerPorCodigoDto item, Validator.MsgErrorPlan) = await IUtil.ObtenerPlanProduccionPorCodigo(Alert, Empresa.Codigo, codigo, SolicitudInsertar.FlagTipoRegistro);
            if (item is not null)
            {
                Solicitud.CodigoPlan = item.CodigoPlan;
                Solicitud.NombrePlan = item.NombrePlan;
                Solicitud.CodigoTipoProduccion = item.CodigoTipoProduccion;
                Solicitud.NombreTipoProduccion = item.NombreTipoProduccion;
                
                Validator.MsgErrorEntidad = null;
                if (TipoProduccion.FlagTipoEncargo != item.FlagTipoEncargo)
                {
                    TipoProduccion = await ITipoProduccion.ConsultaPorCodigo(item.CodigoTipoProduccion);
                    if (item.FlagTipoEncargo is "G")
                    {
                        SolicitudInsertar.CodigoEntidad = Solicitud.CodigoEntidad = EntidadGestionada.CodigoEntidad;
                        Solicitud.NombreEntidad = EntidadGestionada.NombreEntidad;
                        EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidad"));
                    }
                    else
                    {
                        SolicitudInsertar.CodigoEntidad = Solicitud.CodigoEntidad = Solicitud.NombreEntidad = null;
                        EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidad"));
                        EditContext.MarkAsUnmodified(EditContext.Field("CodigoEntidad"));
                    }
                }

                Solicitud.CodigoArticuloTerminado = item.CodigoArticuloTerminado.Trim();
                Solicitud.NombreArticuloTerminado = item.NombreArticuloTerminado;
                Solicitud.UnidadConversionArticulo = Validator.UnidadConversionArticulo = item.UnidadConversionArticulo;
                Solicitud.CodigoUnidadMedida = item.CodigoUnidadMedida;
                Solicitud.NombreUnidadMedida = item.NombreUnidadMedida;
                Solicitud.PresentacionArticulo = item.PresentacionArticulo;
                if (SolicitudInsertar.Cantidad.HasValue)
                    EditContext.NotifyFieldChanged(EditContext.Field("Cantidad"));

                goto exit;
            } 
            EditContext.NotifyFieldChanged(EditContext.Field("CodigoPlan"));
		}
        Solicitud.CodigoPlan = codigo;
        TipoProduccion = new();
        Solicitud.NombrePlan = Solicitud.CodigoTipoProduccion = Solicitud.NombreTipoProduccion = SolicitudInsertar.CodigoEntidad = Solicitud.NombreEntidad = null;
        Solicitud.CodigoArticuloTerminado = Solicitud.NombreArticuloTerminado = Solicitud.CodigoUnidadMedida = Solicitud.NombreUnidadMedida = Solicitud.PresentacionArticulo = null;
        Solicitud.UnidadConversionArticulo = Validator.UnidadConversionArticulo = null;
        if (string.IsNullOrEmpty(codigo)) EditContext.MarkAsUnmodified(EditContext.Field("CodigoPlan"));
	exit:
        IsModified = EditContext.IsModified();
	}

    private void ValueChangeEntidadHandler(string value)
    {
        SolicitudInsertar.CodigoEntidad = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorEntidad) && (Solicitud.CodigoEntidad ?? "").Trim() != (SolicitudInsertar.CodigoEntidad ?? ""))
            Solicitud.CodigoEntidad = Validator.MsgErrorEntidad = null;
    }

    private async Task OnChangeEntidadHandler(object value)
    {
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoEntidad")))
		{ 
            if ((Solicitud.CodigoEntidad ?? "") == codigo) goto exit;
			(EntidadObtenerPorCodigoEmpresaDto item, Validator.MsgErrorEntidad)  = await IUtil.ObtenerEntidadPorCodigoEmpresa(Alert, codigo, Empresa.Codigo, TipoProduccion.FlagTipoEncargo);
            if (item is not null)
            {				
                Solicitud.CodigoEntidad = item.CodigoEntidad.Trim();
			    Solicitud.NombreEntidad = item.NombreEntidad; 
                goto exit;
            }  
            EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidad"));
		}
        Solicitud.CodigoEntidad = codigo;
        Solicitud.NombreEntidad = null;
        if (string.IsNullOrEmpty(codigo)) EditContext.MarkAsUnmodified(EditContext.Field("CodigoEntidad"));
	exit:
        IsModified = EditContext.IsModified();
    }

    private void ValueChangeLocalHandler(string value)
    {
        SolicitudInsertar.CodigoLocalRecepcion = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorLocalRecepcion) && (Solicitud.CodigoLocalRecepcion ?? "").Trim() != (SolicitudInsertar.CodigoLocalRecepcion ?? ""))
            Solicitud.CodigoLocalRecepcion = Validator.MsgErrorLocalRecepcion = null;
    }

    private async Task OnChangeLocalHandler(object value)
    {
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoLocalRecepcion")))
		{ 
			if (!string.IsNullOrEmpty(codigo))
			{ 
                if ((Solicitud.CodigoLocalRecepcion ?? "") == codigo) goto exit;
                (LocalObtenerPorCodigoEmpresaDto item, Validator.MsgErrorLocalRecepcion) = await IUtil.ObtenerLocalPorCodigoEmpresa(Alert, codigo, Empresa.Codigo);
                if (item is not null)
                {
                    Solicitud.CodigoLocalRecepcion = item.CodigoLocal;
                    Solicitud.NombreLocalRecepcion = item.NombreLocal; 
                    goto exit;
                } 
                EditContext.NotifyFieldChanged(EditContext.Field("CodigoLocalRecepcion")); 
			}
			else
            {
                SolicitudInsertar.CodigoLocalRecepcion = null;
				EditContext.MarkAsUnmodified(EditContext.Field("CodigoLocalRecepcion"));
            }
		}
        Solicitud.CodigoLocalRecepcion = codigo;
		Solicitud.NombreLocalRecepcion = null;
	exit:
		IsModified = EditContext.IsModified();
    }

    private void OnChangeObservacionHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "Observacion");

    private void OnChangeMotivoHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "Motivo");

    private void OnChangeCantidadHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(value is not null, EditContext, "Cantidad");

    private void OnChangeFlagNivelPrioridadHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "FlagNivelPrioridad");

    private void OnChangeFechaEntregaHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(value is not null, EditContext, "FechaEntrega");

    private void OnChangeFechaEmisionHandler(object value)
    {
        IsModified = Fnc.VerifyContextIsChanged((value is null && Solicitud.FechaEmision.HasValue) || (!Solicitud.FechaEmision.HasValue && value is not null) || (Solicitud.FechaEmision.HasValue && value is not null && Solicitud.FechaEmision != (DateTime?)value), EditContext, "FechaEmision");
        if (SolicitudInsertar.FechaEntrega.HasValue)
            EditContext.NotifyFieldChanged(EditContext.Field("FechaEntrega"));
    }
    #endregion

    public void Dispose() => GC.SuppressFinalize(this);
}
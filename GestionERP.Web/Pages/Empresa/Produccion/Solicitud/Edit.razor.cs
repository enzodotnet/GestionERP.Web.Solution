using AutoMapper;
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
using Telerik.SvgIcons;
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Global;

namespace GestionERP.Web.Pages.Empresa.Produccion.Solicitud;

public partial class Edit : IDisposable
{
    #region Propiedades
    private const string codigoModulo = "05";
    private const string codigoServicio = "S111";
    private const string rutaAccion = "/editar";
    private const string rutaServicio = "/solicitudes";
    private string rutaEmpresa = "";
    public FluentValidationValidator validator;

    public SolicitudObtenerDto Solicitud { get; set; }
    public SolicitudEditarDto SolicitudEditar { get; set; }
    private EditContext EditContext { get; set; }
    public SolicitudEditarValidator Validator { get; set; } 
    private SerieDocumentoConsultaPorCodigoEmpresaDto Numerador { get; set; }
    public TipoProduccionConsultaPorCodigoDto TipoProduccion { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool IsAuthUser { get; set; }
    private bool IsModified { get; set; } 
    private bool EsVisibleVolver { get; set; }
    private bool IsInitPage { get; set; }
    private bool IsChangedEntidad { get; set; }
	private bool IsChangedLocal { get; set; }
    private bool IsChangedPlan { get; set; }
    public TelerikNotification Alert { get; set; }
    public ISvgIcon IconoAccionModal { get; set; }
    public EntidadConsultaPorEmpresaEsGestionadaDto EntidadGestionada { get; set; }
    private IEnumerable<SolicitudFlag> NivelesPrioridad { get; set; }
    private IEnumerable<SolicitudFlag> TiposRegistro { get; set; }
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    [Parameter] public Guid? Id { get; set; }
    public EmpresaConsultaPorCodigoWebDto Empresa { get; set; }
    [Parameter] public string CodigoWebEmpresa { get; set; }
    [Parameter][SupplyParameterFromQuery(Name = "returnpage")] public string ReturnPage { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    #endregion

    [Inject] public IProduccionSolicitud ISolicitud { get; set; }
    [Inject] public IPrincipalEmpresa IEmpresa { get; set; }
    [Inject] public IPrincipalTipoProduccion ITipoProduccion { get; set; }
    [Inject] public IPrincipalSerieDocumento ISerieDocumento { get; set; } 
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
            Notify.ShowLoading(mensaje: "Obteniendo registro");

            SolicitudEditar = new();

            NivelesPrioridad = SolicitudFlag.NivelesPrioridad();
            TiposRegistro = SolicitudFlag.TiposRegistro();

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio);
            if (!IsAuthUser) return;

            Empresa = await IEmpresa.ConsultaPorCodigoWeb(CodigoWebEmpresa);
            EsVisibleVolver = !string.IsNullOrEmpty(ReturnPage) && ReturnPage is "index" or "view";
            rutaEmpresa = INavigation.Uri.Replace(INavigation.BaseUri, "").Split("?")[0].Replace(rutaServicio, "").Replace($"/{Id}", "").Replace(rutaAccion, "");

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(SolicitudAcceso.Editar, Empresa.Codigo))
            {
                INavigation.NavigateTo($"{rutaEmpresa}/inicio");
                Notify.Show("No tiene permiso para editar registros de [Solicitudes de Produccion]", "error");
                return;
            }

            Solicitud = await ISolicitud.Obtener(Empresa.Codigo, (Guid)Id);
            if (Solicitud is null || Solicitud.CodigoEstado is not "ED")
            {
                INavigation.NavigateTo($"{rutaEmpresa}{rutaServicio}");
                Notify.Show($"El registro de la [Solicitud de Produccion] consultado a editar no está disponible", "error");
                return;
            }

            SolicitudEditar = IMapper.Map<SolicitudEditarDto>(Solicitud);

            Validator = new()
            {
                FechaEmision = Solicitud.FechaEmision
            };
            EditContext = new EditContext(SolicitudEditar);

            Numerador = await ISerieDocumento.ConsultaPorCodigoEmpresa(Solicitud.CodigoSerieDocumento, Solicitud.CodigoDocumento, Empresa.Codigo) ?? new();
            TipoProduccion = await ITipoProduccion.ConsultaPorCodigo(Solicitud.CodigoTipoProduccion);
            EntidadGestionada = await IEntidad.ConsultaPorEmpresaEsGestionada(Empresa.Codigo);
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

            Notify.ShowLoading(mensaje: "Actualización en progreso");

            await ISolicitud.Editar(Empresa.Codigo, (Guid)Id, SolicitudEditar);

            IsModified = false;
            Notify.Show($"La solicitud de producción {Solicitud.Codigo} ha sido editada con éxito", "success");
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

    private void Volver() => INavigation.NavigateTo($"{rutaEmpresa}{rutaServicio}{(ReturnPage == "view" ? $"/{Id}" : "")}");

    private void NotifyChange(bool? isValueByVerify = null) => IsModified = isValueByVerify ?? EditContext.IsModified();

    #region Catalogos
    private void CargarItemCatalogoEntidad(EntidadCatalogoPorEmpresaDto item)
    {
        SolicitudEditar.CodigoEntidad = Solicitud.CodigoEntidad = item.CodigoEntidad.Trim();
        Solicitud.NombreEntidad = item.NombreEntidad;
        IsChangedEntidad = true;
        Validator.MsgErrorEntidad = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidad"));
		NotifyChange();
	}

    private void CargarItemCatalogoLocal(LocalCatalogoPorEmpresaDto item)
    {
        SolicitudEditar.CodigoLocalRecepcion = Solicitud.CodigoLocalRecepcion = item.CodigoLocal;
        Solicitud.NombreLocalRecepcion = item.NombreLocal;
        IsChangedLocal = true;
        Validator.MsgErrorLocalRecepcion = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoLocalRecepcion"));
		NotifyChange();
	}

    private async Task CargarItemCatalogoPlan(PlanCatalogoDto item)
    {
        SolicitudEditar.CodigoPlan = Solicitud.CodigoPlan = item.CodigoPlan;
        Solicitud.NombrePlan = item.NombrePlan;
        Solicitud.CodigoTipoProduccion = item.CodigoTipoProduccion;
        Solicitud.NombreTipoProduccion = item.NombreTipoProduccion;

        Validator.MsgErrorEntidad = null;
        if (TipoProduccion.FlagTipoEncargo != item.FlagTipoEncargo)
        {
            TipoProduccion = await ITipoProduccion.ConsultaPorCodigo(item.CodigoTipoProduccion);
            if (item.FlagTipoEncargo is "G")
            {
                SolicitudEditar.CodigoEntidad = Solicitud.CodigoEntidad = EntidadGestionada.CodigoEntidad;
                Solicitud.NombreEntidad = EntidadGestionada.NombreEntidad;
            }
            else
            {
                SolicitudEditar.CodigoEntidad = Solicitud.CodigoEntidad = Solicitud.NombreEntidad = null;
            }
            EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidad"));
        }

        Solicitud.CodigoArticuloTerminado = item.CodigoArticuloTerminado.Trim();
        Solicitud.NombreArticuloTerminado = item.NombreArticuloTerminado;
        Solicitud.UnidadConversionArticulo = Validator.UnidadConversionArticulo = item.UnidadConversionArticulo;
        Solicitud.CodigoUnidadMedida = item.CodigoUnidadMedida;
        Solicitud.NombreUnidadMedida = item.NombreUnidadMedida;
        Solicitud.PresentacionArticulo = item.PresentacionArticulo;
        if (SolicitudEditar.Cantidad.HasValue)
            EditContext.NotifyFieldChanged(EditContext.Field("Cantidad"));

        IsChangedPlan = true;
        Validator.MsgErrorPlan = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoPlan"));
    }
    #endregion

    #region EditContextHandlers
    private void ValueChangePlanHandler(string value)
    {
        SolicitudEditar.CodigoPlan = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorPlan) && (Solicitud.CodigoPlan ?? "") != (SolicitudEditar.CodigoPlan ?? ""))
            Solicitud.CodigoPlan = Validator.MsgErrorPlan = null;
    }

    private async Task OnChangePlanHandler(object value)
    {
        string codigo = (string)(value ?? "");
        if (EditContext.IsValid(EditContext.Field("CodigoPlan")))
        { 
            if ((Solicitud.CodigoPlan ?? "").Trim() != codigo)
            {
                IsChangedPlan = true;
                (PlanObtenerPorCodigoDto item, Validator.MsgErrorPlan) = await IUtil.ObtenerPlanProduccionPorCodigo(Alert, Empresa.Codigo, codigo, Solicitud.FlagTipoRegistro);
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
                            SolicitudEditar.CodigoEntidad = Solicitud.CodigoEntidad = EntidadGestionada.CodigoEntidad;
                            Solicitud.NombreEntidad = EntidadGestionada.NombreEntidad;
                        }
                        else
                        {
                            SolicitudEditar.CodigoEntidad = Solicitud.CodigoEntidad = Solicitud.NombreEntidad = null;
                        }
                        EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidad"));
                    }

                    Solicitud.CodigoArticuloTerminado = item.CodigoArticuloTerminado.Trim();
                    Solicitud.NombreArticuloTerminado = item.NombreArticuloTerminado;
                    Solicitud.UnidadConversionArticulo = Validator.UnidadConversionArticulo = item.UnidadConversionArticulo;
                    Solicitud.CodigoUnidadMedida = item.CodigoUnidadMedida;
                    Solicitud.NombreUnidadMedida = item.NombreUnidadMedida;
                    Solicitud.PresentacionArticulo = item.PresentacionArticulo;
                    if (SolicitudEditar.Cantidad.HasValue)
                        EditContext.NotifyFieldChanged(EditContext.Field("Cantidad"));

                    goto exit;
                }
                EditContext.NotifyFieldChanged(EditContext.Field("CodigoPlan"));
            }
            else
            {
                if (!IsChangedPlan) EditContext.MarkAsUnmodified(EditContext.Field("CodigoPlan"));
                goto exit;
            }
        }
        Solicitud.CodigoPlan = codigo;
        TipoProduccion = new();
        Solicitud.NombrePlan = Solicitud.CodigoTipoProduccion = Solicitud.NombreTipoProduccion = SolicitudEditar.CodigoEntidad = Solicitud.NombreEntidad = null;
        Solicitud.CodigoArticuloTerminado = Solicitud.NombreArticuloTerminado = Solicitud.CodigoUnidadMedida = Solicitud.NombreUnidadMedida = Solicitud.PresentacionArticulo = null;
        Solicitud.UnidadConversionArticulo = Validator.UnidadConversionArticulo = null; 
    exit:
        NotifyChange();
    }

    private void ValueChangeEntidadHandler(string value)
    {
        SolicitudEditar.CodigoEntidad = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorEntidad) && (Solicitud.CodigoEntidad ?? "").Trim() != (SolicitudEditar.CodigoEntidad ?? ""))
            Solicitud.CodigoEntidad = Validator.MsgErrorEntidad = null;
    }

    private async Task OnChangeEntidadHandler(object value)
    { 
        string codigo = (string)(value ?? "");
        if (EditContext.IsValid(EditContext.Field("CodigoEntidad")))
		{ 
			if ((Solicitud.CodigoEntidad ?? "").Trim() != codigo)
			{
                IsChangedEntidad = true;
				(EntidadObtenerPorCodigoEmpresaDto item, Validator.MsgErrorEntidad) = await IUtil.ObtenerEntidadPorCodigoEmpresa(Alert, codigo, Empresa.Codigo, TipoProduccion.FlagTipoEncargo);
                if (item is not null) 
                { 
                    Solicitud.CodigoEntidad = item.CodigoEntidad.Trim();
				    Solicitud.NombreEntidad = item.NombreEntidad;
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
        Solicitud.CodigoEntidad = codigo;
		Solicitud.NombreEntidad = null;
	exit:
        NotifyChange();
    }

    private void ValueChangeLocalHandler(string value)
    {
        SolicitudEditar.CodigoLocalRecepcion = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorLocalRecepcion) && (Solicitud.CodigoLocalRecepcion ?? "").Trim() != (SolicitudEditar.CodigoLocalRecepcion ?? ""))
            Solicitud.CodigoLocalRecepcion = Validator.MsgErrorLocalRecepcion = null;
    }

    private async Task OnChangeLocalHandler(object value)
    {
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoLocalRecepcion")))
		{ 
			SolicitudEditar.CodigoLocalRecepcion = string.IsNullOrEmpty(codigo) ? null : codigo; 
            if ((Solicitud.CodigoLocalRecepcion ?? "") != codigo)
			{
                IsChangedLocal = true;
                if (!string.IsNullOrEmpty(codigo))
                {
				    (LocalObtenerPorCodigoEmpresaDto item, Validator.MsgErrorLocalRecepcion) = await IUtil.ObtenerLocalPorCodigoEmpresa(Alert, codigo, Empresa.Codigo);
				    if (item is not null)
				    {
					    Solicitud.CodigoLocalRecepcion = item.CodigoLocal;
					    Solicitud.NombreLocalRecepcion = item.NombreLocal;
					    goto exit;
				    } 
                    EditContext.NotifyFieldChanged(EditContext.Field("CodigoLocalRecepcion")); 
                }
			}
			else
            {
                if (!IsChangedLocal) EditContext.MarkAsUnmodified(EditContext.Field("CodigoLocalRecepcion"));
				goto exit;
			}
		}
        Solicitud.CodigoLocalRecepcion = codigo;
		Solicitud.NombreLocalRecepcion = null;
	exit:
		NotifyChange();
    }

    private void OnChangeObservacionHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged((Solicitud.Observacion ?? "") != ((string)value ?? ""), EditContext, "Observacion"));

    private void OnChangeMotivoHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged((Solicitud.Motivo ?? "") != ((string)value ?? ""), EditContext, "Motivo"));

    private void OnChangeCantidadHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Solicitud.Cantidad != (decimal?)value, EditContext, "Cantidad"));

    private void OnChangeFlagNivelPrioridadHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Solicitud.FlagNivelPrioridad != (string)value, EditContext, "FlagNivelPrioridad"));

    private void OnChangeFechaEntregaHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Solicitud.FechaEntrega != (DateTime?)value, EditContext, "FechaEntrega"));
    #endregion 

    public void Dispose() => GC.SuppressFinalize(this);
}
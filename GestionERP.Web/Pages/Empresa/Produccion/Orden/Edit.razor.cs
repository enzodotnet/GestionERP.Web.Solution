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

namespace GestionERP.Web.Pages.Empresa.Produccion.Orden;

public partial class Edit : IDisposable
{
    #region Propiedades
    private const string codigoModulo = "05";
    private const string codigoServicio = "S112";
    private const string rutaAccion = "/editar";
    private const string rutaServicio = "/ordenes";
    private string rutaEmpresa = "";
    public FluentValidationValidator validator;

    public OrdenObtenerDto Orden { get; set; }
    public OrdenEditarDto OrdenEditar { get; set; }
    private EditContext EditContext { get; set; }
    public OrdenEditarValidator Validator { get; set; } 
    private SerieDocumentoConsultaPorCodigoEmpresaDto Numerador { get; set; }
    public TipoProduccionConsultaPorCodigoDto TipoProduccion { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool IsAuthUser { get; set; }
    private bool IsModified { get; set; } 
    private bool EsVisibleVolver { get; set; }
    private bool IsInitPage { get; set; }
    private bool IsChangedEntidad { get; set; }
	private bool IsChangedLocal { get; set; }
    private bool IsChangedCantidad { get; set; }
    private bool IsChangedVersionPlan { get; set; }
    private decimal CantidadPlan { get; set; } 
    public bool EsVisibleListaMateriales { get; set; } 
    private bool IsValidCantidad { get; set; }
    public TelerikNotification Alert { get; set; }
    public ISvgIcon IconoAccionModal { get; set; }
    private IEnumerable<OrdenFlag> Origenes { get; set; }
    private IEnumerable<OrdenFlag> TiposRegistro { get; set; }
    private IEnumerable<VersionPlanFlag> TiposMaterial { get; set; }
    private IEnumerable<VersionPlanFlag> InsumosMaterial { get; set; } 
    public IEnumerable<VersionPlanMaterialConsultaDto> Materiales { get; set; }
    private MonedaConsultaPorTipoDto MN { get; set; }
    private MonedaConsultaPorTipoDto ME { get; set; }
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    [Parameter] public Guid? Id { get; set; }
    public EmpresaConsultaPorCodigoWebDto Empresa { get; set; }
    [Parameter] public string CodigoWebEmpresa { get; set; }
    [Parameter][SupplyParameterFromQuery(Name = "returnpage")] public string ReturnPage { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    #endregion

    [Inject] public IProduccionOrden IOrden { get; set; }
    [Inject] public IProduccionVersionPlan IVersionPlan { get; set; }
    [Inject] public IProduccionPlan IPlan { get; set; }
    [Inject] public IPrincipalEmpresa IEmpresa { get; set; }
    [Inject] public IPrincipalMoneda IMoneda { get; set; }
    [Inject] public IPrincipalTipoProduccion ITipoProduccion { get; set; }
    [Inject] public IPrincipalSerieDocumento ISerieDocumento { get; set; } 
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

            OrdenEditar = new();

            Origenes = OrdenFlag.Origenes();
            TiposRegistro = OrdenFlag.TiposRegistro();
            TiposMaterial = VersionPlanFlag.TiposMaterial();
            InsumosMaterial = VersionPlanFlag.InsumosMaterial();

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio);
            if (!IsAuthUser) return;

            Empresa = await IEmpresa.ConsultaPorCodigoWeb(CodigoWebEmpresa);
            EsVisibleVolver = !string.IsNullOrEmpty(ReturnPage) && ReturnPage is "index" or "view";
            rutaEmpresa = INavigation.Uri.Replace(INavigation.BaseUri, "").Split("?")[0].Replace(rutaServicio, "").Replace($"/{Id}", "").Replace(rutaAccion, "");

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(OrdenAcceso.Editar, Empresa.Codigo))
            {
                INavigation.NavigateTo($"{rutaEmpresa}/inicio");
                Notify.Show("No tiene permiso para editar registros de [Ordenes de Produccion]", "error");
                return;
            }

            Orden = await IOrden.Obtener(Empresa.Codigo, (Guid)Id);
            if (Orden is null || Orden.CodigoEstado is not "ED")
            {
                INavigation.NavigateTo($"{rutaEmpresa}{rutaServicio}");
                Notify.Show($"El registro de la [Orden de Produccion] consultado a editar no está disponible", "error");
                return;
            }

            OrdenEditar = IMapper.Map<OrdenEditarDto>(Orden);
            Validator = new()
            {
                FechaEmision = Orden.FechaEmision
            };
            EditContext = new EditContext(OrdenEditar);
           
            ME = await IMoneda.ConsultaPorTipo("ME");
            MN = await IMoneda.ConsultaPorTipo("MN");
            Numerador = await ISerieDocumento.ConsultaPorCodigoEmpresa(Orden.CodigoSerieDocumento, Orden.CodigoDocumento, Empresa.Codigo) ?? new();
            TipoProduccion = await ITipoProduccion.ConsultaPorCodigo(Orden.CodigoTipoProduccion);
            CantidadPlan = (await IPlan.ConsultaPorCodigo(Empresa.Codigo, Orden.CodigoPlan)).Cantidad;

            IsValidCantidad = true;
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

            await IOrden.Editar(Empresa.Codigo, (Guid)Id, OrdenEditar);

            IsModified = false;
            Notify.Show($"La orden de producción {Orden.Codigo} ha sido editada con éxito", "success");
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

    #region Materiales
    private void VisibleListaMaterialChangedHandler(bool esVisible)
    {
        if (!IsValidCantidad)
            Fnc.MostrarAlerta(Alert, "Es necesario que ingrese una cantidad válida para mostrar los materiales a emplear", "error");
        else
            EsVisibleListaMateriales = esVisible;
    }

    private void CalcularCantidadMateriales()
    {
        if (Orden.Materiales.Count > 0 && OrdenEditar.Cantidad.HasValue && IsValidCantidad)
        {
            decimal multiploIndicador = (decimal)OrdenEditar.Cantidad / CantidadPlan;
            decimal cantidadMaterial;
            foreach (OrdenMaterialObtenerDto itemMaterialOrden in Orden.Materiales)
            {
                cantidadMaterial = Materiales.Where(x => x.CodigoArticulo == itemMaterialOrden.CodigoArticulo).Select(x => x.Cantidad).Single() * (itemMaterialOrden.FlagInsumo is "F" && multiploIndicador < 1 ? 1 : multiploIndicador);
                itemMaterialOrden.Cantidad = itemMaterialOrden.FlagTipo is "A" ? Math.Round(cantidadMaterial, 0) : Math.Round(cantidadMaterial, 3, MidpointRounding.AwayFromZero);
            }
        }
    }
    #endregion

    #region Catalogos
    private void CargarItemCatalogoEntidad(EntidadCatalogoPorEmpresaDto item)
    {
        OrdenEditar.CodigoEntidad = Orden.CodigoEntidad = item.CodigoEntidad.Trim();
        Orden.NombreEntidad = item.NombreEntidad;
        IsChangedEntidad = true;
        Validator.MsgErrorEntidad = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidad"));
		NotifyChange();
	}

    private void CargarItemCatalogoLocal(LocalCatalogoPorEmpresaDto item)
    {
        OrdenEditar.CodigoLocalRecepcion = Orden.CodigoLocalRecepcion = item.CodigoLocal;
        Orden.NombreLocalRecepcion = item.NombreLocal;
        IsChangedLocal = true;
        Validator.MsgErrorLocalRecepcion = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoLocalRecepcion"));
		NotifyChange();
	}

    private async Task CargarItemCatalogoVersionPan(VersionPlanCatalogoDto item)
    {
        OrdenEditar.CodigoVersionPlan = Orden.CodigoVersionPlan = item.CodigoVersionPlan.Trim();
        Orden.NombreVersionPlan = item.NombreVersionPlan;
        IsChangedVersionPlan = true;
        Validator.MsgErrorVersionPlan = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoVersionPlan"));
        Materiales = await IVersionPlan.ConsultaMateriales(Empresa.Codigo, OrdenEditar.CodigoVersionPlan) ?? [];
        IMapper.Map(Materiales, Orden.Materiales);
        CalcularCantidadMateriales();
    }
    #endregion

    #region EditContextHandlers
    private void ValueChangeVersionPlanHandler(string value)
    {
        OrdenEditar.CodigoVersionPlan = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorVersionPlan) && (Orden.CodigoVersionPlan ?? "") != (OrdenEditar.CodigoVersionPlan ?? ""))
            Orden.CodigoVersionPlan = Validator.MsgErrorVersionPlan = null;
    }

    private async Task OnChangeVersionPlanHandler(object value)
    {
        string codigo = (string)(value ?? "");
        bool esCalculable = false;
        if (EditContext.IsValid(EditContext.Field("CodigoVersionPlan")))
        { 
            if ((Orden.CodigoVersionPlan ?? "").Trim() != codigo)
            {
                IsChangedVersionPlan = true;
                (VersionPlanObtenerPorCodigoDto item, Validator.MsgErrorVersionPlan) = await IUtil.ObtenerVersionPlanProduccionPorCodigo(Alert, Empresa.Codigo, codigo, Orden.CodigoPlan);
                if (item is not null)
                {
                    Orden.CodigoVersionPlan = item.CodigoVersionPlan;
                    Orden.NombreVersionPlan = item.NombreVersionPlan;
                    esCalculable = true;
                    goto exit;
                }
                EditContext.NotifyFieldChanged(EditContext.Field("CodigoVersionPlan"));
            }
            else
            {
                if (!IsChangedVersionPlan) EditContext.MarkAsUnmodified(EditContext.Field("CodigoVersionPlan"));
                goto exit;
            }
        }
        Orden.CodigoVersionPlan = codigo;
        Orden.NombreVersionPlan = null;
    exit:
        Materiales = await IVersionPlan.ConsultaMateriales(Empresa.Codigo, Orden.CodigoVersionPlan) ?? [];
        IMapper.Map(Materiales, Orden.Materiales);
        if (esCalculable)
            CalcularCantidadMateriales();
        NotifyChange();
    }

    private void ValueChangeEntidadHandler(string value)
    {
        OrdenEditar.CodigoEntidad = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorEntidad) && (Orden.CodigoEntidad ?? "").Trim() != (OrdenEditar.CodigoEntidad ?? ""))
            Orden.CodigoEntidad = Validator.MsgErrorEntidad = null;
    }

    private async Task OnChangeEntidadHandler(object value)
    { 
        string codigo = (string)(value ?? "");
        if (EditContext.IsValid(EditContext.Field("CodigoEntidad")))
		{ 
			if ((Orden.CodigoEntidad ?? "").Trim() != codigo)
			{
                IsChangedEntidad = true;
				(EntidadObtenerPorCodigoEmpresaDto item, Validator.MsgErrorEntidad) = await IUtil.ObtenerEntidadPorCodigoEmpresa(Alert, codigo, Empresa.Codigo, TipoProduccion.FlagTipoEncargo);
                if (item is not null) 
                { 
                    Orden.CodigoEntidad = item.CodigoEntidad.Trim();
				    Orden.NombreEntidad = item.NombreEntidad;
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
        Orden.CodigoEntidad = codigo;
		Orden.NombreEntidad = null;
	exit:
        NotifyChange();
    }

    private void ValueChangeLocalHandler(string value)
    {
        OrdenEditar.CodigoLocalRecepcion = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorLocalRecepcion) && (Orden.CodigoLocalRecepcion ?? "").Trim() != (OrdenEditar.CodigoLocalRecepcion ?? ""))
            Orden.CodigoLocalRecepcion = Validator.MsgErrorLocalRecepcion = null;
    }

    private async Task OnChangeLocalHandler(object value)
    {
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoLocalRecepcion")))
		{ 
			OrdenEditar.CodigoLocalRecepcion = string.IsNullOrEmpty(codigo) ? null : codigo; 
            if ((Orden.CodigoLocalRecepcion ?? "") != codigo)
			{
                IsChangedLocal = true;
                if (!string.IsNullOrEmpty(codigo))
                {
				    (LocalObtenerPorCodigoEmpresaDto item, Validator.MsgErrorLocalRecepcion) = await IUtil.ObtenerLocalPorCodigoEmpresa(Alert, codigo, Empresa.Codigo);
				    if (item is not null)
				    {
					    Orden.CodigoLocalRecepcion = item.CodigoLocal;
					    Orden.NombreLocalRecepcion = item.NombreLocal;
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
        Orden.CodigoLocalRecepcion = codigo;
		Orden.NombreLocalRecepcion = null;
	exit:
		NotifyChange();
    }

    private void OnChangeObservacionHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged((Orden.Observacion ?? "") != ((string)value ?? ""), EditContext, "Observacion"));

    private void OnChangeMotivoHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged((Orden.Motivo ?? "") != ((string)value ?? ""), EditContext, "Motivo"));

    private void OnChangeCantidadHandler(object value)
    {
        decimal? cantidad = (decimal?)value;
        IsValidCantidad = EditContext.IsValid(EditContext.Field("Cantidad"));
        if (IsValidCantidad)
        {
            if ((Orden.Cantidad ?? 0) != cantidad)
            {
                IsChangedCantidad = true;
                Orden.Cantidad = cantidad;
                CalcularCantidadMateriales();
                EditContext.NotifyFieldChanged(EditContext.Field("Cantidad"));
                goto exit;
            }
            else
            {
                if (!IsChangedCantidad) EditContext.MarkAsUnmodified(EditContext.Field("Cantidad"));
                goto exit;
            } 
        }
        Orden.Cantidad = cantidad;
    exit:
        NotifyChange();
    }

    private void OnChangeFechaInicioHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Orden.FechaInicio != (DateTime?)value, EditContext, "FechaInicio"));

    private void OnChangeFechaTerminoHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Orden.FechaTermino != (DateTime?)value, EditContext, "FechaTermino"));
    #endregion 

    public void Dispose() => GC.SuppressFinalize(this);
}
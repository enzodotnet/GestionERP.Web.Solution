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
using AutoMapper;
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Global; 

namespace GestionERP.Web.Pages.Empresa.Servicio.Solicitud;

public partial class Insert : IDisposable
{
    #region Propiedades
    private const string codigoModulo = "03";
    private const string codigoServicio = "S108";
    private const string rutaAccion = "/emitir";
    private string rutaEmpresa = "";
    private string pathBaseUri = "";
    private const string rutaServicio = "/solicitudes";
    public FluentValidationValidator validator;
     
	public SolicitudInsertarDto SolicitudInsertar { get; set; }
    public SolicitudObtenerDto Solicitud { get; set; }
    public SolicitudDetalleInsertarDto DetalleInsertar { get; set; }
    public SolicitudDetalleObtenerDto Detalle { get; set; }
    public List<SolicitudDetalleGrid> GridDetalles { get; set; }
    public SolicitudDetalleGrid ItemGridDetalle { get; set; }
    private EditContext EditContext { get; set; }
    private EditContext DetalleContext { get; set; }
    public SolicitudInsertarValidator Validator { get; set; }
    public SolicitudDetalleInsertarValidator DetalleValidator { get; set; }
	public bool IsInitPage { get; set; }
	public bool IsEditingGridDetalle { get; set; }
    public string CodigoLocalNumerador { get; set; }
    public string CodigoProcesoDocumento { get; set; }
    public List<DateTime> FechasCerradoOperacion { get; set; }
    public EmpresaEjercicioConsultaIntervaloFechaDto FechaIntervalo { get; set; }
    public bool EsVisibleBotonDocumento { get; set; }
	public string CodigoTipoDocumento { get; set; }
    private bool IsAuthUser { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool IsModified { get; set; }
    private bool IsModifiedDetalle { get; set; }
    public string VerboAccionModal { get; set; }
    public string DescripcionAccionModal { get; set; }
    public string TipoAccionModal { get; set; }
    public string TipoAccionDialog { get; set; }
    public string OrigenDialog { get; set; }
    public string OrigenModal { get; set; }
    public bool EsVisibleDialogDetalle { get; set; }
    public bool EsVisibleModalDetalle { get; set; }
    public string CodigoItemAccion { get; set; } 
    public ISvgIcon IconoAccionModal { get; set; }
    public TelerikNotification AlertDetalle { get; set; }
    public TelerikNotification Alert { get; set; }
    private TelerikGrid<SolicitudDetalleGrid> GridDetalleRef { get; set; }
    private IEnumerable<SolicitudFlag> NivelesPrioridad { get; set; }
    [Parameter][SupplyParameterFromQuery(Name = "returnpage")] public string ReturnPage { get; set; }
    public EmpresaConsultaPorCodigoWebDto Empresa { get; set; }
    private bool EsVisibleVolver { get; set; }
    [Parameter] public string CodigoWebEmpresa { get; set; }
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    #endregion

    [Inject] public IServicioSolicitud ISolicitud { get; set; }
    [Inject] public IPrincipalEmpresa IEmpresa { get; set; }
    [Inject] public IPrincipalUsuario IUsuario { get; set; } 
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
            GridDetalles = [];
            ItemGridDetalle = new();
            DetalleInsertar = new();
            Detalle = new();

            FechasCerradoOperacion = [];
            FechaIntervalo = new();

            NivelesPrioridad = SolicitudFlag.NivelesPrioridad();
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
                Notify.Show("No tiene permiso para emitir registros de [Solicitudes de Servicio]", "error");
                return;
            }

            Solicitud.EsRequeridoRevision = await IPermiso.ConsultaEsAsignadoPorSesion(SolicitudAcceso.RequerirRevision, Empresa.Codigo);
            await CargarConsultaUsuarioEmpresa();

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
            if (!VerificarRegistroEsValido()) return;

            Notify.ShowLoading(mensaje: "Emisión en progreso");

            Guid id = await ISolicitud.Insertar(Empresa.Codigo, SolicitudInsertar);

            IsModified = false;
            Notify.Show("La solicitud de compra ha sido emitida con éxito en la empresa", "success");
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

    private bool VerificarRegistroEsValido()
    {
        bool esValido = true; 

        if (string.IsNullOrEmpty(SolicitudInsertar.CodigoEntidad) && string.IsNullOrEmpty(SolicitudInsertar.ReferenciaProveedor))
        {  
            Fnc.MostrarAlerta(Alert, "Es necesario que seleccione algún proveedor o especifique alguna referencia del mismo", "error");
            esValido = false;
        }

        if (GridDetalles.Count == 0)
        {
            Fnc.MostrarAlerta(Alert, "Es obligatorio que agregue ítem(s) al detalle de la solicitud de compra", "error");
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
                if (GridDetalles.Any(x => !x.Cantidad.HasValue))
                {
                    Fnc.MostrarAlerta(Alert, "Está pendiente completar alguna celda de [cantidad] del detalle de la solicitud de servicio", "error");
                    esValido = false;
                } 
            }
        }

        return esValido;
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
            case "M":
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
                    DetalleContext = new EditContext(DetalleInsertar); 
                EsVisibleModalDetalle = true;
                break;
        }
    }

    private void IniciarAccionDialog(string tipoAccion, string origenDialog)
    {
        TipoAccionDialog = tipoAccion;
        OrigenDialog = origenDialog;
        if(origenDialog is "detalle")
            EsVisibleDialogDetalle = true; 
    }

    public void CerrarModal()
    {
        if (OrigenModal is "detalle")
            IsModifiedDetalle = EsVisibleModalDetalle = false;
        VerboAccionModal = DescripcionAccionModal = TipoAccionModal = CodigoItemAccion = ""; 
        StateHasChanged();
    }

    private void Volver() => INavigation.NavigateTo($"{rutaEmpresa}{rutaServicio}");

    public void CerrarDialog()
    {
        if (OrigenDialog is "detalle")
            EsVisibleDialogDetalle = false;
        TipoAccionDialog = OrigenDialog = "";
        StateHasChanged();
    }

    #region Detalle
    public void InvalidarAccionDetalle(EditContext editContext) => Fnc.MostrarAlerta(AlertDetalle, Cnf.MsgErrorInvalidEditContext, "error");

    private async Task CerrarDetalle()
    {
        if (TipoAccionModal is not "V" && IsModifiedDetalle && !await Dialog.ConfirmAsync("¿Está seguro de cerrar el formulario y que los datos no se carguen?", "Saliendo del formulario"))
            return;
        CerrarModal();
    }

    public void MostrarAgregarDetalle()
    {
        if (string.IsNullOrEmpty(SolicitudInsertar.CodigoDocumento))
        {
            Fnc.MostrarAlerta(Alert, "Es necesario que seleccione el numerador de [Documento]", "error");
            return;
        }
        DetalleInsertar = new();
        Detalle = new();
        DetalleValidator = new();
        IniciarAccionModal("I", "detalle");
    }

    public void MostrarModificarDetalle(SolicitudDetalleGrid item = null)
    {
        if (TipoAccionModal is not "V")
            Detalle = IMapper.Map<SolicitudDetalleObtenerDto>(item);

        DetalleInsertar = IMapper.Map<SolicitudDetalleInsertarDto>(Detalle);
        CodigoItemAccion = Detalle.CodigoArticulo;
        DetalleValidator = new();
        IniciarAccionModal("M", "detalle");
    }

    private void MostrarQuitarDetalle(string codigoItem)
    {
        CodigoItemAccion = codigoItem;
        IniciarAccionDialog("Q", "detalle");
    }

    public void VerItemDetalle(SolicitudDetalleGrid item)
    {
        Detalle = IMapper.Map<SolicitudDetalleObtenerDto>(item);
        CodigoItemAccion = item.CodigoArticulo;
        IniciarAccionModal("V", "detalle");
    }

    public static void OnCellCantidadRenderHandler(GridCellRenderEventArgs args) => args.Class = !(args.Item as SolicitudDetalleGrid).Cantidad.HasValue ? "cell-error" : "cell-editable";

	public void EditDetalleHandler(GridCommandEventArgs args) => IsEditingGridDetalle = args.Field is "Cantidad";

	public void CancelDetalleHandler(GridCommandEventArgs args) => IsEditingGridDetalle = args.Field is not "Cantidad";

	public void UpdateItemDetalleHandler(GridCommandEventArgs args)
    {
        SolicitudDetalleGrid item = (SolicitudDetalleGrid)args.Item; 
        int index = GridDetalles.FindIndex(i => i.CodigoArticulo == item.CodigoArticulo);
        if (args.Field == "Cantidad") 
            SolicitudInsertar.Detalles[index].Cantidad = GridDetalles[index].Cantidad = item.Cantidad;
		IsEditingGridDetalle = false;
	}

    private void OnChangeDetalleCantidadHandler(object value) => IsModifiedDetalle = Fnc.VerifyContextIsChanged(Detalle.Cantidad != (int?)value, DetalleContext, "Cantidad");

    private void OnChangeDetalleObservacionHandler(object value) => IsModifiedDetalle = Fnc.VerifyContextIsChanged((Detalle.Observacion ?? "") != ((string)value ?? ""), DetalleContext, "Observacion");

    private void ValueChangeArticuloHandler(string value)
    {
        DetalleInsertar.CodigoArticulo = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(DetalleValidator.MsgErrorArticulo) && (Detalle.CodigoArticulo ?? "").Trim() != (DetalleInsertar.CodigoArticulo ?? ""))
	        Detalle.CodigoArticulo = DetalleValidator.MsgErrorArticulo = null;
    }

    private async Task OnChangeArticuloHandler(object value)
    {
		string codigo = (string)(value ?? "");
		if (DetalleContext.IsValid(DetalleContext.Field("CodigoArticulo")))
		{
			if ((Detalle.CodigoArticulo ?? "") == codigo) goto exit;
            if (GridDetalles.Any(x => x.CodigoArticulo.Trim() == codigo))
            {
                DetalleValidator.MsgErrorArticulo = "El código ya existe en el detalle del registro";
            }
            else
            {  
                (ArticuloObtenerPorCodigoEmpresaProcesoDocumentoDto item, DetalleValidator.MsgErrorArticulo) = await IUtil.ObtenerArticuloPorCodigoEmpresaProcesoDocumento(AlertDetalle, codigo, Empresa.Codigo, CodigoProcesoDocumento);
			    if (item is not null)
			    {
			        Detalle.CodigoArticulo = item.CodigoArticulo.Trim();
			        Detalle.NombreArticulo = item.NombreArticulo;
			        Detalle.CodigoUnidadMedida = item.CodigoUnidadMedida;
			        Detalle.NombreUnidadMedida = item.NombreUnidadMedida;
				    goto exit;
			    }
            } 
            DetalleContext.NotifyFieldChanged(DetalleContext.Field("CodigoArticulo"));
		}
        Detalle.CodigoArticulo = codigo;
		Detalle.NombreArticulo = Detalle.CodigoUnidadMedida = Detalle.NombreUnidadMedida = null;
		if (string.IsNullOrEmpty(codigo)) 
            DetalleContext.MarkAsUnmodified(DetalleContext.Field("CodigoArticulo"));
	exit:
		IsModifiedDetalle = DetalleContext.IsModified();
    } 

    private async Task AccionarDetalle(string tipoAccion)
    { 
        GridState<SolicitudDetalleGrid> detalleState = GridDetalleRef.GetState();
        int index = GridDetalles.FindIndex(i => i.CodigoArticulo == CodigoItemAccion);

        switch (tipoAccion)
        {
            case "I" or "M":
                ItemGridDetalle = IMapper.Map<SolicitudDetalleGrid>(IMapper.Map(DetalleInsertar, Detalle));
                if (tipoAccion == "I")
                {
                    SolicitudInsertar.Detalles.Add(DetalleInsertar);
                    GridDetalles.Add(ItemGridDetalle);
                    detalleState.InsertedItem = ItemGridDetalle;
                }
                else
                {
                    SolicitudInsertar.Detalles[index] = DetalleInsertar;
                    GridDetalles[index] = ItemGridDetalle;
                }
                break;
            case "Q":
                SolicitudInsertar.Detalles.RemoveAt(index);
                GridDetalles.RemoveAt(index);
                CerrarDialog();
                break;
        }; 
        CerrarModal();
        EsVisibleBotonDocumento = GridDetalles.Count == 0;
        await GridDetalleRef.SetStateAsync(detalleState);
    }
    #endregion

    #region Catalogos
    private void CargarItemCatalogoSerieDocumento(SerieDocumentoCatalogoPorEmpresaSesionDto item)
    { 
        SolicitudInsertar.CodigoDocumento = item.CodigoDocumento;
        SolicitudInsertar.CodigoSerieDocumento = item.CodigoSerieDocumento;
        Solicitud.NombreSerieDocumento = item.NombreSerieDocumento; 
        SolicitudInsertar.CodigoLocalAtencion = CodigoLocalNumerador = item.CodigoLocal;
        Solicitud.NombreLocalAtencion = item.NombreLocal;
        CodigoProcesoDocumento = item.CodigoProcesoDocumento;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoDocumento"));
        IsModified = EditContext.IsModified(); 
    } 
     
    private void CargarItemCatalogoProveedor(EntidadProveedorCatalogoPorEmpresaDto item)
    {
        SolicitudInsertar.CodigoEntidad = Solicitud.CodigoEntidad = item.CodigoEntidad.Trim();
        Solicitud.NombreEntidad = item.NombreEntidad;
        Validator.MsgErrorEntidad = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidad"));
        IsModified = EditContext.IsModified(); 
    }

    private void CargarItemCatalogoLocal(LocalCatalogoPorEmpresaDto item)
    {
        SolicitudInsertar.CodigoLocalAtencion = Solicitud.CodigoLocalAtencion = item.CodigoLocal;
        Solicitud.NombreLocalAtencion = item.NombreLocal;
        Validator.MsgErrorLocalAtencion = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoLocalAtencion"));
        IsModified = EditContext.IsModified(); 
    }
         
    private void CargarItemCatalogoArticuloPorProcesoDocumento(ArticuloCatalogoPorEmpresaProcesoDocumentoDto item)
    {
        DetalleInsertar.CodigoArticulo = Detalle.CodigoArticulo = item.CodigoArticulo.Trim();
        Detalle.NombreArticulo = item.NombreArticulo;  
        Detalle.CodigoUnidadMedida = item.CodigoUnidadMedida;
        Detalle.NombreUnidadMedida = item.NombreUnidadMedida;
        DetalleValidator.MsgErrorArticulo = null;
        DetalleContext.NotifyFieldChanged(DetalleContext.Field("CodigoArticulo"));
        IsModifiedDetalle = DetalleContext.IsModified();  
    }
    #endregion

    #region EditContextHandlers

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
			if (!string.IsNullOrEmpty(codigo))
			{
				if ((Solicitud.CodigoEntidad ?? "") == codigo) goto exit;
				(EntidadProveedorObtenerPorCodigoEmpresaDto item, Validator.MsgErrorEntidad) = await IUtil.ObtenerProveedorPorCodigoEmpresa(Alert, codigo, Empresa.Codigo, "L");
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
				SolicitudInsertar.CodigoEntidad = null;
				EditContext.MarkAsUnmodified(EditContext.Field("CodigoEntidad"));
			}
		}
        Solicitud.CodigoEntidad = codigo;
        Solicitud.NombreEntidad = null; 
	exit:
        IsModified = EditContext.IsModified();
    }

    private void ValueChangeLocalHandler(string value)
    {
        SolicitudInsertar.CodigoLocalAtencion = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorLocalAtencion) && (Solicitud.CodigoLocalAtencion ?? "").Trim() != (SolicitudInsertar.CodigoLocalAtencion ?? ""))
            Solicitud.CodigoLocalAtencion = Validator.MsgErrorLocalAtencion = null;
    }

    private async Task OnChangeLocalHandler(object value)
    {
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoLocalAtencion")))
		{ 
			if (!string.IsNullOrEmpty(codigo))
			{
                if ((Solicitud.CodigoLocalAtencion ?? "") == codigo) goto exit;
                (LocalObtenerPorCodigoEmpresaDto item, Validator.MsgErrorLocalAtencion) = await IUtil.ObtenerLocalPorCodigoEmpresa(Alert, codigo, Empresa.Codigo);
                if (item is not null)
                {
                    Solicitud.CodigoLocalAtencion = item.CodigoLocal;
                    Solicitud.NombreLocalAtencion = item.NombreLocal; 
                    goto exit;
                } 
                EditContext.NotifyFieldChanged(EditContext.Field("CodigoLocalAtencion"));
			}
			else
            {
                SolicitudInsertar.CodigoLocalAtencion = null;
				EditContext.MarkAsUnmodified(EditContext.Field("CodigoLocalAtencion"));
            }
		}
        Solicitud.CodigoLocalAtencion = codigo;
		Solicitud.NombreLocalAtencion = null;
	exit:
		IsModified = EditContext.IsModified();
    }

    private void OnChangeObservacionHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "Observacion");

    private void OnChangeReferenciaProveedorHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "ReferenciaProveedor");

    private void OnChangeMotivoHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "Motivo");

    private void OnChangeFlagNivelPrioridadHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "FlagNivelPrioridad");

    private void OnChangeFechaAtencionHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(value is not null, EditContext, "FechaAtencion");

    private void OnChangeFechaEmisionHandler(object value)
    {
        IsModified = Fnc.VerifyContextIsChanged((value is null && Solicitud.FechaEmision.HasValue) || (!Solicitud.FechaEmision.HasValue && value is not null) || (Solicitud.FechaEmision.HasValue && value is not null && Solicitud.FechaEmision != (DateTime?)value), EditContext, "FechaEmision");
        if (SolicitudInsertar.FechaAtencion.HasValue)
            EditContext.NotifyFieldChanged(EditContext.Field("FechaAtencion"));
    }
    #endregion

    public void Dispose() => GC.SuppressFinalize(this);
}
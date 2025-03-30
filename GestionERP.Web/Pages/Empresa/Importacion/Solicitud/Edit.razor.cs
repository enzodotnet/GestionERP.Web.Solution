using AutoMapper;
using Blazored.FluentValidation;
using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Importacion;
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

namespace GestionERP.Web.Pages.Empresa.Importacion.Solicitud;

public partial class Edit : IDisposable
{
    #region Propiedades
    private const string codigoModulo = "02";
    private const string codigoServicio = "S104";
    private const string rutaAccion = "/editar";
    private const string rutaServicio = "/solicitudes";
    private string rutaEmpresa = "";
    public FluentValidationValidator validator;

    public SolicitudObtenerDto Solicitud { get; set; }
    public SolicitudEditarDto SolicitudEditar { get; set; }
    public List<SolicitudDetalleGrid> GridDetalles { get; set; }
    public SolicitudDetalleGrid ItemGridDetalle { get; set; }
    public SolicitudDetalleObtenerDto Detalle { get; set; }
    public SolicitudDetalleEditarDto DetalleEditar { get; set; }
    public SolicitudDetalleInsertarDto DetalleInsertar { get; set; }
    private EditContext EditContext { get; set; }
    private EditContext DetalleInsertarContext { get; set; }
    private EditContext DetalleEditarContext { get; set; }
    public SolicitudDetalleGridValidator GridDetalleValidator { get; set; }
    public SolicitudEditarValidator Validator { get; set; }
    public SolicitudDetalleInsertarValidator DetalleInsertarValidator { get; set; }
    public SolicitudDetalleEditarValidator DetalleEditarValidator { get; set; }
    private SerieDocumentoConsultaPorCodigoEmpresaDto Numerador { get; set; }
    public bool IsEditingGridDetalle { get; set; }
    private bool IsInitPage { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool IsAuthUser { get; set; }
    private bool IsModified { get; set; }
    private bool IsModifiedDetalle { get; set; }
    public TelerikNotification AlertDetalle { get; set; }
    public TelerikNotification Alert { get; set; }
    public string VerboAccionModal { get; set; }
    public string DescripcionAccionModal { get; set; }
    public string TipoAccionModal { get; set; } 
    public string TipoAccionDialog { get; set; }
    public string OrigenDialog { get; set; }
    public string OrigenModal { get; set; }
    public bool EsVisibleDialogDetalle { get; set; }
    public bool EsVisibleModalDetalle { get; set; }
    private bool IsChangedEntidad { get; set; }
    private bool IsChangedPaisOrigen { get; set; }
    private bool IsChangedPaisProcedencia { get; set; }
    public ISvgIcon IconoAccionModal { get; set; }
    private TelerikGrid<SolicitudDetalleGrid> GridDetalleRef { get; set; }
    private IEnumerable<SolicitudFlag> NivelesPrioridad { get; set; }
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    [Parameter] public Guid? Id { get; set; }
    public EmpresaConsultaPorCodigoWebDto Empresa { get; set; }
    private bool EsVisibleVolver { get; set; }
    [Parameter] public string CodigoWebEmpresa { get; set; }
    [Parameter][SupplyParameterFromQuery(Name = "returnpage")] public string ReturnPage { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    #endregion

    [Inject] public IImportacionSolicitud ISolicitud { get; set; }
    [Inject] public IPrincipalEmpresa IEmpresa { get; set; }
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

            SolicitudEditar = new(); 

            Detalle = new();
            DetalleInsertar = new();
            DetalleEditar = new();
            GridDetalles = [];
            ItemGridDetalle = new();

            NivelesPrioridad = SolicitudFlag.NivelesPrioridad();

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio);
            if (!IsAuthUser) return;

            Empresa = await IEmpresa.ConsultaPorCodigoWeb(CodigoWebEmpresa);
            EsVisibleVolver = !string.IsNullOrEmpty(ReturnPage) && ReturnPage is "index" or "view"; 
            rutaEmpresa = INavigation.Uri.Replace(INavigation.BaseUri, "").Split("?")[0].Replace(rutaServicio, "").Replace($"/{Id}", "").Replace(rutaAccion, "");

			if (!await IPermiso.ConsultaEsAsignadoPorSesion(SolicitudAcceso.Editar, Empresa.Codigo))
			{
				INavigation.NavigateTo($"{rutaEmpresa}/inicio");
				Notify.Show("No tiene permiso para editar registros de [Solicitudes de Importacion]", "error");
				return;
			}

			Solicitud = await ISolicitud.Obtener(Empresa.Codigo, (Guid) Id);
            if (Solicitud is null || Solicitud.CodigoEstado is not "ED")
            {
                INavigation.NavigateTo($"{rutaEmpresa}{rutaServicio}");
                Notify.Show($"El registro de la [Solicitud de Importacion] consultado a editar no está disponible", "error");
                return;
            }

            SolicitudEditar = IMapper.Map<SolicitudEditarDto>(Solicitud);

            Validator = new();
            EditContext = new EditContext(SolicitudEditar);

            GridDetalles = IMapper.Map<List<SolicitudDetalleGrid>>(Solicitud.Detalles);

            Numerador = await ISerieDocumento.ConsultaPorCodigoEmpresa(Solicitud.CodigoSerieDocumento, Solicitud.CodigoDocumento, Empresa.Codigo) ?? new();
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
            if (!VerificarRegistroEsValido()) return;

            Notify.ShowLoading(mensaje: "Actualización en progreso");

            await ISolicitud.Editar(Empresa.Codigo, (Guid) Id, SolicitudEditar);

            IsModified = false;
            Notify.Show($"La solicitud de compra {Solicitud.Codigo} ha sido editada con éxito", "success");
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
            case "M" or "E":
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
                    DetalleInsertarContext = new EditContext(DetalleInsertar);
                else if (tipoAccion is "E")
                    DetalleEditarContext = new EditContext(DetalleEditar);
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
        VerboAccionModal = DescripcionAccionModal = TipoAccionModal = "";
        StateHasChanged();
    }

    private void Volver() => INavigation.NavigateTo($"{rutaEmpresa}{rutaServicio}{(ReturnPage == "view" ? $"/{Id}" : "")}");

    public void CerrarDialog()
    {
        if (OrigenDialog is "detalle")
            EsVisibleDialogDetalle = false;
        TipoAccionDialog = OrigenDialog = "";
        StateHasChanged();
    }

    private bool VerificarRegistroEsValido()
    {
        bool esValido = true;

		if (SolicitudEditar.FechaEstimadaETD?.Date > SolicitudEditar.FechaEstimadaETA?.Date)
		{
			Fnc.MostrarAlerta(Alert, "La fecha estimada ETD debe ser menor o igual a la fecha estimada ETA", "error");
			esValido = false;
		}
		if (SolicitudEditar.FechaEstimadaETA?.Date > SolicitudEditar.FechaReposicionStock?.Date)
		{
			Fnc.MostrarAlerta(Alert, "La fecha estimada ETA debe ser menor o igual a la fecha reposición de stock", "error");
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
                    Fnc.MostrarAlerta(Alert, "Está pendiente completar alguna celda de [cantidad] del detalle de la solicitud de importación", "error");
                    esValido = false;
                } 
            }
        }

        return esValido;
    }

    private void NotifyChange(bool? isValueByVerify = null)
    {
        IsModified = isValueByVerify ?? EditContext.IsModified();
        if (!IsModified)
            IsModified = SolicitudEditar.DetallesInsertar.Count > 0 || SolicitudEditar.DetallesEditar.Count > 0 || SolicitudEditar.DetallesEliminar.Count > 0;
    }

    #region Detalle
    public void InvalidarAccionDetalle(EditContext editContext) => Fnc.MostrarAlerta(AlertDetalle, Cnf.MsgErrorInvalidEditContext, "error");

	private async Task CerrarDetalle()
	{
        if (TipoAccionModal is not "V" && IsModifiedDetalle && !await Dialog.ConfirmAsync("¿Está seguro de cerrar el formulario y que los datos no se carguen?", "Saliendo del formulario"))
            return;
        CerrarModal();
	}

	public static void OnRowDetalleRenderHandler(GridRowRenderEventArgs args) => args.Class = !(args.Item as SolicitudDetalleGrid).Id.HasValue ? "new-row" : "";
    
    public void MostrarAgregarDetalle()
    {
        DetalleInsertar = new(); 
        Detalle = new();
        DetalleInsertarValidator = new();
        IniciarAccionModal("I", "detalle");
    }

    public void MostrarModificarDetalle(SolicitudDetalleGrid item = null)
    {
        if (TipoAccionModal is not "V")
            Detalle = IMapper.Map<SolicitudDetalleObtenerDto>(item);

        string tipoAccion = !Detalle.Id.HasValue ? "M" : "E"; 
        if (tipoAccion is "M")
        {
            DetalleInsertar = IMapper.Map<SolicitudDetalleInsertarDto>(Detalle);
            DetalleInsertarValidator = new()
            {
                UnidadConversion = Detalle.UnidadConversion
            };
        } 
        else
        {
            DetalleEditar = IMapper.Map<SolicitudDetalleEditarDto>(Detalle);
            DetalleEditarValidator = new()
            {
                UnidadConversion = Detalle.UnidadConversion
            };
        }
        IniciarAccionModal(tipoAccion, "detalle");
    }

    private void MostrarQuitarDetalle(SolicitudDetalleGrid item = null)
    {
        if (TipoAccionModal is not "V")
            Detalle = IMapper.Map<SolicitudDetalleObtenerDto>(item);

        IniciarAccionDialog(!Detalle.Id.HasValue ? "Q" : "X", "detalle");
    }

    public void VerItemDetalle(SolicitudDetalleGrid item)
    {
        Detalle = IMapper.Map<SolicitudDetalleObtenerDto>(item);
        IniciarAccionModal("V", "detalle");
    }

    public static void OnCellCantidadRenderHandler(GridCellRenderEventArgs args) => args.Class = !(args.Item as SolicitudDetalleGrid).Cantidad.HasValue ? "cell-error" : "cell-editable";

    public void EditDetalleHandler(GridCommandEventArgs args)
    {
        IsEditingGridDetalle = args.Field is "Cantidad";
        if (IsEditingGridDetalle)
        { 
            GridDetalleValidator = new()
            {
                UnidadConversion = (args.Item as SolicitudDetalleGrid).UnidadConversion
            };
        }
    }

    public void CancelDetalleHandler(GridCommandEventArgs args) => IsEditingGridDetalle = args.Field is not "Cantidad";

	public void UpdateItemDetalleHandler(GridCommandEventArgs args)
    {
        SolicitudDetalleGrid item = (SolicitudDetalleGrid)args.Item;
        int indexGrid = GridDetalles.FindIndex(i => i.CodigoArticulo == item.CodigoArticulo);
        bool esItemNuevo = !item.Id.HasValue;
        int indexEdit = !esItemNuevo ? SolicitudEditar.DetallesEditar.FindIndex(i => i.Id == item.Id) : SolicitudEditar.DetallesInsertar.FindIndex(i => i.CodigoArticulo == item.CodigoArticulo);

        if (args.Field == "Cantidad")
        {
            if (!esItemNuevo && indexEdit > -1)
                SolicitudEditar.DetallesEditar[indexEdit].Cantidad = item.Cantidad;
            else if (esItemNuevo)
                SolicitudEditar.DetallesInsertar[indexEdit].Cantidad = item.Cantidad;

            GridDetalles[indexGrid].Cantidad = item.Cantidad;
        } 
        if (!esItemNuevo && indexEdit == -1)
        {
            SolicitudEditar.DetallesEditar.Add(IMapper.Map(IMapper.Map<SolicitudDetalleObtenerDto>(item), new SolicitudDetalleEditarDto()));
            NotifyChange();
        } 
        IsEditingGridDetalle = false;
    }

    private void OnChangeDetalleCantidadHandler(object value) => IsModifiedDetalle = Fnc.VerifyContextIsChanged(Detalle.Cantidad != (decimal?)value, TipoAccionModal is "E" ? DetalleEditarContext : DetalleInsertarContext, "Cantidad");

    private void OnChangeDetalleObservacionHandler(object value) => IsModifiedDetalle = Fnc.VerifyContextIsChanged((Detalle.Observacion ?? "") != ((string)value ?? ""), TipoAccionModal is "E" ? DetalleEditarContext : DetalleInsertarContext, "Observacion");

    private void ValueChangeArticuloHandler(string value)
    {
        DetalleInsertar.CodigoArticulo = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(DetalleInsertarValidator.MsgErrorArticulo) && (Detalle.CodigoArticulo ?? "").Trim() != (DetalleInsertar.CodigoArticulo ?? ""))
	        Detalle.CodigoArticulo = DetalleInsertarValidator.MsgErrorArticulo = null;
    }

    private async Task OnChangeArticuloHandler(object value)
    {
		string codigo = (string)(value ?? "");
		if (DetalleInsertarContext.IsValid(DetalleInsertarContext.Field("CodigoArticulo")))
		{
			if ((Detalle.CodigoArticulo ?? "") == codigo) goto exit;
            if (GridDetalles.Any(x => x.CodigoArticulo.Trim() == codigo))
            {
                DetalleInsertarValidator.MsgErrorArticulo = "El código ya existe en el detalle del registro";
            }
            else
            {  
                (ArticuloObtenerPorCodigoEmpresaProcesoDocumentoDto item, DetalleInsertarValidator.MsgErrorArticulo) = await IUtil.ObtenerArticuloPorCodigoEmpresaProcesoDocumento(AlertDetalle, codigo, Empresa.Codigo, Numerador.CodigoProcesoDocumento);
			    if (item is not null)
			    {
			        Detalle.CodigoArticulo = item.CodigoArticulo.Trim();
			        Detalle.NombreArticulo = item.NombreArticulo;
			        Detalle.UnidadConversion = DetalleInsertarValidator.UnidadConversion = item.UnidadConversion; 
			        Detalle.CodigoUnidadMedida = item.CodigoUnidadMedida;
			        Detalle.NombreUnidadMedida = item.NombreUnidadMedida;
			        Detalle.Presentacion = item.Presentacion;
			        Detalle.CodigoTipoArticulo = item.CodigoTipoArticulo;
			        Detalle.NombreTipoArticulo = item.NombreTipoArticulo;
			        if (DetalleInsertar.Cantidad.HasValue)
				        DetalleInsertarContext.NotifyFieldChanged(DetalleInsertarContext.Field("Cantidad"));
				    goto exit;
			    }
            } 
            DetalleInsertarContext.NotifyFieldChanged(DetalleInsertarContext.Field("CodigoArticulo"));
		}
        Detalle.CodigoArticulo = codigo;
		Detalle.NombreArticulo = Detalle.CodigoUnidadMedida = Detalle.NombreUnidadMedida = Detalle.CodigoTipoArticulo = Detalle.NombreTipoArticulo = Detalle.Presentacion = null;
		Detalle.UnidadConversion = DetalleInsertarValidator.UnidadConversion = null;
		if (string.IsNullOrEmpty(codigo)) 
            DetalleInsertarContext.MarkAsUnmodified(DetalleInsertarContext.Field("CodigoArticulo")); 
		if (DetalleInsertar.Cantidad.HasValue)
			DetalleInsertarContext.NotifyFieldChanged(DetalleInsertarContext.Field("Cantidad"));
	exit:
		IsModifiedDetalle = DetalleInsertarContext.IsModified();
    } 

    private async Task AccionarDetalle(string tipoAccion)
    {
        GridState<SolicitudDetalleGrid> detalleState = GridDetalleRef.GetState();
        int indexGrid = GridDetalles.FindIndex(i => i.CodigoArticulo == Detalle.CodigoArticulo);
        int indexEdit = tipoAccion is "E" or "X" ? SolicitudEditar.DetallesEditar.FindIndex(i => i.Id == Detalle.Id) : SolicitudEditar.DetallesInsertar.FindIndex(i => i.CodigoArticulo == Detalle.CodigoArticulo);
       
        switch (tipoAccion)
        {
            case "I" or "M":
                ItemGridDetalle = IMapper.Map<SolicitudDetalleGrid>(IMapper.Map(DetalleInsertar, Detalle));
                if (tipoAccion == "I")
                {
                    SolicitudEditar.DetallesInsertar.Add(DetalleInsertar);
                    GridDetalles.Add(ItemGridDetalle);
                    detalleState.InsertedItem = ItemGridDetalle;
                }
                else
                {
                    SolicitudEditar.DetallesInsertar[indexEdit] = DetalleInsertar;
                    GridDetalles[indexGrid] = ItemGridDetalle;
                }
                break; 
            case "E":
                if (indexEdit > -1)
                    SolicitudEditar.DetallesEditar[indexEdit] = DetalleEditar;
                else
                    SolicitudEditar.DetallesEditar.Add(DetalleEditar);
                GridDetalles[indexGrid] = IMapper.Map<SolicitudDetalleGrid>(IMapper.Map(DetalleEditar, Detalle));
                break;
            case "Q" or "X":
                if (tipoAccion == "Q")
                    SolicitudEditar.DetallesInsertar.RemoveAt(indexEdit);
                else
                {
                    SolicitudEditar.DetallesEliminar.Add(new() { Id = (Guid) Detalle.Id });
                    if (indexEdit > -1) 
                        SolicitudEditar.DetallesEditar.RemoveAt(indexEdit); 
                }
                GridDetalles.RemoveAt(indexGrid); 
                CerrarDialog();
                break;
        };
        CerrarModal();
        NotifyChange();
        await GridDetalleRef.SetStateAsync(detalleState);
    }
    #endregion

    #region Catalogos
    private void CargarItemCatalogoProveedor(EntidadProveedorCatalogoPorEmpresaDto item)
    {
        SolicitudEditar.CodigoEntidad = Solicitud.CodigoEntidad = item.CodigoEntidad.Trim();
        Solicitud.NombreEntidad = item.NombreEntidad;
        IsChangedEntidad = true;
        Validator.MsgErrorEntidad = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidad"));
        NotifyChange();
    }

	private void CargarItemCatalogoPaisOrigen(PaisCatalogoDto item)
	{ 
        SolicitudEditar.CodigoPaisOrigen = Solicitud.CodigoPaisOrigen = item.CodigoPais;
        Solicitud.NombrePaisOrigen = item.NombrePais;
        IsChangedPaisOrigen = true;
        Validator.MsgErrorPaisOrigen = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoPaisOrigen"));
        NotifyChange();
	}

	private void CargarItemCatalogoPaisProcedencia(PaisCatalogoDto item)
	{
        SolicitudEditar.CodigoPaisProcedencia = Solicitud.CodigoPaisProcedencia = item.CodigoPais;
        Solicitud.NombrePaisProcedencia = item.NombrePais;
        IsChangedPaisProcedencia = true;
        Validator.MsgErrorPaisProcedencia = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoPaisProcedencia"));
        NotifyChange();
	}

	private void CargarItemCatalogoArticuloPorProcesoDocumento(ArticuloCatalogoPorEmpresaProcesoDocumentoDto item)
    { 
        DetalleInsertar.CodigoArticulo = Detalle.CodigoArticulo = item.CodigoArticulo.Trim();
        Detalle.NombreArticulo = item.NombreArticulo; 
        Detalle.UnidadConversion = DetalleInsertarValidator.UnidadConversion = item.UnidadConversion; 
        Detalle.CodigoUnidadMedida = item.CodigoUnidadMedida;
        Detalle.NombreUnidadMedida = item.NombreUnidadMedida;
        Detalle.Presentacion = item.Presentacion;
        Detalle.CodigoTipoArticulo = item.CodigoTipoArticulo;
        Detalle.NombreTipoArticulo = item.NombreTipoArticulo;
        DetalleInsertarValidator.MsgErrorArticulo = null;
        DetalleInsertarContext.NotifyFieldChanged(DetalleInsertarContext.Field("CodigoArticulo"));
        IsModifiedDetalle = DetalleInsertarContext.IsModified();
        if (DetalleInsertar.Cantidad.HasValue)
            DetalleInsertarContext.NotifyFieldChanged(DetalleInsertarContext.Field("Cantidad")); 
    }
	#endregion

	#region EditContextHandlers
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
				(EntidadProveedorObtenerPorCodigoEmpresaDto item, Validator.MsgErrorEntidad) = await IUtil.ObtenerProveedorPorCodigoEmpresa(Alert, codigo, Empresa.Codigo, "I");
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

    private void ValueChangePaisOrigenHandler(string value)
    {
        SolicitudEditar.CodigoPaisOrigen = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorPaisOrigen) && (Solicitud.CodigoPaisOrigen ?? "").Trim() != (SolicitudEditar.CodigoPaisOrigen ?? ""))
            Solicitud.CodigoPaisOrigen = Validator.MsgErrorPaisOrigen = null;
    }

    private async Task OnChangePaisOrigenHandler(object value)
	{
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoPaisOrigen")))
		{ 
			if ((Solicitud.CodigoPaisOrigen ?? "").Trim() != codigo)
			{
				IsChangedPaisOrigen = true;
                (PaisObtenerPorCodigoDto item, Validator.MsgErrorPaisOrigen) = await IUtil.ObtenerPaisPorCodigo(Alert, codigo);
                if (item is not null)
                {
                    Solicitud.CodigoPaisOrigen = item.CodigoPais;
                    Solicitud.NombrePaisOrigen = item.NombrePais;
                    goto exit;
                } 
                EditContext.NotifyFieldChanged(EditContext.Field("CodigoPaisOrigen"));
            }
			else
			{
				if (!IsChangedPaisOrigen) EditContext.MarkAsUnmodified(EditContext.Field("CodigoPaisOrigen"));
				goto exit;
			}
		}
        Solicitud.CodigoPaisOrigen = codigo;
		Solicitud.NombrePaisOrigen = null;
	exit:
		NotifyChange();
	}

    private void ValueChangePaisProcedenciaHandler(string value)
    {
        SolicitudEditar.CodigoPaisProcedencia = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorPaisProcedencia) && (Solicitud.CodigoPaisProcedencia ?? "").Trim() != (SolicitudEditar.CodigoPaisProcedencia ?? ""))
            Solicitud.CodigoPaisProcedencia = Validator.MsgErrorPaisProcedencia = null;
    }

    private async Task OnChangePaisProcedenciaHandler(object value)
	{
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoPaisProcedencia")))
		{ 
			if ((Solicitud.CodigoPaisProcedencia ?? "").Trim() != codigo)
			{
				IsChangedPaisProcedencia = true;
                (PaisObtenerPorCodigoDto item, Validator.MsgErrorPaisProcedencia) = await IUtil.ObtenerPaisPorCodigo(Alert, codigo);
                if (item is not null)
                {
                    Solicitud.CodigoPaisProcedencia = item.CodigoPais;
                    Solicitud.NombrePaisProcedencia = item.NombrePais;
                    goto exit;
                } 
                EditContext.NotifyFieldChanged(EditContext.Field("CodigoPaisProcedencia"));
            }
			else
			{
				if (!IsChangedPaisProcedencia) EditContext.MarkAsUnmodified(EditContext.Field("CodigoPaisProcedencia"));
				goto exit;
			}
		}
        Solicitud.CodigoPaisProcedencia = codigo;
		Solicitud.NombrePaisProcedencia = null;
	exit:
		NotifyChange();
	}

	private void OnChangeObservacionHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged((Solicitud.Observacion ?? "") != ((string)value ?? ""), EditContext, "Observacion"));

	private void OnChangeMotivoHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged((Solicitud.Motivo ?? "") != ((string)value ?? ""), EditContext, "Motivo"));

	private void OnChangeFlagNivelPrioridadHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Solicitud.FlagNivelPrioridad != (string)value, EditContext, "FlagNivelPrioridad"));

	private void OnChangeFechaReposicionStockHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Solicitud.FechaReposicionStock != (DateTime?)value, EditContext, "FechaReposicionStock"));

	private void OnChangeFechaEstimadaETAHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Solicitud.FechaEstimadaETA != (DateTime?)value, EditContext, "FechaEstimadaETA"));

	private void OnChangeFechaEstimadaETDHandler(object value) => NotifyChange(Fnc.VerifyContextIsChanged(Solicitud.FechaEstimadaETD != (DateTime?)value, EditContext, "FechaEstimadaETD"));
	#endregion

	public void Dispose() => GC.SuppressFinalize(this);
}
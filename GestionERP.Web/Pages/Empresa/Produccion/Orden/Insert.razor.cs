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

namespace GestionERP.Web.Pages.Empresa.Produccion.Orden;

public partial class Insert : IDisposable
{
    #region Propiedades
    private const string codigoModulo = "05";
    private const string codigoServicio = "S112";
    private const string rutaAccion = "/emitir";
    private string rutaEmpresa = "";
    private string pathBaseUri = "";
    private const string rutaServicio = "/ordenes";

    public OrdenInsertarDto OrdenInsertar { get; set; }
    public OrdenObtenerDto Orden { get; set; }
    public OrdenInsertarValidator Validator { get; set; }
    public TipoProduccionConsultaPorCodigoDto TipoProduccion { get; set; }
    private EditContext EditContext { get; set; }
    public bool IsInitPage { get; set; }
    public bool EsVisibleBotonCatalogoSolicitudes { get; set; }
    public bool EsVisibleCatalogoSolicitudes { get; set; }
    public bool EsVisibleQuitarSolicitud { get; set; }
    private bool IsLoadingCatalogoSolicitudes { get; set; }
    public string CodigoLocalNumerador { get; set; }
    public string CodigoProcesoDocumento { get; set; }
    public bool EsVisibleDialogSolicitud { get; set; } 
    public bool EsVisibleListaMateriales { get; set; }  
    private bool IsValidCantidad { get; set; } 
    public TelerikNotification AlertCatalogoSolicitudes { get; set; }
    public List<DateTime> FechasCerradoOperacion { get; set; }
    public EntidadConsultaPorEmpresaEsGestionadaDto EntidadGestionada { get; set; }
    public EmpresaEjercicioConsultaIntervaloFechaDto FechaIntervalo { get; set; }
    public List<SolicitudCatalogoAtenderDto> CatalogoSolicitudes { get; set; }
    public IEnumerable<SolicitudCatalogoAtenderDto> ItemsSelectedSolicitud { get; set; }
    public IEnumerable<VersionPlanMaterialConsultaDto> Materiales { get; set; }
    public string CodigoEjercicio { get; set; }
    private IEnumerable<VersionPlanFlag> TiposMaterial { get; set; }
    private IEnumerable<VersionPlanFlag> InsumosMaterial { get; set; }
    private IEnumerable<EmpresaEjercicioCatalogoDto> CatalogoEjercicios { get; set; }
    public string CodigoTipoDocumento { get; set; }
    public bool EsVisibleBotonDocumento { get; set; }
    private bool IsAuthUser { get; set; }
    private decimal CantidadPlan { get; set; }
    private bool IsLoadingAction { get; set; } 
    private bool IsModified { get; set; }
    public string TituloTipoOrigen { get; set; }
    public string CodigoPermisoEmitir { get; set; }
    public TelerikNotification Alert { get; set; }
    private IEnumerable<OrdenFlag> Origenes { get; set; }
    private IEnumerable<OrdenFlag> TiposRegistro { get; set; }
    [Parameter][SupplyParameterFromQuery(Name = "returnpage")] public string ReturnPage { get; set; }
    public EmpresaConsultaPorCodigoWebDto Empresa { get; set; }
    private bool EsVisibleVolver { get; set; }
    [Parameter] public string CodigoWebEmpresa { get; set; }
    [Parameter] public string Origen { get; set; }
    public string OrigenTemp { get; set; }
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    #endregion

    [Inject] public IProduccionOrden IOrden { get; set; }
    [Inject] public IProduccionSolicitud ISolicitud { get; set; }
    [Inject] public IProduccionVersionPlan IVersionPlan { get; set; }
    [Inject] public IProduccionPlan IPlan { get; set; }
    [Inject] public IPrincipalEmpresa IEmpresa { get; set; } 
    [Inject] public IPrincipalEntidad IEntidad { get; set; }
    [Inject] public IPrincipalTipoProduccion ITipoProduccion { get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public UtilService IUtil { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }
    [Inject] public IMapper IMapper { get; set; }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);
        if (!string.IsNullOrEmpty(Origen))
        {
            if (!string.IsNullOrEmpty(OrigenTemp) && Origen != OrigenTemp)
                await IniciarVistaAccion();

            OrigenTemp = Origen;
        }
    }

    protected override async Task OnInitializedAsync()
    {
        Origenes = OrdenFlag.Origenes();
        TiposRegistro = OrdenFlag.TiposRegistro();
        TiposMaterial = VersionPlanFlag.TiposMaterial();
        InsumosMaterial = VersionPlanFlag.InsumosMaterial();
        Validator = new(); 
        await IniciarVistaAccion();
    }
    
    protected async Task IniciarVistaAccion()
    {
        try
        {
            Notify.ShowLoading(mensaje: "Iniciando formulario");

            OrdenInsertar = new();
            Orden = new()
            {
                Materiales = []
            };

            FechasCerradoOperacion = [];
            FechaIntervalo = new();
            TipoProduccion = new();

            CodigoTipoDocumento = "OR";
            EsVisibleBotonDocumento = true;

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio);
            if (!IsAuthUser) return;

            Empresa = await IEmpresa.ConsultaPorCodigoWeb(CodigoWebEmpresa);

            EsVisibleVolver = !string.IsNullOrEmpty(ReturnPage) && ReturnPage is "index";
            
            pathBaseUri = $"/{INavigation.ToBaseRelativePath(INavigation.Uri).Split("?")[0]}";
            rutaEmpresa = INavigation.Uri.Replace(INavigation.BaseUri, "").Split("?")[0].Replace(rutaServicio, "").Replace(rutaAccion, "").Replace(string.Concat("/", Origen ?? ""), "");

            if (!VerificarTipoOrigenEsValido())
            {
                INavigation.NavigateTo($"{rutaEmpresa}/inicio");
                Notify.Show("La ruta al que intenta acceder para emitir [Orden de Compra] no tiene un tipo de origen válido", "error");
                return;
            }

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(CodigoPermisoEmitir, Empresa.Codigo))
            {
                INavigation.NavigateTo($"{rutaEmpresa}/inicio");
                Notify.Show("No tiene permiso para emitir registros de [Ordenes de Produccion]", "error");
                return;
            }

            EntidadGestionada = await IEntidad.ConsultaPorEmpresaEsGestionada(Empresa.Codigo);

            FechaIntervalo = await IEmpresa.ConsultaEjercicioIntervaloFecha(Empresa.Codigo);
            (await IEmpresa.ConsultaModuloPeriodoFechasEsCerradoOperacion(Empresa.Codigo, codigoModulo))?.ToList().ForEach(x => FechasCerradoOperacion.Add(x.Fecha));
            OrdenInsertar.FechaEmision = !FechasCerradoOperacion.Any(x => x == DateTime.Now.Date) ? DateTime.Now.Date : null;
            if (OrdenInsertar.FechaEmision.HasValue)
                Orden.FechaEmision = (DateTime)OrdenInsertar.FechaEmision;
 
            EditContext = new EditContext(OrdenInsertar);
            IsInitPage = true;
            StateHasChanged();
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
            IsModified = false;
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

            Guid id = await IOrden.Insertar(Empresa.Codigo, OrdenInsertar);

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

    private bool VerificarTipoOrigenEsValido()
    {
        bool esValido = false;
        if (!string.IsNullOrEmpty(Origen) && Origen is "directa" or "solicitud")
        { 
            if (Origen is "directa")
            {
                CodigoPermisoEmitir = OrdenAcceso.EmitirDirecta;
                OrdenInsertar.FlagOrigen = "D";
                TituloTipoOrigen = "directa";
                EsVisibleBotonCatalogoSolicitudes = false;
            }
            else 
            {
                CodigoPermisoEmitir = OrdenAcceso.EmitirPorSolicitud;
                OrdenInsertar.FlagOrigen = "S";
                TituloTipoOrigen = "por solicitud";
                EsVisibleBotonCatalogoSolicitudes = true;
            }
            esValido = true;
        }
        return esValido;
    }

    private async Task Cerrar(LocationChangingContext context)
    {
        if (IsAuthUser && IsModified && pathBaseUri != context.TargetLocation && !await Dialog.ConfirmAsync("¿Está seguro de salir del formulario de emitir sin haber guardado?", "Saliendo del formulario"))
            context.PreventNavigation();
    }

    private void LimpiarCamposPorOrigenSolicitud()
    {
        OrdenInsertar.CodigoEntidad = Validator.MsgErrorEntidad = null;
        OrdenInsertar.CodigoPlan = Orden.CodigoPlan = Validator.MsgErrorPlan = null;
        OrdenInsertar.Motivo = OrdenInsertar.Observacion = null;
        Orden.CodigoArticuloTerminado = Orden.NombreArticuloTerminado = Orden.PresentacionArticulo = Orden.CodigoUnidadMedida = Orden.CodigoTipoProduccion = Orden.NombreTipoProduccion = Orden.NombrePlan = Orden.NombreEntidad = Orden.NombreVersionPlan = "";
        OrdenInsertar.Cantidad = Orden.Cantidad = Orden.UnidadConversionArticulo = Validator.UnidadConversionArticulo = null;
        IsValidCantidad = false;
        OrdenInsertar.CodigoVersionPlan = null;
    }

    private void Volver() => INavigation.NavigateTo($"{rutaEmpresa}{rutaServicio}");

    #region CatalogoSolicitudes
    public async Task MostrarCatalogoSolicitudes()
    {
        IsLoadingCatalogoSolicitudes = true;

        CatalogoEjercicios = await IEmpresa.CatalogoEjercicios(Empresa.Codigo) ?? [];
        CodigoEjercicio = await IEmpresa.ConsultaEjercicioCodigoPorAnio(Empresa.Codigo, DateTime.Now.Year);
        ItemsSelectedSolicitud = [];

        await CargarCatalogoSolicitudes();
        EsVisibleCatalogoSolicitudes = true;
    }

    private async Task VisibleCatalogoSolicitudesChangedHandler(bool esVisible)
    {
        if (!esVisible)
        {
            if (ItemsSelectedSolicitud.Any() && !await Dialog.ConfirmAsync("¿Está seguro de salir del catálogo y que el ítem seleccionado no se cargue?", "Saliendo de catálogo"))
                return; 
            EsVisibleCatalogoSolicitudes = false;
            CatalogoSolicitudes = null;
        }
    }

    private async Task OnRowDoubleClickCatalogoSolicitudHandler(GridRowClickEventArgs args) => await AgregarItemSolicitud(args.Item as SolicitudCatalogoAtenderDto);

    private async Task OnComboEjercicioValueChanged(string value)
    {
        CodigoEjercicio = value;
        if (!string.IsNullOrEmpty(CodigoEjercicio))
            await CargarCatalogoSolicitudes();
    }

    private async Task CargarCatalogoSolicitudes()
    {
        try
        {
            IsAuthUser = (await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio, User.FindFirst("code").Value)).esValido;
            if (!IsAuthUser) return;
            ItemsSelectedSolicitud = [];

            CatalogoSolicitudes = (List<SolicitudCatalogoAtenderDto>)await ISolicitud.CatalogoAtender(Empresa.Codigo, CodigoEjercicio, CodigoProcesoDocumento, CodigoLocalNumerador);
        }
        catch (HttpRequestException)
        {
            if (EsVisibleCatalogoSolicitudes)
                Fnc.MostrarAlerta(AlertCatalogoSolicitudes, Cnf.MsgErrorNotConnectApi, "error");
            else
                Notify.ShowError("NC");
        }
        catch (HttpResponseException ex)
        {
            if (EsVisibleCatalogoSolicitudes)
                Fnc.MostrarAlerta(AlertCatalogoSolicitudes, $"{(ex.Code == "NF" ? Cnf.MsgErrorNotFoundAPi : ex.Message)}", "error");
            else
                Notify.ShowError(ex.Code, ex);
        }
        catch (Exception ex)
        {
            if (EsVisibleCatalogoSolicitudes)
                Fnc.MostrarAlerta(AlertCatalogoSolicitudes, Cnf.MsgErrorFuncAppWeb, "error");
            else
                Notify.ShowError("FA", ex);
        }
        finally
        {
            IsLoadingCatalogoSolicitudes = false;
            Notify.ShowLoading(false);
        }
    }

    private async Task SeleccionarItemSolicitud()
    {
        if (!ItemsSelectedSolicitud.Any())
        {
            Fnc.MostrarAlerta(AlertCatalogoSolicitudes, "Es necesario que seleccione al menos un ítem del catálogo", "warning");
            return;
        }
        await AgregarItemSolicitud(ItemsSelectedSolicitud.First());
    }

    private async Task AgregarItemSolicitud(SolicitudCatalogoAtenderDto item)
    {
        IMapper.Map(item, Orden);
        IMapper.Map(item, OrdenInsertar);
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoSolicitud"));
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoVersionPlan"));
        EditContext.NotifyFieldChanged(EditContext.Field("Cantidad")); 
        Validator.UnidadConversionArticulo = Orden.UnidadConversionArticulo;
        EsVisibleCatalogoSolicitudes = EsVisibleBotonCatalogoSolicitudes = EsVisibleBotonDocumento = false;
        CantidadPlan = (await IPlan.ConsultaPorCodigo(Empresa.Codigo, Orden.CodigoPlan)).Cantidad;
        IsValidCantidad = true;
        EsVisibleQuitarSolicitud = true;
    } 
     
    private void QuitarItemSolicitud()
    {
        LimpiarCamposPorOrigenSolicitud();
        OrdenInsertar.CodigoSolicitudReferencia = null;
        EsVisibleBotonCatalogoSolicitudes = EsVisibleBotonDocumento = true;
        EsVisibleDialogSolicitud = false;
    }
    #endregion

    #region Catalogos
    private void CargarItemCatalogoSerieDocumento(SerieDocumentoCatalogoPorEmpresaSesionDto item)
    {
        OrdenInsertar.CodigoDocumento = item.CodigoDocumento;
        OrdenInsertar.CodigoSerieDocumento = item.CodigoSerieDocumento;
        Orden.NombreSerieDocumento = item.NombreSerieDocumento;
        CodigoProcesoDocumento = item.CodigoProcesoDocumento;
           
        string flagTipoRegistro = TiposRegistro.Where(x => x.CodigoProcesoDocumento == CodigoProcesoDocumento).Select(x => x.Codigo).SingleOrDefault(); 
        if (OrdenInsertar.FlagTipoRegistro != flagTipoRegistro)
        {
            OrdenInsertar.FlagTipoRegistro = flagTipoRegistro;
            if (!string.IsNullOrEmpty(OrdenInsertar.CodigoPlan))
            {
                OrdenInsertar.CodigoPlan = Validator.MsgErrorPlan = null;
                TipoProduccion = new();
                Orden.CodigoTipoProduccion = Orden.NombreTipoProduccion = Orden.CodigoArticuloTerminado = Orden.NombreArticuloTerminado = Orden.PresentacionArticulo = Orden.CodigoUnidadMedida = Orden.NombrePlan = "";
                Orden.UnidadConversionArticulo = null;

                EditContext.NotifyFieldChanged(EditContext.Field("CodigoPlan"));
                EditContext.MarkAsUnmodified(EditContext.Field("CodigoPlan"));
                if (!string.IsNullOrEmpty(OrdenInsertar.CodigoEntidad))
                {
                    Validator.MsgErrorEntidad = null;
                    OrdenInsertar.CodigoEntidad = Orden.CodigoEntidad = Orden.NombreEntidad = null;
                    EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidad"));
                    EditContext.MarkAsUnmodified(EditContext.Field("CodigoEntidad"));
                }
            }
        }

        if (!string.IsNullOrEmpty(item.CodigoLocal))
        {
            Validator.MsgErrorLocalRecepcion = null;
            OrdenInsertar.CodigoLocalRecepcion = Orden.CodigoLocalRecepcion = CodigoLocalNumerador = item.CodigoLocal;
            Orden.NombreLocalRecepcion = item.NombreLocal;
            EditContext.NotifyFieldChanged(EditContext.Field("CodigoLocalRecepcion"));
        }

        EditContext.NotifyFieldChanged(EditContext.Field("CodigoDocumento"));
        IsModified = EditContext.IsModified();
    }

    private async Task CargarItemCatalogoPlan(PlanCatalogoDto item)
    {
        OrdenInsertar.CodigoPlan = Orden.CodigoPlan = item.CodigoPlan;
        Orden.NombrePlan = item.NombrePlan;
        Orden.CodigoTipoProduccion = item.CodigoTipoProduccion;
        Orden.NombreTipoProduccion = item.NombreTipoProduccion;
        CantidadPlan = item.Cantidad;

        Validator.MsgErrorEntidad = null;
        if (TipoProduccion.FlagTipoEncargo != item.FlagTipoEncargo)
        {
            TipoProduccion = await ITipoProduccion.ConsultaPorCodigo(item.CodigoTipoProduccion);
            if (item.FlagTipoEncargo is "G")
            {
                OrdenInsertar.CodigoEntidad = Orden.CodigoEntidad = EntidadGestionada.CodigoEntidad;
                Orden.NombreEntidad = EntidadGestionada.NombreEntidad;
                EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidad"));
            }
            else
            {
                OrdenInsertar.CodigoEntidad = Orden.CodigoEntidad = Orden.NombreEntidad = null;
                EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidad"));
                EditContext.MarkAsUnmodified(EditContext.Field("CodigoEntidad"));
            }
        }

        Orden.CodigoArticuloTerminado = item.CodigoArticuloTerminado.Trim();
        Orden.NombreArticuloTerminado = item.NombreArticuloTerminado;
        Orden.UnidadConversionArticulo = Validator.UnidadConversionArticulo = item.UnidadConversionArticulo;
        Orden.CodigoUnidadMedida = item.CodigoUnidadMedida;
        Orden.NombreUnidadMedida = item.NombreUnidadMedida;
        Orden.PresentacionArticulo = item.PresentacionArticulo;
        if (OrdenInsertar.Cantidad.HasValue)
            EditContext.NotifyFieldChanged(EditContext.Field("Cantidad")); 

        Validator.MsgErrorPlan = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoPlan"));
    }

    private void CargarItemCatalogoEntidad(EntidadCatalogoPorEmpresaDto item)
    {
        OrdenInsertar.CodigoEntidad = Orden.CodigoEntidad = item.CodigoEntidad.Trim();
        Orden.NombreEntidad = item.NombreEntidad;
        Validator.MsgErrorEntidad = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidad"));
    }

    private async Task CargarItemCatalogoVersionPan(VersionPlanCatalogoDto item)
    {
        OrdenInsertar.CodigoVersionPlan = Orden.CodigoVersionPlan = item.CodigoVersionPlan.Trim();
        Orden.NombreVersionPlan = item.NombreVersionPlan;
        Validator.MsgErrorVersionPlan = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoVersionPlan"));
        Materiales = await IVersionPlan.ConsultaMateriales(Empresa.Codigo, OrdenInsertar.CodigoVersionPlan) ?? [];
        IMapper.Map(Materiales, Orden.Materiales);
        CalcularCantidadMateriales();
    }

    private void CargarItemCatalogoLocal(LocalCatalogoPorEmpresaDto item)
    {
        OrdenInsertar.CodigoLocalRecepcion = Orden.CodigoLocalRecepcion = item.CodigoLocal;
        Orden.NombreLocalRecepcion = item.NombreLocal;
        Validator.MsgErrorLocalRecepcion = null;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoLocalRecepcion"));
        IsModified = EditContext.IsModified();
    }
    #endregion

    #region EditContextHandlers
    private void ValueChangePlanHandler(string value)
    {
        OrdenInsertar.CodigoPlan = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorPlan) && (Orden.CodigoPlan ?? "") != (OrdenInsertar.CodigoPlan ?? ""))
            Orden.CodigoPlan = Validator.MsgErrorPlan = null;
    }

    private async Task OnChangePlanHandler(object value)
    {
        string codigo = (string)(value ?? "");
        if (EditContext.IsValid(EditContext.Field("CodigoPlan")))
        {
            if ((Orden.CodigoPlan ?? "") == (codigo ?? "")) goto exit;
            (PlanObtenerPorCodigoDto item, Validator.MsgErrorPlan) = await IUtil.ObtenerPlanProduccionPorCodigo(Alert, Empresa.Codigo, codigo, OrdenInsertar.FlagTipoRegistro);
            if (item is not null)
            {
                Orden.CodigoPlan = item.CodigoPlan;
                Orden.NombrePlan = item.NombrePlan;
                Orden.CodigoTipoProduccion = item.CodigoTipoProduccion;
                Orden.NombreTipoProduccion = item.NombreTipoProduccion;
                CantidadPlan = item.Cantidad;

                Validator.MsgErrorEntidad = null;
                if (TipoProduccion.FlagTipoEncargo != item.FlagTipoEncargo)
                {
                    TipoProduccion = await ITipoProduccion.ConsultaPorCodigo(item.CodigoTipoProduccion);
                    if (item.FlagTipoEncargo is "G")
                    {
                        OrdenInsertar.CodigoEntidad = Orden.CodigoEntidad = EntidadGestionada.CodigoEntidad;
                        Orden.NombreEntidad = EntidadGestionada.NombreEntidad;
                        EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidad"));
                    }
                    else
                    {
                        OrdenInsertar.CodigoEntidad = Orden.CodigoEntidad = Orden.NombreEntidad = null;
                        EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidad"));
                        EditContext.MarkAsUnmodified(EditContext.Field("CodigoEntidad"));
                    }
                }

                Orden.CodigoArticuloTerminado = item.CodigoArticuloTerminado.Trim();
                Orden.NombreArticuloTerminado = item.NombreArticuloTerminado;
                Orden.UnidadConversionArticulo = Validator.UnidadConversionArticulo = item.UnidadConversionArticulo;
                Orden.CodigoUnidadMedida = item.CodigoUnidadMedida;
                Orden.NombreUnidadMedida = item.NombreUnidadMedida;
                Orden.PresentacionArticulo = item.PresentacionArticulo;
                if (OrdenInsertar.Cantidad.HasValue)
                    EditContext.NotifyFieldChanged(EditContext.Field("Cantidad")); 

                goto exit;
            }
            EditContext.NotifyFieldChanged(EditContext.Field("CodigoPlan"));
        }
        Orden.CodigoPlan = codigo;
        TipoProduccion = new();
        Orden.NombrePlan = Orden.CodigoTipoProduccion = Orden.NombreTipoProduccion = OrdenInsertar.CodigoEntidad = Orden.NombreEntidad = null;
        Orden.CodigoArticuloTerminado = Orden.NombreArticuloTerminado = Orden.CodigoUnidadMedida = Orden.NombreUnidadMedida = Orden.PresentacionArticulo = null;
        Orden.UnidadConversionArticulo = Validator.UnidadConversionArticulo = null;
        OrdenInsertar.CodigoVersionPlan = Orden.NombreVersionPlan = null;
        if (string.IsNullOrEmpty(codigo)) EditContext.MarkAsUnmodified(EditContext.Field("CodigoPlan"));
    exit:
        IsModified = EditContext.IsModified();
    }

    private void ValueChangeVersionPlanHandler(string value)
    {
        OrdenInsertar.CodigoVersionPlan = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorVersionPlan) && (Orden.CodigoVersionPlan ?? "") != (OrdenInsertar.CodigoVersionPlan ?? ""))
            Orden.CodigoVersionPlan = Validator.MsgErrorVersionPlan = null;
    }

    private async Task OnChangeVersionPlanHandler(object value)
    {
        string codigo = (string)(value ?? "");
        bool esCalculable = false;
        if (EditContext.IsValid(EditContext.Field("CodigoVersionPlan")))
        {
            if ((Orden.CodigoVersionPlan ?? "") == (codigo ?? "")) goto exit;
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
        Orden.CodigoVersionPlan = codigo; 
        Orden.NombreVersionPlan = null;
        if (string.IsNullOrEmpty(codigo)) EditContext.MarkAsUnmodified(EditContext.Field("CodigoVersionPlan"));
    exit: 
        Materiales = await IVersionPlan.ConsultaMateriales(Empresa.Codigo, Orden.CodigoVersionPlan) ?? [];
        IMapper.Map(Materiales, Orden.Materiales);
        if (esCalculable)
            CalcularCantidadMateriales();
        IsModified = EditContext.IsModified();
    }

    private void ValueChangeEntidadHandler(string value)
    {
        OrdenInsertar.CodigoEntidad = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorEntidad) && (Orden.CodigoEntidad ?? "").Trim() != (OrdenInsertar.CodigoEntidad ?? ""))
            Orden.CodigoEntidad = Validator.MsgErrorEntidad = null;
    }

    private async Task OnChangeEntidadHandler(object value)
    {
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoEntidad")))
		{ 
            if ((Orden.CodigoEntidad ?? "") == codigo) goto exit;
			(EntidadObtenerPorCodigoEmpresaDto item, Validator.MsgErrorEntidad)  = await IUtil.ObtenerEntidadPorCodigoEmpresa(Alert, codigo, Empresa.Codigo, TipoProduccion.FlagTipoEncargo);
            if (item is not null)
            {				
                Orden.CodigoEntidad = item.CodigoEntidad.Trim();
			    Orden.NombreEntidad = item.NombreEntidad; 
                goto exit;
            }  
            EditContext.NotifyFieldChanged(EditContext.Field("CodigoEntidad"));
		}
        Orden.CodigoEntidad = codigo;
        Orden.NombreEntidad = null;
        if (string.IsNullOrEmpty(codigo)) EditContext.MarkAsUnmodified(EditContext.Field("CodigoEntidad"));
	exit:
        IsModified = EditContext.IsModified();
    }

    private void ValueChangeLocalHandler(string value)
    {
        OrdenInsertar.CodigoLocalRecepcion = value?.Trim().ToUpper();
        if (!string.IsNullOrEmpty(Validator.MsgErrorLocalRecepcion) && (Orden.CodigoLocalRecepcion ?? "").Trim() != (OrdenInsertar.CodigoLocalRecepcion ?? ""))
            Orden.CodigoLocalRecepcion = Validator.MsgErrorLocalRecepcion = null;
    }

    private async Task OnChangeLocalHandler(object value)
    {
        string codigo = (string)(value ?? "");
		if (EditContext.IsValid(EditContext.Field("CodigoLocalRecepcion")))
		{ 
			if (!string.IsNullOrEmpty(codigo))
			{ 
                if ((Orden.CodigoLocalRecepcion ?? "") == codigo) goto exit;
                (LocalObtenerPorCodigoEmpresaDto item, Validator.MsgErrorLocalRecepcion) = await IUtil.ObtenerLocalPorCodigoEmpresa(Alert, codigo, Empresa.Codigo);
                if (item is not null)
                {
                    Orden.CodigoLocalRecepcion = item.CodigoLocal;
                    Orden.NombreLocalRecepcion = item.NombreLocal; 
                    goto exit;
                } 
                EditContext.NotifyFieldChanged(EditContext.Field("CodigoLocalRecepcion")); 
			}
			else
            {
                OrdenInsertar.CodigoLocalRecepcion = null;
				EditContext.MarkAsUnmodified(EditContext.Field("CodigoLocalRecepcion"));
            }
		}
        Orden.CodigoLocalRecepcion = codigo;
		Orden.NombreLocalRecepcion = null;
	exit:
		IsModified = EditContext.IsModified();
    }

    private void OnChangeObservacionHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "Observacion");

    private void OnChangeMotivoHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(!string.IsNullOrEmpty((string)value), EditContext, "Motivo");

    private void OnChangeCantidadHandler(object value)
    {  
        decimal? cantidad = (decimal?)value;
        IsValidCantidad = EditContext.IsValid(EditContext.Field("Cantidad"));
        if (IsValidCantidad)
        {  
            if ((Orden.Cantidad ?? 0) != cantidad)
            {
                Orden.Cantidad = cantidad;
                CalcularCantidadMateriales();
            } 
            EditContext.NotifyFieldChanged(EditContext.Field("Cantidad"));
            goto exit;
        }
        Orden.Cantidad = cantidad;
        if (!cantidad.HasValue) EditContext.MarkAsUnmodified(EditContext.Field("Cantidad"));
    exit:
        IsModified = EditContext.IsModified();
    }

    private void OnChangeFechaEmisionHandler(object value)
    {
        IsModified = Fnc.VerifyContextIsChanged((value is null && Orden.FechaEmision.HasValue) || (!Orden.FechaEmision.HasValue && value is not null) || (Orden.FechaEmision.HasValue && value is not null && Orden.FechaEmision != (DateTime?)value), EditContext, "FechaEmision");
        if (OrdenInsertar.FechaInicio.HasValue)
            EditContext.NotifyFieldChanged(EditContext.Field("FechaInicio"));
    }

    private void OnChangeFechaInicioHandler(object value)
    {
        IsModified = Fnc.VerifyContextIsChanged(value is not null, EditContext, "FechaInicio");
        if (OrdenInsertar.FechaTermino.HasValue)
            EditContext.NotifyFieldChanged(EditContext.Field("FechaTermino"));
    }

    private void OnChangeFechaTerminoHandler(object value) => IsModified = Fnc.VerifyContextIsChanged(value is not null, EditContext, "FechaTermino");
    #endregion

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
        if (Orden.Materiales.Count > 0 && OrdenInsertar.Cantidad.HasValue && IsValidCantidad)
        {
            decimal multiploIndicador = (decimal) OrdenInsertar.Cantidad / CantidadPlan; 
            decimal cantidadMaterial;
            foreach (OrdenMaterialObtenerDto itemMaterialOrden in Orden.Materiales)
            {
                cantidadMaterial = Materiales.Where(x => x.CodigoArticulo == itemMaterialOrden.CodigoArticulo).Select(x => x.Cantidad).Single() * (itemMaterialOrden.FlagInsumo is "F" && multiploIndicador < 1 ? 1 : multiploIndicador);
                itemMaterialOrden.Cantidad = itemMaterialOrden.FlagTipo is "A" ? Math.Round(cantidadMaterial, 0) : Math.Round(cantidadMaterial, 3, MidpointRounding.AwayFromZero);
            }
        }
    }
    #endregion
    
    public void Dispose() => GC.SuppressFinalize(this);
}
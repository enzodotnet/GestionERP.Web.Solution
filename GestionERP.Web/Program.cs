using GestionERP.Web;
using GestionERP.Web.AuthProviders;
using GestionERP.Web.Services.Apis;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Services;
using GestionERP.Web.Global;
using Microsoft.AspNetCore.Components.Authorization; 
using Telerik.Blazor.Services;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using System.Globalization;
using Telerik.ReportViewer.BlazorNative.Services;
using GestionERP.Web.Services.Apis.Principal; 
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage; 

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClientInterceptor();
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(Cnf.ServerApiURI),
    Timeout = TimeSpan.FromSeconds(100)
}.EnableIntercept(sp)
);

builder.Services.AddHttpClient("ReportService",
    httpClient =>
    {
        httpClient.BaseAddress = new Uri("https://localhost:7218/api/reports/");
        httpClient.Timeout = TimeSpan.FromSeconds(100);
        httpClient.DefaultRequestHeaders.Clear();
    }
);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IAuthentication, AuthenticationApi>();
builder.Services.AddScoped<IReport, ReportApi>();

#region Principal
builder.Services.AddScoped<IPrincipalActividadComercial, PrincipalActividadComercialApi>();
builder.Services.AddScoped<IPrincipalAduana, PrincipalAduanaApi>();
builder.Services.AddScoped<IPrincipalAlmacen, PrincipalAlmacenApi>();
builder.Services.AddScoped<IPrincipalArea, PrincipalAreaApi>();
builder.Services.AddScoped<IPrincipalArticulo, PrincipalArticuloApi>();
builder.Services.AddScoped<IPrincipalCuentaBalance, PrincipalCuentaBalanceApi>();
builder.Services.AddScoped<IPrincipalCargo, PrincipalCargoApi>();
builder.Services.AddScoped<IPrincipalCasillaBalance, PrincipalCasillaBalanceApi>();
builder.Services.AddScoped<IPrincipalChofer, PrincipalChoferApi>();
builder.Services.AddScoped<IPrincipalCierreContable, PrincipalCierreContableApi>();
builder.Services.AddScoped<IPrincipalGrupoArticulo, PrincipalGrupoArticuloApi>();
builder.Services.AddScoped<IPrincipalCuentaContable, PrincipalCuentaContableApi>();
builder.Services.AddScoped<IPrincipalDiario, PrincipalDiarioApi>();
builder.Services.AddScoped<IPrincipalDocumento, PrincipalDocumentoApi>();
builder.Services.AddScoped<IPrincipalEjercicio, PrincipalEjercicioApi>();
builder.Services.AddScoped<IPrincipalEmpresa, PrincipalEmpresaApi>();
builder.Services.AddScoped<IPrincipalEntidad, PrincipalEntidadApi>();
builder.Services.AddScoped<IPrincipalEstado, PrincipalEstadoApi>();
builder.Services.AddScoped<IPrincipalEvento, PrincipalEventoApi>();
builder.Services.AddScoped<IPrincipalFabricante, PrincipalFabricanteApi>();
builder.Services.AddScoped<IPrincipalFinanciera, PrincipalFinancieraApi>();
builder.Services.AddScoped<IPrincipalGlosarioAnalisis, PrincipalGlosarioAnalisisApi>();
builder.Services.AddScoped<IPrincipalGrupoDevengo, PrincipalGrupoDevengoApi>();
builder.Services.AddScoped<IPrincipalGrupoGastoImportacion, PrincipalGrupoGastoImportacionApi>();
builder.Services.AddScoped<IPrincipalSegmentoArticulo, PrincipalSegmentoArticuloApi>();
builder.Services.AddScoped<IPrincipalLocal, PrincipalLocalApi>();
builder.Services.AddScoped<IPrincipalMarca, PrincipalMarcaApi>();
builder.Services.AddScoped<IPrincipalMenu, PrincipalMenuApi>();
builder.Services.AddScoped<IPrincipalModulo, PrincipalModuloApi>();
builder.Services.AddScoped<IPrincipalMoneda, PrincipalMonedaApi>();
builder.Services.AddScoped<IPrincipalOperacionFinanciera, PrincipalOperacionFinancieraApi>();
builder.Services.AddScoped<IPrincipalOperacionLogistica, PrincipalOperacionLogisticaApi>();
builder.Services.AddScoped<IPrincipalOrigenAsiento, PrincipalOrigenAsientoApi>();
builder.Services.AddScoped<IPrincipalOrigenDocumento, PrincipalOrigenDocumentoApi>();
builder.Services.AddScoped<IPrincipalOrigenTransaccion, PrincipalOrigenTransaccionApi>();
builder.Services.AddScoped<IPrincipalPais, PrincipalPaisApi>();
builder.Services.AddScoped<IPrincipalPeriodo, PrincipalPeriodoApi>();
builder.Services.AddScoped<IPrincipalPermiso, PrincipalPermisoApi>();
builder.Services.AddScoped<IPrincipalPlanContable, PrincipalPlanContableApi>();
builder.Services.AddScoped<IPrincipalPlazoCredito, PrincipalPlazoCreditoApi>();
builder.Services.AddScoped<IPrincipalProcesoDocumento, PrincipalProcesoDocumentoApi>();
builder.Services.AddScoped<IPrincipalProductoEstado, PrincipalProductoEstadoApi>();
builder.Services.AddScoped<IPrincipalPuerto, PrincipalPuertoApi>();
builder.Services.AddScoped<IPrincipalRegimenImportacion, PrincipalRegimenImportacionApi>();
builder.Services.AddScoped<IPrincipalLineaArticulo, PrincipalLineaArticuloApi>();
builder.Services.AddScoped<IPrincipalServicio, PrincipalServicioApi>();
builder.Services.AddScoped<IPrincipalSituacionImportacion, PrincipalSituacionImportacionApi>();
builder.Services.AddScoped<IPrincipalTipoBienServicio, PrincipalTipoBienServicioApi>();
builder.Services.AddScoped<IPrincipalTipoComprobante, PrincipalTipoComprobanteApi>();
builder.Services.AddScoped<IPrincipalTipoIdentificacion, PrincipalTipoIdentificacionApi>();
builder.Services.AddScoped<IPrincipalMedioPago, PrincipalMedioPagoApi>();
builder.Services.AddScoped<IPrincipalModalidadTraslado, PrincipalModalidadTrasladoApi>();
builder.Services.AddScoped<IPrincipalMotivoTraslado, PrincipalMotivoTrasladoApi>();
builder.Services.AddScoped<IPrincipalFamiliaProducto, PrincipalFamiliaProductoApi>();
builder.Services.AddScoped<IPrincipalTipoAfectacion, PrincipalTipoAfectacionApi>();
builder.Services.AddScoped<IPrincipalTipoExistencia, PrincipalTipoExistenciaApi>();
builder.Services.AddScoped<IPrincipalTipoFinanciera, PrincipalTipoFinancieraApi>();
builder.Services.AddScoped<IPrincipalTipoNota, PrincipalTipoNotaApi>();
builder.Services.AddScoped<IPrincipalTipoOperacionAlmacen, PrincipalTipoOperacionAlmacenApi>();
builder.Services.AddScoped<IPrincipalTipoTributo, PrincipalTipoTributoApi>();
builder.Services.AddScoped<IPrincipalUnidadMedida, PrincipalUnidadMedidaApi>();
builder.Services.AddScoped<IPrincipalTipoAdjuntoCertificado, PrincipalTipoAdjuntoCertificadoApi>();
builder.Services.AddScoped<IPrincipalTipoAdjuntoRecepcion, PrincipalTipoAdjuntoRecepcionApi>();
builder.Services.AddScoped<IPrincipalTipoAlmacen, PrincipalTipoAlmacenApi>();
builder.Services.AddScoped<IPrincipalTipoAtencionVenta, PrincipalTipoAtencionVentaApi>();
builder.Services.AddScoped<IPrincipalTipoCambioDia, PrincipalTipoCambioDiaApi>();
builder.Services.AddScoped<IPrincipalTipoDetraccion, PrincipalTipoDetraccionApi>();
builder.Services.AddScoped<IPrincipalTipoDocumento, PrincipalTipoDocumentoApi>();
builder.Services.AddScoped<IPrincipalTipoDevengo, PrincipalTipoDevengoApi>();
builder.Services.AddScoped<IPrincipalTipoGastoImportacion, PrincipalTipoGastoImportacionApi>();
builder.Services.AddScoped<IPrincipalTipoGeneracion, PrincipalTipoGeneracionApi>();
builder.Services.AddScoped<IPrincipalTipoImportacion, PrincipalTipoImportacionApi>();
builder.Services.AddScoped<IPrincipalTipoImpuesto, PrincipalTipoImpuestoApi>();
builder.Services.AddScoped<IPrincipalTipoArticulo, PrincipalTipoArticuloApi>();
builder.Services.AddScoped<IPrincipalTipoMovimiento, PrincipalTipoMovimientoApi>();
builder.Services.AddScoped<IPrincipalTipoOperacionProveedor, PrincipalTipoOperacionProveedorApi>();
builder.Services.AddScoped<IPrincipalTipoProduccion, PrincipalTipoProduccionApi>();
builder.Services.AddScoped<IPrincipalTipoProvision, PrincipalTipoProvisionApi>();
builder.Services.AddScoped<IPrincipalTipoServicio, PrincipalTipoServicioApi>();
builder.Services.AddScoped<IPrincipalTipoTermino, PrincipalTipoTerminoApi>();
builder.Services.AddScoped<IPrincipalTipoTransaccion, PrincipalTipoTransaccionApi>();
builder.Services.AddScoped<IPrincipalTransporteImportacion, PrincipalTransporteImportacionApi>();
builder.Services.AddScoped<IPrincipalTipoFacturaNegociable, PrincipalTipoFacturaNegociableApi>();
builder.Services.AddScoped<IPrincipalTipoSituacionLetra, PrincipalTipoSituacionLetraApi>();
builder.Services.AddScoped<IPrincipalDistrito, PrincipalDistritoApi>();
builder.Services.AddScoped<IPrincipalProvincia, PrincipalProvinciaApi>();
builder.Services.AddScoped<IPrincipalRegion, PrincipalRegionApi>();
builder.Services.AddScoped<IPrincipalUsuario, PrincipalUsuarioApi>();
builder.Services.AddScoped<IPrincipalVehiculo, PrincipalVehiculoApi>();
builder.Services.AddScoped<IPrincipalZonaDespacho, PrincipalZonaDespachoApi>();
builder.Services.AddScoped<IPrincipalZonaVenta, PrincipalZonaVentaApi>();
builder.Services.AddScoped<IPrincipalCentroCosto, PrincipalCentroCostoApi>();
builder.Services.AddScoped<IPrincipalSerieDocumento, PrincipalSerieDocumentoApi>();
#endregion

#region Compra
builder.Services.AddScoped<ICompraSolicitud, CompraSolicitudApi>();
builder.Services.AddScoped<ICompraOrden, CompraOrdenApi>();
#endregion

#region Importacion
builder.Services.AddScoped<IImportacionNotaReporteOrden, ImportacionNotaReporteOrdenApi>();
builder.Services.AddScoped<IImportacionOrden, ImportacionOrdenApi>();
builder.Services.AddScoped<IImportacionPedido, ImportacionPedidoApi>();
builder.Services.AddScoped<IImportacionSolicitud, ImportacionSolicitudApi>();
#endregion

#region Servicio
builder.Services.AddScoped<IServicioContrato, ServicioContratoApi>();
builder.Services.AddScoped<IServicioSolicitud, ServicioSolicitudApi>();
builder.Services.AddScoped<IServicioOrden, ServicioOrdenApi>();
#endregion

#region Almacen
builder.Services.AddScoped<IAlmacenMovimiento, AlmacenMovimientoApi>();
#endregion

#region Produccion
builder.Services.AddScoped<IProduccionEquipo, ProduccionEquipoApi>();
builder.Services.AddScoped<IProduccionFuncion, ProduccionFuncionApi>();
builder.Services.AddScoped<IProduccionOrden, ProduccionOrdenApi>();
builder.Services.AddScoped<IProduccionPersonal, ProduccionPersonalApi>();
builder.Services.AddScoped<IProduccionPlan, ProduccionPlanApi>();
builder.Services.AddScoped<IProduccionSolicitud, ProduccionSolicitudApi>();
builder.Services.AddScoped<IProduccionVersionPlan, ProduccionVersionPlanApi>();
#endregion

#region Archivo
builder.Services.AddScoped<IArchivoPrincipalEmpresa, ArchivoPrincipalEmpresaApi>();
#endregion

builder.Services.AddScoped<RefreshTokenService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UtilService>();

builder.Services.AddTelerikBlazor();
builder.Services.AddOptions();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthenticationCore();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.AddScoped<HttpInterceptorService>();

if (builder.Environment.IsProduction())
{ 
    builder.Services.AddLogging(logging =>
    { 
        logging.SetMinimumLevel(LogLevel.Error);
        logging.ClearProviders(); 
    });
}

builder.Services.AddSingleton<ITelerikStringLocalizer, ResxLocalizer>();
builder.Services.AddSingleton<ITelerikReportingStringLocalizer, ReportingResxLocalizer>();
builder.Services.AddLocalization();
builder.Services.AddAuthorization();
builder.Services.AddScoped<ProtectedLocalStorage>();

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

string result = "es-MX"; //await localStorage.GetItemAsync<string>("BlazorCulture");
if (result != null)
{
    CultureInfo culture = new(result);
    CultureInfo.DefaultThreadCurrentCulture = culture;
    CultureInfo.DefaultThreadCurrentUICulture = culture;
}

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode().AddAdditionalAssemblies().AllowAnonymous();
 
await app.RunAsync(); 
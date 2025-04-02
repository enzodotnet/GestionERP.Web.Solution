using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Requests;
using GestionERP.Web.Models.Dtos.Importacion;
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components;
using GestionERP.Web.Services;
using System.Security.Claims;
using Telerik.Blazor;
using Telerik.Blazor.Components;
using Telerik.SvgIcons;
using Microsoft.AspNetCore.Components.Routing;
using GestionERP.Web.Global; 

namespace GestionERP.Web.Pages.Empresa.Importacion.Pedido;

public partial class View : IDisposable
{
    private const string codigoModulo = "02";
    private const string codigoServicio = "S106";
    private const string rutaServicio = "/pedidos";
    private string rutaEmpresa = ""; 

    public PedidoObtenerDto Pedido { get; set; }
    public PedidoDetalleObtenerDto Detalle { get; set; }
	public EstadoActualizarRequest EstadoActualizar { get; set; }
    public TelerikNotification AlertAccionDialog { get; set; }
	public bool EsVisibleModalDetalle { get; set; }
    public ISvgIcon IconoAccionModal { get; set; }
    public string VerboAccionDialog { get; set; }
    public string TituloAccionDialog { get; set; }
    public string TituloAccionLoading { get; set; }
    public string ResultadoAccion { get; set; } 
    private bool EsVisibleAccionDialog { get; set; } 
    private bool IsAuthUser { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool EsEstadoActualizar { get; set; }
    private bool EsAsignadoEliminar { get; set; }
    private bool EsVisibleEliminar { get; set; }
	private bool EsAsignadoCerrar { get; set; }
	private bool EsVisibleCerrar { get; set; }
	private bool EsAsignadoAperturar { get; set; }
	private bool EsVisibleAperturar { get; set; }
	private bool EsAsignadoCostear { get; set; }
	private bool EsVisibleCostear { get; set; }
	private bool EsAsignadoRetirarCosteo { get; set; }
	private bool EsVisibleRetirarCosteo { get; set; }
	private bool EsAsignadoEnviarIngreso { get; set; } 
	private bool EsAsignadoRevertirEnvioIngreso { get; set; } 
	private bool EsAsignadoExcluir { get; set; }
    private bool EsVisibleExcluir { get; set; }
    private bool EsAsignadoAnular { get; set; } 
    private bool EsAsignadoEditar { get; set; }
    private bool EsVisibleEditar { get; set; } 
	private bool EsAsignadoVerReporte { get; set; } 
    private bool EsAsignadoVerAuditoria { get; set; }
    private string CodigoEstadoAccion { get; set; }
    private MonedaConsultaPorTipoDto MN { get; set; }
    private MonedaConsultaPorTipoDto ME { get; set; }
    public IEnumerable<PedidoFlag> TiposFinanciamiento { get; set; }
    public IEnumerable<PedidoFlag> ViasTransporte { get; set; }
    public IEnumerable<PedidoFlag> Canales { get; set; }
    public IEnumerable<PedidoFlag> ModalidadesEmbarque { get; set; }
	public IEnumerable<PedidoFlag> EstadosIngreso { get; set; }
	[CascadingParameter] public DialogFactory Dialog { get; set; }
    public EmpresaConsultaPorCodigoWebDto Empresa { get; set; }
    [Parameter] public string CodigoWebEmpresa { get; set; } 
    [Parameter] public Guid? Id { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }

    [Inject] public IImportacionPedido IPedido { get; set; }
    [Inject] public IPrincipalEmpresa IEmpresa { get; set; }
    [Inject] public IPrincipalMoneda IMoneda { get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Notify.ShowLoading(mensaje: "Obteniendo registro");

            EstadoActualizar = new();
            ViasTransporte = PedidoFlag.ViasTransporte();
            ModalidadesEmbarque = PedidoFlag.ModalidadesEmbarque();
            Canales = PedidoFlag.Canales();
            TiposFinanciamiento = PedidoFlag.TiposFinanciamiento();
			EstadosIngreso = PedidoFlag.EstadosIngreso();

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio);
            if (!IsAuthUser) return;

            Empresa = await IEmpresa.ConsultaPorCodigoWeb(CodigoWebEmpresa);
            rutaEmpresa = INavigation.Uri.Replace(INavigation.BaseUri, "").Split("?")[0].Replace(rutaServicio, "").Replace($"/{Id}", "");

			ME = await IMoneda.ConsultaPorTipo("ME");
			MN = await IMoneda.ConsultaPorTipo("MN");

			await ObtenerPedido();
            if (Pedido is null)
            {
                INavigation.NavigateTo($"{rutaEmpresa}{rutaServicio}");
                Notify.Show($"El registro del [Pedido de Importación] a visualizar no está disponible", "error");
                return;
            }
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

    private async Task ObtenerPedido() 
    { 
        Pedido = await IPedido.Obtener(Empresa.Codigo, (Guid)Id);
        if (Pedido is not null)
        {
            ValidarEstadoOptionButtons(Pedido.CodigoEstado);
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
                await IPedido.Eliminar(Empresa.Codigo, (Guid) Id);
                IsLoadingAction = false;
                INavigation.NavigateTo($"{rutaEmpresa}{rutaServicio}");
            }
            else
            { 
                await IPedido.ActualizarEstado(Empresa.Codigo, EstadoActualizar);
                await ObtenerPedido();
                CerrarDialog();
            }

            Notify.Show($"El pedido de importación {Pedido.Codigo} ha sido {ResultadoAccion} con éxito", "success");
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

    private async Task ValidarPermisoOptionButtons()
    {
        EsAsignadoEditar = await IPermiso.ConsultaEsAsignadoPorSesion(PedidoAcceso.Editar, Empresa.Codigo);
        EsAsignadoEliminar = await IPermiso.ConsultaEsAsignadoPorSesion(PedidoAcceso.Eliminar, Empresa.Codigo);
        EsAsignadoExcluir = await IPermiso.ConsultaEsAsignadoPorSesion(PedidoAcceso.Excluir, Empresa.Codigo);
        EsAsignadoAnular = await IPermiso.ConsultaEsAsignadoPorSesion(PedidoAcceso.Anular, Empresa.Codigo);
		EsAsignadoCerrar = await IPermiso.ConsultaEsAsignadoPorSesion(PedidoAcceso.Cerrar, Empresa.Codigo);
		EsAsignadoAperturar = await IPermiso.ConsultaEsAsignadoPorSesion(PedidoAcceso.Aperturar, Empresa.Codigo);
		EsAsignadoCostear = await IPermiso.ConsultaEsAsignadoPorSesion(PedidoAcceso.Costear, Empresa.Codigo);
		EsAsignadoRetirarCosteo = await IPermiso.ConsultaEsAsignadoPorSesion(PedidoAcceso.RetirarCosteo, Empresa.Codigo);
		EsAsignadoEnviarIngreso = await IPermiso.ConsultaEsAsignadoPorSesion(PedidoAcceso.EnviarIngreso, Empresa.Codigo);
		EsAsignadoRevertirEnvioIngreso = await IPermiso.ConsultaEsAsignadoPorSesion(PedidoAcceso.RevertirEnvioIngreso, Empresa.Codigo);
		EsAsignadoVerReporte = await IPermiso.ConsultaEsAsignadoPorSesion(PedidoAcceso.VerReporte, Empresa.Codigo); 
        EsAsignadoVerAuditoria = await IPermiso.ConsultaEsAsignadoPorSesion(PedidoAcceso.VerAuditoria, Empresa.Codigo);
    }

    private void ValidarEstadoOptionButtons(string codigoEstado)
    {
        EsVisibleEditar = codigoEstado is "ED";
        EsVisibleExcluir = codigoEstado is "AX";
        EsVisibleEliminar = EsVisibleExcluir || codigoEstado is "EX";
        EsVisibleCerrar = codigoEstado is "ED";
        EsVisibleAperturar = codigoEstado is "CO";
        EsVisibleCostear = codigoEstado is "CO";
        EsVisibleRetirarCosteo = codigoEstado is "CE"; 
    }
     
    private void VerItemDetalle(PedidoDetalleObtenerDto item)
    {
        Detalle = item;
        VisibleDetalleChangedHandler(true);
    }

	private void VisibleDetalleChangedHandler(bool esVisible) => EsVisibleModalDetalle = esVisible;

    private void Volver() => INavigation.NavigateTo($"{rutaEmpresa}{rutaServicio}");

    private void MostrarAccionDialog(string codigoEstado = "")
    {
        CodigoEstadoAccion = codigoEstado;
        EsEstadoActualizar = !string.IsNullOrEmpty(codigoEstado);
		IconoAccionModal = CodigoEstadoAccion switch
		{
			"AX" => SvgIcon.XOutline,
			"EX" => SvgIcon.MinusOutline,
			"EI" => SvgIcon.RotateRight,
            "RI" => SvgIcon.RotateLeft,
			"CO" => SvgIcon.Lock,
			"AU" => SvgIcon.Unlock,
			"CE" => SvgIcon.Dollar,
			"RE" => SvgIcon.DecimalIncrease,
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
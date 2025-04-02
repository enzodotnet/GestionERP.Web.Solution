using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Requests;
using GestionERP.Web.Models.Dtos.Produccion;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components;
using GestionERP.Web.Services;
using System.Security.Claims;  
using GestionERP.Web.Models.Dtos.Principal;
using Telerik.Blazor;
using Telerik.Blazor.Components;
using Telerik.SvgIcons; 
using Microsoft.AspNetCore.Components.Routing;
using GestionERP.Web.Global;
using Microsoft.AspNetCore.Components.Forms; 

namespace GestionERP.Web.Pages.Empresa.Produccion.Orden;

public partial class View : IDisposable
{
    private const string codigoModulo = "05";
    private const string codigoServicio = "S112";
    private const string rutaServicio = "/ordenes";
    private string rutaEmpresa = "";

    public OrdenObtenerDto Orden { get; set; }
    public EstadoActualizarRequest EstadoActualizar { get; set; }
    public OrdenActualizarComentarioProcesoDto ComentarioActualizar { get; set; }
    public bool EsVisibleModalActualizarComentario { get; set; }
    public bool IsModifiedComentarioProceso { get; set; }
    public TelerikNotification AlertModalActualizarComentario { get; set; }
    public OrdenActualizarFechaCostoDto FechaCostoActualizar { get; set; }
    public bool EsVisibleModalActualizarFechaCosto { get; set; }
    public bool IsModifiedFechaCosto { get; set; }
    private EditContext ComentarioActualizarContext { get; set; }
    private EditContext FechaCostoActualizarContext { get; set; }
    public SerieDocumentoConsultaPorCodigoEmpresaDto Numerador { get; set; }
    public TelerikNotification AlertAccionDialog { get; set; } 
    public ISvgIcon IconoAccionModal { get; set; }
    public string VerboAccionDialog { get; set; }
    public string TituloAccionDialog { get; set; }
    public string TituloAccionLoading { get; set; }
    public string ResultadoAccion { get; set; }  
    private bool EsVisibleAccionDialog { get; set; }
    public bool EsVisibleListaMateriales { get; set; }
    public bool EsVisibleListaTareos { get; set; }
    public bool EsVisibleListaMaquilas { get; set; }
    public bool EsVisibleListaLotes { get; set; }
    private bool IsAuthUser { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool EsEstadoActualizar { get; set; }
    private bool EsAsignadoEliminar { get; set; }
    private bool EsVisibleEliminar { get; set; }
    private bool EsAsignadoExcluir { get; set; }
    private bool EsVisibleExcluir { get; set; }
    private bool EsVisibleAnular { get; set; }
    private bool EsAsignadoAnular { get; set; } 
    private bool EsAsignadoEditar { get; set; }
    private bool EsVisibleEditar { get; set; } 
    private bool EsVisibleEnviarAprobacion { get; set; }
    private bool EsAsignadoEnviarAprobacion { get; set; }
    private bool EsAsignadoRevertirEnvioAprobacion { get; set; }
    private bool EsVisibleRevertirEnvioAprobacion { get; set; }
    private bool EsAsignadoEnviarProceso { get; set; }
    private bool EsVisibleEnviarProceso { get; set; }
    private bool EsAsignadoRevertirEnvioProceso { get; set; }
    private bool EsVisibleRevertirEnvioProceso { get; set; }
    private bool EsAsignadoEnviarIngreso { get; set; }
    private bool EsAsignadoRevertirEnvioIngreso { get; set; }
    private bool EsAsignadoCerrar { get; set; }
    private bool EsVisibleCerrar { get; set; }
    private bool EsAsignadoAperturar { get; set; }
    private bool EsVisibleAperturar { get; set; }
    private bool EsAsignadoCostear { get; set; }
    private bool EsVisibleCostear { get; set; }
    private bool EsAsignadoRetirarCosteo { get; set; }
    private bool EsVisibleRetirarCosteo { get; set; }
    private bool EsAsignadoVerReporte { get; set; }
    private bool EsAsignadoVerAuditoria { get; set; } 
    private string CodigoEstadoAccion { get; set; }
    public string VerboAccionModal { get; set; }
    public string DescripcionAccionModal { get; set; }
    public string TipoAccionModal { get; set; }
    public string TipoAccionDialog { get; set; }
    public string OrigenDialog { get; set; }
    public string OrigenModal { get; set; }
    private bool IsLoadingActualizar { get; set; } 
    public TipoProduccionConsultaPorCodigoDto TipoProduccion { get; set; }
    private IEnumerable<OrdenFlag> Origenes { get; set; }
    private IEnumerable<OrdenFlag> TiposRegistro { get; set; }
    private IEnumerable<VersionPlanFlag> TiposMaterial { get; set; }
    private IEnumerable<VersionPlanFlag> InsumosMaterial { get; set; }
    private IEnumerable<OrdenFlag> EstadosIngreso { get; set; }
    private IEnumerable<OrdenFlag> EstadosTransferencia { get; set; }
    private IEnumerable<OrdenFlag> EstadosConsumo { get; set; }
    private MonedaConsultaPorTipoDto MN { get; set; }
    private MonedaConsultaPorTipoDto ME { get; set; }
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    public EmpresaConsultaPorCodigoWebDto Empresa { get; set; }
    [Parameter] public string CodigoWebEmpresa { get; set; } 
    [Parameter] public Guid? Id { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }

    [Inject] public IProduccionOrden IOrden { get; set; }
    [Inject] public IPrincipalTipoProduccion ITipoProduccion { get; set; }
    [Inject] public IPrincipalSerieDocumento ISerieDocumento { get; set; }
    [Inject] public IPrincipalMoneda IMoneda { get; set; }
    [Inject] public IPrincipalEmpresa IEmpresa { get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Notify.ShowLoading(mensaje: "Obteniendo registro");

            EstadoActualizar = new();
            Origenes = OrdenFlag.Origenes();
            TiposRegistro = OrdenFlag.TiposRegistro();
            TiposMaterial = VersionPlanFlag.TiposMaterial();
            InsumosMaterial = VersionPlanFlag.InsumosMaterial();
            EstadosConsumo = OrdenFlag.EstadosConsumo();
            EstadosIngreso = OrdenFlag.EstadosIngreso();
            EstadosTransferencia = OrdenFlag.EstadosTransferencia();

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio);
            if (!IsAuthUser) return;

            Empresa = await IEmpresa.ConsultaPorCodigoWeb(CodigoWebEmpresa);
            rutaEmpresa = INavigation.Uri.Replace(INavigation.BaseUri, "").Split("?")[0].Replace(rutaServicio, "").Replace($"/{Id}", "");

            ME = await IMoneda.ConsultaPorTipo("ME");
            MN = await IMoneda.ConsultaPorTipo("MN");

            await ObtenerOrden();
            if (Orden is null)
            {
                INavigation.NavigateTo($"{rutaEmpresa}{rutaServicio}");
                Notify.Show($"El registro de la [Orden de Producción] a visualizar no está disponible", "error");
                return;
            }

            TipoProduccion = await ITipoProduccion.ConsultaPorCodigo(Orden.CodigoTipoProduccion);
            await CargarConsultaSerieDocumento();
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

    private async Task ObtenerOrden() 
    { 
        Orden = await IOrden.Obtener(Empresa.Codigo, (Guid)Id);
        if (Orden is not null)
        {
            ValidarEstadoOptionButtons(Orden.CodigoEstado);
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
                await IOrden.Eliminar(Empresa.Codigo, (Guid) Id);
                IsLoadingAction = false;
                INavigation.NavigateTo($"{rutaEmpresa}{rutaServicio}");
            }
            else
            { 
                await IOrden.ActualizarEstado(Empresa.Codigo, EstadoActualizar);
                await ObtenerOrden();
                CerrarDialog();
            }
            
            Notify.Show($"La orden de producción {Orden.Codigo} ha sido {ResultadoAccion} con éxito", "success");
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

    private async Task CargarConsultaSerieDocumento()
    {
        Numerador = await ISerieDocumento.ConsultaPorCodigoEmpresa(Orden.CodigoSerieDocumento, Orden.CodigoDocumento, Empresa.Codigo);
    }

    private async Task ValidarPermisoOptionButtons()
    {
        EsAsignadoEditar = await IPermiso.ConsultaEsAsignadoPorSesion(OrdenAcceso.Editar, Empresa.Codigo);
        EsAsignadoEliminar = await IPermiso.ConsultaEsAsignadoPorSesion(OrdenAcceso.Eliminar, Empresa.Codigo);
        EsAsignadoExcluir = await IPermiso.ConsultaEsAsignadoPorSesion(OrdenAcceso.Excluir, Empresa.Codigo);
        EsAsignadoAnular = await IPermiso.ConsultaEsAsignadoPorSesion(OrdenAcceso.Anular, Empresa.Codigo);
        EsAsignadoVerReporte = await IPermiso.ConsultaEsAsignadoPorSesion(OrdenAcceso.VerReporte, Empresa.Codigo);
        EsAsignadoVerAuditoria = await IPermiso.ConsultaEsAsignadoPorSesion(OrdenAcceso.VerAuditoria, Empresa.Codigo);
        EsAsignadoEnviarAprobacion = await IPermiso.ConsultaEsAsignadoPorSesion(OrdenAcceso.EnviarAprobacion, Empresa.Codigo);
        EsAsignadoRevertirEnvioAprobacion = await IPermiso.ConsultaEsAsignadoPorSesion(OrdenAcceso.RevertirEnvioAprobacion, Empresa.Codigo);
        EsAsignadoEnviarProceso = await IPermiso.ConsultaEsAsignadoPorSesion(OrdenAcceso.EnviarProceso, Empresa.Codigo);
        EsAsignadoRevertirEnvioProceso = await IPermiso.ConsultaEsAsignadoPorSesion(OrdenAcceso.RevertirEnvioProceso, Empresa.Codigo);
        EsAsignadoEnviarIngreso = await IPermiso.ConsultaEsAsignadoPorSesion(OrdenAcceso.EnviarIngreso, Empresa.Codigo);
        EsAsignadoRevertirEnvioIngreso = await IPermiso.ConsultaEsAsignadoPorSesion(OrdenAcceso.RevertirEnvioIngreso, Empresa.Codigo);
        EsAsignadoCerrar = await IPermiso.ConsultaEsAsignadoPorSesion(OrdenAcceso.Cerrar, Empresa.Codigo);
        EsAsignadoAperturar = await IPermiso.ConsultaEsAsignadoPorSesion(OrdenAcceso.Aperturar, Empresa.Codigo);
        EsAsignadoCostear = await IPermiso.ConsultaEsAsignadoPorSesion(OrdenAcceso.Costear, Empresa.Codigo);
        EsAsignadoRetirarCosteo = await IPermiso.ConsultaEsAsignadoPorSesion(OrdenAcceso.RetirarCosteo, Empresa.Codigo);
    }

    private void ValidarEstadoOptionButtons(string codigoEstado)
    {
        EsVisibleEditar = codigoEstado is "ED";
        EsVisibleExcluir = codigoEstado is "AX" or "DX" or "CX";
        EsVisibleEliminar = EsVisibleExcluir || codigoEstado is "EX";
        EsVisibleEnviarAprobacion = EsVisibleAnular = codigoEstado is "ED";
        EsVisibleRevertirEnvioAprobacion = codigoEstado is "EA";
        EsVisibleEnviarProceso = codigoEstado is "AP";
        EsVisibleRevertirEnvioProceso = codigoEstado is "EP";
        EsVisibleCerrar = codigoEstado is "EP";
        EsVisibleAperturar = codigoEstado is "CO";
        EsVisibleCostear = codigoEstado is "CO";
        EsVisibleRetirarCosteo = codigoEstado is "CE";
    }

    private void Volver() => INavigation.NavigateTo($"{rutaEmpresa}{rutaServicio}");

    private void MostrarAccionDialog(string codigoEstado = "")
    {
        CodigoEstadoAccion = codigoEstado;
        EsEstadoActualizar = !string.IsNullOrEmpty(codigoEstado);

		IconoAccionModal = CodigoEstadoAccion switch
		{
			"AX" => SvgIcon.XOutline,
			"EX" => SvgIcon.MinusOutline,
            "EA" or "EP" or "EI" => SvgIcon.RotateRight,
            "RC" or "RP" or "RI" => SvgIcon.RotateLeft,
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

    private void VisibleListaMaterialChangedHandler(bool esVisible) => EsVisibleListaMateriales = esVisible;

    private void VisibleListaTareoChangedHandler(bool esVisible) => EsVisibleListaTareos = esVisible;

    private void VisibleListaMaquilaChangedHandler(bool esVisible) => EsVisibleListaMaquilas = esVisible;

    private void VisibleListaLoteChangedHandler(bool esVisible) => EsVisibleListaLotes = esVisible;


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
            case "E":
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
                //if (tipoAccion is "I" or "M")
                //    DetalleInsertarContext = new EditContext(DetalleInsertar);
                //else if (tipoAccion is "E")
                //    DetalleEditarContext = new EditContext(DetalleEditar);
                //EsVisibleModalDetalle = true;
                break;
        }
    }

    public void CerrarModal()
    {
        if (OrigenModal is "comentarioProceso")
            IsModifiedComentarioProceso = EsVisibleModalActualizarComentario = false;
        
        StateHasChanged();
    }

    #region ComentarioProceso
    public void MostrarActualizarComentarioProceso()
    {
        ComentarioActualizar = new()
        {
            OrdenId = Orden.Id,
            ComentarioProceso = Orden.ComentarioProceso
        };
        OrigenModal = "comentarioProceso";
        ComentarioActualizarContext = new EditContext(ComentarioActualizar);
        EsVisibleModalActualizarComentario = true;
    }

    private async Task CerrarActualizacionComentario()
    {
        if (!IsModifiedComentarioProceso || await Dialog.ConfirmAsync("¿Está seguro de cerrar el formulario y que el comentario no se grabe?", "Deshaciendo actualización"))
             CerrarModal();
    }

    private void OnChangeComentarioProcesoHandler(object value) => IsModifiedComentarioProceso = Fnc.VerifyContextIsChanged(Orden.ComentarioProceso != (string)value, ComentarioActualizarContext, "ComentarioProceso");

    private async Task ActualizarComentarioProceso()
    {
        try
        {
            IsLoadingAction = true;
            IsAuthUser = (await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa, codigoModulo, codigoServicio, User.FindFirst("code").Value)).esValido;
            if (!IsAuthUser)
            {
                CerrarModal();
                return;
            }

            TituloAccionLoading = "Actualizando en progreso";
            IsLoadingActualizar = true;
            StateHasChanged();

            await IOrden.ActualizarComentarioProceso(Empresa.Codigo, ComentarioActualizar);
            await ObtenerOrden();
            CerrarModal();

            Notify.Show("El comentario del proceso ha sido actualizado con éxito", "success");
        } 
        catch (Exception ex)
        {
            if (ex is HttpRequestException)
                Fnc.MostrarAlerta(AlertModalActualizarComentario, Cnf.MsgErrorNotConnectApi, "error");
            else if (ex is HttpResponseException)
            { 
                string codeError = (ex as HttpResponseException).Code;
                if ((ex as HttpResponseException).Code == "AU")
                {
                    CerrarModal();
                    Notify.ShowError(codeError, ex);
                }
                else
                {
                    Fnc.MostrarAlerta(AlertModalActualizarComentario, ex.Message, "error");
                }
            } 
            else
                Fnc.MostrarAlerta(AlertModalActualizarComentario, Cnf.MsgErrorFuncAppWeb, "error");
        }
        finally
        {
            IsLoadingAction = IsLoadingActualizar = false;
        }
    } 
    #endregion

    private void IrEditar() => INavigation.NavigateTo(INavigation.GetUriWithQueryParameters($"{rutaEmpresa}{rutaServicio}/{Id}/editar", new Dictionary<string, object> { ["returnpage"] = "view" }));

    private void IrReporte() => INavigation.NavigateTo($"{rutaEmpresa}{rutaServicio}/{Id}/reporte");

    public void Dispose() => GC.SuppressFinalize(this);
}
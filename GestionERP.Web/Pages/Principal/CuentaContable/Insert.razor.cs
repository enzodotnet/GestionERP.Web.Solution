using AutoMapper;
using Blazored.FluentValidation;
using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Models.Dtos.Principal.Types;
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
using GestionERP.Web.Global;

namespace GestionERP.Web.Pages.Principal.CuentaContable;

public partial class Insert : IDisposable
{
    #region Propiedades
    private const string codigoServicio = "S004";
    public FluentValidationValidator validator;
    public CuentaContableInsertarDto CuentaContableInsertar { get; set; }
    public CuentaContableDetalleInsertarDto DetalleInsertar { get; set; }
    public CuentaContableDestinoInsertarDto DestinoInsertar { get; set; }
    public List<CuentaContableDestinoObtenerDto> ListaDestinos { get; set; }
    public List<CuentaContableDetalleObtenerDto> ListaDetalle { get; set; }
    public CuentaContableObtenerDto CuentaContableObtener { get; set; }
    public CuentaContableDetalleObtenerDto Detalle { get; set; }
    public CuentaContableDestinoObtenerDto DestinoObtener { get; set; }
    public bool EsVisibleModalDetalle { get; set; }
    public bool EsVisibleModalDestino { get; set; }
    public TelerikNotification AlertDetalle { get; set; }
    public TelerikNotification AlertDestino { get; set; }
    public TelerikNotification Alert { get; set; }
    public string VerboAccionModal { get; set; }
    public string DescripcionAccionModal { get; set; }
    public string TipoAccionModal { get; set; }
    public string TipoAccionDialog { get; set; }
    public string OrigenDialog { get; set; }
    public string OrigenModal { get; set; }
    public bool EsVisibleDialogDetalle { get; set; }
    public bool EsVisibleDialogDestino { get; set; }
    public string CodigoItemAccion { get; set; }
    public ISvgIcon IconoAccionModal { get; set; }
    private IEnumerable<CuentaContableTipoCuentaCorrienteType> TiposCuentaCorriente { get; set; }
    private IEnumerable<CuentaContableTipoEntidadFinancieraType> TiposEntidadFinanciera { get; set; }
    private IEnumerable<CuentaContableTipoNaturalezaType> TiposNaturaleza { get; set; }
    private IEnumerable<CuentaContableTipoType> Tipos { get; set; }
    private IEnumerable<CuentaContableFormatoFuncionType> FormatosFuncion { get; set; }
    private IEnumerable<CuentaContableDetalleTipoType> DetalleTipos { get; set; }
    private IEnumerable<CuentaContableDestinoTipoType> DestinoTipos { get; set; }
    private IEnumerable<CuentaContableDetalleTipoCambioType> DetalleTiposCambio { get; set; }
    private IEnumerable<CuentaContableDetalleRegistroType> DetalleRegistros { get; set; }
    private TelerikGrid<CuentaContableDetalleObtenerDto> GridDetalleRef { get; set; }
    private TelerikGrid<CuentaContableDestinoObtenerDto> GridDestinoRef { get; set; }
    private EditContext EditContext { get; set; }
    private EditContext EditContextDestino { get; set; }
    private EditContext DetalleContext { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool IsAuthUser { get; set; }
    private bool IsModified { get; set; }
    private bool IsModifiedDestino { get; set; }
    private bool IsModifiedDetalle { get; set; }
    private bool EsVisibleVolver { get; set; }
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    [Parameter][SupplyParameterFromQuery(Name = "returnpage")] public string ReturnPage { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    #endregion

    [Inject] public IPrincipalCuentaContable ICuentaContable { get; set; } 
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }
    [Inject] public IMapper IMapper { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            CuentaContableObtener = new();
            CuentaContableInsertar = new()
            {
                Detalles = [],
                Destinos = []
            };
            DetalleInsertar = new();
            DestinoInsertar = new();
            ListaDestinos = [];
            ListaDetalle = [];
            Detalle = new();
            DestinoObtener = new();

            TiposCuentaCorriente = CuentaContableTipoCuentaCorrienteType.ObtenerTipos();
            TiposEntidadFinanciera = CuentaContableTipoEntidadFinancieraType.ObtenerTipos();
            TiposNaturaleza = CuentaContableTipoNaturalezaType.ObtenerTipos();
            Tipos = CuentaContableTipoType.ObtenerTipos();
            FormatosFuncion = CuentaContableFormatoFuncionType.ObtenerTipos();
            DetalleTipos = CuentaContableDetalleTipoType.ObtenerTipos();
            DetalleTiposCambio = CuentaContableDetalleTipoCambioType.ObtenerTipos();
            DetalleRegistros = CuentaContableDetalleRegistroType.ObtenerTipos();
            DestinoTipos = CuentaContableDestinoTipoType.ObtenerTipos();

            EditContext = new EditContext(CuentaContableInsertar);

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio);
            if (!IsAuthUser) return;
                
            EsVisibleVolver = !string.IsNullOrEmpty(ReturnPage) && ReturnPage is "index";

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(CuentaContableAcceso.Insertar))
            {
                INavigation.NavigateTo("cuentas-contables");
                Notify.Show("No tiene permiso para insertar registros de [Cuentas Contables]", "error");
            }
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
    }

    private async Task Insertar()
    {
        try
        {
            IsLoadingAction = true;

            IsAuthUser = (await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio, codigoUser: User.FindFirst("code").Value)).esValido;
            if (!IsAuthUser) return;
            if (!EditContext.Validate())
            {
                Fnc.MostrarAlerta(Alert, Cnf.MsgErrorInvalidEditContext, "error");
                return;
            }

            Notify.ShowLoading(mensaje: "Inserción en progreso");

            Guid id = await ICuentaContable.Insertar(CuentaContableInsertar);
            IsModified = false;
            Notify.Show("La cuenta contable ha sido insertada con éxito", "success");
            INavigation.NavigateTo($"cuentas-contables/{id}");
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
        if (IsAuthUser && IsModified && !await Dialog.ConfirmAsync("¿Está seguro de salir del formulario de insertar sin haber guardado?", "Saliendo del formulario"))
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
            case "destino":
                if (tipoAccion is "I" or "M") 
                    EditContextDestino = new EditContext(DestinoInsertar);  
                EsVisibleModalDestino = true;
                break;
        }
    }

    private void IniciarAccionDialog(string tipoAccion, string origenDialog)
    {
        TipoAccionDialog = tipoAccion;
        OrigenDialog = origenDialog;
        if(origenDialog is "detalle")
            EsVisibleDialogDetalle = true;
        else if(origenDialog is "destino")
            EsVisibleDialogDestino = true;
    }

    public void CerrarModal()
    {
        if (OrigenModal is "detalle")
            IsModifiedDetalle = EsVisibleModalDetalle = false;
        else if (OrigenModal is "destino")
            IsModifiedDestino = EsVisibleModalDestino = false; 
        VerboAccionModal = DescripcionAccionModal = TipoAccionModal = CodigoItemAccion = "";
        StateHasChanged();
    }

    private void Volver() => INavigation.NavigateTo("cuentas-contables");

    public void CerrarDialog()
    {
        if (OrigenDialog is "detalle")
            EsVisibleDialogDetalle = false;
        else if (OrigenDialog is "destino")
            EsVisibleDialogDestino = false;
        TipoAccionDialog = OrigenDialog = "";
        StateHasChanged();
    }

    #region Detalle
    private async Task CerrarDetalle()
    {
        if (TipoAccionModal is not "V" && IsModifiedDetalle && !await Dialog.ConfirmAsync("¿Está seguro de cerrar el formulario y que los datos no se carguen?", "Saliendo del formulario"))
            return;
        CerrarModal();
    }

    public void MostrarAgregarDetalle()
    {
        DetalleInsertar = new();
        Detalle = new();
        IniciarAccionModal("I", "detalle");
    }

    public void MostrarModificarDetalle(CuentaContableDetalleObtenerDto item)
    {
        DetalleInsertar = IMapper.Map<CuentaContableDetalleInsertarDto>(item);
        CodigoItemAccion = item.FlagRegistro;
        Detalle = item;
        IniciarAccionModal("M", "detalle");
    }

    private void MostrarQuitarDetalle(string codigoItem)
    {
        CodigoItemAccion = codigoItem;
        IniciarAccionDialog("Q", "detalle");
    }

    public void VerItemDetalle(CuentaContableDetalleObtenerDto item)
    {
        Detalle = item;
        CodigoItemAccion = item.FlagRegistro;
        IniciarAccionModal("V", "detalle");
    }

    private bool VerificarItemDetalleEsValido()
    {
        foreach (CuentaContableDetalleInsertarDto item in CuentaContableInsertar.Detalles.Where(x => x.FlagRegistro != CodigoItemAccion))
        {
            if (item.FlagRegistro.Trim() == DetalleInsertar.FlagRegistro.Trim())
            {
                Fnc.MostrarAlerta(AlertDetalle, "El valor del campo [Registro] ya existe en el listado del Detalle", "warning");
                return false;
            }
        }
        return true;
    }

    private async Task AccionarDetalle(string tipoAccion)
    {
        GridState<CuentaContableDetalleObtenerDto> detalleState = GridDetalleRef.GetState();
        int index = ListaDetalle.FindIndex(i => i.FlagRegistro == CodigoItemAccion);

        switch (tipoAccion)
        {
            case "I" or "M":
                if (VerificarItemDetalleEsValido())
                    return;
                IMapper.Map(DetalleInsertar, Detalle);
                if (tipoAccion == "I")
                {
                    CuentaContableInsertar.Detalles.Add(DetalleInsertar);
                    ListaDetalle.Add(Detalle);
                    detalleState.InsertedItem = Detalle;
                }
                else
                {
                    CuentaContableInsertar.Detalles[index] = DetalleInsertar;
                    ListaDetalle[index] = Detalle;
                }
                break;
            case "Q":
                CuentaContableInsertar.Detalles.RemoveAt(index);
                ListaDetalle.RemoveAt(index);
                CerrarDialog();
                break;
        };
        CerrarModal();
        IsModified = true;
        await GridDetalleRef.SetStateAsync(detalleState);
    }
    #endregion

    #region Destino
    private async Task CerrarDestino()
    {
        if (TipoAccionModal is not "V" && IsModifiedDestino && !await Dialog.ConfirmAsync("¿Está seguro de cerrar el formulario y que los datos no se carguen?", "Saliendo del formulario"))
            return;
        CerrarModal();
    }

    public void MostrarAgregarDestino()
    {
        DestinoInsertar = new();
        DestinoObtener = new();
        IniciarAccionModal("I", "destino");
    }

    public void MostrarModificarDestino(CuentaContableDestinoObtenerDto item)
    {
        DestinoInsertar = IMapper.Map<CuentaContableDestinoInsertarDto>(item);
        CodigoItemAccion = item.FlagTipo;
        DestinoObtener = item;
        IniciarAccionModal("M", "destino");
    }

    private void MostrarQuitarDestino(string codigoItem)
    {
        CodigoItemAccion = codigoItem;
        IniciarAccionDialog("Q", "destino");
    }

    public void VerItemDestino(CuentaContableDestinoObtenerDto item)
    {
        DestinoObtener = item;
        CodigoItemAccion = item.FlagTipo;
        IniciarAccionModal("V", "destino");
    }

    private bool ValidarDestinos()
    {
        foreach (CuentaContableDestinoInsertarDto item in CuentaContableInsertar.Destinos.Where(x => x.FlagTipo != CodigoItemAccion))
        {
            if (item.FlagTipo.Trim() == DestinoInsertar.FlagTipo.Trim())
            {
                Fnc.MostrarAlerta(AlertDestino, "El valor del campo [Tipo] ya existe en el listado de Destinos", "warning");
                return false;
            }
        }
        return true;
    }

    private async Task AccionarDestino(string tipoAccion)
    {
        GridState<CuentaContableDestinoObtenerDto> destinoState = GridDestinoRef.GetState();
        int index = ListaDestinos.FindIndex(i => i.FlagTipo == CodigoItemAccion);

        switch (tipoAccion)
        {
            case "I" or "M":
                if (ValidarDestinos())
                    return;
                IMapper.Map(DestinoInsertar, DestinoObtener);
                if (tipoAccion == "I")
                {
                    CuentaContableInsertar.Destinos.Add(DestinoInsertar);
                    ListaDestinos.Add(DestinoObtener);
                    destinoState.InsertedItem = DestinoObtener;
                }
                else
                {
                    CuentaContableInsertar.Destinos[index] = DestinoInsertar;
                    ListaDestinos[index] = DestinoObtener;
                }
                break;
            case "Q":
                CuentaContableInsertar.Destinos.RemoveAt(index);
                ListaDestinos.RemoveAt(index);
                CerrarDialog();
                break;
        };
        CerrarModal();
        IsModified = true;
        await GridDestinoRef.SetStateAsync(destinoState);
    }
    #endregion

    #region Catalogos
    private void CargarItemCatalogoTipoBienServicio(TipoBienServicioCatalogoDto item)
    {
        CuentaContableInsertar.CodigoTipoBienServicio = item.CodigoTipoBienServicio;
        CuentaContableObtener.NombreTipoBienServicio = item.NombreTipoBienServicio;
        IsModified = true;
    }

    private void CargarItemCatalogoPlanContable(PlanContableCatalogoDto item)
    {
        CuentaContableInsertar.CodigoPlanContable = item.CodigoPlanContable;
        CuentaContableObtener.NombrePlanContable = item.NombrePlanContable;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoPlanContable"));
        IsModified = true;
    }

    private void CargarItemCatalogoCuentaContableGanancia(CuentaContableCatalogoDto item)
    {
        DetalleInsertar.CodigoCuentaContableGanancia = item.CodigoCuentaContable;
        Detalle.NombreCuentaContableGanancia = item.NombreCuentaContable;
        IsModifiedDetalle = true;
    }

    private void CargarItemCatalogoCuentaContablePerdida(CuentaContableCatalogoDto item)
    {
        DetalleInsertar.CodigoCuentaContablePerdida = item.CodigoCuentaContable;
        Detalle.NombreCuentaContablePerdida = item.NombreCuentaContable;
        IsModifiedDetalle = true;
    }

    private void CargarItemCatalogoCuentaContableGenera(CuentaContableCatalogoDto item)
    {
        DestinoInsertar.CodigoCuentaContableGenera = item.CodigoCuentaContable;
        DestinoObtener.NombreCuentaContableGenera = item.NombreCuentaContable;
        IsModifiedDestino = true;
    }

    private void CargarItemCatalogoCuentaBalance(CuentaBalanceCatalogoDto item)
    {
        CuentaContableInsertar.CodigoCuentaBalance = item.CodigoCuentaBalance;
        CuentaContableObtener.NombreCuentaBalance = item.NombreCuentaBalance;
        IsModified = true;
    }

    private void CargarItemCatalogoCasillaBalance(CasillaBalanceCatalogoDto item)
    {
        CuentaContableInsertar.CodigoCasillaBalance = item.CodigoCasillaBalance;
        CuentaContableObtener.NombreCasillaBalance = item.NombreCasillaBalance;
        IsModified = true;
    }

    private void CargarItemCatalogoCasillaBalanceDetalle(CasillaBalanceCatalogoDto item)
    {
        CuentaContableInsertar.CodigoCasillaBalanceDetalle = item.CodigoCasillaBalance;
        CuentaContableObtener.NombreCasillaBalanceDetalle = item.NombreCasillaBalance;
        IsModified = true;
    }
    #endregion

    public void Dispose() => GC.SuppressFinalize(this);
}
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

public partial class Edit : IDisposable
{
    #region Propiedades
    private const string codigoServicio = "S004";
    public FluentValidationValidator validator;
    public CuentaContableEditarDto CuentaContableEditar { get; set; }
    public CuentaContableDetalleInsertarDto DetalleInsertar { get; set; }
    public CuentaContableDestinoInsertarDto DestinoInsertar { get; set; }
    public CuentaContableDetalleEditarDto DetalleEditar { get; set; }
    public CuentaContableDestinoEditarDto DestinoEditar { get; set; }
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
    private EditContext EditContextDestinoInsertar { get; set; }
    private EditContext EditContextDestinoEditar { get; set; }
    private EditContext DetalleInsertarContext { get; set; }
    private EditContext DetalleEditarContext { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool IsAuthUser { get; set; }
    private bool IsModified { get; set; }
    private bool IsModifiedDestino { get; set; }
    private bool IsModifiedDetalle { get; set; }
    private bool EsVisibleVolver { get; set; }
    private bool IsInitPage { get; set; }
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    [Parameter] public Guid? Id { get; set; }
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
            Notify.ShowLoading(mensaje: "Obteniendo registro");

            CuentaContableEditar = new();
            DetalleInsertar = new();
            DestinoInsertar = new();
            DetalleEditar = new();
            DestinoEditar = new();
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

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio);
            if (!IsAuthUser) return;
                
            EsVisibleVolver = !string.IsNullOrEmpty(ReturnPage) && ReturnPage is "index" or "view";

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(CuentaContableAcceso.Editar))
            {
                INavigation.NavigateTo("cuentas-contables");
                Notify.Show("No tiene permiso para editar registros de [Cuentas Contables]", "error");
                return;
            }

            CuentaContableObtener = await ICuentaContable.Obtener((Guid) Id); 
            if (CuentaContableObtener is null)
            {
                INavigation.NavigateTo("cuentas-contables");
                Notify.Show($"El registro de la [Cuenta Contable] consultado a editar no existe", "error");
                return;
            }
            CuentaContableEditar = IMapper.Map<CuentaContableEditarDto>(CuentaContableObtener);

            EditContext = new EditContext(CuentaContableEditar);
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

    private async Task Editar()
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
             
            Notify.ShowLoading(mensaje: "Actualización en progreso");

            await ICuentaContable.Editar((Guid) Id, CuentaContableEditar);
            
            IsModified = false;
            Notify.Show("La cuenta contable ha sido editada con éxito", "success");
            INavigation.NavigateTo($"cuentas-contables/{Id}");
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
            case "destino":
                if (tipoAccion is "I" or "M") 
                    EditContextDestinoInsertar = new EditContext(DestinoInsertar); 
                else if (tipoAccion is "E") 
                    EditContextDestinoEditar = new EditContext(DestinoEditar);
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
        VerboAccionModal = DescripcionAccionModal = TipoAccionModal = ""; 
        StateHasChanged();
    }

    private void Volver() => INavigation.NavigateTo($"cuentas-contables{(ReturnPage == "view" ? $"/{Id}" : "")}");

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

    public static void OnRowDetalleRenderHandler(GridRowRenderEventArgs args) => args.Class = !(args.Item as CuentaContableDetalleObtenerDto).Id.HasValue ? "new-row" : "";
    
    public void MostrarAgregarDetalle()
    {
        DetalleInsertar = new();
        Detalle = new(); 
        IniciarAccionModal("I", "detalle");
    }

    public void MostrarModificarDetalle(CuentaContableDetalleObtenerDto item)
    {
        string tipoAccion = !item.Id.HasValue ? "M" : "E"; 
        if (tipoAccion is "M") 
            DetalleEditar = IMapper.Map<CuentaContableDetalleEditarDto>(item); 
        else 
            DetalleInsertar = IMapper.Map<CuentaContableDetalleInsertarDto>(item);
        
        Detalle = item;
        IniciarAccionModal(tipoAccion, "detalle");
    }

    private bool VerificarItemDetalleEsValido()
    {
        foreach (CuentaContableDetalleObtenerDto item in CuentaContableObtener.Detalles.Where(x => string.Concat(x.Id.HasValue ? 1 : 0, x.FlagRegistro) != string.Concat(0, DetalleInsertar.FlagRegistro)))
        {
            if (item.FlagRegistro.Trim() == DetalleInsertar.FlagRegistro.Trim())
            {
                Fnc.MostrarAlerta(AlertDetalle, "El valor del campo [Registro] ya existe en el listado del Detalle", "warning");
                return false;
            }
        }
        return true;
    }

    public void VerItemDetalle(CuentaContableDetalleObtenerDto item)
    {
        Detalle = item;
        IniciarAccionModal("V", "detalle");
    }

    private void MostrarQuitarDetalle(CuentaContableDetalleObtenerDto item)
    {
        Detalle = item;
        IniciarAccionDialog(!item.Id.HasValue ? "Q" : "X", "detalle");
    }

    private void NotifyChange()
    {
        IsModified = EditContext.IsModified();
        if (!IsModified)
            IsModified = CuentaContableEditar.DetallesInsertar.Count > 0 || CuentaContableEditar.DetallesEditar.Count > 0 || CuentaContableEditar.DetallesEliminar.Count > 0;
    }

    private async Task AccionarDetalle(string tipoAccion)
    {
        GridState<CuentaContableDetalleObtenerDto> detalleState = GridDetalleRef.GetState();
        int indexGrid = CuentaContableObtener.Detalles.FindIndex(i => i.FlagRegistro == Detalle.FlagRegistro);
        int indexEdit = tipoAccion is "E" or "X" ? CuentaContableEditar.DetallesEditar.FindIndex(i => i.Id == Detalle.Id) : CuentaContableEditar.DetallesInsertar.FindIndex(i => i.FlagRegistro == Detalle.FlagRegistro); 
        
        switch (tipoAccion)
        {
            case "I" or "M":
                if (VerificarItemDetalleEsValido())
                    return;
                IMapper.Map(DetalleInsertar, Detalle);
                if (tipoAccion == "I")
                {
                    CuentaContableEditar.DetallesInsertar.Add(DetalleInsertar);
                    CuentaContableObtener.Detalles.Add(Detalle);
                    detalleState.InsertedItem = Detalle;
                }
                else
                {
                    CuentaContableEditar.DetallesInsertar[indexEdit] = DetalleInsertar;
                    CuentaContableObtener.Detalles[indexGrid] = Detalle;
                }
                break;
            case "E":
                if (indexEdit > -1)
                    CuentaContableEditar.DetallesEditar[indexEdit] = DetalleEditar;
                else
                    CuentaContableEditar.DetallesEditar.Add(DetalleEditar);
                CuentaContableObtener.Detalles[indexGrid] = IMapper.Map(DetalleEditar, Detalle);
                break;
            case "Q" or "X":
                if (tipoAccion == "Q")
                    CuentaContableEditar.DetallesInsertar.RemoveAt(indexEdit);
                else
                {
                    CuentaContableEditar.DetallesEliminar.Add(new() { Id = (Guid) Detalle.Id });
                    if (indexEdit > -1)
                        CuentaContableEditar.DetallesEditar.RemoveAt(indexEdit);
                }
                CuentaContableObtener.Detalles.RemoveAt(indexGrid);
                CerrarDialog();
                break;
        };
        CerrarModal();
        NotifyChange();
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

    public static void OnRowDestinosRenderHandler(GridRowRenderEventArgs args) => args.Class = !(args.Item as CuentaContableDestinoObtenerDto).Id.HasValue ? "new-row" : "";
    
    public void MostrarAgregarDestino()
    {
        DestinoInsertar = new();
        DestinoObtener = new(); 
        IniciarAccionModal("I", "destino");
    }

    public void MostrarModificarDestino(CuentaContableDestinoObtenerDto item)
    {
        string tipoAccion = !item.Id.HasValue ? "M" : "E";
        if (tipoAccion is "M")
            DestinoEditar = IMapper.Map<CuentaContableDestinoEditarDto>(item); 
        else 
            DestinoInsertar = IMapper.Map<CuentaContableDestinoInsertarDto>(item);

        DestinoObtener = item;
        IniciarAccionModal(tipoAccion, "destino");
    }

    private void MostrarQuitarDestino(CuentaContableDestinoObtenerDto item)
    {
        DestinoObtener = item;
        IniciarAccionDialog(!item.Id.HasValue ? "Q" : "X", "destino");
    }

    public void VerItemDestino(CuentaContableDestinoObtenerDto item)
    {
        DestinoObtener = item; 
        IniciarAccionModal("V", "destino");
    }

    private bool ValidarDestinos()
    {
        foreach (CuentaContableDestinoObtenerDto item in CuentaContableObtener.Destinos.Where(x => string.Concat(x.Id.HasValue ? 1 : 0, x.FlagTipo) != string.Concat(0, DestinoInsertar.FlagTipo)))
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
        int indexGrid = CuentaContableObtener.Destinos.FindIndex(i => i.FlagTipo == DestinoObtener.FlagTipo);
        int indexEdit = tipoAccion is "E" or "X" ? CuentaContableEditar.DestinosEditar.FindIndex(i => i.Id == DestinoObtener.Id) : CuentaContableEditar.DestinosInsertar.FindIndex(i => i.FlagTipo == DestinoObtener.FlagTipo); 
       
        switch (tipoAccion)
        {
            case "I" or "M":
                if (ValidarDestinos())
                    return;
                IMapper.Map(DestinoInsertar, DestinoObtener);
                if (tipoAccion == "I")
                {
                    CuentaContableEditar.DestinosInsertar.Add(DestinoInsertar);
                    CuentaContableObtener.Destinos.Add(DestinoObtener);
                    destinoState.InsertedItem = DestinoObtener;
                }
                else
                {
                    CuentaContableEditar.DestinosInsertar[indexEdit] = DestinoInsertar;
                    CuentaContableObtener.Destinos[indexGrid] = DestinoObtener;
                }
                break;
            case "E":
                if (indexEdit > -1)
                    CuentaContableEditar.DestinosEditar[indexEdit] = DestinoEditar;
                else
                    CuentaContableEditar.DestinosEditar.Add(DestinoEditar);
                CuentaContableObtener.Destinos[indexGrid] = IMapper.Map(DestinoEditar, DestinoObtener);
                break;
            case "Q" or "X":
                if (tipoAccion == "Q")
                    CuentaContableEditar.DestinosInsertar.RemoveAt(indexEdit);
                else
                { 
                    CuentaContableEditar.DestinosEliminar.Add(new() { Id = (Guid) DestinoObtener.Id });
                    if (indexEdit > -1)
                        CuentaContableEditar.DestinosEditar.RemoveAt(indexEdit);
                }
                CuentaContableObtener.Destinos.RemoveAt(indexGrid);
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
        if (string.IsNullOrEmpty(CuentaContableEditar.CodigoTipoBienServicio) || CuentaContableEditar.CodigoTipoBienServicio != item.CodigoTipoBienServicio)
        {
            CuentaContableEditar.CodigoTipoBienServicio = item.CodigoTipoBienServicio;
            CuentaContableObtener.NombreTipoBienServicio = item.NombreTipoBienServicio;
            IsModified = true;
        } 
    }

    private void CargarItemCatalogoCuentaBalance(CuentaBalanceCatalogoDto item)
    {
        if (string.IsNullOrEmpty(CuentaContableEditar.CodigoCuentaBalance) || CuentaContableEditar.CodigoCuentaBalance != item.CodigoCuentaBalance)
        {
            CuentaContableEditar.CodigoCuentaBalance = item.CodigoCuentaBalance;
            CuentaContableObtener.NombreCuentaBalance = item.NombreCuentaBalance;
            IsModified = true;
        } 
    }

    private void CargarItemCatalogoCasillaBalance(CasillaBalanceCatalogoDto item)
    {
        if (string.IsNullOrEmpty(CuentaContableEditar.CodigoCasillaBalance) || CuentaContableEditar.CodigoCasillaBalance != item.CodigoCasillaBalance)
        {
            CuentaContableEditar.CodigoCasillaBalance = item.CodigoCasillaBalance;
            CuentaContableObtener.NombreCasillaBalance = item.NombreCasillaBalance;
            IsModified = true;
        } 
    }

    private void CargarItemCatalogoCasillaBalanceDetalle(CasillaBalanceCatalogoDto item)
    {
        if (string.IsNullOrEmpty(CuentaContableEditar.CodigoCasillaBalanceDetalle) || CuentaContableEditar.CodigoCasillaBalanceDetalle != item.CodigoCasillaBalance)
        {
            CuentaContableEditar.CodigoCasillaBalanceDetalle = item.CodigoCasillaBalance;
            CuentaContableObtener.NombreCasillaBalanceDetalle = item.NombreCasillaBalance;
            IsModified = true;
        } 
    }

    private void CargarItemCatalogoCuentaContableGanancia(CuentaContableCatalogoDto item)
    {
        if (string.IsNullOrEmpty(Detalle.CodigoCuentaContableGanancia) || Detalle.CodigoCuentaContableGanancia != item.CodigoCuentaContable)
        {
            if (TipoAccionModal is "I" or "M") 
                DetalleInsertar.CodigoCuentaContableGanancia = item.CodigoCuentaContable; 
            else if (TipoAccionModal is "E") 
                DetalleEditar.CodigoCuentaContableGanancia = item.CodigoCuentaContable;

            Detalle.NombreCuentaContableGanancia = item.NombreCuentaContable;
            IsModifiedDetalle = true;
        } 
    }

    private void CargarItemCatalogoCuentaContablePerdida(CuentaContableCatalogoDto item)
    {
        if (string.IsNullOrEmpty(Detalle.CodigoCuentaContablePerdida) || Detalle.CodigoCuentaContablePerdida != item.CodigoCuentaContable)
        {
            if (TipoAccionModal is "I" or "M")
                DetalleInsertar.CodigoCuentaContablePerdida = item.CodigoCuentaContable;
            else if (TipoAccionModal is "E")
                DetalleEditar.CodigoCuentaContablePerdida = item.CodigoCuentaContable;

            Detalle.NombreCuentaContablePerdida = item.NombreCuentaContable;
            IsModifiedDetalle = true;
        } 
    }

    private void CargarItemCatalogoCuentaContableGenera(CuentaContableCatalogoDto item)
    {
        if (string.IsNullOrEmpty(DestinoObtener.CodigoCuentaContableGenera) || DestinoObtener.CodigoCuentaContableGenera != item.CodigoCuentaContable)
        {
            if (TipoAccionModal is "I" or "M")
                DestinoInsertar.CodigoCuentaContableGenera = item.CodigoCuentaContable;
            else if (TipoAccionModal is "E")
                DestinoEditar.CodigoCuentaContableGenera = item.CodigoCuentaContable;

            DestinoObtener.NombreCuentaContableGenera = item.NombreCuentaContable;
            IsModifiedDestino = true;
        } 
    }
    #endregion

    public void Dispose() => GC.SuppressFinalize(this);
}
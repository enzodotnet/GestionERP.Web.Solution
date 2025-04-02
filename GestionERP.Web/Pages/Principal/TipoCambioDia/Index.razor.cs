
using Microsoft.AspNetCore.Components; 
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Models.Dtos.Principal.Types;
using GestionERP.Web.Components;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Services;
using System.Security.Claims;
using GestionERP.Web.Handlers;
using Telerik.Blazor.Components;
using Telerik.Blazor;
using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;
using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components.Routing;
using GestionERP.Web.Global;

namespace GestionERP.Web.Pages.Principal.TipoCambioDia;

public partial class Index : IDisposable
{
    #region Propiedades
    private const string codigoServicio = "S005";
    public FluentValidationValidator validator;
    public TelerikScheduler<TipoCambioDiaListarDto> schedulerRef;
    private IEnumerable<TipoCambioDiaListarDto> ListaTipoCambioDias { get; set; }
    private TipoCambioDiaActualizarMontoDto TipoCambioDiaActualizar { get; set; }
    private TipoCambioDiaObtenerDto TipoCambioDiaObtener { get; set; }
    private bool EsVisibleModalActualizar { get; set; }
    private EditContext EditContextTipoCambioDiaActualizar { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool IsAuthUser { get; set; }
    private MonedaConsultaPorTipoDto MonedaTipoCambio { get; set; }
    private MonedaConsultaPorTipoDto MonedaConversion { get; set; }
    private string CodigoPeriodo { get; set; }
    private bool IsModifiedActualizar { get; set; }
    public SchedulerView VistaCalendar { get; set; }
    public DateTime FechaInicio { get; set; }
    public TelerikNotification Alert { get; set; }
    public TelerikNotification AlertModalActualizar { get; set; }
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    private IEnumerable<TipoCambioDiaTipoType> Tipos { get; set; }

    #endregion

    [Inject] public IPrincipalTipoCambioDia ITipoCambioDia { get; set; }
    [Inject] public IPrincipalPeriodo IPeriodo { get; set; }
    [Inject] public IPrincipalMoneda IMoneda { get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; } 
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }
    [Inject] public IMapper IMapper { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Notify.ShowLoading(mensaje: "Listando registro(s)");

            TipoCambioDiaActualizar = new();
            MonedaTipoCambio = new();
            MonedaConversion = new();

            EsVisibleModalActualizar = false;
            Tipos = TipoCambioDiaTipoType.ObtenerTipos();

            VistaCalendar = SchedulerView.Month;
            FechaInicio = DateTime.Now;

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio);
            if (!IsAuthUser) return;

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(TipoCambioDiaAcceso.VerRegistros))
            {
                INavigation.NavigateTo("inicio");
                Notify.Show("No tiene permiso para ver registros del servicio principal de [Tipos de Cambio del Día]", "error");
                return;
            }

            MonedaTipoCambio = await IMoneda.ConsultaPorTipo("ME");
            MonedaConversion = await IMoneda.ConsultaPorTipo("MN");

            CodigoPeriodo = await IPeriodo.ConsultaCodigoPorFecha(FechaInicio) ?? ""; 

            await Listar();
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

    private async Task ActualizarTipoCambioDia()
    {
        try
        {
            IsLoadingAction = true;
            EsVisibleModalActualizar = false;

            IsAuthUser = (await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio, codigoUser: User.FindFirst("code").Value)).esValido;
            if (!IsAuthUser) return;

            Notify.ShowLoading(mensaje: "Actualización en progreso");

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(TipoCambioDiaAcceso.ActualizarRegistro))
            {
                Notify.Show("No tiene permiso para actualizar el [Tipo de cambio del día]", "error");
                return;
            }

            await ITipoCambioDia.ActualizarMonto(TipoCambioDiaActualizar);

            IsModifiedActualizar = false;
            Notify.Show($"El tipo de cambio {Tipos.Where(x => x.Codigo == TipoCambioDiaObtener.FlagTipo).Select(x => x.Nombre).First()} del día {TipoCambioDiaObtener.Fecha:dd-MM-yyyy} ha sido actualizado con éxito", "success");

            DeshacerAccionModalActualizar();
            await Listar();
            schedulerRef.Rebind();
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
        if (IsAuthUser && IsModifiedActualizar && !await Dialog.ConfirmAsync("¿Está seguro de salir de la vista principal sin haber actualizado?", "Saliendo del formulario"))
            context.PreventNavigation();
    }

    private async Task DateChangedHandler(DateTime currDate)
    {
        FechaInicio = currDate;
        CodigoPeriodo = await IPeriodo.ConsultaCodigoPorFecha(FechaInicio) ?? "";  
        await Listar();
    }

    private static void OnItemRenderHandler(SchedulerItemRenderEventArgs e) => e.Class = (!(e.Item as TipoCambioDiaListarDto).Monto.HasValue) ? "exchange-rate-nothing" : (e.Item as TipoCambioDiaListarDto).FlagTipo == "C" ? "exchange-rate-purchase" : "exchange-rate-sale";

    public void InvalidarAccionTipoCambio(EditContext editContext) => Fnc.MostrarAlerta(AlertModalActualizar, Cnf.MsgErrorInvalidEditContext, "error");

    public async void EditHandler(SchedulerEditEventArgs args)
    {
        try
        {
            args.IsCancelled = true;
            TipoCambioDiaListarDto item = args.Item as TipoCambioDiaListarDto;
            if (item.Codigo is not null & !args.IsNew)
            {
                if (!await IPermiso.ConsultaEsAsignadoPorSesion(TipoCambioDiaAcceso.ActualizarRegistro))
                {
                    Notify.Show("No tiene permiso para actualizar el [Tipo de cambio del día]", "error");
                    return;
                }

                TipoCambioDiaObtener = await ITipoCambioDia.Obtener(item.Id) ?? new();
                TipoCambioDiaActualizar = IMapper.Map<TipoCambioDiaActualizarMontoDto>(TipoCambioDiaObtener);
                EditContextTipoCambioDiaActualizar = new EditContext(TipoCambioDiaActualizar);
                EsVisibleModalActualizar = true;
                StateHasChanged();
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
        finally
        {
            Notify.ShowLoading(false);
        }
    }

    private async Task CerrarTipoCambio()
    {
        if (!IsModifiedActualizar || await Dialog.ConfirmAsync("¿Está seguro de cerrar el formulario y que los datos no se actualicen?", "Saliendo del formulario"))
            DeshacerAccionModalActualizar();
    }

    private void DeshacerAccionModalActualizar()
    {
        IsModifiedActualizar = false;
        TipoCambioDiaObtener = null;
        TipoCambioDiaActualizar = null;
        EsVisibleModalActualizar = false;
    }

    private async Task Listar() => ListaTipoCambioDias = await ITipoCambioDia.Listar(CodigoPeriodo) ?? [];

    public void Dispose() => GC.SuppressFinalize(this); 
}
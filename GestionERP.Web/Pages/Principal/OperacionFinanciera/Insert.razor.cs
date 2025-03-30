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
using GestionERP.Web.Global;

namespace GestionERP.Web.Pages.Principal.OperacionFinanciera;

public partial class Insert : IDisposable
{
    #region Propiedades
    private const string codigoServicio = "S008";
    public FluentValidationValidator validator;
    public OperacionFinancieraInsertarDto OperacionFinancieraInsertar { get; set; }
    public OperacionFinancieraObtenerDto OperacionFinancieraObtener { get; set; }
    private IEnumerable<OperacionFinancieraTipoCambioType> TiposCambio { get; set; }
    private EditContext EditContext { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool IsAuthUser { get; set; }
    private bool IsModified { get; set; }
    private bool EsVisibleVolver { get; set; }
    public TelerikNotification Alert { get; set; }
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    [Parameter][SupplyParameterFromQuery(Name = "returnpage")] public string ReturnPage { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    #endregion

    [Inject] public IPrincipalOperacionFinanciera IOperacionFinanciera { get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        { 
            OperacionFinancieraObtener = new();
            OperacionFinancieraInsertar = new();

            TiposCambio = OperacionFinancieraTipoCambioType.ObtenerTipos();

            EditContext = new EditContext(OperacionFinancieraInsertar);

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio);
            if (!IsAuthUser) return;
                
            if (!await IPermiso.ConsultaEsAsignadoPorSesion(OperacionFinancieraAcceso.Insertar))
            {
                INavigation.NavigateTo("operaciones-financieras");
                Notify.Show("No tiene permiso para insertar registros de [Operaciones Financieras]", "error");
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

            Guid id = await IOperacionFinanciera.Insertar(OperacionFinancieraInsertar);

            IsModified = false;
            Notify.Show("La operación financiera ha sido insertada con éxito", "success");
            INavigation.NavigateTo($"operaciones-financieras/{id}"); 
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


    private void ValueCodigoUpperChanged(object codigo) => OperacionFinancieraInsertar.Codigo = codigo?.ToString().ToUpper().Trim();

    private void Volver() => INavigation.NavigateTo("operaciones-financieras");

    #region Catalogos 
    private void CargarItemCatalogoTipoTransaccion(TipoTransaccionCatalogoDto item)
    {
        OperacionFinancieraInsertar.CodigoTipoTransaccion = item.CodigoTipoTransaccion;
        OperacionFinancieraObtener.NombreTipoTransaccion = item.NombreTipoTransaccion;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoTipoTransaccion"));
        IsModified = true;
    }
     
    #endregion

    public void Dispose() => GC.SuppressFinalize(this);
}
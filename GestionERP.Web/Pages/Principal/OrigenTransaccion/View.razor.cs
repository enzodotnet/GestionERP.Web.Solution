using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components;
using GestionERP.Web.Services; 

namespace GestionERP.Web.Pages.Principal.OrigenTransaccion;

public partial class View : IDisposable
{
    private const string codigoServicio = "S077";
    public OrigenTransaccionObtenerDto OrigenTransaccionObtener { get; set; }
    [Parameter] public Guid? Id { get; set; } 
    [CascadingParameter] public NotifyComponent Notify { get; set; }

    [Inject] public IPrincipalOrigenTransaccion IOrigenTransaccion { get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Notify.ShowLoading(mensaje: "Obteniendo registro");

            if (!(await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio)).esValido)
                return;

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(OrigenTransaccionAcceso.VerRegistros))
            {
                INavigation.NavigateTo("inicio");
                Notify.Show("No tiene permiso para ver registros del servicio principal de [Orígenes de Transacciones]", "error");
                return;
            }

            OrigenTransaccionObtener = await IOrigenTransaccion.Obtener((Guid) Id);

            if (OrigenTransaccionObtener is null)
            {
                INavigation.NavigateTo("origenes-transaccion");
                Notify.Show("El registro del [Origen de Transaccion] consultado a visualizar no está disponible", "error");
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

    private void Volver() => INavigation.NavigateTo("origenes-transaccion");

    public void Dispose() => GC.SuppressFinalize(this);
}
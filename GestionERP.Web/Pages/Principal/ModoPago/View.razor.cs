using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Models.Dtos.Principal.Types;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components;
using GestionERP.Web.Services; 

namespace GestionERP.Web.Pages.Principal.ModoPago;

public partial class View : IDisposable
{
    private const string codigoServicio = "S091";
    public ModoPagoObtenerDto ModoPagoObtener { get; set; }
    public IEnumerable<ModoPagoVinculoModoType> VinculosModo { get; set; }
    [Parameter] public Guid? Id { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }

    [Inject] public IPrincipalModoPago IModoPago { get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Notify.ShowLoading(mensaje: "Obteniendo registro"); 
            VinculosModo = ModoPagoVinculoModoType.ObtenerTipos();

            if (!(await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio)).esValido)
                return;


            if (!await IPermiso.ConsultaEsAsignadoPorSesion(ModoPagoAcceso.VerRegistros))
            {
                INavigation.NavigateTo("inicio");
                Notify.Show("No tiene permiso para ver registros del servicio principal de [Modos de Pago]", "error");
                return;
            }

            ModoPagoObtener = await IModoPago.Obtener((Guid) Id);

            if (ModoPagoObtener is null)
            {
                INavigation.NavigateTo("modos-pago");
                Notify.Show("El registro del [Modo de Pago] consultado a visualizar no está disponible", "error");
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

    private void Volver() => INavigation.NavigateTo("modos-pago");

    public void Dispose() => GC.SuppressFinalize(this);
}
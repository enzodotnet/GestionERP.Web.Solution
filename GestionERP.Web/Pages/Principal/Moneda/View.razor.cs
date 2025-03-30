using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Models.Dtos.Principal.Types;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components;
using GestionERP.Web.Services; 

namespace GestionERP.Web.Pages.Principal.Moneda;

public partial class View : IDisposable
{
    private const string codigoServicio = "S056";
    public MonedaObtenerDto MonedaObtener { get; set; }
    public IEnumerable<MonedaTipoType> Tipos { get; set; }
    [Parameter] public Guid? Id { get; set; } 
    [CascadingParameter] public NotifyComponent Notify { get; set; }

    [Inject] public IPrincipalMoneda IMoneda { get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Notify.ShowLoading(mensaje: "Obteniendo registro"); 
            Tipos = MonedaTipoType.ObtenerTipos();

            if (!(await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio)).esValido)
                return;


            if (!await IPermiso.ConsultaEsAsignadoPorSesion(MonedaAcceso.VerRegistros))
            {
                INavigation.NavigateTo("inicio");
                Notify.Show("No tiene permiso para ver registros del servicio principal de [Monedas]", "error");
                return;
            }

            MonedaObtener = await IMoneda.Obtener((Guid) Id);

            if (MonedaObtener is null)
            {
                INavigation.NavigateTo("monedas");
                Notify.Show($"El registro de la [Moneda] consultado a visualizar no está disponible", "error");
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

    private void Volver() => INavigation.NavigateTo("monedas");

    public void Dispose() => GC.SuppressFinalize(this);
}
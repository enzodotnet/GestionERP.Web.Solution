using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components;
using GestionERP.Web.Services; 
using Telerik.Blazor.Components; 

namespace GestionERP.Web.Pages.Principal.TipoDetraccion;

public partial class View : IDisposable
{
    private const string codigoServicio = "S048";
    public TipoDetraccionObtenerDto TipoDetraccionObtener { get; set; }
    private MonedaObtenerPorTipoDto MN { get; set; }
    public TelerikNotification Alert { get; set; }
    [Parameter] public Guid? Id { get; set; } 
    [CascadingParameter] public NotifyComponent Notify { get; set; }

    [Inject] public IPrincipalTipoDetraccion ITipoDetraccion { get; set; }
    [Inject] public IPrincipalMoneda IMoneda { get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Notify.ShowLoading(mensaje: "Obteniendo registro"); 
            MN = new();

			if (!(await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio)).esValido)
				return;

			if (!await IPermiso.ConsultaEsAsignadoPorSesion(TipoDetraccionAcceso.VerRegistros))
            {
                INavigation.NavigateTo("inicio");
                Notify.Show("No tiene permiso para ver registros del servicio principal de [Tipos de Detracciones]", "error");
                return;
            }

            TipoDetraccionObtener = await ITipoDetraccion.Obtener((Guid) Id); 
            if (TipoDetraccionObtener is null)
            {
                INavigation.NavigateTo("tipos-detraccion");
                Notify.Show("El registro del [Tipo de detracción] consultado a visualizar no está disponible", "error");
                return;
            }

            MN = await IMoneda.ObtenerPorTipo("MN");
            if (MN is null)
                Notify.Show("Falta configurar la moneda nacional por defecto en el sistema", "error"); 
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

    private void Volver() => INavigation.NavigateTo("tipos-detraccion");

    public void Dispose() => GC.SuppressFinalize(this);
}
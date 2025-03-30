using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Principal; 
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components;
using GestionERP.Web.Services; 

namespace GestionERP.Web.Pages.Principal.TipoAtencionVenta;

public partial class View : IDisposable
{
    private const string codigoServicio = "S060";
    public TipoAtencionVentaObtenerDto TipoAtencionVentaObtener { get; set; }
    [Parameter] public Guid? Id { get; set; } 
    [CascadingParameter] public NotifyComponent Notify { get; set; }

    [Inject] public IPrincipalTipoAtencionVenta ITipoAtencionVenta { get; set; }
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

			if (!await IPermiso.ConsultaEsAsignadoPorSesion(TipoAtencionVentaAcceso.VerRegistros))
            {
                INavigation.NavigateTo("inicio");
                Notify.Show("No tiene permiso para ver registros del servicio principal de [Tipos de Atención Venta]", "error");
                return;
            }

            TipoAtencionVentaObtener = await ITipoAtencionVenta.Obtener((Guid) Id);
            if (TipoAtencionVentaObtener is null)
            {
                INavigation.NavigateTo("tipos-atencion-venta");
                Notify.Show("El registro del [Tipo de Atención Venta] consultado a visualizar no está disponible", "error");
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

    private void Volver() => INavigation.NavigateTo("tipos-atencion-venta");

    public void Dispose() => GC.SuppressFinalize(this);
}
using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Principal; 
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components;
using GestionERP.Web.Services; 

namespace GestionERP.Web.Pages.Principal.TipoOperacionProveedor;

public partial class View : IDisposable
{
    private const string codigoServicio = "S061";
    public TipoOperacionProveedorObtenerDto TipoOperacionProveedorObtener { get; set; }
    [Parameter] public Guid? Id { get; set; } 
    [CascadingParameter] public NotifyComponent Notify { get; set; }

    [Inject] public IPrincipalTipoOperacionProveedor ITipoOperacionProveedor { get; set; }
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

			if (!await IPermiso.ConsultaEsAsignadoPorSesion(TipoOperacionProveedorAcceso.VerRegistros))
            {
                INavigation.NavigateTo("inicio");
                Notify.Show("No tiene permiso para ver registros del servicio principal de [Tipos de Operacion Proveedor]", "error");
                return;
            }

            TipoOperacionProveedorObtener = await ITipoOperacionProveedor.Obtener((Guid) Id);

            if (TipoOperacionProveedorObtener is null)
            {
                INavigation.NavigateTo("tipos-operacion-proveedor");
                Notify.Show("El registro del [Tipo de Operacion Proveedor] consultado a visualizar no está disponible", "error");
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

    private void Volver() => INavigation.NavigateTo("tipos-operacion-proveedor");

    public void Dispose() => GC.SuppressFinalize(this);
}
using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Principal;  
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components;
using GestionERP.Web.Services;

namespace GestionERP.Web.Pages.Principal.ProductoEstado;

public partial class View : IDisposable
{
    private const string codigoServicio = "S081";
    public ProductoEstadoObtenerDto ProductoEstadoObtener { get; set; }
    [Parameter] public Guid? Id { get; set; } 
    [CascadingParameter] public NotifyComponent Notify { get; set; }

    [Inject] public IPrincipalProductoEstado IProductoEstado { get; set; }
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

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(ProductoEstadoAcceso.VerRegistros))
            {
                INavigation.NavigateTo("inicio");
                Notify.Show("No tiene permiso para ver registros del servicio principal de [Estados de Producto]", "error");
                return;
            }

            ProductoEstadoObtener = await IProductoEstado.Obtener((Guid) Id);

            if (ProductoEstadoObtener is null)
            {
                INavigation.NavigateTo("producto-estados");
                Notify.Show("El registro del [Estado de Producto] consultado a visualizar no está disponible", "error");
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

    private void Volver() => INavigation.NavigateTo("producto-estados");

    public void Dispose() => GC.SuppressFinalize(this);
}
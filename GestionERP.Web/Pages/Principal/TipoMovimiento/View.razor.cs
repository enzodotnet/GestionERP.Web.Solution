using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Models.Dtos.Principal.Types;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components;
using GestionERP.Web.Services; 

namespace GestionERP.Web.Pages.Principal.TipoMovimiento;

public partial class View : IDisposable
{
    private const string codigoServicio = "S074";
    public TipoMovimientoObtenerDto TipoMovimientoObtener { get; set; }
    private IEnumerable<TipoMovimientoTipoAccionType> TiposAccion { get; set; }
    private IEnumerable<TipoMovimientoTipoProcesoType> TiposProceso { get; set; }
    private IEnumerable<TipoMovimientoTipoEntidadType> TiposEntidad { get; set; }
    [Parameter] public Guid? Id { get; set; } 
    [CascadingParameter] public NotifyComponent Notify { get; set; }

    [Inject] public IPrincipalTipoMovimiento ITipoMovimiento { get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Notify.ShowLoading(mensaje: "Obteniendo registro"); 
            TiposAccion = TipoMovimientoTipoAccionType.ObtenerTipos();
            TiposProceso = TipoMovimientoTipoProcesoType.ObtenerTipos();
            TiposEntidad = TipoMovimientoTipoEntidadType.ObtenerTipos();

			if (!(await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio)).esValido)
				return;

			if (!await IPermiso.ConsultaEsAsignadoPorSesion(TipoMovimientoAcceso.VerRegistros))
            {
                INavigation.NavigateTo("inicio");
                Notify.Show("No tiene permiso para ver registros del servicio principal de [Tipos de Movimientos]", "error");
                return;
            }

            TipoMovimientoObtener = await ITipoMovimiento.Obtener((Guid) Id);

            if (TipoMovimientoObtener is null)
            {
                INavigation.NavigateTo("tipos-movimiento");
                Notify.Show("El registro del [Tipo de Movimiento] consultado a visualizar no está disponible", "error");
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

    private void Volver() => INavigation.NavigateTo("tipos-movimiento");

    public void Dispose() => GC.SuppressFinalize(this);
}
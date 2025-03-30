using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Models.Dtos.Principal.Types;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components;
using GestionERP.Web.Services; 

namespace GestionERP.Web.Pages.Principal.TipoIdentificacion;

public partial class View : IDisposable
{
    private const string codigoServicio = "S041";
    public TipoIdentificacionObtenerDto TipoIdentificacionObtener { get; set; }
    private IEnumerable<TipoIdentificacionTipoOrigenType> TiposOrigen { get; set; }
    [Parameter] public Guid? Id { get; set; } 
    [CascadingParameter] public NotifyComponent Notify { get; set; }

    [Inject] public IPrincipalTipoIdentificacion ITipoIdentificacion { get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Notify.ShowLoading(mensaje: "Obteniendo registro");
            TiposOrigen = TipoIdentificacionTipoOrigenType.ObtenerTipos();

			if (!(await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio)).esValido)
				return;

			if (!await IPermiso.ConsultaEsAsignadoPorSesion(TipoIdentificacionAcceso.VerRegistros))
            {
                INavigation.NavigateTo("inicio");
                Notify.Show("No tiene permiso para ver registros del servicio principal de [Tipos de identificación]", "error");
                return;
            }

            TipoIdentificacionObtener = await ITipoIdentificacion.Obtener((Guid) Id); 
            if (TipoIdentificacionObtener is null)
            {
                INavigation.NavigateTo("tipos-identificacion");
                Notify.Show($"El registro de la [Tipo de identificación] consultado a visualizar no está disponible", "error");
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

    private void Volver() => INavigation.NavigateTo("tipos-identificacion");

    public void Dispose() => GC.SuppressFinalize(this);
}
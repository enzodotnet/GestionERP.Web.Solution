using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components;
using GestionERP.Web.Services; 

namespace GestionERP.Web.Pages.Principal.Region;

public partial class View : IDisposable
{
    private const string codigoServicio = "S063";
    public RegionObtenerDto RegionObtener { get; set; }
    private bool EsAsignadoVerProvincia { get; set; }
    [Parameter] public Guid? Id { get; set; } 
    [CascadingParameter] public NotifyComponent Notify { get; set; }

    [Inject] public IPrincipalRegion IRegion { get; set; }
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

			EsAsignadoVerProvincia = await IPermiso.ConsultaEsAsignadoPorSesion(ProvinciaAcceso.VerRegistros);

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(RegionAcceso.VerRegistros))
            {
                INavigation.NavigateTo("inicio");
                Notify.Show("No tiene permiso para ver registros del servicio principal de [Regiones]", "error");
                return;
            }

            RegionObtener = await IRegion.Obtener((Guid) Id);

            if (RegionObtener is null)
            {
                INavigation.NavigateTo("regiones");
                Notify.Show("El registro del [región] consultado a visualizar no está disponible", "error");
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

    private void Volver() => INavigation.NavigateTo("regiones");

    private void IrVerProvincias() => INavigation.NavigateTo(INavigation.GetUriWithQueryParameters("provincias", new Dictionary<string, object> { ["codigoRegion"] = RegionObtener.Codigo.Trim() }));

    public void Dispose() => GC.SuppressFinalize(this);
}
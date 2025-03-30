using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components;
using GestionERP.Web.Services;

namespace GestionERP.Web.Pages.Principal.Pais;

public partial class View : IDisposable
{
    private const string codigoServicio = "S062";
    public PaisObtenerDto PaisObtener { get; set; }
    private bool EsAsignadoVerRegion { get; set; }
    [Parameter] public Guid? Id { get; set; } 
    [CascadingParameter] public NotifyComponent Notify { get; set; }

    [Inject] public IPrincipalPais IPais { get; set; }
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

            EsAsignadoVerRegion = await IPermiso.ConsultaEsAsignadoPorSesion(RegionAcceso.VerRegistros);

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(PaisAcceso.VerRegistros))
            {
                INavigation.NavigateTo("inicio");
                Notify.Show("No tiene permiso para ver registros del servicio principal de [Países]", "error");
                return;
            }

            PaisObtener = await IPais.Obtener((Guid) Id);

            if (PaisObtener is null)
            {
                INavigation.NavigateTo("paises");
                Notify.Show("El registro del [País] consultado a visualizar no está disponible", "error");
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

    private void Volver() => INavigation.NavigateTo("paises");

    private void IrVerRegiones() => INavigation.NavigateTo(INavigation.GetUriWithQueryParameters("regiones", new Dictionary<string, object> { ["codigoPais"] = PaisObtener.Codigo.Trim() }));

    public void Dispose() => GC.SuppressFinalize(this);
}
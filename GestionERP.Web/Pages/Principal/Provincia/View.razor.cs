using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components;
using GestionERP.Web.Services; 

namespace GestionERP.Web.Pages.Principal.Provincia;

public partial class View : IDisposable
{
    private const string codigoServicio = "S064";
    public ProvinciaObtenerDto ProvinciaObtener { get; set; }
    private bool EsAsignadoVerDistrito { get; set; }
    [Parameter] public Guid? Id { get; set; } 
    [CascadingParameter] public NotifyComponent Notify { get; set; }

    [Inject] public IPrincipalProvincia IProvincia { get; set; }
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

            EsAsignadoVerDistrito = await IPermiso.ConsultaEsAsignadoPorSesion(DistritoAcceso.VerRegistros);

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(ProvinciaAcceso.VerRegistros))
            {
                INavigation.NavigateTo("inicio");
                Notify.Show("No tiene permiso para ver registros del servicio principal de [Provincias]", "error");
                return;
            }

            ProvinciaObtener = await IProvincia.Obtener((Guid) Id);

            if (ProvinciaObtener is null)
            {
                INavigation.NavigateTo("provincias");
                Notify.Show("El registro del [Provincia] consultado a visualizar no está disponible", "error");
            }
        }
        catch (HttpRequestException)
        {
            Notify.ShowError("NC");
        }
        catch (HttpResponseException ex)
        {
            Notify.ShowError(ex.Code, ex);
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
      
    private void Volver() => INavigation.NavigateTo("provincias");

    private void IrVerDistritos() => INavigation.NavigateTo(INavigation.GetUriWithQueryParameters("distritos", new Dictionary<string, object> { ["codigoProvincia"] = ProvinciaObtener.Codigo.Trim() }));

    public void Dispose() => GC.SuppressFinalize(this);
} 
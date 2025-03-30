using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components;
using GestionERP.Web.Services;

namespace GestionERP.Web.Pages.Principal.Provincia;

public partial class Index : IDisposable
{
    #region Propiedades
    private const string codigoServicio = "S064";

    private IEnumerable<ProvinciaListarDto> ListaProvincias { get; set; }
    private bool IsInitGrid { get; set; } 
    private string TituloIndex { get; set; }
    [Parameter][SupplyParameterFromQuery(Name = "codigoRegion")] public string CodigoRegion { get; set; } 
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    #endregion

    [Inject] public IPrincipalProvincia IProvincia { get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {  
            Notify.ShowLoading(mensaje: "Listando registro(s)");
            
            TituloIndex = "Lista de provincias";
            CodigoRegion ??= "";

            if (CodigoRegion != "") 
                TituloIndex += " por región"; 

			if (!(await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio)).esValido)
				return;
                    
            if (!await IPermiso.ConsultaEsAsignadoPorSesion(ProvinciaAcceso.VerRegistros))
            {
                INavigation.NavigateTo("inicio");
                Notify.Show("No tiene permiso para ver registros del servicio principal de [Provincias]", "error");
                return;
            }

			ListaProvincias = await IProvincia.Listar(CodigoRegion);
			IsInitGrid = true;
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

    private void IrVer(Guid id) => INavigation.NavigateTo($"provincias/{id}");
     
    public void Dispose() => GC.SuppressFinalize(this);
}
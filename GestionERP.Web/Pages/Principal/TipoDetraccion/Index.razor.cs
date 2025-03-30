using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components;
using GestionERP.Web.Services; 

namespace GestionERP.Web.Pages.Principal.TipoDetraccion;

public partial class Index : IDisposable
{
    #region Propiedades
    private const string codigoServicio = "S048";

    private IEnumerable<TipoDetraccionListarDto> ListaTiposDetraccion { get; set; }
    private bool IsInitGrid { get; set; } 
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    #endregion

    [Inject] public IPrincipalTipoDetraccion ITipoDetraccion { get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {  
            Notify.ShowLoading(mensaje: "Listando registro(s)");

			if (!(await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio)).esValido)
				return;
                    
            if (!await IPermiso.ConsultaEsAsignadoPorSesion(TipoDetraccionAcceso.VerRegistros))
            {
                INavigation.NavigateTo("inicio");
                Notify.Show("No tiene permiso para ver registros del servicio principal de [Tipos de Detracciones]", "error");
                return;
            }

			ListaTiposDetraccion = await ITipoDetraccion.Listar();
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

    private void IrVer(Guid id) => INavigation.NavigateTo($"tipos-detraccion/{id}");
     
    public void Dispose() => GC.SuppressFinalize(this);
} 
using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components;
using GestionERP.Web.Services;

namespace GestionERP.Web.Pages.Principal.TipoImportacion;

public partial class Index : IDisposable
{
    #region Propiedades
    private const string codigoServicio = "S082";

    private IEnumerable<TipoImportacionListarDto> ListaTiposImportacion { get; set; }
    private bool IsInitGrid { get; set; } 
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    #endregion

    [Inject] public IPrincipalTipoImportacion ITipoImportacion { get; set; }
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
                    
            if (!await IPermiso.ConsultaEsAsignadoPorSesion(TipoImportacionAcceso.VerRegistros))
            {
                INavigation.NavigateTo("inicio");
                Notify.Show("No tiene permiso para ver registros del servicio principal de [Típos de Importaciones]", "error");
                return;
            }

			ListaTiposImportacion = await ITipoImportacion.Listar();
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

    private void IrVer(Guid id) => INavigation.NavigateTo($"tipos-importacion/{id}");
     
    public void Dispose() => GC.SuppressFinalize(this);
}
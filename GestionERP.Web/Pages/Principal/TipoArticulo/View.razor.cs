using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Models.Dtos.Principal.Types;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components;
using GestionERP.Web.Services;

namespace GestionERP.Web.Pages.Principal.TipoArticulo;

public partial class View : IDisposable
{
    private const string codigoServicio = "S080";
    public TipoArticuloObtenerDto TipoArticuloObtener { get; set; }
    private IEnumerable<TipoArticuloCategoriaType> Categorias { get; set; }
    [Parameter] public Guid? Id { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }

    [Inject] public IPrincipalTipoArticulo ITipoArticulo { get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Notify.ShowLoading(mensaje: "Obteniendo registro"); 
            Categorias = TipoArticuloCategoriaType.ObtenerTipos();

			if (!(await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio)).esValido)
				return;

			if (!await IPermiso.ConsultaEsAsignadoPorSesion(TipoArticuloAcceso.VerRegistros))
            {
                INavigation.NavigateTo("inicio");
                Notify.Show("No tiene permiso para ver registros del servicio principal de [Tipos de artículo]", "error");
                return;
            }

            TipoArticuloObtener = await ITipoArticulo.Obtener((Guid) Id);

            if (TipoArticuloObtener is null)
            {
                INavigation.NavigateTo("tipos-articulo");
                Notify.Show("El registro del [Tipo de artículo] consultado a visualizar no está disponible", "error");
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

    private void Volver() => INavigation.NavigateTo("tipos-articulo");

    public void Dispose() => GC.SuppressFinalize(this);
}
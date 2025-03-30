using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components;  
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Services;
namespace GestionERP.Web.Pages;

public partial class Index : IDisposable
{
    private IEnumerable<UsuarioEmpresaCatalogoPorSesionDto> CatalogoEmpresas { get; set; } 
    private bool IsLoading { get; set; }
    public bool EsVisibleAlert { get; set; }
    public string MensajeAlert { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }

    [Inject] public IPrincipalUsuario IUsuario { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            IsLoading = true;
            CatalogoEmpresas = [];
            if (!(await IUser.VerificarAccesoEsValido(Notify)).esValido)
                return;

            CatalogoEmpresas = await IUsuario.CatalogoEmpresasPorSesion(); 
            if (CatalogoEmpresas is null)
            {
                MensajeAlert = "No cuenta con acceso a empresas dentro del sistema. Por favor contáctese con administración.";
                EsVisibleAlert = true;
            }
            else
            {
                foreach (UsuarioEmpresaCatalogoPorSesionDto item in CatalogoEmpresas.Where(x => string.IsNullOrEmpty(x.UrlArchivoLogo)))
                    item.UrlArchivoLogo = "img/default/img_unavailable.jpg";
            }
        }
        catch (Exception ex)
        {
            if (ex is HttpRequestException)
                Notify.ShowError("NC", isClosable: true);
            else if (ex is HttpResponseException)
                Notify.ShowError((ex as HttpResponseException).Code, ex);
            else
                Notify.ShowError("FA", ex);
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void IrEmpresa(string codigoWebEmpresa) => INavigation.NavigateTo($"{codigoWebEmpresa}/inicio");
     
    public void Dispose() => GC.SuppressFinalize(this);
} 
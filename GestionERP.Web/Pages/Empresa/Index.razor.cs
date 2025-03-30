using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components;  
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Services;

namespace GestionERP.Web.Pages.Empresa;

public partial class Index : IDisposable
{
    private bool IsLoading { get; set; }
    public bool EsVisibleAlert { get; set; }
    public string MensajeAlert { get; set; }  
    public EmpresaConsultaPorCodigoWebDto Empresa { get; set; }
    [Parameter] public string CodigoWebEmpresa { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    
    [Inject] public IPrincipalEmpresa IEmpresa { get; set; } 
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        { 
            IsLoading = true;
            Empresa = new();
            if (!(await IUser.VerificarAccesoEsValido(Notify, CodigoWebEmpresa)).esValido)
                return; 
            Empresa = await IEmpresa.ConsultaPorCodigoWeb(CodigoWebEmpresa);
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

    public void Dispose() => GC.SuppressFinalize(this);
} 
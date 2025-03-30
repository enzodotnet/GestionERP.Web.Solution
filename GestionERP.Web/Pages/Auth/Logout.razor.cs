using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components;

namespace GestionERP.Web.Pages.Auth;

public partial class Logout : IDisposable
{
    [CascadingParameter] public NotifyComponent Notify { get; set; } = new NotifyComponent();
    [Inject] public IAuthentication IAuth { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await IAuth.CerrarSesionUsuario(); 
        INavigation.NavigateTo("/");
    }

    public void Dispose()
    {
        Notify.Dispose();
        GC.SuppressFinalize(this);
    }
} 
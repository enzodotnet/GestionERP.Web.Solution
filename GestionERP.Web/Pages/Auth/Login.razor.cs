using Microsoft.AspNetCore.Components; 
using Microsoft.AspNetCore.Components.Authorization;
using GestionERP.Web.Models.Dtos.Auth;
using Blazored.FluentValidation;
using Telerik.Blazor.Components;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Handlers;
using static Telerik.Blazor.ThemeConstants;
using GestionERP.Web.Global;

namespace GestionERP.Web.Pages.Auth;

public partial class Login : IDisposable
{
    private UsuarioAutenticarCredencialDto UsuarioAuth { get; set; } = new();
    public FluentValidationValidator validator;
    public bool EsCredencialCorrecto { get; set; }
    public bool IsLoadingAction { get; set; }
    public readonly UsuarioAutenticarCredencialValidator validationLogin = new();
    private int Anio { get; set; }
    private bool EsVisibleLoader { get; set; }
    private string TextLogin { get; set; }
    private TelerikNotification NotificacionLogin { get; set; } = new TelerikNotification();

    [Inject] public IAuthentication IAuth { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }
    [Inject] public AuthenticationStateProvider IAuthState { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Anio = DateTime.Now.Year;
            SwitchLoader();
            EsCredencialCorrecto = true;
            UsuarioAuth = new();

            if ((await IAuthState.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated)
                INavigation.NavigateTo("/inicio");
        }
        catch (Exception)
        {
            throw;
        }   
    }

    // protected override void OnInitialized()
    // {
    //     Anio = DateTime.Now.Year;
    //     SwitchLoader();
    //     EsCredencialCorrecto = true;
    //     UsuarioAuth = new(); 
    // }

    // protected override async Task OnAfterRenderAsync(bool firstRender)
    // { 
    //     if ((await IAuthState.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated)
    //         INavigation.NavigateTo("/inicio");
    // }

    private void SwitchLoader(bool visibleLoader = false, bool activadoSubmit = true)
    {
        EsVisibleLoader = visibleLoader;
        IsLoadingAction = activadoSubmit;
        TextLogin = visibleLoader ? "Verificando credenciales" : "Iniciar sesión";
    }

    private async Task HandleValidLogin()
    {
        try
        {
            if (EsCredencialCorrecto)
            {
                SwitchLoader(activadoSubmit: false);
                await Task.Delay(500);
            }

            if (await validator!.ValidateAsync())
            {
                SwitchLoader(true, false);
                StateHasChanged();
                await Task.Delay(1000);

                await IAuth.AutenticarUsuario(UsuarioAuth);

                SwitchLoader();
                OcultarNotificacion();
                INavigation.NavigateTo("/inicio");
            }
            else
            {
                EsCredencialCorrecto = false;
                SwitchLoader();
            }
        }
        catch (Exception ex)
        {
            SwitchLoader();
            if (ex is HttpRequestException)
                MostrarError(Cnf.MsgErrorNotConnectApi);
            else if (ex is HttpResponseException)
                MostrarError($"{((ex as HttpResponseException).Code == "NF" ? Cnf.MsgErrorNotFoundAPi : ex.Message)}");     
            else
                MostrarError(Cnf.MsgErrorFuncAppWeb);
        }
    }

    private void MostrarError(string message) => NotificacionLogin.Show(new NotificationModel()
    {
        Text = message,
        ThemeColor = Notification.ThemeColor.Error,
        CloseAfter = 5000,
        Closable = false
    });

    public void OcultarNotificacion()
    {
        NotificacionLogin.HideAll();
        Dispose();
    }

    public void Dispose()
    {
        NotificacionLogin.Dispose();
        GC.SuppressFinalize(this);
    }
}
using Microsoft.AspNetCore.Components.Authorization; 
using System.Security.Claims; 
using System.Net.Http.Headers;
using GestionERP.Web.Global;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace GestionERP.Web.AuthProviders;

public class AuthStateProvider(HttpClient httpClient, ProtectedLocalStorage protectedLocalStorage) : AuthenticationStateProvider
{
    private readonly AuthenticationState anonymous = new(new ClaimsPrincipal(new ClaimsIdentity()));

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            ProtectedBrowserStorageResult<string> authToken = await protectedLocalStorage.GetAsync<string>("authToken");
            if (!authToken.Success)
                return anonymous;

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authToken.Value);
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(Fnc.ParseClaimsFromJwt(authToken.Value), "jwtAuthType")));
        }
        catch (Exception)
        {
            await protectedLocalStorage.DeleteAsync("authToken");
            return anonymous;
        } 
    }

    public void NotifyUserAuthentication(string authToken)
    {
        ClaimsPrincipal authenticatedUser = new(new ClaimsIdentity(Fnc.ParseClaimsFromJwt(authToken), "jwtAuthType"));
        Task<AuthenticationState> authState = Task.FromResult(new AuthenticationState(authenticatedUser));
        NotifyAuthenticationStateChanged(authState);
    }

    public void NotifyUserLogout()
    {
        Task<AuthenticationState> authState = Task.FromResult(anonymous);
        NotifyAuthenticationStateChanged(authState);
    }
} 
using GestionERP.Web.Models.Dtos.Auth;
using GestionERP.Web.Models.Responses;
using GestionERP.Web.Models.Requests;
using GestionERP.Web.AuthProviders;
using Microsoft.AspNetCore.Components.Authorization;
using GestionERP.Web.Services.Interfaces; 
using System.Net.Http.Headers; 
using GestionERP.Web.Handlers;
using System.Net;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage; 

namespace GestionERP.Web.Services.Apis;

public class AuthenticationApi(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider, ProtectedLocalStorage protectedLocalStorage) : IAuthentication
{
    protected ErrorEndpointResponse error = new();

    public async Task AutenticarUsuario(UsuarioAutenticarCredencialDto login)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.PostAsJsonAsync("auth/login", login);
            
            if (response.IsSuccessStatusCode)
            { 
                AutenticacionResponse result = await response.Content.ReadFromJsonAsync<AutenticacionResponse>(); 
                await protectedLocalStorage.SetAsync("authToken", result.AuthToken);

                ((AuthStateProvider)authenticationStateProvider).NotifyUserAuthentication(result.AuthToken);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.AuthToken);
            }
            else
            { 
                error = response.StatusCode == HttpStatusCode.NotFound ? new(){ Code = "NF" } : await response.Content.ReadFromJsonAsync<ErrorEndpointResponse>();
                throw new HttpResponseException(error.Message, error.Code);
            }
        }
        catch (HttpRequestException)
        {
            throw new HttpRequestException();
        }
    }

    public async Task CerrarSesionUsuario()
    {
        try
        {
            ProtectedBrowserStorageResult<string> authToken = await protectedLocalStorage.GetAsync<string>("authToken");
            if (authToken.Success)
            {
                TokenActualizarRequest tokenRequest = new()
                {
                    AuthToken = authToken.Value
                };
                using HttpResponseMessage response = await httpClient.PostAsJsonAsync("auth/logout", tokenRequest);
        
                await protectedLocalStorage.DeleteAsync("authToken");  
            } 
            ((AuthStateProvider)authenticationStateProvider).NotifyUserLogout();
            httpClient.DefaultRequestHeaders.Authorization = null;
        }
        catch (HttpRequestException)
        {
            throw new HttpRequestException();
        } 
    }

    public async Task<string> ActualizarToken()
    {
        try
        {
            ProtectedBrowserStorageResult<string> authToken = await protectedLocalStorage.GetAsync<string>("authToken");
            TokenActualizarRequest tokenRequest = new()
            {
                AuthToken = authToken.Value
            };
            using HttpResponseMessage response = await httpClient.PutAsJsonAsync("auth/refresh-token", tokenRequest);
                
            if (response.IsSuccessStatusCode)
            {
                AutenticacionResponse result = await response.Content.ReadFromJsonAsync<AutenticacionResponse>();  
                await protectedLocalStorage.SetAsync("authToken", result.AuthToken);
                ((AuthStateProvider)authenticationStateProvider).NotifyUserAuthentication(result.AuthToken);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.AuthToken);
                return result.AuthToken;       
            }
            else
            { 
                error = await response.Content.ReadFromJsonAsync<ErrorEndpointResponse>();
                throw new HttpResponseException(message: error.Message[0].ToString(), code: error.Code.ToString());
            } 
        }
        catch (HttpRequestException)
        {
            throw new HttpRequestException();
        } 
    }
}
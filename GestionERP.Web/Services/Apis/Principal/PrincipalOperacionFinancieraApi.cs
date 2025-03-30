using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Services.Interfaces;
using System.Net.Http.Json; 
using System.Net;
using GestionERP.Web.Models.Responses;
using GestionERP.Web.Handlers;
using Microsoft.AspNetCore.WebUtilities;

namespace GestionERP.Web.Services.Apis;
 
public class PrincipalOperacionFinancieraApi : IPrincipalOperacionFinanciera
{
    private readonly HttpClient _httpClient;
    protected ErrorEndpointResponse error;
    private const string pathApi = "operaciones-financieras";

    public PrincipalOperacionFinancieraApi(HttpClient httpClient)
    {
        _httpClient = httpClient;
        error = new();
    }

    public async Task<IEnumerable<OperacionFinancieraListarDto>> Listar()
    {
        try
        {
            using HttpResponseMessage response = await _httpClient.GetAsync(pathApi); 
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent) 
                    return default;

                return await response.Content.ReadFromJsonAsync<IEnumerable<OperacionFinancieraListarDto>>(); 
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

    public async Task<IEnumerable<OperacionFinancieraCatalogoDto>> Catalogo()
    {
        try
        {
            using HttpResponseMessage response = await _httpClient.GetAsync($"{pathApi}/catalogo"); 
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent) 
                    return default;

                return await response.Content.ReadFromJsonAsync<IEnumerable<OperacionFinancieraCatalogoDto>>(); 
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

    public async Task Editar(Guid id, OperacionFinancieraEditarDto operacionFinanciera)
    {
        try
        {
            using HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"{pathApi}/{id}", operacionFinanciera); 
            if (!response.IsSuccessStatusCode)
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

    public async Task Eliminar(Guid id)
    {
        try
        {
            using HttpResponseMessage response = await _httpClient.DeleteAsync($"{pathApi}/{id}"); 
            if (!response.IsSuccessStatusCode)
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

    public async Task<Guid> Insertar(OperacionFinancieraInsertarDto operacionFinanciera)
    {
        try
        {
            using HttpResponseMessage response = await _httpClient.PostAsJsonAsync(pathApi, operacionFinanciera); 
            if (response.IsSuccessStatusCode)
            {
                return (await response.Content.ReadFromJsonAsync<OperacionFinancieraObtenerDto>()).Id; 
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

    public async Task<OperacionFinancieraObtenerDto> Obtener(Guid id)
    {
        try
        {
            using HttpResponseMessage response = await _httpClient.GetAsync($"{pathApi}/{id}"); 
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent) 
                    return default;

                return await response.Content.ReadFromJsonAsync<OperacionFinancieraObtenerDto>(); 
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
}
 

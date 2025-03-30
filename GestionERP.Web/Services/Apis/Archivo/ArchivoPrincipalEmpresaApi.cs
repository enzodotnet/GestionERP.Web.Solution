using GestionERP.Web.Services.Interfaces;
using System.Net.Http.Json; 
using System.Net;
using GestionERP.Web.Models.Responses;
using GestionERP.Web.Handlers; 
using Microsoft.AspNetCore.WebUtilities;
using GestionERP.Web.Models.Dtos.Archivo;

namespace GestionERP.Web.Services.Apis;
 
public class ArchivoPrincipalEmpresaApi(HttpClient httpClient) : IArchivoPrincipalEmpresa
{
    private readonly HttpClient _httpClient = httpClient;
    protected ErrorEndpointResponse error = new();
    private const string pathApi = "archivo/principal-empresas";

    public async Task<IEnumerable<PrincipalEmpresaListarDto>> Listar(Guid origenArchivoId)
    {
        try
        {
            Dictionary<string, string> query = new()
            {
                ["origenArchivoId"] = origenArchivoId.ToString()
            };
            using HttpResponseMessage response = await _httpClient.GetAsync(QueryHelpers.AddQueryString(pathApi, query)); 
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent) 
                    return default;

                return await response.Content.ReadFromJsonAsync<IEnumerable<PrincipalEmpresaListarDto>>(); 
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
    
    public async Task Eliminar(Guid archivoId)
    {
        try
        {
            using HttpResponseMessage response = await _httpClient.DeleteAsync($"{pathApi}/{archivoId}"); 
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
     
    public async Task<Guid> Insertar(string codigoEmpresa, PrincipalEmpresaInsertarDto archivo)
    {
        try
        {
            Dictionary<string, string> query = new()
            {
                ["codigoEmpresa"] = codigoEmpresa
            }; 
            using HttpResponseMessage response = await _httpClient.PostAsJsonAsync(QueryHelpers.AddQueryString(pathApi, query), archivo);
            if (response.IsSuccessStatusCode)
            {
                PrincipalEmpresaObtenerDto archivoCreado = await response.Content.ReadFromJsonAsync<PrincipalEmpresaObtenerDto>(); 
                return archivoCreado.Id;
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

    public async Task<PrincipalEmpresaObtenerDto> Obtener(Guid archivoId)
    {
        try
        {
            using HttpResponseMessage response = await _httpClient.GetAsync($"{pathApi}/{archivoId}"); 
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent) 
                    return default;

                return await response.Content.ReadFromJsonAsync<PrincipalEmpresaObtenerDto>(); 
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
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Services.Interfaces;
using System.Net.Http.Json; 
using System.Net;
using GestionERP.Web.Models.Responses;
using GestionERP.Web.Handlers; 

namespace GestionERP.Web.Services.Apis;
 
public class PrincipalChoferApi : IPrincipalChofer
{
    private readonly HttpClient _httpClient;
    protected ErrorEndpointResponse error;
    private const string pathApi = "choferes";

    public PrincipalChoferApi(HttpClient httpClient)
    {
        _httpClient = httpClient;
        error = new();
    }

    public async Task<IEnumerable<ChoferListarDto>> Listar()
    {
        try
        {
            using HttpResponseMessage response = await _httpClient.GetAsync(pathApi); 
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent) 
                    return default;

                return await response.Content.ReadFromJsonAsync<IEnumerable<ChoferListarDto>>(); 
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

    public async Task<IEnumerable<ChoferCatalogoDto>> Catalogo()
    {
        try
        {
            using HttpResponseMessage response = await _httpClient.GetAsync($"{pathApi}/catalogo"); 
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent) 
                    return default;

                return await response.Content.ReadFromJsonAsync<IEnumerable<ChoferCatalogoDto>>(); 
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

    public async Task Editar(Guid id, ChoferEditarDto chofer)
    {
        try
        {
            using HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"{pathApi}/{id}", chofer); 
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

    public async Task<Guid> Insertar(ChoferInsertarDto chofer)
    {
        try
        {
            using HttpResponseMessage response = await _httpClient.PostAsJsonAsync(pathApi, chofer); 
            if (response.IsSuccessStatusCode)
            {
                return (await response.Content.ReadFromJsonAsync<ChoferObtenerDto>()).Id; 
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

    public async Task<ChoferObtenerDto> Obtener(Guid id)
    {
        try
        {
            using HttpResponseMessage response = await _httpClient.GetAsync($"{pathApi}/{id}"); 
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent) 
                    return default;

                return await response.Content.ReadFromJsonAsync<ChoferObtenerDto>(); 
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
 

using GestionERP.Web.Models.Dtos.Produccion;
using GestionERP.Web.Services.Interfaces;
using System.Net.Http.Json;
using System.Net;
using GestionERP.Web.Models.Responses;
using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Produccion.Funcion;

namespace GestionERP.Web.Services.Apis;
 
public class ProduccionFuncionApi(HttpClient httpClient) : IProduccionFuncion
{ 
    protected ErrorEndpointResponse error = new();
    private const string pathApi = "empresa/{ce}/produccion/funciones";

    public async Task<IEnumerable<FuncionListarDto>> Listar(string codigoEmpresa)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.GetAsync(pathApi.Replace("{ce}", codigoEmpresa));
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent) 
                    return default;

                return await response.Content.ReadFromJsonAsync<IEnumerable<FuncionListarDto>>(); 
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

    public async Task<Guid> Insertar(string codigoEmpresa, FuncionInsertarDto plan)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.PostAsJsonAsync(pathApi.Replace("{ce}", codigoEmpresa), plan); 
            if (response.IsSuccessStatusCode)
            {
                return (await response.Content.ReadFromJsonAsync<FuncionStruct>()).Id;
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

    public async Task Editar(string codigoEmpresa, Guid id, FuncionEditarDto plan)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.PutAsJsonAsync($"{pathApi.Replace("{ce}", codigoEmpresa)}/{id}", plan);
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

    public async Task<FuncionObtenerDto> Obtener(string codigoEmpresa, Guid id)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.GetAsync($"{pathApi.Replace("{ce}", codigoEmpresa)}/{id}");
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return default;

                return await response.Content.ReadFromJsonAsync<FuncionObtenerDto>();
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

    public async Task Eliminar(string codigoEmpresa, Guid id)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.DeleteAsync($"{pathApi.Replace("{ce}", codigoEmpresa)}/{id}");
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

    public async Task<IEnumerable<FuncionCatalogoDto>> Catalogo(string codigoEmpresa)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.GetAsync($"{pathApi.Replace("{ce}", codigoEmpresa)}/catalogo"); 
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return default;
                return await response.Content.ReadFromJsonAsync<IEnumerable<FuncionCatalogoDto>>();
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
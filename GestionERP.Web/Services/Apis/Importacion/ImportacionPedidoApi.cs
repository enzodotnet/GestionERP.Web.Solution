using GestionERP.Web.Models.Dtos.Importacion;
using GestionERP.Web.Services.Interfaces;
using System.Net.Http.Json;
using System.Net;
using GestionERP.Web.Models.Responses;
using GestionERP.Web.Handlers;
using Microsoft.AspNetCore.WebUtilities;
using GestionERP.Web.Models.Requests; 

namespace GestionERP.Web.Services.Apis;
 
public class ImportacionPedidoApi(HttpClient httpClient) : IImportacionPedido
{
    private readonly HttpClient _httpClient = httpClient;
    protected ErrorEndpointResponse error = new();
    private const string pathApi = "empresa/{ce}/importacion/pedidos";

    public async Task<IEnumerable<PedidoListarDto>> Listar(string codigoEmpresa, string codigoEjercicio, string codigoPeriodo = null)
    {
        try
        { 
            Dictionary<string, string> query = new()
            {  
                ["codigoEjercicio"] = codigoEjercicio,
                ["codigoPeriodo"] = codigoPeriodo ?? ""
            };
            using HttpResponseMessage response = await _httpClient.GetAsync(QueryHelpers.AddQueryString(pathApi.Replace("{ce}",codigoEmpresa), query));
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent) 
                    return default;

                return await response.Content.ReadFromJsonAsync<IEnumerable<PedidoListarDto>>(); 
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

    public async Task<Guid> Insertar(string codigoEmpresa, PedidoInsertarDto orden)
    {
        try
        {
            using HttpResponseMessage response = await _httpClient.PostAsJsonAsync(pathApi.Replace("{ce}", codigoEmpresa), orden); 
            if (response.IsSuccessStatusCode)
            {
				return (await response.Content.ReadFromJsonAsync<PedidoStruct>()).Id;
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

    public async Task Editar(string codigoEmpresa, Guid id, PedidoEditarDto orden)
    {
        try
        {
            using HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"{pathApi.Replace("{ce}", codigoEmpresa)}/{id}", orden);
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

    public async Task<PedidoObtenerDto> Obtener(string codigoEmpresa, Guid id)
    {
        try
        {
            using HttpResponseMessage response = await _httpClient.GetAsync($"{pathApi.Replace("{ce}", codigoEmpresa)}/{id}");
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return default;

                return await response.Content.ReadFromJsonAsync<PedidoObtenerDto>();
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
            using HttpResponseMessage response = await _httpClient.DeleteAsync($"{pathApi.Replace("{ce}", codigoEmpresa)}/{id}");
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

    public async Task ActualizarEstado(string codigoEmpresa, EstadoActualizarRequest estadoActualizar)
    {
        try
        {
            using HttpResponseMessage response = await _httpClient.PatchAsJsonAsync($"{pathApi.Replace("{ce}", codigoEmpresa)}/estado", estadoActualizar); 
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
}
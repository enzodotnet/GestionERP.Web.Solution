using GestionERP.Web.Models.Dtos.Servicio;
using GestionERP.Web.Services.Interfaces;
using System.Net.Http.Json;
using System.Net;
using GestionERP.Web.Models.Responses;
using GestionERP.Web.Handlers;
using Microsoft.AspNetCore.WebUtilities;
using GestionERP.Web.Models.Requests;

namespace GestionERP.Web.Services.Apis;
 
public class ServicioContratoApi(HttpClient httpClient) : IServicioContrato
{
    private readonly HttpClient _httpClient = httpClient;
    protected ErrorEndpointResponse error = new();
    private const string pathApi = "empresa/{ce}/servicio/contratos";

    public async Task<IEnumerable<ContratoListarDto>> Listar(string codigoEmpresa, string codigoEjercicio, string codigoPeriodo = null, string flagTipoRegistro = null, string flagEstadoDevengo = null)
    {
        try
        { 
            Dictionary<string, string> query = new()
            {  
                ["codigoEjercicio"] = codigoEjercicio,
                ["codigoPeriodo"] = codigoPeriodo ?? "",
                ["flagTipoRegistro"] = flagTipoRegistro ?? "",
                ["flagEstadoDevengo"] = flagEstadoDevengo ?? ""
            };
            using HttpResponseMessage response = await _httpClient.GetAsync(QueryHelpers.AddQueryString(pathApi.Replace("{ce}",codigoEmpresa), query));
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent) 
                    return default;

                return await response.Content.ReadFromJsonAsync<IEnumerable<ContratoListarDto>>(); 
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

    public async Task<Guid> Insertar(string codigoEmpresa, ContratoInsertarDto contrato)
    {
        try
        {
            using HttpResponseMessage response = await _httpClient.PostAsJsonAsync(pathApi.Replace("{ce}", codigoEmpresa), contrato); 
            if (response.IsSuccessStatusCode)
            {
                return (await response.Content.ReadFromJsonAsync<ContratoStruct>()).Id; 
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

    public async Task Editar(string codigoEmpresa, Guid id, ContratoEditarDto contrato)
    {
        try
        {
            using HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"{pathApi.Replace("{ce}", codigoEmpresa)}/{id}", contrato);
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

    public async Task<ContratoObtenerDto> Obtener(string codigoEmpresa, Guid id)
    {
        try
        {
            using HttpResponseMessage response = await _httpClient.GetAsync($"{pathApi.Replace("{ce}", codigoEmpresa)}/{id}");
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return default;

                return await response.Content.ReadFromJsonAsync<ContratoObtenerDto>();
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

    public async Task<IEnumerable<ContratoCatalogoActualizarEstadoDto>> CatalogoActualizarEstado(string codigoEmpresa, string codigoEstado)
    {
        try
        { 
            Dictionary<string, string> query = new()
            {  
                ["codigoEstado"] = codigoEstado 
            };
            using HttpResponseMessage response = await _httpClient.GetAsync(QueryHelpers.AddQueryString($"{pathApi.Replace("{ce}", codigoEmpresa)}/catalogo/actualizar/estado", query)); 
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return default;

                return await response.Content.ReadFromJsonAsync<IEnumerable<ContratoCatalogoActualizarEstadoDto>>();
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
      

    public async Task<IEnumerable<ContratoCatalogoProvisionarDto>> CatalogoProvisionar(string codigoEmpresa, string flagRegistroProvision, string flagRegistroOrigen)
    {
        try
        {
            Dictionary<string, string> query = new()
            {  
                ["flagRegistroProvision"] = flagRegistroProvision,
                ["flagRegistroOrigen"] = flagRegistroOrigen
            };
            using HttpResponseMessage response = await _httpClient.GetAsync(QueryHelpers.AddQueryString($"{pathApi.Replace("{ce}", codigoEmpresa)}/catalogo/provisionar", query));
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return default;

                return await response.Content.ReadFromJsonAsync<IEnumerable<ContratoCatalogoProvisionarDto>>();
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

    public async Task<IEnumerable<ContratoCatalogoDevengarDto>> CatalogoDevengar(string codigoEmpresa)
    {
        try
        {
            using HttpResponseMessage response = await _httpClient.GetAsync($"{pathApi.Replace("{ce}", codigoEmpresa)}/catalogo/devengar");
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return default;

                return await response.Content.ReadFromJsonAsync<IEnumerable<ContratoCatalogoDevengarDto>>();
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
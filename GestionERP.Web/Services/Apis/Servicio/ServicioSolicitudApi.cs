using GestionERP.Web.Models.Dtos.Servicio;
using GestionERP.Web.Services.Interfaces;
using System.Net.Http.Json;
using System.Net;
using GestionERP.Web.Models.Responses;
using GestionERP.Web.Handlers;
using Microsoft.AspNetCore.WebUtilities;
using GestionERP.Web.Models.Requests;

namespace GestionERP.Web.Services.Apis;
 
public class ServicioSolicitudApi(HttpClient httpClient) : IServicioSolicitud
{
    private readonly HttpClient _httpClient = httpClient;
    protected ErrorEndpointResponse error = new();
    private const string pathApi = "empresa/{ce}/servicio/solicitudes";

    public async Task<IEnumerable<SolicitudListarDto>> Listar(string codigoEmpresa, string codigoEjercicio, string codigoPeriodo = null)
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

                return await response.Content.ReadFromJsonAsync<IEnumerable<SolicitudListarDto>>(); 
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

    public async Task<Guid> Insertar(string codigoEmpresa, SolicitudInsertarDto solicitud)
    {
        try
        {
            using HttpResponseMessage response = await _httpClient.PostAsJsonAsync(pathApi.Replace("{ce}", codigoEmpresa), solicitud); 
            if (response.IsSuccessStatusCode)
            {
				return (await response.Content.ReadFromJsonAsync<SolicitudStruct>()).Id;
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

    public async Task Editar(string codigoEmpresa, Guid id, SolicitudEditarDto solicitud)
    {
        try
        {
            using HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"{pathApi.Replace("{ce}", codigoEmpresa)}/{id}", solicitud);
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

    public async Task<SolicitudObtenerDto> Obtener(string codigoEmpresa, Guid id)
    {
        try
        {
            using HttpResponseMessage response = await _httpClient.GetAsync($"{pathApi.Replace("{ce}", codigoEmpresa)}/{id}");
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return default;

                return await response.Content.ReadFromJsonAsync<SolicitudObtenerDto>();
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

    public async Task<IEnumerable<SolicitudCatalogoActualizarEstadoDto>> CatalogoActualizarEstado(string codigoEmpresa, string codigoEstado)
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

                return await response.Content.ReadFromJsonAsync<IEnumerable<SolicitudCatalogoActualizarEstadoDto>>();
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

    public async Task<IEnumerable<SolicitudCatalogoAtenderDto>> CatalogoAtender(string codigoEmpresa, string codigoEjercicio, string codigoProcesoDocumento, string codigoLocal = null)
    {
        try
        { 
            Dictionary<string, string> query = new()
            {  
                ["codigoEjercicio"] = codigoEjercicio,
                ["codigoProcesoDocumento"] = codigoProcesoDocumento,
                ["codigoLocal"] = codigoLocal ?? "", 
            };
            using HttpResponseMessage response = await _httpClient.GetAsync(QueryHelpers.AddQueryString($"{pathApi.Replace("{ce}", codigoEmpresa)}/catalogo/atender", query)); 
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return default;

                return await response.Content.ReadFromJsonAsync<IEnumerable<SolicitudCatalogoAtenderDto>>();
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
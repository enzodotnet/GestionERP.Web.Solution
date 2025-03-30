using GestionERP.Web.Models.Dtos.Produccion;
using GestionERP.Web.Services.Interfaces;
using System.Net.Http.Json;
using System.Net;
using GestionERP.Web.Models.Responses;
using GestionERP.Web.Handlers;
using Microsoft.AspNetCore.WebUtilities;
using GestionERP.Web.Models.Requests;

namespace GestionERP.Web.Services.Apis;
 
public class ProduccionOrdenApi(HttpClient httpClient) : IProduccionOrden
{ 
    protected ErrorEndpointResponse error = new();
    private const string pathApi = "empresa/{ce}/produccion/ordenes";

    public async Task<IEnumerable<OrdenListarDto>> Listar(string codigoEmpresa, string codigoEjercicio, string codigoPeriodo = null, string flagTipoRegistro = null, string flagOrigen = null)
    {
        try
        { 
            Dictionary<string, string> query = new()
            {  
                ["codigoEjercicio"] = codigoEjercicio,
                ["codigoPeriodo"] = codigoPeriodo ?? "",
                ["flagTipoRegistro"] = flagTipoRegistro ?? "",
                ["flagOrigen"] = flagOrigen ?? ""
            };
            using HttpResponseMessage response = await httpClient.GetAsync(QueryHelpers.AddQueryString(pathApi.Replace("{ce}",codigoEmpresa), query));
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent) 
                    return default;

                return await response.Content.ReadFromJsonAsync<IEnumerable<OrdenListarDto>>(); 
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

    public async Task<Guid> Insertar(string codigoEmpresa, OrdenInsertarDto orden)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.PostAsJsonAsync(pathApi.Replace("{ce}", codigoEmpresa), orden); 
            if (response.IsSuccessStatusCode)
            {
				return (await response.Content.ReadFromJsonAsync<OrdenStruct>()).Id;
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

    public async Task Editar(string codigoEmpresa, Guid id, OrdenEditarDto orden)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.PutAsJsonAsync($"{pathApi.Replace("{ce}", codigoEmpresa)}/{id}", orden);
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


    public async Task<OrdenObtenerDto> Obtener(string codigoEmpresa, Guid id)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.GetAsync($"{pathApi.Replace("{ce}", codigoEmpresa)}/{id}");
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return default;

                return await response.Content.ReadFromJsonAsync<OrdenObtenerDto>();
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

    public async Task<IEnumerable<OrdenCatalogoActualizarEstadoDto>> CatalogoActualizarEstado(string codigoEmpresa, string codigoEstado)
    {
        try
        { 
            Dictionary<string, string> query = new()
            {  
                ["codigoEstado"] = codigoEstado 
            };
            using HttpResponseMessage response = await httpClient.GetAsync(QueryHelpers.AddQueryString($"{pathApi.Replace("{ce}", codigoEmpresa)}/catalogo/actualizar/estado", query)); 
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return default;

                return await response.Content.ReadFromJsonAsync<IEnumerable<OrdenCatalogoActualizarEstadoDto>>();
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
            using HttpResponseMessage response = await httpClient.PatchAsJsonAsync($"{pathApi.Replace("{ce}", codigoEmpresa)}/estado", estadoActualizar); 
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

    public async Task ActualizarComentarioProceso(string codigoEmpresa, OrdenActualizarComentarioProcesoDto orden)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.PatchAsJsonAsync($"{pathApi.Replace("{ce}", codigoEmpresa)}/comentario-proceso", orden);
            if (!response.IsSuccessStatusCode)
            {
                error = response.StatusCode == HttpStatusCode.NotFound ? new() { Code = "NF" } : await response.Content.ReadFromJsonAsync<ErrorEndpointResponse>();
                throw new HttpResponseException(error.Message, error.Code);
            }
        }
        catch (HttpRequestException)
        {
            throw new HttpRequestException();
        }
    }

    public async Task ActualizarFechaCosto(string codigoEmpresa, OrdenActualizarFechaCostoDto orden)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.PatchAsJsonAsync($"{pathApi.Replace("{ce}", codigoEmpresa)}/fecha-costo", orden);
            if (!response.IsSuccessStatusCode)
            {
                error = response.StatusCode == HttpStatusCode.NotFound ? new() { Code = "NF" } : await response.Content.ReadFromJsonAsync<ErrorEndpointResponse>();
                throw new HttpResponseException(error.Message, error.Code);
            }
        }
        catch (HttpRequestException)
        {
            throw new HttpRequestException();
        }
    }

    public async Task<IEnumerable<OrdenLoteListarDto>> ListarLotes(string codigoEmpresa, string codigoOrden)
    {
        try
        {
            Dictionary<string, string> query = new()
            {
                ["codigoOrden"] = codigoOrden
            };
            using HttpResponseMessage response = await httpClient.GetAsync(QueryHelpers.AddQueryString($"{pathApi.Replace("{ce}", codigoEmpresa)}/lotes", query));
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return default;

                return await response.Content.ReadFromJsonAsync<IEnumerable<OrdenLoteListarDto>>();
            }
            else
            {
                error = response.StatusCode == HttpStatusCode.NotFound ? new() { Code = "NF" } : await response.Content.ReadFromJsonAsync<ErrorEndpointResponse>();
                throw new HttpResponseException(error.Message, error.Code);
            }
        }
        catch (HttpRequestException)
        {
            throw new HttpRequestException();
        } 
    }

    public async Task InsertarLote(string codigoEmpresa, OrdenLoteInsertarDto lote)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.PostAsJsonAsync($"{pathApi.Replace("{ce}", codigoEmpresa)}/lotes", lote);
            if (!response.IsSuccessStatusCode)
            {
                error = response.StatusCode == HttpStatusCode.NotFound ? new() { Code = "NF" } : await response.Content.ReadFromJsonAsync<ErrorEndpointResponse>();
                throw new HttpResponseException(error.Message, error.Code);
            }
        }
        catch (HttpRequestException)
        {
            throw new HttpRequestException();
        }
    }

    public async Task EditarLote(string codigoEmpresa, Guid id, OrdenLoteEditarDto lote)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.PutAsJsonAsync($"{pathApi.Replace("{ce}", codigoEmpresa)}/lotes/{id}", lote);
            if (!response.IsSuccessStatusCode)
            {
                error = response.StatusCode == HttpStatusCode.NotFound ? new() { Code = "NF" } : await response.Content.ReadFromJsonAsync<ErrorEndpointResponse>();
                throw new HttpResponseException(error.Message, error.Code);
            }
        }
        catch (HttpRequestException)
        {
            throw new HttpRequestException();
        }
    }

    public async Task EliminarLote(string codigoEmpresa, Guid id)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.DeleteAsync($"{pathApi.Replace("{ce}", codigoEmpresa)}/lotes/{id}");
            if (!response.IsSuccessStatusCode)
            {
                error = response.StatusCode == HttpStatusCode.NotFound ? new() { Code = "NF" } : await response.Content.ReadFromJsonAsync<ErrorEndpointResponse>();
                throw new HttpResponseException(error.Message, error.Code);
            }
        }
        catch (HttpRequestException)
        {
            throw new HttpRequestException();
        }
    }

    public async Task<IEnumerable<OrdenMaquilaListarDto>> ListarMaquilas(string codigoEmpresa, string codigoOrden)
    { 
        try
        {
            Dictionary<string, string> query = new()
            {
                ["codigoOrden"] = codigoOrden
            };
            using HttpResponseMessage response = await httpClient.GetAsync(QueryHelpers.AddQueryString($"{pathApi.Replace("{ce}", codigoEmpresa)}/maquilas", query));
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return default;

                return await response.Content.ReadFromJsonAsync<IEnumerable<OrdenMaquilaListarDto>>();
            }
            else
            {
                error = response.StatusCode == HttpStatusCode.NotFound ? new() { Code = "NF" } : await response.Content.ReadFromJsonAsync<ErrorEndpointResponse>();
                throw new HttpResponseException(error.Message, error.Code);
            }
        }
        catch (HttpRequestException)
        {
            throw new HttpRequestException();
        } 
    }

    public async Task InsertarMaquila(string codigoEmpresa, OrdenMaquilaInsertarDto maquila)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.PostAsJsonAsync($"{pathApi.Replace("{ce}", codigoEmpresa)}/maquilas", maquila);
            if (!response.IsSuccessStatusCode) 
            {
                error = response.StatusCode == HttpStatusCode.NotFound ? new() { Code = "NF" } : await response.Content.ReadFromJsonAsync<ErrorEndpointResponse>();
                throw new HttpResponseException(error.Message, error.Code);
            }
        }
        catch (HttpRequestException)
        {
            throw new HttpRequestException();
        }
    }

    public async Task EditarMaquila(string codigoEmpresa, Guid id, OrdenMaquilaEditarDto maquila)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.PutAsJsonAsync($"{pathApi.Replace("{ce}", codigoEmpresa)}/maquilas/{id}", maquila);
            if (!response.IsSuccessStatusCode)
            {
                error = response.StatusCode == HttpStatusCode.NotFound ? new() { Code = "NF" } : await response.Content.ReadFromJsonAsync<ErrorEndpointResponse>();
                throw new HttpResponseException(error.Message, error.Code);
            }
        }
        catch (HttpRequestException)
        {
            throw new HttpRequestException();
        }
    }

    public async Task EliminarMaquila(string codigoEmpresa, Guid id)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.DeleteAsync($"{pathApi.Replace("{ce}", codigoEmpresa)}/maquilas/{id}");
            if (!response.IsSuccessStatusCode)
            {
                error = response.StatusCode == HttpStatusCode.NotFound ? new() { Code = "NF" } : await response.Content.ReadFromJsonAsync<ErrorEndpointResponse>();
                throw new HttpResponseException(error.Message, error.Code);
            }
        }
        catch (HttpRequestException)
        {
            throw new HttpRequestException();
        }
    }

    public async Task<IEnumerable<OrdenTareoListarDto>> ListarTareos(string codigoEmpresa, string codigoOrden)
    {
        try
        {
            Dictionary<string, string> query = new()
            {
                ["codigoOrden"] = codigoOrden
            };
            using HttpResponseMessage response = await httpClient.GetAsync(QueryHelpers.AddQueryString($"{pathApi.Replace("{ce}", codigoEmpresa)}/tareos", query));
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return default;

                return await response.Content.ReadFromJsonAsync<IEnumerable<OrdenTareoListarDto>>();
            }
            else
            {
                error = response.StatusCode == HttpStatusCode.NotFound ? new() { Code = "NF" } : await response.Content.ReadFromJsonAsync<ErrorEndpointResponse>();
                throw new HttpResponseException(error.Message, error.Code);
            }
        }
        catch (HttpRequestException)
        {
            throw new HttpRequestException();
        } 
    }

    public async Task InsertarTareo(string codigoEmpresa, OrdenTareoInsertarDto tareo)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.PostAsJsonAsync($"{pathApi.Replace("{ce}", codigoEmpresa)}/tareos", tareo);
            if (!response.IsSuccessStatusCode)
            {
                error = response.StatusCode == HttpStatusCode.NotFound ? new() { Code = "NF" } : await response.Content.ReadFromJsonAsync<ErrorEndpointResponse>();
                throw new HttpResponseException(error.Message, error.Code);
            }
        }
        catch (HttpRequestException)
        {
            throw new HttpRequestException();
        }
    }

    public async Task EditarTareo(string codigoEmpresa, Guid id, OrdenTareoEditarDto tareo)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.PutAsJsonAsync($"{pathApi.Replace("{ce}", codigoEmpresa)}/tareos/{id}", tareo);
            if (!response.IsSuccessStatusCode)
            {
                error = response.StatusCode == HttpStatusCode.NotFound ? new() { Code = "NF" } : await response.Content.ReadFromJsonAsync<ErrorEndpointResponse>();
                throw new HttpResponseException(error.Message, error.Code);
            }
        }
        catch (HttpRequestException)
        {
            throw new HttpRequestException();
        }
    }

    public async Task EliminarTareo(string codigoEmpresa, Guid id)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.DeleteAsync($"{pathApi.Replace("{ce}", codigoEmpresa)}/tareos/{id}");
            if (!response.IsSuccessStatusCode)
            {
                error = response.StatusCode == HttpStatusCode.NotFound ? new() { Code = "NF" } : await response.Content.ReadFromJsonAsync<ErrorEndpointResponse>();
                throw new HttpResponseException(error.Message, error.Code);
            }
        }
        catch (HttpRequestException)
        {
            throw new HttpRequestException();
        }
    }
}
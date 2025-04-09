using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Services.Interfaces; 
using System.Net;
using GestionERP.Web.Models.Responses;
using GestionERP.Web.Handlers;
using Microsoft.AspNetCore.WebUtilities;

namespace GestionERP.Web.Services.Apis;
 
public class PrincipalAlmacenApi(HttpClient httpClient) : IPrincipalAlmacen
{
    protected ErrorEndpointResponse error = new();
    private const string pathApi = "almacenes";

    public async Task<IEnumerable<AlmacenListarDto>> Listar()
    {
        try
        {
            using HttpResponseMessage response = await httpClient.GetAsync(pathApi); 
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent) 
                    return default;

                return await response.Content.ReadFromJsonAsync<IEnumerable<AlmacenListarDto>>(); 
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

    public async Task<IEnumerable<AlmacenCatalogoDto>> Catalogo()
    {
        try
        {
            using HttpResponseMessage response = await httpClient.GetAsync($"{pathApi}/catalogo"); 
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent) 
                    return default;

                return await response.Content.ReadFromJsonAsync<IEnumerable<AlmacenCatalogoDto>>(); 
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

    public async Task Editar(Guid id, AlmacenEditarDto almacen)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.PutAsJsonAsync($"{pathApi}/{id}", almacen); 
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
            using HttpResponseMessage response = await httpClient.DeleteAsync($"{pathApi}/{id}"); 
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

    public async Task<Guid> Insertar(AlmacenInsertarDto almacen)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.PostAsJsonAsync(pathApi, almacen); 
            if (response.IsSuccessStatusCode)
            {
                return (await response.Content.ReadFromJsonAsync<AlmacenObtenerDto>()).Id; 
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

    public async Task<AlmacenObtenerDto> Obtener(Guid id)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.GetAsync($"{pathApi}/{id}"); 
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent) 
                    return default;

                return await response.Content.ReadFromJsonAsync<AlmacenObtenerDto>(); 
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

    public async Task<IEnumerable<AlmacenCatalogoPorEmpresaOperacionLogisticaSesionDto>> CatalogoPorEmpresaOperacionLogisticaSesion(string codigoEmpresa, string codigoOperacionLogistica, string codigoTipoArticulo = null, string codigoAlmacenDestino = null)
    {
        try
        {
            Dictionary<string, string> query = new()
            {
                ["codigoEmpresa"] = codigoEmpresa,
                ["codigoOperacionLogistica"] = codigoOperacionLogistica,
                ["codigoTipoArticulo"] = codigoTipoArticulo ?? "",
                ["codigoAlmacenDestino"] = codigoAlmacenDestino ?? ""
            };
            using HttpResponseMessage response = await httpClient.GetAsync(QueryHelpers.AddQueryString($"{pathApi}/catalogo/empresa/operacion-logistica/sesion", query));
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return default;
                return await response.Content.ReadFromJsonAsync<IEnumerable<AlmacenCatalogoPorEmpresaOperacionLogisticaSesionDto>>();
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

    public async Task<AlmacenObtenerPorCodigoEmpresaOperacionLogisticaSesionDto> ObtenerPorCodigoEmpresaOperacionLogisticaSesion(string codigoAlmacen, string codigoEmpresa, string codigoOperacionLogistica, string codigoTipoArticulo = null, string codigoAlmacenDestino = null)
    {
        try
        {
            Dictionary<string, string> query = new()
            {
                ["codigoEmpresa"] = codigoEmpresa,
                ["codigoOperacionLogistica"] = codigoOperacionLogistica,
                ["codigoTipoArticulo"] = codigoTipoArticulo ?? "",
                ["codigoAlmacenDestino"] = codigoAlmacenDestino ?? ""
            };
            using HttpResponseMessage response = await httpClient.GetAsync(QueryHelpers.AddQueryString($"{pathApi}/codigo/{codigoAlmacen}/empresa/operacion-logistica/sesion",query));
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AlmacenObtenerPorCodigoEmpresaOperacionLogisticaSesionDto>();
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
 

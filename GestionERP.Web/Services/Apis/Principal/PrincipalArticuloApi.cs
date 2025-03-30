using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Services.Interfaces;
using System.Net.Http.Json; 
using System.Net;
using GestionERP.Web.Models.Responses;
using GestionERP.Web.Handlers;
using Microsoft.AspNetCore.WebUtilities;

namespace GestionERP.Web.Services.Apis;
 
public class PrincipalArticuloApi(HttpClient httpClient) : IPrincipalArticulo
{
    protected ErrorEndpointResponse error = new();
    private const string pathApi = "articulos";

    public async Task<IEnumerable<ArticuloListarDto>> Listar()
    {
        try
        {
            using HttpResponseMessage response = await httpClient.GetAsync(pathApi); 
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent) 
                    return default;

                return await response.Content.ReadFromJsonAsync<IEnumerable<ArticuloListarDto>>(); 
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

    public async Task Editar(Guid id, ArticuloEditarDto articulo)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.PutAsJsonAsync($"{pathApi}/{id}", articulo); 
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

    public async Task<Guid> Insertar(ArticuloInsertarDto articulo)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.PostAsJsonAsync(pathApi, articulo); 
            if (response.IsSuccessStatusCode)
            {
                return (await response.Content.ReadFromJsonAsync<ArticuloObtenerDto>()).Id; 
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

    public async Task<ArticuloObtenerDto> Obtener(Guid id)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.GetAsync($"{pathApi}/{id}"); 
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent) 
                    return default;

                return await response.Content.ReadFromJsonAsync<ArticuloObtenerDto>(); 
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

    public async Task<ArticuloObtenerPorCodigoEmpresaDto> ObtenerPorCodigoEmpresa(string codigoArticulo, string codigoEmpresa)
    {
        try
        {
            Dictionary<string, string> query = new()
			{
				["codigoEmpresa"] = codigoEmpresa
			};
            using HttpResponseMessage response = await httpClient.GetAsync(QueryHelpers.AddQueryString($"{pathApi}/codigo/{codigoArticulo}/empresa", query));
            if (response.IsSuccessStatusCode)
            { 
                return await response.Content.ReadFromJsonAsync<ArticuloObtenerPorCodigoEmpresaDto>();
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

    public async Task<ArticuloObtenerPorCodigoEmpresaProcesoDocumentoDto> ObtenerPorCodigoEmpresaProcesoDocumento(string codigoEmpresa, string codigoArticulo, string codigoProcesoDocumento)
    {
        try
        {
            Dictionary<string, string> query = new()
			{
				["codigoEmpresa"] = codigoEmpresa,
				["codigoProcesoDocumento"] = codigoProcesoDocumento
			};
            using HttpResponseMessage response = await httpClient.GetAsync(QueryHelpers.AddQueryString($"{pathApi}/{codigoArticulo}/empresa/proceso-documento", query));
            if (response.IsSuccessStatusCode) 
                return await response.Content.ReadFromJsonAsync<ArticuloObtenerPorCodigoEmpresaProcesoDocumentoDto>(); 
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

    public async Task<IEnumerable<ArticuloCatalogoPorEmpresaDto>> CatalogoPorEmpresa(string codigoEmpresa)
    {
        try
        {
            Dictionary<string, string> query = new()
			{
				["codigoEmpresa"] = codigoEmpresa
			};
            using HttpResponseMessage response = await httpClient.GetAsync(QueryHelpers.AddQueryString($"{pathApi}/catalogo/empresa", query)); 
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return default;

                return await response.Content.ReadFromJsonAsync<IEnumerable<ArticuloCatalogoPorEmpresaDto>>();
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

	public async Task<IEnumerable<ArticuloCatalogoPorEmpresaProcesoDocumentoDto>> CatalogoPorEmpresaProcesoDocumento(string codigoEmpresa, string codigoProcesoDocumento)
	{
		try
		{
			Dictionary<string, string> query = new()
			{
                ["codigoEmpresa"] = codigoEmpresa,
				["codigoProcesoDocumento"] = codigoProcesoDocumento
			};
			using HttpResponseMessage response = await httpClient.GetAsync(QueryHelpers.AddQueryString($"{pathApi}/catalogo/empresa/proceso-documento", query));
			if (response.IsSuccessStatusCode)
			{
				if (response.StatusCode == HttpStatusCode.NoContent)
					return default;

				return await response.Content.ReadFromJsonAsync<IEnumerable<ArticuloCatalogoPorEmpresaProcesoDocumentoDto>>();
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
} 
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Services.Interfaces;
using System.Net.Http.Json;
using System.Net;
using GestionERP.Web.Models.Responses;
using GestionERP.Web.Handlers;

namespace GestionERP.Web.Services.Apis;
 
public class PrincipalTipoDevengoApi(HttpClient httpClient) : IPrincipalTipoDevengo
{
    protected ErrorEndpointResponse error = new();
    private const string pathApi = "tipos-devengo";

    public async Task<IEnumerable<TipoDevengoListarDto>> Listar()
    {
        try
        { 
            using HttpResponseMessage response = await httpClient.GetAsync(pathApi);
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent) 
                    return default;

                return await response.Content.ReadFromJsonAsync<IEnumerable<TipoDevengoListarDto>>(); 
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

    public async Task<Guid> Insertar(TipoDevengoInsertarDto tipoDevengo)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.PostAsJsonAsync(pathApi, tipoDevengo); 
            if (response.IsSuccessStatusCode)
            {
                TipoDevengoObtenerDto tipoDevengoCreado = await response.Content.ReadFromJsonAsync<TipoDevengoObtenerDto>(); 
                return tipoDevengoCreado.Id;
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

    public async Task Editar(Guid id, TipoDevengoEditarDto tipoDevengo)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.PutAsJsonAsync($"{pathApi}/{id}", tipoDevengo);
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

    public async Task<TipoDevengoObtenerDto> Obtener(Guid id)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.GetAsync($"{pathApi}/{id}");
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return default;

                return await response.Content.ReadFromJsonAsync<TipoDevengoObtenerDto>();
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

	public async Task<TipoDevengoObtenerPorCodigoDto> ObtenerPorCodigo(string codigoTipoDevengo)
	{
		try
		{
			using HttpResponseMessage response = await httpClient.GetAsync($"{pathApi}/codigo/{codigoTipoDevengo}");
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<TipoDevengoObtenerPorCodigoDto>();
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

	public async Task<IEnumerable<TipoDevengoCatalogoDto>> Catalogo()
	{
		try
		{
			using HttpResponseMessage response = await httpClient.GetAsync($"{pathApi}/catalogo");
			if (response.IsSuccessStatusCode)
			{
				if (response.StatusCode == HttpStatusCode.NoContent)
					return default;

				return await response.Content.ReadFromJsonAsync<IEnumerable<TipoDevengoCatalogoDto>>();
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
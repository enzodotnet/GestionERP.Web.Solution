using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Services.Interfaces;
using System.Net.Http.Json; 
using System.Net;
using GestionERP.Web.Models.Responses;
using GestionERP.Web.Handlers;
using Microsoft.AspNetCore.WebUtilities;

namespace GestionERP.Web.Services.Apis;
 
public class PrincipalMonedaApi(HttpClient httpClient) : IPrincipalMoneda
{
	protected ErrorEndpointResponse error = new();
    private const string pathApi = "monedas";

	public async Task<IEnumerable<MonedaListarDto>> Listar()
    {
        try
        {
            using HttpResponseMessage response = await httpClient.GetAsync(pathApi); 
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent) 
                    return default;

                return await response.Content.ReadFromJsonAsync<IEnumerable<MonedaListarDto>>(); 
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

    public async Task<IEnumerable<MonedaCatalogoDto>> Catalogo()
    {
        try
        {
            using HttpResponseMessage response = await httpClient.GetAsync($"{pathApi}/catalogo"); 
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent) 
                    return default;

                return await response.Content.ReadFromJsonAsync<IEnumerable<MonedaCatalogoDto>>(); 
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

    public async Task<MonedaObtenerDto> Obtener(Guid id)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.GetAsync($"{pathApi}/{id}"); 
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent) 
                    return default;

                return await response.Content.ReadFromJsonAsync<MonedaObtenerDto>(); 
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

    public async Task<MonedaObtenerPorCodigoDto> ObtenerPorCodigo(string codigoMoneda)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.GetAsync($"{pathApi}/codigo/{codigoMoneda}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<MonedaObtenerPorCodigoDto>();
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

    public async Task<MonedaConsultaPorTipoDto> ConsultaPorTipo(string flagTipo)
    {
        try
        { 
            using HttpResponseMessage response = await httpClient.GetAsync($"{pathApi}/consulta/tipo/{flagTipo}");
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return default;

                return await response.Content.ReadFromJsonAsync<MonedaConsultaPorTipoDto>();
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
 

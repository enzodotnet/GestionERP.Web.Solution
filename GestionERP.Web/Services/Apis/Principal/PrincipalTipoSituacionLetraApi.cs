using GestionERP.Web.Services.Interfaces;
using System.Net.Http.Json;
using System.Net;
using GestionERP.Web.Models.Responses;
using GestionERP.Web.Handlers;
using Microsoft.AspNetCore.WebUtilities;
using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Services.Apis.Principal;

public class PrincipalTipoSituacionLetraApi(HttpClient httpClient) : IPrincipalTipoSituacionLetra
{
	protected ErrorEndpointResponse error = new();
    private const string pathApi = "tipos-situacion-letra";

	public async Task<IEnumerable<TipoSituacionLetraListarDto>> Listar()
    {
        try
        {
            using HttpResponseMessage response = await httpClient.GetAsync(pathApi);
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return default;

                return await response.Content.ReadFromJsonAsync<IEnumerable<TipoSituacionLetraListarDto>>();
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

    public async Task<Guid> Insertar(TipoSituacionLetraInsertarDto tipoSituacionLetra)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.PostAsJsonAsync(pathApi, tipoSituacionLetra);
            if (response.IsSuccessStatusCode)
            {
                TipoSituacionLetraObtenerDto tipoSituacionLetraCreado = await response.Content.ReadFromJsonAsync<TipoSituacionLetraObtenerDto>();
                return tipoSituacionLetraCreado.Id;
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

    public async Task Editar(Guid id, TipoSituacionLetraEditarDto tipoSituacionLetra)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.PutAsJsonAsync($"{pathApi}/{id}", tipoSituacionLetra);
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

    public async Task<TipoSituacionLetraObtenerDto> Obtener(Guid id)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.GetAsync($"{pathApi}/{id}");
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return default;

                return await response.Content.ReadFromJsonAsync<TipoSituacionLetraObtenerDto>();
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

    public async Task Eliminar(Guid id)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.DeleteAsync($"{pathApi}/{id}");
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

    public async Task<IEnumerable<TipoSituacionLetraCatalogoDto>> Catalogo()
    {
        try
        {
            using HttpResponseMessage response = await httpClient.GetAsync($"{pathApi}/catalogo");
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return default;

                return await response.Content.ReadFromJsonAsync<IEnumerable<TipoSituacionLetraCatalogoDto>>();
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
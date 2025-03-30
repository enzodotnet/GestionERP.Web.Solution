using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Services.Interfaces;
using System.Net.Http.Json; 
using System.Net;
using GestionERP.Web.Models.Responses;
using GestionERP.Web.Handlers;

namespace GestionERP.Web.Services.Apis;

public class PrincipalGrupoGastoImportacionApi : IPrincipalGrupoGastoImportacion
{
    private readonly HttpClient _httpClient;
    protected ErrorEndpointResponse error;
    private const string pathApi = "grupos-gasto-importacion";

    public PrincipalGrupoGastoImportacionApi(HttpClient httpClient)
    {
        _httpClient = httpClient;
        error = new();
    }

    public async Task<IEnumerable<GrupoGastoImportacionListarDto>> Listar()
    {
        try
        {
            using HttpResponseMessage response = await _httpClient.GetAsync(pathApi);
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return default;

                return await response.Content.ReadFromJsonAsync<IEnumerable<GrupoGastoImportacionListarDto>>();
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

    public async Task<IEnumerable<GrupoGastoImportacionCatalogoDto>> Catalogo()
    {
        try
        {
            using HttpResponseMessage response = await _httpClient.GetAsync($"{pathApi}/catalogo");
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return default;

                return await response.Content.ReadFromJsonAsync<IEnumerable<GrupoGastoImportacionCatalogoDto>>();
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

    public async Task Editar(Guid id, GrupoGastoImportacionEditarDto grupoGastoImportacion)
    {
        try
        {
            using HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"{pathApi}/{id}", grupoGastoImportacion);
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
            using HttpResponseMessage response = await _httpClient.DeleteAsync($"{pathApi}/{id}");
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

    public async Task<Guid> Insertar(GrupoGastoImportacionInsertarDto grupoGastoImportacion)
    {
        try
        {
            using HttpResponseMessage response = await _httpClient.PostAsJsonAsync(pathApi, grupoGastoImportacion);
            if (response.IsSuccessStatusCode)
            {
                return (await response.Content.ReadFromJsonAsync<GrupoGastoImportacionObtenerDto>()).Id; 
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

    public async Task<GrupoGastoImportacionObtenerDto> Obtener(Guid id)
    {
        try
        {
            using HttpResponseMessage response = await _httpClient.GetAsync($"{pathApi}/{id}");
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return default;

                return await response.Content.ReadFromJsonAsync<GrupoGastoImportacionObtenerDto>();
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
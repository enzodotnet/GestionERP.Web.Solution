using GestionERP.Web.Models.Dtos.Importacion;
using GestionERP.Web.Services.Interfaces;
using System.Net.Http.Json;
using System.Net;
using GestionERP.Web.Models.Responses;
using GestionERP.Web.Handlers; 

namespace GestionERP.Web.Services.Apis;
 
public class ImportacionNotaReporteOrdenApi(HttpClient httpClient) : IImportacionNotaReporteOrden
{
	protected ErrorEndpointResponse error = new();
    private const string pathApi = "empresa/{ce}/importacion/notas-reporte-orden";

	public async Task<IEnumerable<NotaReporteOrdenListarDto>> Listar(string codigoEmpresa)
    {
        try
        { 
            using HttpResponseMessage response = await httpClient.GetAsync(pathApi.Replace("{ce}",codigoEmpresa));
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent) 
                    return default;

                return await response.Content.ReadFromJsonAsync<IEnumerable<NotaReporteOrdenListarDto>>(); 
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

    public async Task<Guid> Insertar(string codigoEmpresa, NotaReporteOrdenInsertarDto notaReporteOrden)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.PostAsJsonAsync(pathApi.Replace("{ce}",codigoEmpresa), notaReporteOrden); 
            if (response.IsSuccessStatusCode)
            {
                NotaReporteOrdenObtenerDto notaOrdenImportacionCreado = await response.Content.ReadFromJsonAsync<NotaReporteOrdenObtenerDto>(); 
                return notaOrdenImportacionCreado.Id;
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

    public async Task Editar(string codigoEmpresa, Guid id, NotaReporteOrdenEditarDto notaReporteOrden)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.PutAsJsonAsync($"{pathApi.Replace("{ce}",codigoEmpresa)}/{id}", notaReporteOrden);
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

    public async Task<NotaReporteOrdenObtenerDto> Obtener(string codigoEmpresa, Guid id)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.GetAsync($"{pathApi.Replace("{ce}",codigoEmpresa)}/{id}");
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return default;

                return await response.Content.ReadFromJsonAsync<NotaReporteOrdenObtenerDto>();
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
            using HttpResponseMessage response = await httpClient.DeleteAsync($"{pathApi.Replace("{ce}",codigoEmpresa)}/{id}");
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

    public async Task<IEnumerable<NotaReporteOrdenCatalogoDto>> Catalogo(string codigoEmpresa)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.GetAsync($"{pathApi.Replace("{ce}",codigoEmpresa)}/catalogo"); 
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return default;

                return await response.Content.ReadFromJsonAsync<IEnumerable<NotaReporteOrdenCatalogoDto>>();
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

	public async Task<NotaReporteOrdenObtenerPorCodigoDto> ObtenerPorCodigo(string codigoEmpresa, string codigoNotaReporteOrden)
	{
		try
		{
			using HttpResponseMessage response = await httpClient.GetAsync($"{pathApi.Replace("{ce}", codigoEmpresa)}/codigo/{codigoNotaReporteOrden}");
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<NotaReporteOrdenObtenerPorCodigoDto>();
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
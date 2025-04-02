using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Services.Interfaces; 
using System.Net;
using GestionERP.Web.Models.Responses;
using GestionERP.Web.Handlers;
using Microsoft.AspNetCore.WebUtilities;

namespace GestionERP.Web.Services.Apis;
 
public class PrincipalTipoCambioDiaApi(HttpClient httpClient) : IPrincipalTipoCambioDia
{
    private readonly HttpClient _httpClient = httpClient;
    protected ErrorEndpointResponse error = new();
    private const string pathApi = "tipo-cambio-dias";

    public async Task<IEnumerable<TipoCambioDiaListarDto>> Listar(string codigoPeriodo)
    {
        try
        {
            Dictionary<string, string> query = new()
            {
                ["codigoPeriodo"] = codigoPeriodo
            };
            using HttpResponseMessage response = await _httpClient.GetAsync(QueryHelpers.AddQueryString(pathApi, query)); 
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent) 
                    return default;

                return await response.Content.ReadFromJsonAsync<IEnumerable<TipoCambioDiaListarDto>>(); 
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

    public async Task ActualizarMonto(TipoCambioDiaActualizarMontoDto tipoCambioDia)
    {
        try
        {
            using HttpResponseMessage response = await _httpClient.PatchAsJsonAsync($"{pathApi}/monto", tipoCambioDia); 
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

    public async Task<TipoCambioDiaObtenerDto> Obtener(Guid id)
    {
        try
        {
            using HttpResponseMessage response = await _httpClient.GetAsync($"{pathApi}/{id}"); 
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent) 
                    return default;

                return await response.Content.ReadFromJsonAsync<TipoCambioDiaObtenerDto>(); 
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

	public async Task<TipoCambioDiaConsultaPorFechaDto> ConsultaPorFecha(DateTime fecha, string flagTipo)
	{
		try
		{
			Dictionary<string, string> query = new()
			{
				["flagTipo"] = flagTipo
			};
			using HttpResponseMessage response = await _httpClient.GetAsync(QueryHelpers.AddQueryString($"{pathApi}/consulta/fecha/{fecha:yyyy-MM-dd}", query));
			if (response.IsSuccessStatusCode)
			{
				if (response.StatusCode == HttpStatusCode.NoContent)
					return default;

				return await response.Content.ReadFromJsonAsync<TipoCambioDiaConsultaPorFechaDto>();
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
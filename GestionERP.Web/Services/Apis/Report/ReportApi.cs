using GestionERP.Web.Services.Interfaces;
using System.Net.Http.Json; 
using System.Net;
using GestionERP.Web.Models.Responses;
using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Report; 

namespace GestionERP.Web.Services.Apis;

public class ReportApi(IHttpClientFactory httpClientFactory) : IReport
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    protected ErrorEndpointResponse error = new();

    public async Task Print(ReportPrintDto reportPrint)
    {
        try 
        {
            HttpClient httpClient = _httpClientFactory.CreateClient("ReportService");

            using HttpResponseMessage response = await httpClient.PostAsJsonAsync("print", reportPrint);
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
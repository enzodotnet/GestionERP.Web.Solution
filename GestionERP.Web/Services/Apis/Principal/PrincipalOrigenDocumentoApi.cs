﻿using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Services.Interfaces;
using System.Net.Http.Json; 
using System.Net;
using GestionERP.Web.Models.Responses;
using GestionERP.Web.Handlers;

namespace GestionERP.Web.Services.Apis;
 
public class PrincipalOrigenDocumentoApi : IPrincipalOrigenDocumento
{
    private readonly HttpClient _httpClient;
    protected ErrorEndpointResponse error;
    private const string pathApi = "origenes-documento";

    public PrincipalOrigenDocumentoApi(HttpClient httpClient)
    {
        _httpClient = httpClient;
        error = new();
    }

    public async Task<IEnumerable<OrigenDocumentoListarDto>> Listar()
    {
        try
        {
            using HttpResponseMessage response = await _httpClient.GetAsync(pathApi); 
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent) 
                    return default;

                return await response.Content.ReadFromJsonAsync<IEnumerable<OrigenDocumentoListarDto>>(); 
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

    public async Task<OrigenDocumentoObtenerDto> Obtener(Guid id)
    {
        try
        {
            using HttpResponseMessage response = await _httpClient.GetAsync($"{pathApi}/{id}"); 
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent) 
                    return default;

                return await response.Content.ReadFromJsonAsync<OrigenDocumentoObtenerDto>(); 
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
 

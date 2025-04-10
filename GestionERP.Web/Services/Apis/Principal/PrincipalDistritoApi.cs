﻿using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Services.Interfaces;
using System.Net.Http.Json; 
using System.Net;
using GestionERP.Web.Models.Responses;
using GestionERP.Web.Handlers;
using Microsoft.AspNetCore.WebUtilities;

namespace GestionERP.Web.Services.Apis;
 
public class PrincipalDistritoApi : IPrincipalDistrito
{
    private readonly HttpClient _httpClient;
    protected ErrorEndpointResponse error;
    private const string pathApi = "distritos";

    public PrincipalDistritoApi(HttpClient httpClient)
    {
        _httpClient = httpClient;
        error = new();
    }

    public async Task<IEnumerable<DistritoListarDto>> Listar(string codigoProvincia)
    {
        try
        {
            Dictionary<string, string> query = new()
            {
                ["codigoProvincia"] = codigoProvincia
            };
            using HttpResponseMessage response = await _httpClient.GetAsync(QueryHelpers.AddQueryString(pathApi, query));  
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent) 
                    return default;

                return await response.Content.ReadFromJsonAsync<IEnumerable<DistritoListarDto>>(); 
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

    public async Task<IEnumerable<DistritoCatalogoPorProvinciaDto>> CatalogoPorProvincia(string codigoProvincia)
    {
        try
        {
            Dictionary<string, string> query = new()
            {
                ["codigoProvincia"] = codigoProvincia
            };
            using HttpResponseMessage response = await _httpClient.GetAsync(QueryHelpers.AddQueryString($"{pathApi}/catalogo/provincia", query)); 
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent) 
                    return default;

                return await response.Content.ReadFromJsonAsync<IEnumerable<DistritoCatalogoPorProvinciaDto>>(); 
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
  

    public async Task<DistritoObtenerDto> Obtener(Guid id)
    {
        try
        {
            using HttpResponseMessage response = await _httpClient.GetAsync($"{pathApi}/{id}"); 
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent) 
                    return default;

                return await response.Content.ReadFromJsonAsync<DistritoObtenerDto>(); 
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
 

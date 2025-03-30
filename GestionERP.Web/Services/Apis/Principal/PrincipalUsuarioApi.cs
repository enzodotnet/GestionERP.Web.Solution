using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Services.Interfaces;
using System.Net.Http.Json;
using System.Net;
using GestionERP.Web.Models.Responses;
using GestionERP.Web.Handlers;
using Microsoft.AspNetCore.WebUtilities;

namespace GestionERP.Web.Services.Apis;
 
public class PrincipalUsuarioApi(HttpClient httpClient) : IPrincipalUsuario
{ 
    protected ErrorEndpointResponse error = new();
    private const string pathApi = "usuarios";

    public async Task<IEnumerable<UsuarioListarDto>> Listar()
    {
        try
        {
            using HttpResponseMessage response = await httpClient.GetAsync(pathApi);
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return default;

                return await response.Content.ReadFromJsonAsync<IEnumerable<UsuarioListarDto>>();
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

    public async Task<IEnumerable<UsuarioCatalogoDto>> Catalogo()
    {
        try
        {
            using HttpResponseMessage response = await httpClient.GetAsync($"{pathApi}/catalogo");
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return default;

                return await response.Content.ReadFromJsonAsync<IEnumerable<UsuarioCatalogoDto>>();
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

    public async Task Editar(Guid id, UsuarioEditarDto usuario)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.PutAsJsonAsync($"{pathApi}/{id}", usuario);
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

    public async Task<Guid> Insertar(UsuarioInsertarDto usuario)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.PostAsJsonAsync(pathApi, usuario);
            if (response.IsSuccessStatusCode)
            {
                return (await response.Content.ReadFromJsonAsync<UsuarioObtenerDto>()).Id; 
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

    public async Task<UsuarioObtenerDto> Obtener(Guid id)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.GetAsync($"{pathApi}/{id}");
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return default;

                return await response.Content.ReadFromJsonAsync<UsuarioObtenerDto>();
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

    public async Task<UsuarioObtenerPorSesionDto> ObtenerPorSesion()
    {
        try
        {
            using HttpResponseMessage response = await httpClient.GetAsync($"{pathApi}/sesion");
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return default;

                return await response.Content.ReadFromJsonAsync<UsuarioObtenerPorSesionDto>();
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

    public async Task<UsuarioConsultaAccesoPorSesionDto> ConsultaAccesoPorSesion(string codigoWebEmpresa = null, string codigoModulo = null, string codigoServicio = null)
    {
        try
        {
            Dictionary<string, string> query = new()
            {
                ["codigoWebEmpresa"] = codigoWebEmpresa ?? "",
                ["codigoModulo"] = codigoModulo ?? "",
                ["codigoServicio"] = codigoServicio ?? ""
            };    
            using HttpResponseMessage response = await httpClient.GetAsync(QueryHelpers.AddQueryString($"{pathApi}/consulta/acceso/sesion",query));
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return default;

                return await response.Content.ReadFromJsonAsync<UsuarioConsultaAccesoPorSesionDto>();
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

    public async Task<UsuarioObtenerPorCodigoEmpresaDto> ObtenerPorCodigoEmpresa(string codigoUsuario, string codigoEmpresa)
    {
        try
        {
            Dictionary<string, string> query = new()
            {
                ["codigoEmpresa"] = codigoEmpresa
            }; 
            using HttpResponseMessage response = await httpClient.GetAsync(QueryHelpers.AddQueryString($"{pathApi}/codigo/{codigoUsuario}/empresa",query));
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UsuarioObtenerPorCodigoEmpresaDto>();
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

    public async Task<IEnumerable<UsuarioCatalogoPorEmpresaDto>> CatalogoPorEmpresa(string codigoEmpresa)
    {
        try
        { 
            Dictionary<string, string> query = new()
            {
                ["codigoEmpresa"] = codigoEmpresa
            }; 
            using HttpResponseMessage response = await httpClient.GetAsync(QueryHelpers.AddQueryString($"{pathApi}/catalogo/empresa",query)); 
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return default;

                return await response.Content.ReadFromJsonAsync<IEnumerable<UsuarioCatalogoPorEmpresaDto>>();
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

    public async Task<UsuarioConsultaPorEmpresaSesionDto> ConsultaPorEmpresaSesion(string codigoEmpresa)
    {
        try
        {
            Dictionary<string, string> query = new()
            {
                ["codigoEmpresa"] = codigoEmpresa
            };
            using HttpResponseMessage response = await httpClient.GetAsync(QueryHelpers.AddQueryString($"{pathApi}/consulta/empresa/sesion",query));
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return default; 
                return await response.Content.ReadFromJsonAsync<UsuarioConsultaPorEmpresaSesionDto>();
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

    public async Task<IEnumerable<UsuarioEmpresaCatalogoPorSesionDto>> CatalogoEmpresasPorSesion()
    {
        try
        {
            using HttpResponseMessage response = await httpClient.GetAsync($"{pathApi}/empresas/catalogo/sesion");
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return default;

                return await response.Content.ReadFromJsonAsync<IEnumerable<UsuarioEmpresaCatalogoPorSesionDto>>();
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

    public async Task<IEnumerable<UsuarioModuloCatalogoPorEmpresaSesionDto>> CatalogoModulosPorEmpresaSesion(string codigoEmpresa)
    {
        try
        {
            Dictionary<string, string> query = new()
            {
                ["codigoEmpresa"] = codigoEmpresa
            };
            using HttpResponseMessage response = await httpClient.GetAsync(QueryHelpers.AddQueryString($"{pathApi}/modulos/catalogo/empresa/sesion",query));
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return default;

                return await response.Content.ReadFromJsonAsync<IEnumerable<UsuarioModuloCatalogoPorEmpresaSesionDto>>();
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
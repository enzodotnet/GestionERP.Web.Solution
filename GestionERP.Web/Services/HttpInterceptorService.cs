using System.Net.Http.Headers;  
using Toolbelt.Blazor;

namespace GestionERP.Web.Services;

public class HttpInterceptorService(HttpClientInterceptor interceptorHttpClient, RefreshTokenService refreshTokenService)
{
	private readonly HttpClientInterceptor _interceptorHttpClient = interceptorHttpClient;
	private readonly RefreshTokenService _refreshTokenService = refreshTokenService;

    public void RegisterEvent() => _interceptorHttpClient.BeforeSendAsync += InterceptBeforeHttpAsync;

	public async Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs e)
	{
		string absPath = e.Request.RequestUri.AbsolutePath; 

		if (!absPath.Contains("auth") && !absPath.Contains("empresas/catalogo/sesion") && !absPath.Contains("modulos/catalogo/sesion"))
		{
			string token = await _refreshTokenService.TryRefreshToken(absPath);

			if(!string.IsNullOrEmpty(token))
				e.Request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
		}
	}

	public void DisposeEvent() => _interceptorHttpClient.BeforeSendAsync -= InterceptBeforeHttpAsync;
} 
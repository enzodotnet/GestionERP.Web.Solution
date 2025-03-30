using Microsoft.AspNetCore.Components.Authorization;
using GestionERP.Web.Services.Interfaces;
using System.Security.Claims;

namespace GestionERP.Web.Services; 

public class RefreshTokenService(AuthenticationStateProvider authProvider, IAuthentication authService)
{
	private readonly AuthenticationStateProvider _authProvider = authProvider;
	private readonly IAuthentication _authService = authService;

    public async Task<string> TryRefreshToken(string path)
	{
		AuthenticationState authState = await _authProvider.GetAuthenticationStateAsync();

		ClaimsPrincipal user = authState.User;
		if (user.Identity.IsAuthenticated)
		{ 
			string exp = user.FindFirst(c => c.Type.Equals("exp")).Value;
			DateTimeOffset expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp)).ToLocalTime();
			DateTime time = DateTime.Now;
			TimeSpan diff = expTime - time;
			if (diff.TotalMinutes <= 2)
				return await _authService.ActualizarToken();
		}
		return string.Empty;
	}
} 
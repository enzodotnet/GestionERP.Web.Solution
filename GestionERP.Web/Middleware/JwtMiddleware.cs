using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    private readonly ILogger<JwtMiddleware> _logger;

    public JwtMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<JwtMiddleware> logger)
    {
        _next = next;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        // Intentamos recuperar el token JWT desde las cabeceras de la solicitud
        var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
        {
            try
            {
                // Validar y parsear el token JWT
                var secretKey = _configuration["Jwt:SecretKey"];
                var handler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(secretKey);

                var tokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = false, // Puedes validar el emisor si lo deseas
                    ValidateAudience = false, // Puedes validar la audiencia si lo deseas
                    ValidateLifetime = true,
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key)
                };

                var principal = handler.ValidateToken(token, tokenValidationParameters, out var validatedToken);

                // Establecer el usuario autenticado en el contexto
                httpContext.User = principal;
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Token JWT inválido: " + ex.Message);
                // Si el token no es válido, puedes manejar el error de la manera que desees
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await httpContext.Response.WriteAsync("Unauthorized");
                return;
            }
        }

        // Llamamos al siguiente middleware
        await _next(httpContext);
    }
}
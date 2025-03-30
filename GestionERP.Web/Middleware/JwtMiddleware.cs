namespace GestionERP.Web.Middleware;

public class JwtMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<JwtMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        // Intentamos recuperar el token JWT desde las cabeceras de la solicitud
        var token = httpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

        if (token != null)
        {
            try
            {
                var secretKey = configuration["Jwt:SecretKey"];
            }
            catch (Exception ex)
            {
                logger.LogWarning("Token JWT inválido: " + ex.Message);
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await httpContext.Response.WriteAsync("Unauthorized");
                return;
            }
        }

        await next(httpContext);
    }
}
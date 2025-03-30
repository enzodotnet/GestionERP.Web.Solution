using GestionERP.Web.Models.Dtos.Auth; 

namespace GestionERP.Web.Services.Interfaces;

public interface IAuthentication
{
    Task AutenticarUsuario(UsuarioAutenticarCredencialDto login);
    Task CerrarSesionUsuario();
    Task<string> ActualizarToken();
}
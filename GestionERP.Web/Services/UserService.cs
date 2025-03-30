using GestionERP.Web.Components;
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization; 

namespace GestionERP.Web.Services;

public class UserService(IPrincipalUsuario usuario, NavigationManager navigation, IAuthentication auth, AuthenticationStateProvider authState)
{
    public async Task<(bool esValido, ClaimsPrincipal userSesion)> VerificarAccesoEsValido(NotifyComponent notificacion, string codigoWebEmpresa = null, string codigoModulo = null, string codigoServicio = null, string codigoUser = null)
    {
        (bool esValido, ClaimsPrincipal) resultado;
        ClaimsPrincipal usuarioSesion = (await authState.GetAuthenticationStateAsync()).User;
        if (!usuarioSesion.Identity.IsAuthenticated)
        {
            notificacion.ShowAlertAuth("NA");
            resultado = (false, null);
        }
        else if (!string.IsNullOrEmpty(codigoUser) && usuarioSesion.FindFirst("code").Value != codigoUser)
        {
            notificacion.ShowAlertSession(usuarioSesion.FindFirst("name").Value);
            resultado = (false, null);
        }
        else
        {
            UsuarioConsultaAccesoPorSesionDto usuarioAcceso = await usuario.ConsultaAccesoPorSesion(codigoWebEmpresa, codigoModulo, codigoServicio);
            if (!usuarioAcceso.EsAccesoValido)
            {
                if (!usuarioAcceso.EsSesionValida)
                    await auth.CerrarSesionUsuario();

                notificacion.Show(usuarioAcceso.MensajeAlerta, usuarioAcceso.TipoAlerta);
                navigation.NavigateTo(usuarioAcceso.UrlRetorno);

                resultado = (false, null);
            }
            else
            {
                resultado = (true, usuarioSesion);
            }
        }
        return resultado;
    }
} 
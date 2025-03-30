namespace GestionERP.Web.Models.Dtos.Principal;

public class UsuarioObtenerPorSesionDto
{
    public string Codigo { get; set; }
    public string Email { get; set; }
    public string NickUser { get; set; }
    public string Nombre { get; set; }
    public string FlagTipoPerfil { get; set; }
    public bool Activo { get; set; }
}
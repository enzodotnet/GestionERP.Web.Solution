namespace GestionERP.Web.Models.Dtos.Principal;

public class UsuarioObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Email { get; set; }
    public string NickUser { get; set; }
    public string Nombre { get; set; }
    public string FlagTipoPerfil { get; set; }
    public string CodigoTipoIdentificacion { get; set; }
    public string SiglaTipoIdentificacion { get; set; }
    public string NombreTipoIdentificacion { get; set; }
    public string NumeroTipoIdentificacion { get; set; }
    public bool Activo { get; set; }
    public List<UsuarioModuloObtenerDto> Modulos { get; set; }
    public List<UsuarioServicioObtenerDto> Servicios { get; set; }
    public List<UsuarioPermisoObtenerDto> Permisos { get; set; }
}
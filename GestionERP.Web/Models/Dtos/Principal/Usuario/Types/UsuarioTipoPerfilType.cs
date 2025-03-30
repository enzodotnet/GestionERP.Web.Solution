namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class UsuarioTipoPerfilType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    
    public static IEnumerable<UsuarioTipoPerfilType> ObtenerTipos()
    {
        return new List<UsuarioTipoPerfilType>
        {
            new UsuarioTipoPerfilType {Codigo = "S", Nombre = "Super administrador"},
            new UsuarioTipoPerfilType {Codigo = "A", Nombre = "Administrador"},
            new UsuarioTipoPerfilType {Codigo = "U", Nombre = "Usuario"}
        };
    }
}
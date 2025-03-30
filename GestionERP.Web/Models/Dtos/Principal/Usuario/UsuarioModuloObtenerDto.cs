namespace GestionERP.Web.Models.Dtos.Principal;

public class UsuarioModuloObtenerDto
{ 
    public Guid? Id { get; set; }
    public string CodigoModulo { get; set; }
    public string NombreModulo { get; set; }
    public bool EsAsignado { get; set; }
    public bool EsAsignadoTemp { get; set; }
    public bool EsBloqueado { get; set;}
}
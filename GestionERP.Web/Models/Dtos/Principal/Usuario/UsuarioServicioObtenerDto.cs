namespace GestionERP.Web.Models.Dtos.Principal;

public class UsuarioServicioObtenerDto
{ 
    public Guid? Id { get; set; }
    public string CodigoServicio { get; set; }
    public string NombreServicio { get; set; }
    public bool EsAsignado { get; set; }
    public bool EsAsignadoTemp { get; set; }
    public bool EsBloqueado { get; set; }
    public string CodigoModulo { get; set; }
    public string NombreModulo { get; set; }
    public bool EsAsignadoModulo { get; set; }
}
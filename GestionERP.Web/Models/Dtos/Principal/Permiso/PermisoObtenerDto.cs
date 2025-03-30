namespace GestionERP.Web.Models.Dtos.Principal;

public class PermisoObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string CodigoEvento { get; set; }
    public string NombreEvento { get; set; }
    public string CodigoServicio { get; set; }
    public string NombreServicio { get; set; }
    public string CodigoModulo { get; set; }
    public string NombreModulo { get; set; }
    public bool EsOpcionMenu { get; set; }
    public string Descripcion { get; set; }
    public bool Activo { get; set; }
}
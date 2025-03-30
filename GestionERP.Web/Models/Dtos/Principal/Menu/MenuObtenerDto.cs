namespace GestionERP.Web.Models.Dtos.Principal;

public class MenuObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string CodigoModulo { get; set; }
    public string NombreModulo { get; set; }
    public string CodigoServicio { get; set; }
    public string NombreServicio { get; set; }
    public string CodigoPermiso { get; set; }
    public string NombrePermiso { get; set; }
    public string Descripcion { get; set; }
    public string Nivel { get; set; }
    public string RutaWebServicio { get; set; }
    public string Icono { get; set; }
    public string NumeroOrden { get; set; }
    public bool EsGrupo { get; set; }
    public bool EsPredeterminado { get; set; }
    public string CodigoMenuReferencia { get; set; }
    public string NombreMenuReferencia { get; set; }
    public bool Activo { get; set; }
}
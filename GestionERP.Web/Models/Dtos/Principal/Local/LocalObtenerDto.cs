namespace GestionERP.Web.Models.Dtos.Principal;

public class LocalObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string CodigoRegion { get; set; }
    public string NombreRegion { get; set; }
    public string CodigoProvincia { get; set; }
    public string NombreProvincia { get; set; } 
    public string CodigoDistrito { get; set; }
    public string NombreDistrito { get; set; }
    public string Direccion { get; set; } 
    public string Observacion { get; set; }
    public bool Activo { get; set; }
}
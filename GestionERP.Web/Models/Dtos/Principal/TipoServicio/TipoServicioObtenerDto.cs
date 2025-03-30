namespace GestionERP.Web.Models.Dtos.Principal;

public class TipoServicioObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }    
    public string FlagRegistro { get; set; } 
    public bool Activo { get; set; }
}
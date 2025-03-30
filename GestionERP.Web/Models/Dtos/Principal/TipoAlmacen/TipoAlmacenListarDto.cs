namespace GestionERP.Web.Models.Dtos.Principal;

public class TipoAlmacenListarDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; } 
    public string Descripcion { get; set; } 
    public bool Activo { get; set; }
}
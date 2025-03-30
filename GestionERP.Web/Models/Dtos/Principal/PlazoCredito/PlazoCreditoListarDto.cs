namespace GestionERP.Web.Models.Dtos.Principal;

public class PlazoCreditoListarDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; } 
    public int CantidadDias { get; set; } 
    public string Descripcion { get; set; }
    public bool Activo { get; set; }
}
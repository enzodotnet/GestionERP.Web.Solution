namespace GestionERP.Web.Models.Dtos.Produccion;

public class PersonalListarDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; } 
    public string NombreFuncion { get; set; } 
    public bool Activo { get; set; }
}
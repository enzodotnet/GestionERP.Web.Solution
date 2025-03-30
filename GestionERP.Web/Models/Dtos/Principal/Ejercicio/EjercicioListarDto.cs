namespace GestionERP.Web.Models.Dtos.Principal;

public class EjercicioListarDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public int Anio { get; set; }
    public string Prefijo { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; } 
    public bool Activo { get; set; }
}
namespace GestionERP.Web.Models.Dtos.Produccion;

public class VersionPlanListarDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public DateTime FechaRegistro { get; set; }
    public string NombrePlan { get; set; }
    public string NombreArticuloTerminado { get; set; }
    public string CodigoEstado { get; set; }
    public string NombreEstado { get; set; }
}
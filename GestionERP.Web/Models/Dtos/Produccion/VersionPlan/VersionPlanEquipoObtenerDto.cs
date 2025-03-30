namespace GestionERP.Web.Models.Dtos.Produccion;

public class VersionPlanEquipoObtenerDto
{
    public Guid? Id { get; set; } 
    public string CodigoEquipo { get; set; }
    public string NombreEquipo { get; set; } 
    public decimal? CantidadHorasUso { get; set; } 
    public string Observacion { get; set; }
}
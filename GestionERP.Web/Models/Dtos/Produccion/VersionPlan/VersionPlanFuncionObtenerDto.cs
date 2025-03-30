namespace GestionERP.Web.Models.Dtos.Produccion;

public class VersionPlanFuncionObtenerDto
{
    public Guid? Id { get; set; } 
    public string CodigoFuncion { get; set; }
    public string NombreFuncion { get; set; }
    public decimal? CantidadHorasTrabajo { get; set; } 
    public string Observacion { get; set; }
}
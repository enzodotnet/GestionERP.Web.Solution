namespace GestionERP.Web.Models.Dtos.Principal;

public class TipoCambioDiaObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string FlagTipo { get; set; }
    public string CodigoPeriodoDia { get; set; }
    public DateTime Fecha { get; set; }
    public decimal? Monto { get; set; } 
}
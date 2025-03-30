namespace GestionERP.Web.Models.Dtos.Principal;

public class TipoCambioDiaListarDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Descripcion { get; set; }
    public string FlagTipo { get; set; }
    public string CodigoPeriodoDia { get; set; }
    public DateTime Fecha { get; set; }
    public decimal? Monto { get; set; } 
    public bool AllDay { get; set; }
}
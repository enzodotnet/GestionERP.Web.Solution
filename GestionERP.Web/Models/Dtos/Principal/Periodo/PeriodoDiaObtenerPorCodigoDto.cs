namespace GestionERP.Web.Models.Dtos.Principal;

public class PeriodoDiaObtenerPorCodigoDto
{ 
    public DateTime Fecha { get; set; }
    public int NumeroAnual { get; set; } 
    public string Nombre { get; set; }
    public int NumeroMensual { get; set; }
    public int NumeroSemanalAnio { get; set; }
    public int NumeroTrimestral { get; set; }
}
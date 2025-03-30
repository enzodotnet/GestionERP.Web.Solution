namespace GestionERP.Web.Models.Dtos.Servicio;

public class ContratoCuotaCatalogoActualizarEstadoDto
{
    public int NumeroCuota { get; set; }
    public decimal ImporteBruto { get; set; } 
    public DateTime FechaVencimiento { get; set; }
    public string Observacion { get; set; }
    public decimal ImporteBrutoAtendido { get; set; }
}
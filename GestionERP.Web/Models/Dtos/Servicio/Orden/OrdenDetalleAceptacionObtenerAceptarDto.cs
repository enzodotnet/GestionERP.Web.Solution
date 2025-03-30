namespace GestionERP.Web.Models.Dtos.Servicio;

public class OrdenDetalleAceptacionObtenerAceptarDto
{
    public Guid OrdenDetalleAceptacionId { get; set; }
    public int NumeroAceptacion { get; set; }
    public DateTime FechaAceptacion { get; set; } 
    public decimal ImporteBruto { get; set; }
    public string Observacion { get; set; }
}
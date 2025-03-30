using GestionERP.Web.Models.Dtos.Control;

namespace GestionERP.Web.Models.Dtos.Servicio;

public class OrdenDetalleAceptacionObtenerDto
{ 
    public Guid Id { get; set; }
    public int NumeroAceptacion { get; set; }
    public DateTime FechaAceptacion { get; set; } 
    public decimal ImporteBruto { get; set; }
    public string Observacion { get; set; }
    public decimal ImporteBrutoAtendido { get; set; }
    public decimal ImporteBrutoRescindido { get; set; }
    public IEnumerable<AceptacionProvisionObtenerDto> Provisiones { get; set; }
}
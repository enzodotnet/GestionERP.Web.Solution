namespace GestionERP.Web.Models.Dtos.Servicio;

public class OrdenDetalleObtenerAceptarDto
{
    public Guid OrdenDetalleId { get; set; } 
    public string CodigoArticulo { get; set; }
    public string NombreArticulo { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal ImporteBruto { get; set; }
    public decimal ImporteBrutoAceptado { get; set; }
    public decimal ImporteBrutoSaldo { get; set; }
    public IEnumerable<OrdenDetalleAceptacionObtenerAceptarDto> Aceptaciones { get; set; }
}
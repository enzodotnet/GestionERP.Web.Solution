namespace GestionERP.Web.Models.Dtos.Servicio;

public class ContratoDetalleCatalogoActualizarEstadoDto
{ 
    public string CodigoArticulo { get; set; }
    public string NombreArticulo { get; set; } 
    public decimal ImporteBruto { get; set; }
    public decimal ImporteImpuesto { get; set; }
    public decimal ImporteNeto { get; set; }
    public string Observacion { get; set; } 
}
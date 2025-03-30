namespace GestionERP.Web.Models.Dtos.Servicio;

public class OrdenDetalleCatalogoActualizarEstadoDto
{ 
    public string CodigoArticulo { get; set; }
    public string NombreArticulo { get; set; }
    public string CodigoUnidadMedida { get; set; }
    public string NombreUnidadMedida { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal ImporteBruto { get; set; }
    public decimal ImporteImpuesto { get; set; }
    public decimal ImporteNeto { get; set; }
    public decimal ImporteBrutoAtendido { get; set; }
    public string Observacion { get; set; }
    public string CodigoSolicitudReferencia { get; set; }
}
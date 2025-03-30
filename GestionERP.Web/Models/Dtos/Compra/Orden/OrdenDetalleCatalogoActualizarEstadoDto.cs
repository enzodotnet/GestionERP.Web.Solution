namespace GestionERP.Web.Models.Dtos.Compra;

public class OrdenDetalleCatalogoActualizarEstadoDto
{ 
    public string CodigoTipoArticulo { get; set; }
    public string NombreTipoArticulo { get; set; }
    public string CodigoArticulo { get; set; }
    public string NombreArticulo { get; set; }
    public string CodigoUnidadMedida { get; set; }
    public string NombreUnidadMedida { get; set; }
    public string Presentacion { get; set; }
    public decimal Cantidad { get; set; }
    public decimal CantidadAtendida { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal ImporteBruto { get; set; }
    public decimal ImporteImpuesto { get; set; }
    public decimal ImporteNeto { get; set; }
    public string Observacion { get; set; }
    public string CodigoSolicitudReferencia { get; set; }
}
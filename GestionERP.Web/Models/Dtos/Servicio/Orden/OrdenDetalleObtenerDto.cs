namespace GestionERP.Web.Models.Dtos.Servicio;

public class OrdenDetalleObtenerDto
{
    public Guid? Id { get; set; } 
    private string codigoArticulo;
    public string CodigoArticulo { get => codigoArticulo; set => codigoArticulo = value?.TrimEnd(); }
    public string NombreArticulo { get; set; }
    public bool EsAfectoImpuesto { get; set; } 
    public string CodigoUnidadMedida { get; set; }
    public string NombreUnidadMedida { get; set; }
    public int? Cantidad { get; set; }
    public decimal? PrecioUnitario { get; set; } 
    public decimal ImporteBruto { get; set; }
    public decimal ImporteImpuesto { get; set; }
    public decimal ImporteNeto { get; set; }
    public string Observacion { get; set; }
    public decimal ImporteBrutoAceptado { get; set; }
    public decimal ImporteBrutoAtendido { get; set; }
    public decimal ImporteBrutoRescindido { get; set; }
	public string CodigoSolicitudReferencia { get; set; }
    public IEnumerable<OrdenDetalleAceptacionObtenerDto> Aceptaciones { get; set; }
}
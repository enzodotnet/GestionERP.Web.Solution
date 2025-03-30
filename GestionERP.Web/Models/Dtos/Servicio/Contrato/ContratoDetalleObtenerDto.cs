namespace GestionERP.Web.Models.Dtos.Servicio;

public class ContratoDetalleObtenerDto
{
    public Guid? Id { get; set; } 
    private string codigoArticulo;
    public string CodigoArticulo { get => codigoArticulo; set => codigoArticulo = value?.TrimEnd(); }
    public string NombreArticulo { get; set; }
    public bool EsAfectoImpuesto { get; set; } 
    public decimal? ImporteBruto { get; set; }
	public decimal ImporteImpuesto { get; set; }
    public decimal ImporteNeto { get; set; }
    public string Observacion { get; set; }
}
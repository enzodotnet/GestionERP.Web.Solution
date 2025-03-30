namespace GestionERP.Web.Models.Dtos.Importacion;

public class SolicitudDetalleObtenerDto
{
    public Guid? Id { get; set; } 
    public string CodigoTipoArticulo { get; set; }
    public string NombreTipoArticulo { get; set; }
    private string codigoArticulo;
    public string CodigoArticulo { get => codigoArticulo; set => codigoArticulo = value?.TrimEnd(); }
    public string NombreArticulo { get; set; }
    public string CodigoUnidadMedida { get; set; }
    public string NombreUnidadMedida { get; set; }
    public string Presentacion { get; set; }
    public decimal? UnidadConversion { get; set; }
    public decimal? Cantidad { get; set; }
    public string Observacion { get; set; }
    public decimal CantidadAtendida { get; set; }
    public decimal CantidadRescindida { get; set; }
}
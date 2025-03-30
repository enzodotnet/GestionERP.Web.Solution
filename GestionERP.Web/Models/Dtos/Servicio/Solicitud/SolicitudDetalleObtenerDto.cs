namespace GestionERP.Web.Models.Dtos.Servicio;

public class SolicitudDetalleObtenerDto
{
    public Guid? Id { get; set; } 
    private string codigoArticulo;
    public string CodigoArticulo { get => codigoArticulo; set => codigoArticulo = value?.TrimEnd(); }
    public string NombreArticulo { get; set; }
    public string CodigoUnidadMedida { get; set; }
    public string NombreUnidadMedida { get; set; }
    public int? Cantidad { get; set; }
    public string Observacion { get; set; }
    public int CantidadAtendida { get; set; }
    public int CantidadRescindida { get; set; }
}
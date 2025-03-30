namespace GestionERP.Web.Models.Dtos.Almacen;

public class MovimientoDetalleLoteObtenerDto
{
    public Guid? Id { get; set; }
    public int NumeroItem { get; set; }
    public string NumeroLote { get; set; }
    public string CodigoMarca { get; set; }
    public string NombreMarca { get; set; }
    public DateTime FechaFabricacion { get; set; }
    public DateTime FechaVencimiento { get; set; } 
    public decimal Cantidad { get; set; }
    public IEnumerable<MovimientoDetalleLoteEstadoObtenerDto> Estados { get; set; }
}
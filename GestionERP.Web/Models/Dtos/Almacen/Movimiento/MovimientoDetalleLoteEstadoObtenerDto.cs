namespace GestionERP.Web.Models.Dtos.Almacen;

public class MovimientoDetalleLoteEstadoObtenerDto
{
    public Guid Id { get; set; }
    public int NumeroItem { get; set; }
    public string CodigoProductoEstado { get; set; }
    public string NombreProductoEstado  { get; set; }
    public decimal Cantidad { get; set; }
}
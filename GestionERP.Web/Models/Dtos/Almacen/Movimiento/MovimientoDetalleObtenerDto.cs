namespace GestionERP.Web.Models.Dtos.Almacen;

public class MovimientoDetalleObtenerDto
{
    public Guid Id { get; set; }
    public int NumeroItem { get; set; }
    public string CodigoTipoArticulo { get; set; }
    public string NombreTipoArticulo { get; set; }
    public string CodigoAlmacen { get; set; }
    public string NombreAlmacen { get; set; }
    public string CodigoArticulo { get; set; }
    public string NombreArticulo { get; set; }
    public string Presentacion { get; set; }
    public decimal? UnidadConversion { get; set; }
    public string CodigoUnidadMedida { get; set; }
    public string NombreUnidadMedida { get; set; }
    public decimal Cantidad { get; set; }
    public decimal CostoUnitarioMN { get; set; }
    public decimal CostoUnitarioME { get; set; }
    public decimal ValorCostoMN { get; set; }
    public decimal ValorCostoME { get; set; }
    public IEnumerable<MovimientoDetalleLoteObtenerDto> Lotes { get; set; } 
}
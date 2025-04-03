namespace GestionERP.Web.Models.Dtos.Compra;

public class OrdenDetalleCatalogoIngresarDto
{
    public string CodigoTipoArticulo { get; set; }
    public string NombreTipoArticulo { get; set; }
    public string CodigoArticulo { get; set; }
    public string NombreArticulo { get; set; } 
    public string CodigoUnidadMedida { get; set; }
    public string NombreUnidadMedida { get; set; }
    public string Presentacion { get; set; }
    public decimal? UnidadConversion { get; set; }
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; } 
    public bool IsErrorSelected { get; set; }
}
namespace GestionERP.Web.Models.Dtos.Importacion;

public class OrdenDetalleCatalogoAtenderDto
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
    
    /// <summary>
    /// Propiedad alternativa para determinar si el registro seleccionado se encuentra repetido en el mismo catalogo.
    /// </summary>
    public bool IsErrorSelected { get; set; }
}
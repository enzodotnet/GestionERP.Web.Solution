namespace GestionERP.Web.Models.Dtos.Produccion;

public class OrdenMaterialCatalogoProcesarDto
{ 
    public int NumeroItem { get; set; }
    public string CodigoTipoArticulo { get; set; }
    public string NombreTipoArticulo { get; set; }
    public string CodigoArticulo { get; set; }
    public string NombreArticulo { get; set; } 
    public string CodigoUnidadMedida { get; set; }
    public string NombreUnidadMedida { get; set; }
    public decimal Cantidad { get; set; } 
}
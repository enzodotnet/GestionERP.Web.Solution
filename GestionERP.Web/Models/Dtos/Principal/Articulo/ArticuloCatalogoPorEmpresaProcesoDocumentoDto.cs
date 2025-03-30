namespace GestionERP.Web.Models.Dtos.Principal;

public class ArticuloCatalogoPorEmpresaProcesoDocumentoDto
{
    public string CodigoArticulo { get; set; }
    public string NombreArticulo { get; set; }
    public string CodigoTipoArticulo { get; set; }
    public string NombreTipoArticulo { get; set; }
    public string CodigoTipoExistencia { get; set; }
    public string NombreTipoExistencia { get; set; } 
    public string CodigoUnidadMedida { get; set; }
    public string NombreUnidadMedida { get; set; }
    public string Presentacion { get; set; }
    public decimal? UnidadConversion { get; set; }
    public bool EsAfectoImpuesto { get; set; }
    public bool EsRequeridoLote { get; set; } 
}
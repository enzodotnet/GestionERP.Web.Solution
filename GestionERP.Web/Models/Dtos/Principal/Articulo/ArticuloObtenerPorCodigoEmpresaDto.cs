namespace GestionERP.Web.Models.Dtos.Principal;

public class ArticuloObtenerPorCodigoEmpresaDto
{ 
    public string CodigoArticulo { get; set; }
    public string NombreArticulo { get; set; }
    public string CodigoTipoArticulo { get; set; }
    public string NombreTipoArticulo { get; set; }
    public bool EsItemFisico { get; set; }
    public string CodigoUnidadMedida { get; set; }
    public string NombreUnidadMedida { get; set; }
    public string Presentacion { get; set; }
    public decimal? CantidadPresentacion { get; set; }
    public bool EsAfectoImpuesto { get; set; }
    public bool? EsRequeridoLote { get; set; } 
}
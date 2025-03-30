namespace GestionERP.Web.Models.Dtos.Produccion;

public class OrdenMaterialObtenerDto
{
    public Guid Id { get; set; } 
    public string FlagTipo { get; set; }
    public string CodigoArticulo { get; set; }
    public string NombreArticulo { get; set; }
    public string FlagInsumo { get; set; }
    public string CodigoUnidadMedida { get; set; }
    public string NombreUnidadMedida { get; set; }
    public decimal Cantidad { get; set; }
}
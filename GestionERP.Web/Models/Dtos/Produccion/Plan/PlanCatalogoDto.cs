namespace GestionERP.Web.Models.Dtos.Produccion;

public class PlanCatalogoDto
{
    public string CodigoPlan { get; set; }
    public string NombrePlan { get; set; } 
    public string CodigoTipoProduccion { get; set; }
    public string NombreTipoProduccion { get; set; }
    public string FlagTipoEncargo { get; set; }
    public string CodigoArticuloTerminado { get; set; }
    public string NombreArticuloTerminado { get; set; }
    public string CodigoUnidadMedida { get; set; }
    public string NombreUnidadMedida { get; set; }
    public string PresentacionArticulo { get; set; }
    public decimal UnidadConversionArticulo { get; set; }
    public decimal Cantidad { get; set; }
}
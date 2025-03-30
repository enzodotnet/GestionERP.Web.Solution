namespace GestionERP.Web.Models.Dtos.Produccion;

public class PlanConsultaPorCodigoDto
{
    public string NombrePlan { get; set; }
    public string CodigoTipoProduccion { get; set; }
    public string NombreTipoProduccion { get; set; }
    public string CodigoArticuloTerminado { get; set; }
    public string NombreArticuloTerminado { get; set; }
    public decimal Cantidad { get; set; }
}
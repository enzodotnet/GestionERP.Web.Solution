namespace GestionERP.Web.Models.Dtos.Servicio;

public class ContratoCatalogoDevengarDto
{
    public Guid ContratoId { get; set; }
    public string FlagTipoRegistro { get; set; }
    public string CodigoComprobante { get; set; }
    public string CodigoEjercicio { get; set; }
    public string CodigoPeriodo { get; set; } 
    public string CodigoEntidad { get; set; }
    public string NombreEntidad { get; set; }
    public string CodigoDocumento { get; set; }
    public string CodigoSerieDocumento { get; set; } 
    public string NumeroSerieDocumento { get; set; }
    public DateTime FechaEmision { get; set; } 
    public string CodigoMoneda { get; set; }
    public string NombreMoneda { get; set; } 
    public decimal TotalImporteBruto { get; set; }
    public decimal TotalImporteImpuesto { get; set; }
    public decimal TotalImporteNeto { get; set; }
    public DateTime FechaInicio { get; set; } 
    public DateTime FechaFin { get; set; } 
    public string CodigoTipoDevengo { get; set; }
    public string NombreTipoDevengo { get; set; }
}
namespace GestionERP.Web.Models.Dtos.Servicio;

public class ContratoCatalogoProvisionarDto
{
    public Guid ContratoId { get; set; }
    public string CodigoComprobante { get; set; }
    public string CodigoEjercicio { get; set; }
    public string CodigoPeriodo { get; set; } 
    public string CodigoEntidad { get; set; }
    public string NombreEntidad { get; set; }
    public string CodigoDocumento { get; set; }
    public string CodigoSerieDocumento { get; set; } 
    public string NumeroSerieDocumento { get; set; }
    public DateTime FechaEmision { get; set; }
    public string CodigoTipoProvision { get; set; }
    public string NombreTipoProvision { get; set; } 
    public string CodigoMoneda { get; set; }
    public string NombreMoneda { get; set; }
}
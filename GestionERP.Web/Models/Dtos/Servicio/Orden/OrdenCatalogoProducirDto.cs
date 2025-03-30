namespace GestionERP.Web.Models.Dtos.Servicio;

public class OrdenCatalogoProducirDto
{
    public string CodigoPeriodo { get; set; }
    public string CodigoOrden { get; set; }
    public string CodigoEntidad { get; set; }
    public string NombreEntidad { get; set; } 
    public DateTime FechaEmision { get; set; } 
    public string CodigoMoneda { get; set; }
    public string NombreMoneda { get; set; }
    public string SimboloMoneda { get; set; }
    public decimal TotalImporteBruto { get; set; }
    public string Observacion { get; set; }
}
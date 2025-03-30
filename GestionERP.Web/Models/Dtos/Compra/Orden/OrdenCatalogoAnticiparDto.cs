namespace GestionERP.Web.Models.Dtos.Compra;

public class OrdenCatalogoAnticiparDto
{
    public Guid OrdenId { get; set; }
    public Guid NexoDocumentoId { get; set; }
    public string CodigoEjercicio { get; set; }
    public string CodigoPeriodo { get; set; } 
    public string CodigoEntidad { get; set; }
    public string NombreEntidad { get; set; }
    public string CodigoDocumento { get; set; }
    public string CodigoSerieDocumento { get; set; }
    public string NombreSerieDocumento { get; set; }
    public string NumeroSerieDocumento { get; set; }
    public DateTime FechaEmision { get; set; }
    public string CodigoTipoProvision { get; set; }
    public string NombreTipoProvision { get; set; } 
    public string CodigoMoneda { get; set; }
    public string NombreMoneda { get; set; } 
    public decimal TotalImporteBrutoSaldo { get; set; } 
    public IEnumerable<OrdenDetalleCatalogoAnticiparDto> Detalle { get; set; }
}
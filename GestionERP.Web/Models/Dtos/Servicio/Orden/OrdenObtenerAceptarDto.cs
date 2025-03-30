namespace GestionERP.Web.Models.Dtos.Servicio;

public class OrdenObtenerAceptarDto
{
    public Guid OrdenId { get; set; }
    public string CodigoEmpresa { get; set; }
    public string NombreEmpresa { get; set; }
    public string CodigoPeriodo { get; set; }
    public string CodigoEntidad { get; set; }
    public string NombreEntidad { get; set; }
    public string CodigoDocumento { get; set; }
    public string CodigoSerieDocumento { get; set; }
    public string NombreSerieDocumento { get; set; }
    public string NumeroSerieDocumento { get; set; }
    public DateTime FechaEmision { get; set; }
    public DateTime FechaAtencion { get; set; }
    public string CodigoMoneda { get; set; }
    public string SimboloMoneda { get; set; }
    public string NombreMoneda { get; set; }
    public decimal TotalImporteBruto { get; set; }
    public string FlagEstadoAceptacion { get; set; }
    public IEnumerable<OrdenDetalleObtenerAceptarDto> Detalles { get; set; }
}
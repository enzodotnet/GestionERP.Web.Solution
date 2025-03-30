namespace GestionERP.Web.Models.Dtos.Servicio;

public class OrdenCatalogoActualizarEstadoDto
{
    public Guid OrdenId { get; set; }
    public string CodigoOrden { get; set; }
    public string CodigoEjercicio { get; set; }
    public string CodigoPeriodo { get; set; }
    public string FlagOrigen { get; set; }
    public string CodigoEntidad { get; set; }
    public string NombreEntidad { get; set; }
    public string CodigoDocumento { get; set; }
    public string CodigoSerieDocumento { get; set; }
    public string NombreSerieDocumento { get; set; }
    public string NumeroSerieDocumento { get; set; }
    public DateTime FechaEmision { get; set; }
    public DateTime FechaAtencion { get; set; } 
    public string NombreTipoProvision { get; set; }
    public string NombreLocalAtencion { get; set; }
    public string NombreArea { get; set; }
    public string NombreCentroCosto { get; set; }
    public string DescripcionLugarAtencion { get; set; }
    public string NombreModoPago { get; set; }    
    public string NombrePlazoCredito { get; set; }
    public string FlagMedioPago { get; set; }
    public bool EsPagoAnticipado { get; set; }
    public bool EsVinculableProduccion { get; set; }
    public string FlagEstadoAceptacion { get; set; }
    public bool EsAfectoImpuesto { get; set; }
    public string CodigoTipoImpuesto { get; set; }
    public string NombreTipoImpuesto { get; set; }
    public decimal? PorcentajeImpuesto { get; set; }
    public string CodigoMoneda { get; set; }
    public string SimboloMoneda { get; set; }
    public string NombreMoneda { get; set; }
    public decimal TotalImporteBruto { get; set; }
    public decimal TotalImporteImpuesto { get; set; }
    public decimal TotalImporteNeto { get; set; }
    public string Observacion { get; set; }
    public string Motivo { get; set; }
    public string NombreEstado { get; set; }
    public IEnumerable<OrdenDetalleCatalogoActualizarEstadoDto> Detalles { get; set; }
}
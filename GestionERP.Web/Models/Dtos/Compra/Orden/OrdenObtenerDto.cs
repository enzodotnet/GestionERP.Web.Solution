using GestionERP.Web.Models.Objects;

namespace GestionERP.Web.Models.Dtos.Compra;

public class OrdenObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string CodigoPeriodo { get; set; }
    public string FlagOrigen { get; set; }
    private string codigoEntidad;
    public string CodigoEntidad { get => codigoEntidad; set => codigoEntidad = value?.TrimEnd(); }
    public string NombreEntidad { get; set; }
    public string CodigoDocumento { get; set; }
    public string CodigoSerieDocumento { get; set; }
    public string NombreSerieDocumento { get; set; }
    public string NumeroSerieDocumento { get; set; }
    public DateTime? FechaEmision { get; set; }
    public string CodigoTipoProvision { get; set; }
    public string NombreTipoProvision { get; set; }
    public bool EsAfectoImpuesto { get; set; }
    public string CodigoTipoImpuesto { get; set; }
    public string NombreTipoImpuesto { get; set; }
    public decimal? PorcentajeImpuesto { get; set; }
    public string CodigoMoneda { get; set; }
    public string NombreMoneda { get; set; }
    public string SimboloMoneda { get; set; }
    public decimal TotalImporteBruto { get; set; }
    public decimal TotalImporteImpuesto { get; set; }
    public decimal TotalImporteNeto { get; set; }
    public DateTime FechaEntrega { get; set; }
    public string CodigoArea { get; set; }
    public string NombreArea { get; set; }
    public string CodigoLocalRecepcion { get; set; }
    public string NombreLocalRecepcion { get; set; }
    public string DescripcionLugarEntrega { get; set; }
    public string CodigoModoPago { get; set; }
    public string NombreModoPago { get; set; }
    public string CodigoPlazoCredito { get; set; }
    public string NombrePlazoCredito { get; set; }
    public string FlagMedioPago { get; set; }
    public bool EsPagoAnticipado { get; set; }
    private string codigoCentroCosto;
    public string CodigoCentroCosto { get => codigoCentroCosto; set => codigoCentroCosto = value?.TrimEnd(); }
    public string NombreCentroCosto { get; set; }
    public string Observacion { get; set; }
    public string Motivo { get; set; }
    public string FlagEstadoAnticipo { get; set; } 
    public bool EsRescindido { get; set; }
    public bool EsAtendidoParcial { get; set; }
    public string FlagEstadoIngreso { get; set; }
    public string CodigoEstado { get; set; }
    public string NombreEstado { get; set; }
    public List<OrdenDetalleObtenerDto> Detalles { get; set; }
    public IEnumerable<AuditoriaObject> Auditorias { get; set;}
}
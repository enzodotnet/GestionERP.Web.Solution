namespace GestionERP.Web.Models.Dtos.Importacion;

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
    public string CodigoMoneda { get; set; }
    public string NombreMoneda { get; set; }
    public string SimboloMoneda { get; set; }
    public decimal TotalImporteBruto { get; set; }
    public string CodigoTipoImportacion { get; set; }
    public string NombreTipoImportacion { get; set; }
    public string NombreAduana { get; set; }
    public DateTime FechaEstimadaETD { get; set; }
    public DateTime FechaEstimadaETA { get; set; }
    public DateTime FechaReposicionStock { get; set; }
    public string NombrePaisOrigen { get; set; }
    public string NombrePaisProcedencia { get; set; }
    public string DescripcionLugarEntrega { get; set; }
    public string NombreModoPago { get; set; }
    public string NombrePlazoCredito { get; set; }
    public string FlagMedioPago { get; set; }
    public string DescripcionFormaPago { get; set; }
    public string Observacion { get; set; }
    public string NombreEstado { get; set; }
    public IEnumerable<OrdenDetalleCatalogoActualizarEstadoDto> Detalles { get; set; }
    public IEnumerable<OrdenNotaCatalogoActualizarEstadoDto> Notas { get; set; }
}
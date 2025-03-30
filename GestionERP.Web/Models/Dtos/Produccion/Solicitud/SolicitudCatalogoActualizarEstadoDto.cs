namespace GestionERP.Web.Models.Dtos.Produccion;

public class SolicitudCatalogoActualizarEstadoDto
{
    public Guid SolicitudId { get; set; }
    public string CodigoSolicitud { get; set; }
    public string CodigoEjercicio { get; set; }
    public string CodigoPeriodo { get; set; }
    public string FlagTipoRegistro { get; set; }
    public string CodigoEntidad { get; set; }
    public string NombreEntidad { get; set; }
    public string CodigoDocumento { get; set; }
    public string CodigoSerieDocumento { get; set; }
    public string NombreSerieDocumento { get; set; }
    public string NumeroSerieDocumento { get; set; }
    public string NombrePlan { get; set; }
    public string NombreTipoProduccion { get; set; }
    public string CodigoArticuloTerminado { get; set; }
    public string NombreArticuloTerminado { get; set; }
    public string CodigoUnidadMedida { get; set; }
    public string NombreUnidadMedida { get; set; }
    public string PresentacionArticulo { get; set; }
    public decimal Cantidad { get; set; }
    public decimal CantidadAtendida { get; set; }
    public DateTime FechaEmision { get; set; }
    public DateTime FechaEntrega { get; set; }
    public string NombreLocalRecepcion { get; set; }
    public string NombreUsuarioSolicitante { get; set; }
    public string NombreArea { get; set; }
    public string Observacion { get; set; }
    public string Motivo { get; set; }
    public string FlagNivelPrioridad { get; set; }
    public string NombreEstado { get; set; }
}
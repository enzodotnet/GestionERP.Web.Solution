namespace GestionERP.Web.Models.Dtos.Produccion;

public class SolicitudCatalogoAtenderDto
{
    public string CodigoPeriodo { get; set; }
    public string CodigoEntidad { get; set; }
    public string NombreEntidad { get; set; }
    public string CodigoSolicitud { get; set; }
    public string NombreSerieDocumentoSolicitud { get; set; }
    public string CodigoPlan { get; set; }
    public string NombrePlan { get; set; } 
    public string CodigoTipoProduccion { get; set; }
    public string NombreTipoProduccion { get; set; }
    public string CodigoArticuloTerminado { get; set; }
    public string NombreArticuloTerminado { get; set; }
    public string CodigoUnidadMedida { get; set; }
    public string NombreUnidadMedida { get; set; }
    public string PresentacionArticulo { get; set; }
    public decimal UnidadConversionArticulo { get; set; }
    public decimal Cantidad { get; set; }
    public DateTime FechaEmision { get; set; }
    public DateTime FechaEntrega { get; set; }
    public string CodigoLocalRecepcion { get; set; }
    public string NombreLocalRecepcion { get; set; }
    public string Observacion { get; set; }
    public string Motivo { get; set; }
    public bool IsErrorSelected { get; set; } 
}
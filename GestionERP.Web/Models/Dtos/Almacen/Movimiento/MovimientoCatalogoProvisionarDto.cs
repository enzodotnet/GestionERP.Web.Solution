namespace GestionERP.Web.Models.Dtos.Almacen;

public class MovimientoCatalogoProvisionarDto
{
    public Guid MovimientoId { get; set; }
    public string CodigoComprobanteReferencia { get; set; }
    public string CodigoDocumentoReferencia { get; set; }
    public string SerieDocumentoReferencia { get; set; }
    public string NumeroDocumentoReferencia { get; set; }
    public string CodigoTipoProvision { get; set; }
    public string NombreTipoProvision { get; set; }
    public string CodigoEntidad { get; set; }
    public string NombreEntidad { get; set; }
    public string CodigoEjercicio { get; set; }
    public string CodigoPeriodo { get; set; }
    public string CodigoOperacionLogistica { get; set; }
    public string NumeroOperacionLogistica { get; set; }
    public DateTime FechaHoraOperacion { get; set; }
    public string CodigoMoneda { get; set; }
    public string NombreMoneda { get; set; }
    public decimal ImporteBruto { get; set; }
    public bool EsPagoAnticipado { get; set; }
}
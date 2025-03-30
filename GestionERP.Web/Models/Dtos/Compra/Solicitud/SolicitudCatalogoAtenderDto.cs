namespace GestionERP.Web.Models.Dtos.Compra;

public class SolicitudCatalogoAtenderDto
{
    public string CodigoPeriodo { get; set; }
    public string CodigoEntidad { get; set; }
    public string NombreEntidad { get; set; }
    public string CodigoSolicitud { get; set; } 
    public string NombreSerieDocumentoSolicitud { get; set; }
    public DateTime FechaEmision { get; set; }
    public DateTime FechaEntrega { get; set; }
    public string CodigoLocalRecepcion { get; set; }
    public string NombreLocalRecepcion { get; set; }
    public string CodigoArea { get; set; }
    public string NombreArea { get; set; }
    public string Observacion { get; set; }
    public string Motivo { get; set; }
    public string CodigoModoPago { get; set; }
    public string NombreModoPago { get; set; }
    public string CodigoPlazoCredito { get; set; }
    public string NombrePlazoCredito { get; set; }
    public string CodigoTipoArticulo { get; set; }
    public string NombreTipoArticulo { get; set; }
    public string CodigoArticulo { get; set; }
    public string NombreArticulo { get; set; }
    public string CodigoUnidadMedida { get; set; }
    public string NombreUnidadMedida { get; set; }
    public string Presentacion { get; set; }
    public decimal? UnidadConversion { get; set; }
    public bool EsAfectoImpuesto { get; set; }
    public decimal Cantidad { get; set; }
	public bool IsErrorSelected { get; set; } 
}
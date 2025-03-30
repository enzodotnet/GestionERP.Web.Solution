using GestionERP.Web.Models.Objects;

namespace GestionERP.Web.Models.Dtos.Almacen;

public class MovimientoObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string CodigoPeriodo { get; set; }
    public string FlagTipoRegistro { get; set; }
    public string CodigoOperacionLogistica { get; set; }
    public string NombreOperacionLogistica { get; set; }
    public string NumeroOperacionLogistica { get; set; }
    private string codigoEntidad;
    public string CodigoEntidad { get => codigoEntidad; set => codigoEntidad = value?.TrimEnd(); }
    public string NombreEntidad { get; set; }
    public DateTime? FechaHoraOperacion { get; set; }
    public string CodigoLocal { get; set; }
    public string NombreLocal { get; set; }
    public string CodigoAlmacenDestino { get; set; }
    public string NombreAlmacenDestino { get; set; }
    public string CodigoMoneda { get; set; }
    public string SimboloMoneda { get; set; }
    public string NombreMoneda { get; set; }
    public string CodigoTipoCambioDia { get; set; }
    public decimal MontoTipoCambioDia { get; set; }
    public decimal TotalValorCostoMN { get; set; }
    public decimal TotalValorCostoME { get; set; }
    public string CodigoDocumentoReferencia { get; set; }
    public string SerieDocumentoReferencia { get; set; }
    public string NumeroDocumentoReferencia { get; set; }
    public string DocumentoReferencia { get; set; }
    public string DescripcionReferencia { get; set; }
    public string CodigoCentroCosto { get; set; }
    public string NombreCentroCosto { get; set; }
    public string CodigoMovimientoReferencia { get; set; }
    public string Observacion { get; set; }
    public string Comentario { get; set; }
    public bool EsOrigenAutomatico { get; set; }
    public bool EsGeneradoAsiento { get; set; }
    public string CodigoEstado { get; set; }
    public string NombreEstado { get; set; }
    public List<MovimientoDetalleObtenerDto> Detalles { get; set; }
    public IEnumerable<AuditoriaObject> Auditorias { get; set; }
}
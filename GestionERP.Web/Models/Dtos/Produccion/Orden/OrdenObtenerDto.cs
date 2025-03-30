using GestionERP.Web.Models.Objects;

namespace GestionERP.Web.Models.Dtos.Produccion;

public class OrdenObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string CodigoPeriodo { get; set; }
    public string FlagOrigen { get; set; }
    public string FlagTipoRegistro { get; set; }
    private string codigoEntidad;
    public string CodigoEntidad { get => codigoEntidad; set => codigoEntidad = value?.TrimEnd(); }
    public string NombreEntidad { get; set; }
    public string CodigoDocumento { get; set; }
    public string CodigoSerieDocumento { get; set; }
    public string NombreSerieDocumento { get; set; }
    public string NumeroSerieDocumento { get; set; }
    public string CodigoPlan { get; set; }
    public string NombrePlan { get; set; }
    public string CodigoVersionPlan { get; set; }
    public string NombreVersionPlan { get; set; }
    public string CodigoTipoProduccion { get; set; }
    public string NombreTipoProduccion { get; set; }     
    public string CodigoArticuloTerminado { get; set; }
    public string NombreArticuloTerminado { get; set; }
    public string CodigoUnidadMedida { get; set; }
    public string NombreUnidadMedida { get; set; }
    public string PresentacionArticulo { get; set; }
    public decimal? UnidadConversionArticulo { get; set; }
    public decimal? Cantidad { get; set; }
    public DateTime? FechaEmision { get; set; }
    public DateTime? FechaCosto { get; set; } 
    public string CodigoTipoCambioDia { get; set; }
    public decimal? MontoTipoCambioDia { get; set; }
    public DateTime? FechaInicio { get; set; } 
    public DateTime? FechaTermino { get; set; } 
    public string CodigoLocalRecepcion { get; set; }
    public string NombreLocalRecepcion { get; set; }
    public string CodigoSolicitudReferencia { get; set; }
    public string Observacion { get; set; }
    public string Motivo { get; set; }
    public string ComentarioProceso { get; set; }
    public decimal? CostoEstimadoMN { get; set; }
    public decimal? CostoEstimadoME { get; set; }
    public decimal? CostoCalculadoMN { get; set; }
    public decimal? CostoCalculadoME { get; set; }
    public string FlagEstadoTransferencia { get; set; }
    public string FlagEstadoConsumo { get; set; } 
    public string FlagEstadoIngreso { get; set; }
    public decimal CantidadIngresada { get; set; }
    public string CodigoEstado { get; set; }
    public string NombreEstado { get; set; }
    public List<OrdenMaterialObtenerDto> Materiales { get; set; }
    public List<OrdenLoteObtenerDto> Lotes { get; set; }
    public List<OrdenMaquilaObtenerDto> Maquilas { get; set; }
    public List<OrdenTareoObtenerDto> Tareos { get; set; }
    public IEnumerable<AuditoriaObject> Auditorias { get; set; }
}
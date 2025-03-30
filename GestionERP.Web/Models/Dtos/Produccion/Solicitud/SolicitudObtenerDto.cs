using GestionERP.Web.Models.Objects;

namespace GestionERP.Web.Models.Dtos.Produccion;

public class SolicitudObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string CodigoPeriodo { get; set; }
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
    public string CodigoTipoProduccion { get; set; }
    public string NombreTipoProduccion { get; set; } 
    private string codigoArticuloTerminado;
    public string CodigoArticuloTerminado { get => codigoArticuloTerminado; set => codigoArticuloTerminado = value?.TrimEnd(); }
    public string NombreArticuloTerminado { get; set; }
    public string CodigoUnidadMedida { get; set; }
    public string NombreUnidadMedida { get; set; }
    public string PresentacionArticulo { get; set; }
    public decimal? UnidadConversionArticulo { get; set; }
    public decimal? Cantidad { get; set; }
    public DateTime? FechaEmision { get; set; }
    public DateTime? FechaEntrega { get; set; } 
    public string CodigoArea { get; set; }
    public string NombreArea { get; set; }
    public string CodigoLocalRecepcion { get; set; }
    public string NombreLocalRecepcion { get; set; }
    public string FlagNivelPrioridad { get; set; }
    public string Observacion { get; set; }
    public string Motivo { get; set; }
    public string CodigoUsuarioSolicitante { get; set; }
    public string NombreUsuarioSolicitante { get; set; }
    public bool EsRescindido { get; set; }
    public bool EsAtendidoParcial { get; set; }
    public decimal CantidadAtendida { get; set; }
    public decimal CantidadRescindida { get; set; }
    public string CodigoEstado { get; set; }
    public string NombreEstado { get; set; }
    public IEnumerable<AuditoriaObject> Auditorias { get; set;}
}
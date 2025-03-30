using GestionERP.Web.Models.Objects;

namespace GestionERP.Web.Models.Dtos.Servicio;

public class SolicitudObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string CodigoPeriodo { get; set; }
    private string codigoEntidad;
    public string CodigoEntidad { get => codigoEntidad; set => codigoEntidad = value?.TrimEnd(); }
    public string NombreEntidad { get; set; }
    public string ReferenciaProveedor { get; set; } 
    public string CodigoDocumento { get; set; }
    public string CodigoSerieDocumento { get; set; }
    public string NombreSerieDocumento { get; set; }
    public string NumeroSerieDocumento { get; set; }
    public DateTime? FechaEmision { get; set; }
    public DateTime FechaAtencion { get; set; } 
    public string CodigoArea { get; set; }
    public string NombreArea { get; set; }
    public string CodigoLocalAtencion { get; set; }
    public string NombreLocalAtencion { get; set; }
    public string FlagNivelPrioridad { get; set; }
    public bool EsRequeridoRevision { get; set; }
    public string Observacion { get; set; }
    public string Motivo { get; set; }
    public string CodigoUsuarioSolicitante { get; set; }
    public string NombreUsuarioSolicitante { get; set; }
    public bool EsRescindido { get; set; }
    public bool EsAtendidoParcial { get; set; }
    public string CodigoEstado { get; set; }
    public string NombreEstado { get; set; }
    public List<SolicitudDetalleObtenerDto> Detalles { get; set; }
    public IEnumerable<AuditoriaObject> Auditorias { get; set;}
}
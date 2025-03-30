using GestionERP.Web.Models.Objects;

namespace GestionERP.Web.Models.Dtos.Importacion;

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
    public string CodigoMoneda { get; set; }
    public string NombreMoneda { get; set; }
    public string SimboloMoneda { get; set; }
    public decimal TotalImporteBruto { get; set; }
    public string CodigoTipoImportacion { get; set; }
    public string NombreTipoImportacion { get; set; }
    public string CodigoAduana { get; set; }
    public string NombreAduana { get; set; }
    public DateTime FechaEstimadaETD { get; set; }
    public DateTime FechaEstimadaETA { get; set; }
    public DateTime FechaReposicionStock { get; set; }
    public string CodigoPaisOrigen { get; set; }
    public string NombrePaisOrigen { get; set; }
    public string CodigoPaisProcedencia { get; set; }
    public string NombrePaisProcedencia { get; set; }
    public string DescripcionLugarEntrega { get; set; }
    public string CodigoModoPago { get; set; }
    public string NombreModoPago { get; set; }
    public string CodigoPlazoCredito { get; set; }
    public string NombrePlazoCredito { get; set; }
    public string FlagMedioPago { get; set; }
    public string DescripcionFormaPago { get; set; }
    public string Observacion { get; set; } 
    public bool EsRescindido { get; set; }
    public bool EsAtendidoParcial { get; set; }
    public string CodigoEstado { get; set; }
    public string NombreEstado { get; set; }
    public List<OrdenDetalleObtenerDto> Detalles { get; set; }
    public List<OrdenNotaObtenerDto> Notas { get; set; }
    public IEnumerable<AuditoriaObject> Auditorias { get; set; }
}
using GestionERP.Web.Models.Objects;

namespace GestionERP.Web.Models.Dtos.Servicio;

public class ContratoObtenerDto
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
    public DateTime? FechaEmision { get; set; }
    public string CodigoTipoServicio { get; set; }
    public string NombreTipoServicio { get; set; }
    public string CodigoTipoProvision { get; set; }
    public string NombreTipoProvision { get; set; }
    public bool EsAfectoImpuesto { get; set; }
    public string CodigoTipoImpuesto { get; set; }
    public string NombreTipoImpuesto { get; set; }
    public decimal? PorcentajeImpuesto { get; set; }
    public string CodigoMoneda { get; set; }
    public string NombreMoneda { get; set; }
    public string SimboloMoneda { get; set; }
    public decimal TotalImporteBruto { get; set; }
    public decimal TotalImporteImpuesto { get; set; }
    public decimal TotalImporteNeto { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public int CantidadAnios { get; set; }
    public int CantidadMeses { get; set; }
    public int CantidadDias { get; set; }
    public string CodigoArea { get; set; }
    public string NombreArea { get; set; }
    public string FlagIntervaloInforme { get; set; }
    public string CodigoUsuarioVerifica { get; set; }
    public string NombreUsuarioVerifica { get; set; }
    public string CodigoUsuarioAutoriza { get; set; }
    public string NombreUsuarioAutoriza { get; set; }
    public string Referencia { get; set; }  
    public string CodigoModoPago { get; set; }
    public string NombreModoPago { get; set; }
    public string CodigoPlazoCredito { get; set; }
    public string NombrePlazoCredito { get; set; }
    public string FlagMedioPago { get; set; }
    public int CantidadCuotas { get; set; }
    public bool EsGenerableDevengo { get; set; }
    public string CodigoTipoDevengo { get; set; }
    public string NombreTipoDevengo { get; set; }
    public string FlagEstadoDevengo { get; set; } 
    private string codigoCentroCosto;
    public string CodigoCentroCosto { get => codigoCentroCosto; set => codigoCentroCosto = value?.TrimEnd(); }
    public string NombreCentroCosto { get; set; }
    public string Observacion { get; set; }
    public bool EsRescindido { get; set; }
    public bool EsAtendidoParcial { get; set; }
    public string CodigoEstado { get; set; }
    public string NombreEstado { get; set; }
    public List<ContratoDetalleObtenerDto> Detalles { get; set; }
    public List<ContratoCuotaObtenerDto> Cuotas { get; set; }
    public List<ContratoTerminoObtenerDto> Terminos { get; set; }
    public IEnumerable<AuditoriaObject> Auditorias { get; set;}
}
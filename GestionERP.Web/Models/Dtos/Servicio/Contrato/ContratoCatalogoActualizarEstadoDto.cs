namespace GestionERP.Web.Models.Dtos.Servicio;

public class ContratoCatalogoActualizarEstadoDto
{
    public Guid ContratoId { get; set; }
    public string CodigoContrato { get; set; }
    public string CodigoEjercicio { get; set;}
    public string CodigoPeriodo { get; set; }
    public string FlagTipoRegistro { get; set; }
    public string CodigoEntidad { get; set; }
    public string NombreEntidad { get; set; }
    public string CodigoDocumento { get; set; }
    public string CodigoSerieDocumento { get; set; }
    public string NombreSerieDocumento { get; set; }
    public string NumeroSerieDocumento { get; set; }
    public DateTime FechaEmision { get; set; }
    public string NombreTipoServicio { get; set; }
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
    public string NombreArea { get; set; }
    public string FlagIntervaloInforme { get; set; }
    public string NombreUsuarioVerifica { get; set; }
    public string NombreUsuarioAutoriza { get; set; }
    public string Referencia { get; set; }  
    public string NombreModoPago { get; set; }
    public string NombrePlazoCredito { get; set; }
    public string FlagMedioPago { get; set; }
    public int CantidadCuotas { get; set; }
    public bool EsGenerableDevengo { get; set; }
    public string NombreTipoDevengo { get; set; }
    public string NombreCentroCosto { get; set; }
    public string Observacion { get; set; }
    public string NombreEstado { get; set; }
    public IEnumerable<ContratoDetalleCatalogoActualizarEstadoDto> Detalles { get; set; }
    public IEnumerable<ContratoCuotaCatalogoActualizarEstadoDto> Cuotas { get; set; }
    public IEnumerable<ContratoTerminoCatalogoActualizarEstadoDto> Terminos { get; set; }
}
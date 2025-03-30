using GestionERP.Web.Models.Objects;

namespace GestionERP.Web.Models.Dtos.Importacion;

public class PedidoObtenerDto
{ 
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string CodigoPeriodo { get; set; }
    public string CodigoDocumento { get; set; }
    public string CodigoSerieDocumento { get; set; }
    public string NombreSerieDocumento { get; set; }
    public string NumeroSerieDocumento { get; set; }
    public string CodigoOrdenReferencia { get; set; }
    public string CodigoEntidad { get; set; }
    public string NombreEntidad { get; set; }
    public DateTime? FechaEmision { get; set; }
    public string DescripcionFormaPago { get; set; }
    public string CodigoMoneda { get; set; }
    public string NombreMoneda { get; set; }
    public string SimboloMoneda { get; set; }
    public DateTime? FechaCosto { get; set; }
    public string CodigoTipoCambioDia { get; set; }
    public decimal? MontoTipoCambioDia { get; set; }
    public string CodigoTipoImportacion { get; set; }
    public string NombreTipoImportacion { get; set; }
    public string NumeroPoliza { get; set; }
    public DateTime? FechaEmisionPoliza { get; set; }
    public string NumeroDUA { get; set; }
    public DateTime? FechaEmisionDUA { get; set; }
    public DateTime? FechaCancelacionDUA { get; set; }
    public string ComentarioDUA { get; set; }
    public string FlagCanal { get; set; }
    public string NumeroBL { get; set; }
    public DateTime? FechaEmisionBL { get; set; }
    public string NumeroInvoice { get; set; }
    public DateTime? FechaEmisionInvoice { get; set; }
    public int? CantidadDiasVenceInvoice { get; set; }
    public DateTime? FechaVencimientoInvoice { get; set; }
    public decimal TotalImporteBruto { get; set; }
    public decimal TotalImporteFlete { get; set; }
    public decimal TotalImporteSeguro { get; set; }
    public decimal TotalImporteNeto { get; set; }
    public bool EsAfectoLiberaArancel { get; set; }
    public bool EsPrevioAforo { get; set; }
    public bool EsSujetoReclamo { get; set; }
    public string ComentarioReclamo { get; set; }
    public DateTime? FechaPackList { get; set; }
    public string NumeroPackList { get; set; }
    public string Referencia { get; set; }
    public string Observacion { get; set; }
    public string Comentario { get; set; }
    public string DescripcionMarca { get; set; }
    public DateTime? FechaCopiaDocumento { get; set; }
    public DateTime? FechaOriginalDocumento { get; set; }
    public string ComentarioDocumentacion { get; set; }
    public int? CantidadDiasSobreestadia { get; set; }
    public int? CantidadDiasAlmacenaje { get; set; }
    public DateTime? FechaDireccionamiento { get; set; }
    public DateTime? FechaEntregaDocumento { get; set; }
    public DateTime? FechaVistoBueno { get; set; }
    public string CodigoSituacionImportacion { get; set; }
    public string NombreSituacionImportacion { get; set; }
    public DateTime? FechaEmbarque { get; set; }
    public DateTime? FechaEstimadaETD { get; set; }
    public DateTime? FechaEstimadaETA { get; set; }
    public DateTime? FechaReposicionStock { get; set; }
    public string DescripcionTramiteAduana { get; set; }
    public string DescripcionModalidad { get; set; }
    public string CodigoRegimenImportacion { get; set; }
    public string NombreRegimenImportacion { get; set; }
    public DateTime? FechaSobreestadia { get; set; }
    public DateTime? FechaLugarAlmacenaje { get; set; }
    public DateTime? FechaRecojoDocumentoAduana { get; set; }
    public string CreditoAgenteAduana { get; set; }
    public DateTime? FechaPagoAgenteAduana { get; set; }
    public string NumeroPresupuesto { get; set; }
    public DateTime? FechaAforoCanal { get; set; }
    public DateTime? FechaLevanteCanal { get; set; }
    public bool EsRequeridoInspeccion { get; set; }
    public DateTime? FechaInspeccion { get; set; }
    public DateTime? FechaLevanteInspeccion { get; set; }
    public string FlagTipoFinanciamiento { get; set; }
    public DateTime? FechaPagoFinanciamiento { get; set; }
    public string NumeroManifiesto { get; set; }
    public DateTime? FechaLlegada { get; set; }
    public DateTime? FechaDescarga { get; set; }
    public DateTime? FechaTarja { get; set; }
    public DateTime? FechaDesgloseCarga { get; set; }
    public DateTime? FechaEstimadaArribo { get; set; }
    public string NombreTipoEmbarque { get; set; }
    public string DescripcionEmpaque { get; set; }
    public bool EsTransbordo { get; set; }
    public string FlagModalidadEmbarque { get; set; }
    public string CodigoPaisOrigen { get; set; }
    public string NombrePaisOrigen { get; set; }
    public string CodigoPaisProcedencia { get; set; }
    public string NombrePaisProcedencia { get; set; }
    public string CodigoPaisEmbarque { get; set; }
    public string NombrePaisEmbarque { get; set; }
    public string CodigoPuertoEmbarque { get; set; }
    public string NombrePuertoEmbarque { get; set; }
    public string CodigoPaisLlegada { get; set; }
    public string NombrePaisLlegada { get; set; }
    public string CodigoPuertoLlegada { get; set; }
    public string NombrePuertoLlegada { get; set; }
    public string CodigoTransporteImportacion { get; set; }
    public string NombreTransporteImportacion { get; set; }
    public string FlagViaTransporte { get; set; }
    public string NombreConsolidador { get; set; }
    public string CodigoAduana { get; set; }
    public string NombreAduana { get; set; }
    private string codigoEntidadAgenteAduana;
    private string codigoEntidadMuelle;
    private string codigoEntidadAgenciaCarga;
    private string codigoEntidadTerminal;
    private string codigoEntidadAlmacen;
    private string codigoEntidadNaviera;
    public string CodigoEntidadAgenteAduana { get => codigoEntidadAgenteAduana; set => codigoEntidadAgenteAduana = value?.TrimEnd(); }
    public string NombreEntidadAgenteAduana { get; set; }
    public string CodigoEntidadMuelle { get => codigoEntidadMuelle; set => codigoEntidadMuelle = value?.TrimEnd(); }
    public string NombreEntidadMuelle { get; set; }
    public string CodigoEntidadAgenciaCarga { get => codigoEntidadAgenciaCarga; set => codigoEntidadAgenciaCarga = value?.TrimEnd(); }
    public string NombreEntidadAgenciaCarga { get; set; }
    public string CodigoEntidadTerminal { get => codigoEntidadTerminal; set => codigoEntidadTerminal = value?.TrimEnd(); }
    public string NombreEntidadTerminal { get; set; }
    public string CodigoEntidadAlmacen { get => codigoEntidadAlmacen; set => codigoEntidadAlmacen = value?.TrimEnd(); }
    public string NombreEntidadAlmacen { get; set; }
    public string CodigoEntidadNaviera { get => codigoEntidadNaviera; set => codigoEntidadNaviera = value?.TrimEnd(); }
    public string NombreEntidadNaviera { get; set; }
    public string NombreNave { get; set; }
    public string DescripcionTransporte { get; set; }
    public string DescripcionLocalDestino { get; set; }
    public bool EsVentaSucesiva { get; set; }
    public string FlagEstadoIngreso { get; set; }
    public string CodigoEstado { get; set; }
    public string NombreEstado { get; set; }
    public List<PedidoDetalleObtenerDto> Detalles { get; set; }
	public IEnumerable<AuditoriaObject> Auditorias { get; set; }
}
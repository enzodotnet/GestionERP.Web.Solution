namespace GestionERP.Web.Models.Dtos.Principal;

public class EmpresaAtributoObtenerDto
{ 
    public string FlagTipoRubro { get; set; }
    public string DireccionFiscal { get; set; }
    public string DireccionDeposito { get; set; }
    public string TelefonoCorporativo { get; set; }
    public string TelefonoVenta { get; set; }
    public string EmailCorporativo { get; set; }
    public string EmailVenta { get; set; }
    public string EmailCompra { get; set; }
    public string EmailCobranza { get; set; }
    public string PaginaWeb { get; set; }
    public bool EsTransportista { get; set; }
    public bool EsAgenteRetenedor { get; set; }
    public bool EnFacturacionSunatPortal { get; set; }
    public bool EnFacturacionExternoOse { get; set; }
    public string CodigoCuentaContableImpuestoExtorno { get; set; }
    public string NombreCuentaContableImpuestoExtorno { get; set; }
    public string CodigoCuentaContableOrdenDeudor { get; set; }
    public string NombreCuentaContableOrdenDeudor { get; set; }
    public string CodigoCuentaContableOrdenAcreedor { get; set; }
    public string NombreCuentaContableOrdenAcreedor { get; set; }
    public string CuentaCorrienteDetraccion { get; set; }
    public int? CantidadDiasPlazoFacturaVencida { get; set; }
    public bool EsEmisorCertificadoAnalisis { get; set; }
    public decimal? PorcentajeIndicadorVenta { get; set; }
}
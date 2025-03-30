namespace GestionERP.Web.Models.Dtos.Principal;

public class DocumentoListarDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string FlagTipoEntidad { get; set; }
    public string CodigoTipoDocumento { get; set; }
    public string NombreTipoDocumento { get; set; }
    public string FlagAtributo { get; set; }
    public string CodigoTipoComprobante { get; set; }
    public string NombreTipoComprobante { get; set; }
    public bool GeneraCuentaCorriente { get; set; } 
    public bool EsRegistroCompraVenta { get; set; }
    public bool EsAfectoRetencion { get; set; }
    public bool EsElectronico { get; set; }
    public bool EsTransferenciaGratuita { get; set; }
    public bool ConservaTipoCambioOrigen { get; set; }
    public bool Activo { get; set; } 
}
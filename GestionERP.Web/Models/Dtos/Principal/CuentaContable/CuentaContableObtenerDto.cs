namespace GestionERP.Web.Models.Dtos.Principal;

public class CuentaContableObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string CodigoPlanContable { get; set; }
    public string NombrePlanContable { get; set; }
    public string Descripcion { get; set; }
    public string FlagTipo { get; set; }
    public string FlagFormatoFuncion { get; set; }
    public string FlagTipoNaturaleza { get; set; }
    public string FlagTipoEntidadFinanciera { get; set; }
    public string FlagTipoCuentaCorriente { get; set; }
    public bool ExigeEntidadComprobante { get; set; }
    public bool ExigeCentroCosto { get; set; }
    public bool ExigeMonedaExtranjera { get; set; }
    public bool ExigeCuentaExistencia { get; set; }
    public string CodigoTipoBienServicio { get; set; }
    public string NombreTipoBienServicio { get; set; }
    public string CodigoCuentaBalance { get; set; }
    public string NombreCuentaBalance { get; set; } 
    public string CodigoCasillaBalance { get; set; }
    public string NombreCasillaBalance { get; set; }
    public string CodigoCasillaBalanceDetalle { get; set; }
    public string NombreCasillaBalanceDetalle { get; set; } 
    public bool EsCuentaDestino { get; set; }
    public bool EsCuentaOrden { get; set; }
    public bool EsCuentaVinculoBanco { get; set; }
    public bool Activo { get; set; }
    public List<CuentaContableDetalleObtenerDto> Detalles { get; set; }
    public List<CuentaContableDestinoObtenerDto> Destinos { get; set; } 
}
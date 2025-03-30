namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class CuentaContableDetalleTipoCambioType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    
    public static IEnumerable<CuentaContableDetalleTipoCambioType> ObtenerTipos()
    {
        return new List<CuentaContableDetalleTipoCambioType>
        {
            new CuentaContableDetalleTipoCambioType {Codigo = "C", Nombre = "Compra"},
            new CuentaContableDetalleTipoCambioType {Codigo = "V", Nombre = "Venta"}
        };
    }
}
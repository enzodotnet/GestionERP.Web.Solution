namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class CuentaContableDetalleRegistroType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    
    public static IEnumerable<CuentaContableDetalleRegistroType> ObtenerTipos()
    {
        return new List<CuentaContableDetalleRegistroType>
        {
            new CuentaContableDetalleRegistroType {Codigo = "D", Nombre = "Diferencia de cambio"},
            new CuentaContableDetalleRegistroType {Codigo = "T", Nombre = "Traduccion monetaria"} 
        };
    }
}
namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class CuentaContableTipoCuentaCorrienteType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    
    public static IEnumerable<CuentaContableTipoCuentaCorrienteType> ObtenerTipos()
    {
        
        return new List<CuentaContableTipoCuentaCorrienteType>
        {
            new CuentaContableTipoCuentaCorrienteType {Codigo = "N", Nombre = "Ninguno"},
            new CuentaContableTipoCuentaCorrienteType {Codigo = "C", Nombre = "Cliente"},
            new CuentaContableTipoCuentaCorrienteType {Codigo = "P", Nombre = "Proveedor"}
        };
    }
}
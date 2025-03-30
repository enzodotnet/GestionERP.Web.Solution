namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class CuentaContableDetalleTipoType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    
    public static IEnumerable<CuentaContableDetalleTipoType> ObtenerTipos()
    {
        return new List<CuentaContableDetalleTipoType>
        {
            new CuentaContableDetalleTipoType {Codigo = "S", Nombre = "Por saldo"},
            new CuentaContableDetalleTipoType {Codigo = "A", Nombre = "Por saldo analizado"}
        };
    }
}
namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class CuentaContableDestinoTipoType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    
    public static IEnumerable<CuentaContableDestinoTipoType> ObtenerTipos()
    {
        return new List<CuentaContableDestinoTipoType>
        {
            new CuentaContableDestinoTipoType {Codigo = "D", Nombre = "DEBE"},
            new CuentaContableDestinoTipoType {Codigo = "H", Nombre = "HABER"}
        };
    }
}
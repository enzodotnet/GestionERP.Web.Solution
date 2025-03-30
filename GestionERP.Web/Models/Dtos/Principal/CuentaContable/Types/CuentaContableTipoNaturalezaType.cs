namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class CuentaContableTipoNaturalezaType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    
    public static IEnumerable<CuentaContableTipoNaturalezaType> ObtenerTipos()
    {
        
        return new List<CuentaContableTipoNaturalezaType>
        {
            new CuentaContableTipoNaturalezaType {Codigo = "N", Nombre = "Ninguno"},
            new CuentaContableTipoNaturalezaType {Codigo = "D", Nombre = "Debe"},
            new CuentaContableTipoNaturalezaType {Codigo = "H", Nombre = "Haber"},
            new CuentaContableTipoNaturalezaType {Codigo = "A", Nombre = "Ambos"}
        };
    }
}
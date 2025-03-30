namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class CuentaContableTipoType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    
    public static IEnumerable<CuentaContableTipoType> ObtenerTipos()
    {
        
        return new List<CuentaContableTipoType>
        {
            new CuentaContableTipoType {Codigo = "A", Nombre = "Activo"},
            new CuentaContableTipoType {Codigo = "P", Nombre = "Pasivo"},
            new CuentaContableTipoType {Codigo = "C", Nombre = "Capital y Patrimonio"},
            new CuentaContableTipoType {Codigo = "I", Nombre = "Resultado de Ingreso"},
            new CuentaContableTipoType {Codigo = "G", Nombre = "Resultado de Gasto"}
        };
    }
}
namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class CuentaContableFormatoFuncionType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    
    public static IEnumerable<CuentaContableFormatoFuncionType> ObtenerTipos()
    {
        
        return new List<CuentaContableFormatoFuncionType>
        {
            new CuentaContableFormatoFuncionType {Codigo = "I", Nombre = "Ninguna"},
            new CuentaContableFormatoFuncionType {Codigo = "B", Nombre = "Cuenta de CuentaBalance"},
            new CuentaContableFormatoFuncionType {Codigo = "N", Nombre = "Cuenta de Resultado por Naturaleza"},
            new CuentaContableFormatoFuncionType {Codigo = "F", Nombre = "Cuenta de Resultado por Fnc"},
            new CuentaContableFormatoFuncionType {Codigo = "A", Nombre = "Cuenta de Resultado por Naturaleza y Fnc"}
        };
    }
}
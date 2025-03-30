namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class CuentaContableTipoEntidadFinancieraType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    
    public static IEnumerable<CuentaContableTipoEntidadFinancieraType> ObtenerTipos()
    {
        
        return new List<CuentaContableTipoEntidadFinancieraType>
        {
            new CuentaContableTipoEntidadFinancieraType {Codigo = "N", Nombre = "Ninguno"},
            new CuentaContableTipoEntidadFinancieraType {Codigo = "B", Nombre = "Banco"},
            new CuentaContableTipoEntidadFinancieraType {Codigo = "C", Nombre = "Caja"}
        };
    }
}
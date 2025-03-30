namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class OperacionFinancieraTipoCambioType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; } 

    public static IEnumerable<OperacionFinancieraTipoCambioType> ObtenerTipos()
    { 
        return new List<OperacionFinancieraTipoCambioType>
        {
            new OperacionFinancieraTipoCambioType {Codigo = "C", Nombre = "Compra"},
            new OperacionFinancieraTipoCambioType {Codigo = "V", Nombre = "Venta" } 
        };
    }
}
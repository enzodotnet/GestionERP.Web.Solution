namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class DiarioTipoCambioType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; } 

    public static IEnumerable<DiarioTipoCambioType> ObtenerTipos()
    { 
        return new List<DiarioTipoCambioType>
        {
            new DiarioTipoCambioType {Codigo = "C", Nombre = "Compra"},
            new DiarioTipoCambioType {Codigo = "V", Nombre = "Venta" } 
        };
    }
}
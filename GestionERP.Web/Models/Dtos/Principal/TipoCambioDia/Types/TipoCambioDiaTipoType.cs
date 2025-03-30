namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class TipoCambioDiaTipoType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; } 

    public static IEnumerable<TipoCambioDiaTipoType> ObtenerTipos()
    { 
        return new List<TipoCambioDiaTipoType>
        {
            new TipoCambioDiaTipoType {Codigo = "C", Nombre = "Compra"},
            new TipoCambioDiaTipoType {Codigo = "V", Nombre = "Venta" } 
        };
    }
}
namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class DiarioTipoRegistroType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; } 

    public static IEnumerable<DiarioTipoRegistroType> ObtenerTipos()
    { 
        return new List<DiarioTipoRegistroType>
        {
            new DiarioTipoRegistroType {Codigo = "C", Nombre = "Compra"},
            new DiarioTipoRegistroType {Codigo = "V", Nombre = "Venta" },
            new DiarioTipoRegistroType {Codigo = "T", Nombre = "Tesorería" },
            new DiarioTipoRegistroType {Codigo = "D", Nombre = "Diario" }
        };
    }
}
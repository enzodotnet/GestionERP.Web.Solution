namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class GlosarioAnalisisRegistroType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; } 

    public static IEnumerable<GlosarioAnalisisRegistroType> ObtenerTipos()
    { 
        return new List<GlosarioAnalisisRegistroType>
        {
            new GlosarioAnalisisRegistroType {Codigo = "I", Nombre = "Interno"},
            new GlosarioAnalisisRegistroType {Codigo = "E", Nombre = "Externo" }
        };
    }
}
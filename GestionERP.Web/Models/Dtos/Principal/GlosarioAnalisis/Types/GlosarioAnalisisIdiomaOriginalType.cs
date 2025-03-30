namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class GlosarioAnalisisIdiomaOriginalType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; } 

    public static IEnumerable<GlosarioAnalisisIdiomaOriginalType> ObtenerTipos()
    { 
        return new List<GlosarioAnalisisIdiomaOriginalType>
        {
            new GlosarioAnalisisIdiomaOriginalType {Codigo = "IN", Nombre = "Inglés"},
            new GlosarioAnalisisIdiomaOriginalType {Codigo = "ES", Nombre = "Español" }
        };
    }
}
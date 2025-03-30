namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class TipoArticuloCategoriaType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; } 

    public static IEnumerable<TipoArticuloCategoriaType> ObtenerTipos()
    { 
        return new List<TipoArticuloCategoriaType>
        {
            new TipoArticuloCategoriaType {Codigo = "B", Nombre = "Bien"},
            new TipoArticuloCategoriaType {Codigo = "S", Nombre = "Servicio" } 
        };
    }
}
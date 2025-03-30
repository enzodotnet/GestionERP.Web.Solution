namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class ArticuloTipoUnidadType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; } 

    public static IEnumerable<ArticuloTipoUnidadType> ObtenerTipos()
    { 
        return new List<ArticuloTipoUnidadType>
        {
            new ArticuloTipoUnidadType {Codigo = "B", Nombre = "Bien"},
            new ArticuloTipoUnidadType {Codigo = "S", Nombre = "Servicio" } 
        };
    }
}
namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class TipoIdentificacionTipoOrigenType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; } 

    public static IEnumerable<TipoIdentificacionTipoOrigenType> ObtenerTipos()
    { 
        return new List<TipoIdentificacionTipoOrigenType>
        {
            new TipoIdentificacionTipoOrigenType {Codigo = "DO", Nombre = "Domiciliado"},
            new TipoIdentificacionTipoOrigenType {Codigo = "ND", Nombre = "No domiciliado" } 
        };
    }
}
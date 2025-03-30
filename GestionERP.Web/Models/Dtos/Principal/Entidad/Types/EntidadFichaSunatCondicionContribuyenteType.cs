namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class EntidadFichaSunatCondicionContribuyenteType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    
    public static IEnumerable<EntidadFichaSunatCondicionContribuyenteType> ObtenerTipos()
    { 
        return new List<EntidadFichaSunatCondicionContribuyenteType>
        {
            new EntidadFichaSunatCondicionContribuyenteType {Codigo = "NH", Nombre = "No Habido"},
            new EntidadFichaSunatCondicionContribuyenteType {Codigo = "HA", Nombre = "Habido"},
            new EntidadFichaSunatCondicionContribuyenteType {Codigo = "DB", Nombre = "Dado de Baja"} 
        };
    }
}
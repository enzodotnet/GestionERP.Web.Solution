namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class TipoDocumentoAtributoType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; } 

    public static IEnumerable<TipoDocumentoAtributoType> ObtenerTipos()
    { 
        return new List<TipoDocumentoAtributoType>
        {
            new TipoDocumentoAtributoType {Codigo = "A", Nombre = "Administrativo"},
            new TipoDocumentoAtributoType {Codigo = "C", Nombre = "Contable" } 
        };
    }
}
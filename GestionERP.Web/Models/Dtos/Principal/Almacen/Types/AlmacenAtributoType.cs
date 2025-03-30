namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class AlmacenAtributoType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; } 

    public static IEnumerable<AlmacenAtributoType> ObtenerTipos()
    { 
        return new List<AlmacenAtributoType>
        {
            new() {Codigo = "A", Nombre = "Administrativo"},
            new() {Codigo = "C", Nombre = "Contable"} 
        };
    }
}
namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class TipoProvisionRegistroType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; } 

    public static IEnumerable<TipoProvisionRegistroType> ObtenerTipos()
    { 
        return new List<TipoProvisionRegistroType>
        {
            new() {Codigo = "L", Nombre = "Local"},
            new() {Codigo = "H", Nombre = "Honorario" },
            new() {Codigo = "I", Nombre = "Importación" }
        };
    }
}
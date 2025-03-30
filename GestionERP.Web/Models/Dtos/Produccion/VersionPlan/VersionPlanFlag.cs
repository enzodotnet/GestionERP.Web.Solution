namespace GestionERP.Web.Models.Dtos.Produccion;

public class VersionPlanFlag
{
    public string Codigo { get; set; }
    public string Nombre { get; set; }  

    public static IEnumerable<VersionPlanFlag> TiposMaterial()
    {
        return
        [
            new() {Codigo = "M", Nombre = "Materia prima"},
            new() {Codigo = "A", Nombre = "Acondicionado"}
        ];
    }

    public static IEnumerable<VersionPlanFlag> InsumosMaterial()
    {
        return
        [
            new() {Codigo = "V", Nombre = "Variable"},
            new() {Codigo = "F", Nombre = "Fijo"}
        ];
    }
}
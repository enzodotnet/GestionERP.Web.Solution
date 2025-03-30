namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class CentroCostoOrigenType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    
    public static IEnumerable<CentroCostoOrigenType> ObtenerTipos()
    { 
        return
        [
            new() {Codigo = "M", Nombre = "Registro Manual"},
            new() {Codigo = "I", Nombre = "Orden de Importación"},
            new() {Codigo = "C", Nombre = "Orden de Compra"}
        ];
    }
}
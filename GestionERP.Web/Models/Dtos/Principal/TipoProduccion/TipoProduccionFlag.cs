namespace GestionERP.Web.Models.Dtos.Principal;

public class TipoProduccionFlag
{
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    
    public static IEnumerable<TipoProduccionFlag> TiposProceso()
    { 
        return
        [
            new() {Codigo = "P", Nombre = "Fabricación"},
            new() {Codigo = "F", Nombre = "Fraccionamiento"},
            new() {Codigo = "A", Nombre = "Acondicionado"} 
        ];
    }

    public static IEnumerable<TipoProduccionFlag> Ambitos()
    {
        return
        [
            new() {Codigo = "I", Nombre = "Interno"},
            new() {Codigo = "E", Nombre = "Externo"}
        ];
    }

    public static IEnumerable<TipoProduccionFlag> Condiciones()
    {
        return
        [
            new() {Codigo = "PR", Nombre = "Propio"},
            new() {Codigo = "ET", Nombre = "Encargado a Tercero"},
            new() {Codigo = "EC", Nombre = "Encargado por Cliente"}
        ];
    }
}
namespace GestionERP.Web.Models.Dtos.Almacen;

public class MovimientoFlag
{
    public string Codigo { get; set; }
    public string Nombre { get; set; }

    public static IEnumerable<MovimientoFlag> TiposRegistro()
    { 
        return
        [
            new() {Codigo = "I", Nombre = "Ingreso"},
            new() {Codigo = "S", Nombre = "Salida"},
			new() {Codigo = "T", Nombre = "Transferencia"}
        ];
    } 
}
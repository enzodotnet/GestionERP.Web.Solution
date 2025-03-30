namespace GestionERP.Web.Models.Dtos.Compra;

public class SolicitudFlag
{
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    
    public static IEnumerable<SolicitudFlag> NivelesPrioridad()
    { 
        return
		[
			new() {Codigo = "1", Nombre = "Inmediato"},
            new() {Codigo = "2", Nombre = "Muy urgente"},
            new() {Codigo = "3", Nombre = "Urgente"},
            new() {Codigo = "4", Nombre = "Normal"}, 
            new() {Codigo = "5", Nombre = "No urgente"}
        ];
    }
}
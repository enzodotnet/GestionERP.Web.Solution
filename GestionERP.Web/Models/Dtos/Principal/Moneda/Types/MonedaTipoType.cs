namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class MonedaTipoType
{ 
	public string Codigo { get; set; }
    public string Nombre { get; set; } 

    public static IEnumerable<MonedaTipoType> ObtenerTipos()
    { 
        return
		[
			new() {Codigo = "MN", Nombre = "Moneda Nacional" },
            new() {Codigo = "ME", Nombre = "Moneda Extranjera" } 
        ]; 
    }
}
namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class TipoServicioRegistroType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; } 

    public static IEnumerable<TipoServicioRegistroType> ObtenerTipos()
    { 
        return
		[
			new() {Codigo = "SH", Nombre = "Servicio Honorario"},
            new() {Codigo = "SP", Nombre = "Seguro Póliza" } 
        ];
    }
}
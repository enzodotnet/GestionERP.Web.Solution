namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class ProcesoDocumentoTipoAccionType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; } 

    public static IEnumerable<ProcesoDocumentoTipoAccionType> ObtenerTiposAccion()
    { 
        return
		[
			new() {Codigo = "C", Nombre = "Compra" },
            new() {Codigo = "S", Nombre = "Servicio" },
            new() {Codigo = "P", Nombre = "Producci�n" },
            new() {Codigo = "I", Nombre = "Importaci�n" },
            new() {Codigo = "V", Nombre = "Venta" },
            new() {Codigo = "A", Nombre = "Almac�n" },
            new() {Codigo = "F", Nombre = "Finanza" }
        ];
    } 
}
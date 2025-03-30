namespace GestionERP.Web.Models.Dtos.Importacion;

public class OrdenFlag
{
    public string Codigo { get; set; }
    public string Nombre { get; set; }

    public static IEnumerable<OrdenFlag> Origenes()
    { 
        return
        [
            new() {Codigo = "D", Nombre = "Directo"},
            new() {Codigo = "S", Nombre = "Solicitud"}
        ];
    }

	public static IEnumerable<OrdenFlag> MediosPago()
	{
		return
		[
			new() {Codigo = "N", Nombre = "No especificado"},
			new() {Codigo = "D", Nombre = "Depósito"},
			new() {Codigo = "T", Nombre = "Transferencia"},
			new() {Codigo = "E", Nombre = "Efectivo"},
			new() {Codigo = "C", Nombre = "Cheque"}
		];
	}
}
namespace GestionERP.Web.Models.Dtos.Compra;

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

	public static IEnumerable<OrdenFlag> EstadosAnticipo()
	{
		return
		[
			new() {Codigo = "NA", Nombre = "No anticipable"},
			new() {Codigo = "PA", Nombre = "Pendiente anticipo"},
			new() {Codigo = "RA", Nombre = "Parcialmente anticipado"},
			new() {Codigo = "TA", Nombre = "Totalmente anticipado"}
		];
	}

    public static IEnumerable<OrdenFlag> EstadosIngreso()
    {
        return
        [
            new() {Codigo = "NI", Nombre = "No ingresable"},
            new() {Codigo = "PI", Nombre = "Pendiente ingresar"},
            new() {Codigo = "RI", Nombre = "Ingreso parcial"},
            new() {Codigo = "TI", Nombre = "Ingresado total"}
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
namespace GestionERP.Web.Models.Dtos.Servicio;

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

	public static IEnumerable<OrdenFlag> EstadosAceptacion()
	{
		return
		[
            new() {Codigo = "NA", Nombre = "No aceptable"},
            new() {Codigo = "PA", Nombre = "Pendiente aceptar"},
			new() {Codigo = "RA", Nombre = "Aceptado parcial"},
			new() {Codigo = "TA", Nombre = "Aceptado total"}
		];
	}

	public static IEnumerable<OrdenFlag> EstadosAnticipo()
	{
		return
		[
			new() {Codigo = "NA", Nombre = "No anticipable"},
			new() {Codigo = "PA", Nombre = "Pendiente anticipar"},
			new() {Codigo = "RA", Nombre = "Anticipo parcial"},
			new() {Codigo = "TA", Nombre = "Anticipado total"}
		];
	}

	public static IEnumerable<OrdenFlag> EstadosProduccion()
	{
		return
		[
			new() {Codigo = "NV", Nombre = "No vinculable"},
			new() {Codigo = "PV", Nombre = "Pendiente víncular"},
            new() {Codigo = "RV", Nombre = "Vinculado parcial"},
            new() {Codigo = "TV", Nombre = "Vinculado total"}
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
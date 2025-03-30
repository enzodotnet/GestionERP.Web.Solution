namespace GestionERP.Web.Models.Dtos.Servicio;

public class ContratoFlag
{
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string CodigoProcesoDocumento { get; set; }

    public static IEnumerable<ContratoFlag> TiposRegistro()
    {
        return
        [
            new() {Codigo = "SH", Nombre = "Servicio honorario",CodigoProcesoDocumento = "COSH"},
            new() {Codigo = "SP", Nombre = "Seguro póliza", CodigoProcesoDocumento = "COSP"}
        ];
    }

	public static IEnumerable<ContratoFlag> EstadosDevengo()
	{
		return
		[
			new() {Codigo = "ND", Nombre = "No devengable"},
			new() {Codigo = "PD", Nombre = "Pendiente"},
			new() {Codigo = "GD", Nombre = "Generado"}
		];
	}

	public static IEnumerable<ContratoFlag> IntervalosInforme()
	{
		return
		[
			new() {Codigo = "N", Nombre = "No requiere"},
			new() {Codigo = "M", Nombre = "Mensual"},
			new() {Codigo = "T", Nombre = "Trimestral"},
			new() {Codigo = "C", Nombre = "Culminación"}
		];
	}

	public static IEnumerable<ContratoFlag> MediosPago()
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
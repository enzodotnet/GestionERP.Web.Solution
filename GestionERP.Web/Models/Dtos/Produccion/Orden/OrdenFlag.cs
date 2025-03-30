namespace GestionERP.Web.Models.Dtos.Produccion;

public class OrdenFlag
{
    public string Codigo { get; set; }
    public string Nombre { get; set; } 
    public string CodigoProcesoDocumento { get; set; }

    public static IEnumerable<OrdenFlag> TiposRegistro()
    {
        return
        [
            new() {Codigo = "P", Nombre = "Fabricación", CodigoProcesoDocumento = "ORPP" },
            new() {Codigo = "F", Nombre = "Fraccionado", CodigoProcesoDocumento = "ORPF" },
            new() {Codigo = "A", Nombre = "Acondicionado", CodigoProcesoDocumento = "ORPA" }
        ];
    }

    public static IEnumerable<OrdenFlag> Origenes()
    { 
        return
        [
            new() {Codigo = "D", Nombre = "Directo"},
            new() {Codigo = "S", Nombre = "Solicitud"}
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

    public static IEnumerable<OrdenFlag> EstadosTransferencia()
    {
        return
        [
            new() {Codigo = "NT", Nombre = "No transferible"},
            new() {Codigo = "PT", Nombre = "Pendiente de transferir"},
            new() {Codigo = "TM", Nombre = "Transferido materiales"}
        ];
    }

    public static IEnumerable<OrdenFlag> EstadosConsumo()
    {
        return
        [
            new() {Codigo = "NC", Nombre = "No consumible"},
            new() {Codigo = "PC", Nombre = "Pendiente de consumir"},
            new() {Codigo = "CM", Nombre = "Consumido materiales"}
        ];
    }
}
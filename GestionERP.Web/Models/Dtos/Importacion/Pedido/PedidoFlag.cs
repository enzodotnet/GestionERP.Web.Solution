namespace GestionERP.Web.Models.Dtos.Importacion;

public class PedidoFlag
{
    public string Codigo { get; set; }
    public string Nombre { get; set; }

    public static IEnumerable<PedidoFlag> ViasTransporte()
    {
        return
        [
            new() {Codigo = "M", Nombre = "Marítimo"},
            new() {Codigo = "A", Nombre = "Aéreo"},
            new() {Codigo = "T", Nombre = "Terrestre"}
        ];
    }

    public static IEnumerable<PedidoFlag> Canales()
    {
        return
        [
            new() {Codigo = "R", Nombre = "Rojo"},
            new() {Codigo = "V", Nombre = "Verde"},
            new() {Codigo = "N", Nombre = "Naranja"}
        ];
    }

    public static IEnumerable<PedidoFlag> ModalidadesEmbarque()
    {
        return
        [
            new() {Codigo = "P", Nombre = "Parcial"},
            new() {Codigo = "T", Nombre = "Total"}
        ];
    }

    public static IEnumerable<PedidoFlag> EstadosIngreso()
    {
        return
        [
            new() {Codigo = "NI", Nombre = "No enviado a ingreso"},
            new() {Codigo = "PI", Nombre = "Pendiente de ingresar"},
            new() {Codigo = "RI", Nombre = "Parcialmente ingresado"},
            new() {Codigo = "TI", Nombre = "Totalmente ingresado"}
        ];
    }

    public static IEnumerable<PedidoFlag> TiposFinanciamiento()
    {
        return
        [
            new() {Codigo = "N", Nombre = "Ninguno"},
            new() {Codigo = "C", Nombre = "Contado"},
            new() {Codigo = "F", Nombre = "Financiado"},
            new() {Codigo = "G", Nombre = "Garantía global"}
        ];
    }
}
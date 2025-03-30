namespace GestionERP.Web.Models.Dtos.Importacion;

public class NotaReporteOrdenFlag
{
    public string Codigo { get; set; }
    public string Nombre { get; set; }

    public static IEnumerable<NotaReporteOrdenFlag> Secciones()
    {
        return
        [
            new() {Codigo = "P", Nombre = "Pie de página"},
            new() {Codigo = "S", Nombre = "Subdetalle"}
        ];
    }
}
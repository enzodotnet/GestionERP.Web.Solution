namespace GestionERP.Web.Models.Dtos.Produccion;

public class EquipoFlag
{
    public string Codigo { get; set; }
    public string Nombre { get; set; } 

    public static IEnumerable<EquipoFlag> TiposMaquinaria()
    { 
        return
        [
            new() {Codigo = "P", Nombre = "Primaria" },
            new() {Codigo = "S", Nombre = "Secundaria" }
        ];
    }
}
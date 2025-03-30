namespace GestionERP.Web.Models.Dtos.Principal.TipoSituacionLetra.Types;

public class TipoSituacionLetraTipoLetraType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; }

    public static IEnumerable<TipoSituacionLetraTipoLetraType> ObtenerTipos()
    {
        return new List<TipoSituacionLetraTipoLetraType>
        {
            new() {Codigo = "G", Nombre = "Garantia"},
            new() {Codigo = "D", Nombre = "Descuento"},
            new() {Codigo = "L", Nombre = "Libre"},
            new() {Codigo = "P", Nombre = "Protesta"},
            new() {Codigo = "O", Nombre = "Otros"}
        };
    }
}
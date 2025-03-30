namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class ModoPagoVinculoModoType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; } 

    public static IEnumerable<ModoPagoVinculoModoType> ObtenerTipos()
    { 
        return
        [
            new() { Codigo = "0", Nombre = "Ninguno" },
            new() { Codigo = "1", Nombre = "Vinculo a modo Gratuito" },
            new() { Codigo = "2", Nombre = "Vinculo a modo Contado - Gratuito" }
        ];
    }
}
namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class TipoAdjuntoCertificadoOrigenType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; } 

    public static IEnumerable<TipoAdjuntoCertificadoOrigenType> ObtenerTipos()
    { 
        return new List<TipoAdjuntoCertificadoOrigenType>
        {
            new TipoAdjuntoCertificadoOrigenType {Codigo = "P", Nombre = "Proveedor"},
            new TipoAdjuntoCertificadoOrigenType {Codigo = "E", Nombre = "Empresa gestionada" },
            new TipoAdjuntoCertificadoOrigenType {Codigo = "F", Nombre = "Fabricante" }
        };
    }
}
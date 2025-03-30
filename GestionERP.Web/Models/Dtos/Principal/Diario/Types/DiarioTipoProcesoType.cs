namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class DiarioTipoProcesoType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; } 

    public static IEnumerable<DiarioTipoProcesoType> ObtenerTipos()
    { 
        return new List<DiarioTipoProcesoType>
        {
            new DiarioTipoProcesoType {Codigo = "M", Nombre = "Mensual"},
            new DiarioTipoProcesoType {Codigo = "A", Nombre = "Apertura" },
            new DiarioTipoProcesoType {Codigo = "C", Nombre = "Cierre" }
        };
    }
}
namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class TipoTerminoRegistroType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; } 

    public static IEnumerable<TipoTerminoRegistroType> ObtenerTipos()
    { 
        return new List<TipoTerminoRegistroType>
        {
            new () {Codigo = "SH", Nombre = "Servicio - Honorario"},
            new () {Codigo = "SP", Nombre = "Seguro - Póliza" } 
        };
    }
}
namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class TipoMovimientoTipoAccionType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; } 

    public static IEnumerable<TipoMovimientoTipoAccionType> ObtenerTipos()
    { 
        return new List<TipoMovimientoTipoAccionType>
        {
            new TipoMovimientoTipoAccionType {Codigo = "I", Nombre = "Ingreso"},
            new TipoMovimientoTipoAccionType {Codigo = "S", Nombre = "Salida" } 
        };
    }
}
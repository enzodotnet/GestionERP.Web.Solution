namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class TipoMovimientoTipoEntidadType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; } 

    public static IEnumerable<TipoMovimientoTipoEntidadType> ObtenerTipos()
    { 
        return new List<TipoMovimientoTipoEntidadType>
        {
            new TipoMovimientoTipoEntidadType {Codigo = "C", Nombre = "Cliente"},
            new TipoMovimientoTipoEntidadType {Codigo = "P", Nombre = "Proveedor" },
            new TipoMovimientoTipoEntidadType {Codigo = "E", Nombre = "Empresa gestionada" }
        };
    }
}
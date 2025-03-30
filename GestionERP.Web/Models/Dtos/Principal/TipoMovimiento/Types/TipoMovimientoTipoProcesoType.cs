namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class TipoMovimientoTipoProcesoType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; } 

    public static IEnumerable<TipoMovimientoTipoProcesoType> ObtenerTipos()
    { 
        return
        [
            new TipoMovimientoTipoProcesoType {Codigo = "C", Nombre = "Compra"},
            new TipoMovimientoTipoProcesoType {Codigo = "D", Nombre = "Devolucion" },
            new TipoMovimientoTipoProcesoType {Codigo = "V", Nombre = "Venta"},
            new TipoMovimientoTipoProcesoType {Codigo = "A", Nombre = "Apertura" },
            new TipoMovimientoTipoProcesoType {Codigo = "B", Nombre = "Cambio"},
            new TipoMovimientoTipoProcesoType {Codigo = "T", Nombre = "Transferencia" },
            new TipoMovimientoTipoProcesoType {Codigo = "P", Nombre = "Produccion"},
            new TipoMovimientoTipoProcesoType {Codigo = "I", Nombre = "Inventario" },
            new TipoMovimientoTipoProcesoType {Codigo = "E", Nombre = "Requerimiento" }
        ];
    }
}
 
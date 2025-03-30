namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class TipoAdjuntoRecepcionSeccionType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; } 

    public static IEnumerable<TipoAdjuntoRecepcionSeccionType> ObtenerTipos()
    { 
        return new List<TipoAdjuntoRecepcionSeccionType>
        {
            new TipoAdjuntoRecepcionSeccionType {Codigo = "C", Nombre = "Cabecera"},
            new TipoAdjuntoRecepcionSeccionType {Codigo = "D", Nombre = "Detalle" } 
        };
    }
}
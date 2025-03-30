namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class TipoNotaTipoType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; } 

    public static IEnumerable<TipoNotaTipoType> ObtenerTipos()
    { 
        return
        [
            new TipoNotaTipoType {Codigo = "C", Nombre = "Crédito"},
            new TipoNotaTipoType {Codigo = "D", Nombre = "Débito" } 
        ];
    }
}
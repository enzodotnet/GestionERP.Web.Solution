namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class TipoGastoImportacionTipoType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; } 

    public static IEnumerable<TipoGastoImportacionTipoType> ObtenerTipos()
    { 
        return new List<TipoGastoImportacionTipoType>
        {
            new TipoGastoImportacionTipoType {Codigo = "F", Nombre = "Financiero"},
            new TipoGastoImportacionTipoType {Codigo = "C", Nombre = "Contable" } 
        };
    }
}
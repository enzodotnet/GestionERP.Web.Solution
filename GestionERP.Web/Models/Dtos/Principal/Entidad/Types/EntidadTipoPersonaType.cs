namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class EntidadTipoPersonaType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string Origen { get; set; }

    public static IEnumerable<EntidadTipoPersonaType> ObtenerTipos()
    { 
        return new List<EntidadTipoPersonaType>
        {
            new EntidadTipoPersonaType {Codigo = "JU", Nombre = "Jurídica", Origen = "DO"},
            new EntidadTipoPersonaType {Codigo = "NA", Nombre = "Natural", Origen = "DO" },
            new EntidadTipoPersonaType {Codigo = "ND", Nombre = "No domiciliada", Origen = "ND"}
        };
    }
}
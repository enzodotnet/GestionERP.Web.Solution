namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class EmpresaAtributoTipoRubroType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; }

    public static IEnumerable<EmpresaAtributoTipoRubroType> ObtenerTipos()
    { 
        return new List<EmpresaAtributoTipoRubroType>
        {
            new EmpresaAtributoTipoRubroType {Codigo = "C", Nombre = "Comercial"},
            new EmpresaAtributoTipoRubroType {Codigo = "S", Nombre = "Servicio" }
        };
    }
}
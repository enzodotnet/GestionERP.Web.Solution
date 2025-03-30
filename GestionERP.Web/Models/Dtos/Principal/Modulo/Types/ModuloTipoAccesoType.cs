namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class ModuloTipoAccesoType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    
    public static IEnumerable<ModuloTipoAccesoType> ObtenerTipos()
    {
        return
        [
            new() {Codigo = "P", Nombre = "Principal"},
            new() {Codigo = "E", Nombre = "Empresa"}
        ];
    }
}
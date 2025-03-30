namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class DocumentoTipoEntidadType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; } 

    public static IEnumerable<DocumentoTipoEntidadType> ObtenerTipos()
    { 
        return
        [
            new() {Codigo = "C", Nombre = "Cliente"},
            new() {Codigo = "P", Nombre = "Proveedor"},
            new() {Codigo = "E", Nombre = "Cliente/Proveedor"},
            new() {Codigo = "G", Nombre = "Empresa Gestionada"}
        ];
    }
}
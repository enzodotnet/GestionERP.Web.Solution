namespace GestionERP.Web.Models.Dtos.Archivo;

public class PrincipalEmpresaObtenerDto
{
    public Guid Id { get; set; }
    public Guid OrigenArchivoId { get; set; }
    public string FlagTipoArchivo { get; set; }
    public string CodigoBucket { get; set; }
    public string NombreArchivo { get; set; }
}
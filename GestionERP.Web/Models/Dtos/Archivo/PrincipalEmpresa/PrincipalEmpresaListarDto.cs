namespace GestionERP.Web.Models.Dtos.Archivo;

public class PrincipalEmpresaListarDto
{
    public Guid Id { get; set; }
    public string CodigoBucket { get; set; }
    public string FlagTipoArchivo { get; set; }
    public string NombreArchivo { get; set; }
    public DateTime FechaHoraRegistro { get; set; }
    public string UrlArchivo { get; set; } = null;
}
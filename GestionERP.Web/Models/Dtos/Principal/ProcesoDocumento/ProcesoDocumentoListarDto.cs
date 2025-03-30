namespace GestionERP.Web.Models.Dtos.Principal;

public class ProcesoDocumentoListarDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; } 
    public string NombreTipoDocumento { get; set; } 
    public bool Activo { get; set; }
}
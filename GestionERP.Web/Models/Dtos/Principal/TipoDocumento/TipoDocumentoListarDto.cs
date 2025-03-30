namespace GestionERP.Web.Models.Dtos.Principal;

public class TipoDocumentoListarDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; } 
    public string FlagAtributo { get; set; } 
    public bool Activo { get; set; }
}
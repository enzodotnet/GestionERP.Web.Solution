namespace GestionERP.Web.Models.Dtos.Principal;

public class TipoIdentificacionListarDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Sigla { get; set; }
    public string Nombre { get; set; }
    public string FlagTipoOrigen { get; set; }
    public bool ManejaFichaSunat { get; set; }
    public bool Activo { get; set; }
}
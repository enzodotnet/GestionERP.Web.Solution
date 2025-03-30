namespace GestionERP.Web.Models.Dtos.Principal;

public class TipoNotaListarDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string FlagTipo { get; set; }
    public string TipoCodigo { get; set; }
    public string Nombre { get; set; } 
    public bool Activo { get; set; }
}
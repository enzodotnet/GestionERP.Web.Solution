namespace GestionERP.Web.Models.Dtos.Principal;

public class MonedaListarDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string Simbolo { get; set; }
    public string FlagTipo { get; set; }
    public bool Activo { get; set; }
}
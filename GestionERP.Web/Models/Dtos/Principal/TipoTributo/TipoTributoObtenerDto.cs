namespace GestionERP.Web.Models.Dtos.Principal;

public class TipoTributoObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string InternationalCode { get; set; }
    public string NombreAbreviado { get; set; }
    public bool EsPredeterminado { get; set; } 
    public bool Activo { get; set; }
}
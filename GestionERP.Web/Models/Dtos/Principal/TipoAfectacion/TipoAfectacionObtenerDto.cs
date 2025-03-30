namespace GestionERP.Web.Models.Dtos.Principal;

public class TipoAfectacionObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string CodigoTipoTributo { get; set; }
    public string NombreTipoTributo { get; set; } 
    public bool EsPredeterminado { get; set; } 
    public bool Activo { get; set; }
}
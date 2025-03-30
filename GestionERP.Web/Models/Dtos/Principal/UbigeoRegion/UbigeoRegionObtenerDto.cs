namespace GestionERP.Web.Models.Dtos.Principal;

public class RegionObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
    public string CodigoPais { get; set; }
    public string NombrePais { get; set; }
    public bool Activo { get; set; }
}
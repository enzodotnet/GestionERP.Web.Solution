namespace GestionERP.Web.Models.Dtos.Principal;

public class ProvinciaObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
    public string CodigoRegion { get; set; }
    public string NombreRegion { get; set; }
    public bool Activo { get; set; }
}
namespace GestionERP.Web.Models.Dtos.Principal;

public class ChoferObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string CodigoTipoIdentificacion { get; set; }
    public string SiglaTipoIdentificacion { get; set; }
    public string NombreTipoIdentificacion { get; set; }
    public string NumeroTipoIdentificacion { get; set; }
    public string Nombre { get; set; }
    public string TipoCategoria { get; set; }
    public DateTime? FechaExpiracionLicencia { get; set; }
    public string Observacion { get; set; }
    public bool Activo { get; set; }
}
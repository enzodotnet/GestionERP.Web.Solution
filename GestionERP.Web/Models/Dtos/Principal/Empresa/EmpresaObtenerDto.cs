namespace GestionERP.Web.Models.Dtos.Principal;

public class EmpresaObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string CodigoTipoIdentificacion { get; set; }
    public string SiglaTipoIdentificacion { get; set; }
    public string NombreTipoIdentificacion { get; set; }
    public string NumeroTipoIdentificacion { get; set; }
    public string CodigoWeb { get; set; }
    public string Acronimo { get; set; } 
    public string Descripcion { get; set; }
    public DateTime FechaInicioSistema { get; set; }
    public string NombreArchivoLogo { get; set; }
    public string UrlArchivoLogo { get; set; }
    public bool Activo { get; set; }
    public EmpresaAtributoObtenerDto Atributo { get; set; }
    public EmpresaFacturacionObtenerDto Facturacion { get; set; }
}
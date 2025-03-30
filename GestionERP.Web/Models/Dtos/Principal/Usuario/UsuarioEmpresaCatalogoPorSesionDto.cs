namespace GestionERP.Web.Models.Dtos.Principal;

public class UsuarioEmpresaCatalogoPorSesionDto
{
    public string CodigoEmpresa { get; set; }
    public string NombreEmpresa { get; set; }
    public string SiglaTipoIdentificacion { get; set; }
    public string NumeroTipoIdentificacion { get; set; }
    public string CodigoWebEmpresa { get; set; }
    public string AcronimoEmpresa { get; set; }
    public string DireccionFiscal { get; set; }
    public string BucketArchivoEmpresa { get; set; }
    public string NombreArchivoLogo { get; set; }
    public string UrlArchivoLogo { get; set; } = null;
}
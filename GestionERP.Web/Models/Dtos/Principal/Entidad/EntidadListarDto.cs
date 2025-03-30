namespace GestionERP.Web.Models.Dtos.Principal;

public class EntidadListarDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string FlagTipoPersona { get; set; }
    public string CodigoTipoIdentificacion { get; set; }
    public string SiglaTipoIdentificacion { get; set; }
    public string NombreTipoIdentificacion { get; set; }
    public string Nombre { get; set; }
    public string CodigoPais { get; set; }
    public string NombrePais { get; set; }
    public bool EsTransportista { get; set; }
    public bool EsEmpresaGestionada { get; set; }
    public bool EsSeguro { get; set; }
    public bool EsBanco { get; set; }
    public bool EsAFP { get; set; }
    public bool Activo { get; set; }
}
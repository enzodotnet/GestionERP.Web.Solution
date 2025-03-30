namespace GestionERP.Web.Models.Dtos.Principal;

public class PaisObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string PostalCode { get; set; }
    public string CountryPortCode { get; set; }
    public string CountryName { get; set; }
    public string ConvenioTributoCode { get; set; }
    public bool EsPredeterminado { get; set; }
    public bool Activo { get; set; }
}
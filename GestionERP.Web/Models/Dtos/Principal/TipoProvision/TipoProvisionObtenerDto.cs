namespace GestionERP.Web.Models.Dtos.Principal;

public class TipoProvisionObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string CodigoMoneda { get; set; }
    public string NombreMoneda { get; set; }
    public string FlagRegistro { get; set; }
    public bool EsAfectoImpuesto { get; set; }
    public bool Activo { get; set; }
}
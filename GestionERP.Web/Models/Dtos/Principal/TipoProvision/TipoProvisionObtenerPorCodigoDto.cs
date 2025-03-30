namespace GestionERP.Web.Models.Dtos.Principal;

public class TipoProvisionObtenerPorCodigoDto
{
    public string CodigoTipoProvision { get; set; }
    public string NombreTipoProvision { get; set; }
    public string CodigoMoneda { get; set; }
    public string NombreMoneda { get; set; }
    public string SimboloMoneda { get; set; }
    public bool EsAfectoImpuesto { get; set; }
}
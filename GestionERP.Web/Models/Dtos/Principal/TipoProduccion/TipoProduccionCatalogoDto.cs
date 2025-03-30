namespace GestionERP.Web.Models.Dtos.Principal;

public class TipoProduccionCatalogoDto
{
    public string CodigoTipoProduccion { get; set; }
    public string NombreTipoProduccion { get; set; }
    public string FlagTipoProceso { get; set; }
    public string FlagAmbito { get; set; }
    public string FlagTipoEncargo { get; set; }
    public string CodigoAlmacenProceso { get; set; }
    public string NombreAlmacenProceso { get; set; }
    public bool EsExigibleTareo { get; set; }
    public bool EsExigibleMaquila { get; set; }
    public bool EsValidadorIngreso { get; set; }
    public bool EsValidadorSobrante { get; set; }
}
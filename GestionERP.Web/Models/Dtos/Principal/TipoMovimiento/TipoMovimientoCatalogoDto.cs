namespace GestionERP.Web.Models.Dtos.Principal;

public class TipoMovimientoCatalogoDto
{
    public string CodigoTipoMovimiento { get; set; }
    public string NombreTipoMovimiento { get; set; }
    public bool EsAsignableDestino { get; set; }
    public string CodigoTipoMovimientoDestino { get; set; }
    public string NombreTipoMovimientoDestino { get; set; }
}
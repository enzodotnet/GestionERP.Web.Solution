namespace GestionERP.Web.Models.Dtos.Principal;

public class TipoDetraccionCatalogoDto
{
    public string CodigoTipoDetraccion { get; set; }
    public string NombreTipoDetraccion { get; set; }
    public decimal PorcentajeDetraccion { get; set; }
    public decimal MontoMinimoMN { get; set; }
}
namespace GestionERP.Web.Models.Dtos.Principal;

public class TipoDetraccionObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; } 
    public decimal Porcentaje { get; set; }
    public decimal MontoMinimoMN { get; set; }
    public bool EsAfectoValorReferencial { get; set; }
    public bool Activo { get; set; }
}
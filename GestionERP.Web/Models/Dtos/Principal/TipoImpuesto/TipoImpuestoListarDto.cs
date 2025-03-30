namespace GestionERP.Web.Models.Dtos.Principal;

public class TipoImpuestoListarDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public decimal Porcentaje { get; set; }
    public decimal MontoMinimoMN { get; set; }
    public bool EsReservado { get; set; }
    public bool EsPredeterminado { get; set; }
    public bool EsOperacionVenta { get; set; }
    public bool Activo { get; set; } 
}
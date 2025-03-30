namespace GestionERP.Web.Models.Dtos.Principal;

public class OperacionFinancieraObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public string FlagTipoCambio { get; set; } 
    public string CodigoTipoTransaccion { get; set; }
    public string NombreTipoTransaccion { get; set; } 
    public bool EsReservado { get; set; }
    public bool Activo { get; set; }
}
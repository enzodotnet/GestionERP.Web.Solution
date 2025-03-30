namespace GestionERP.Web.Models.Dtos.Principal;

public class CasillaBalanceListarDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public bool EsActivoFijo { get; set; }
    public bool EsPasivoPatrimonio { get; set; }
    public bool EsGananciaPerdida { get; set; }
    public bool Activo { get; set; }
}
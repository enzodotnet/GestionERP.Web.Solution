namespace GestionERP.Web.Models.Dtos.Principal;

public class TipoMovimientoObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string FlagTipoAccion { get; set; }
    public string FlagTipoProceso { get; set; }
    public string FlagTipoEntidad { get; set; }
    public bool EsAsignableDestino { get; set; }
    public string CodigoTipoMovimientoDestino { get; set; }
    public string NombreTipoMovimientoDestino { get; set; }
    public string Descripcion { get; set; }  
    public bool Activo { get; set; }
}
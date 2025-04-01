namespace GestionERP.Web.Models.Dtos.Principal;

public class TipoMovimientoConsultaPorCodigoDto
{  
    public string Nombre { get; set; }
    public string FlagTipoAccion { get; set; }
    public string FlagTipoEntidad { get; set; }
    public string FlagTipoProceso { get; set; }
    public bool EsAgregableDetalle { get; set; }
    public bool EsRequeridoReferencia { get; set; }
}
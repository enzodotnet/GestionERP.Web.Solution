namespace GestionERP.Web.Models.Dtos.Almacen;

public class MovimientoListarDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string CodigoPeriodo { get; set; }
    public string FlagTipoRegistro { get; set; } 
    public string CodigoEntidad { get; set; }
    public string NombreEntidad { get; set; } 
    public string NombreOperacionLogistica { get; set; } 
    public DateTime FechaHoraOperacion { get; set; }
    public string NombreLocal { get; set; }
    public string DocumentoReferencia { get; set; }
    public bool EsOrigenAutomatico { get; set; }
    public string CodigoMoneda { get; set; }
    public decimal TotalValorCosto { get; set; }
    public string CodigoEstado { get; set; }
    public string NombreEstado { get; set; }
}
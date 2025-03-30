namespace GestionERP.Web.Models.Dtos.Importacion;

public class PedidoListarDto
{
    public Guid Id { get; set; }
    public string CodigoPeriodo { get; set; } 
    public string CodigoEntidad { get; set; }
    public string NombreEntidad { get; set; }
    public string Codigo { get; set; }
    public DateTime FechaEmision { get; set; }
    public string CodigoTipoImportacion { get; set; }
    public string CodigoOrdenReferencia { get; set; }
    public string CodigoMoneda { get; set; }
    public decimal TotalImporteBruto { get; set; }
    public string Observacion { get; set; }
    public string CodigoEstado { get; set; }
    public string NombreEstado { get; set; }
}
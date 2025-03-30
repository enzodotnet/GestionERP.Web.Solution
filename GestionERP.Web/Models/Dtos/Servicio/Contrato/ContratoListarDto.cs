namespace GestionERP.Web.Models.Dtos.Servicio;

public class ContratoListarDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string CodigoPeriodo { get; set; }
    public string FlagTipoRegistro { get; set; }
    public string CodigoEntidad { get; set; }
    public string NombreEntidad { get; set; }
    public string CodigoDocumento { get; set; }
    public string CodigoSerieDocumento { get; set; }
    public string NombreSerieDocumento { get; set; }
    public string NumeroSerieDocumento { get; set; }
    public DateTime FechaEmision { get; set; }
    public string CodigoMoneda { get; set; }
    public decimal TotalImporteNeto { get; set; }
    public string FlagEstadoDevengo { get; set; } 
    public string CodigoEstado { get; set; }
    public string NombreEstado { get; set; }
}
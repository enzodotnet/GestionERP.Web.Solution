namespace GestionERP.Web.Models.Dtos.Compra;

public class OrdenCatalogoIngresarDto
{
    public string CodigoOrden { get; set; }  
    public string CodigoPeriodo { get; set; }
    public string CodigoEntidad { get; set; }
    public string NombreEntidad { get; set; } 
    public string CodigoDocumento { get; set; }
    public string CodigoSerieDocumento { get; set; }
    public string NombreSerieDocumento { get; set; }
    public string NumeroSerieDocumento { get; set; }
    public DateTime FechaEmision { get; set; }
    public DateTime FechaEntrega { get; set; }
    public string CodigoMoneda { get; set; }
    public string SimboloMoneda { get; set; }
    public string NombreMoneda { get; set; }
    public string CodigoLocalRecepcion { get; set; }
    public string NombreLocalRecepcion { get; set; }
    public string CodigoCentroCostoOrden { get; set; }
    public string NombreCentroCostoOrden { get; set; }
}
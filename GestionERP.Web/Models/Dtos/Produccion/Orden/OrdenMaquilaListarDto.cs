namespace GestionERP.Web.Models.Dtos.Produccion;

public class OrdenMaquilaListarDto
{ 
    public Guid Id { get; set; } 
    public string CodigoEntidad { get; set; }
    public string NombreEntidad { get; set; }
    public string CodigoOrdenServicio { get; set; }
    public DateTime FechaEntrega { get; set; }
    public string CodigoMoneda { get; set; }
    public string NombreMoneda { get; set; }
    public string SimboloMoneda { get; set; }
    public string CodigoTipoCambioDia { get; set; }
    public decimal MontoTipoCambioDia { get; set; }
    public decimal ImporteBrutoVinculadoMN { get; set; }
    public decimal ImporteBrutoVinculadoME { get; set; }
    public string Observacion { get; set; }
}
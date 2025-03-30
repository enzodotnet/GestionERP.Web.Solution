namespace GestionERP.Web.Models.Dtos.Produccion;

public class OrdenLoteListarDto
{  
    public Guid Id { get; set; } 
    public string NumeroLote { get; set; }
    public string CodigoMarca { get; set; }
    public string NombreMarca { get; set; }
    public DateTime FechaFabricacion { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public string CodigoProductoEstado { get; set; }
    public string NombreProductoEstado { get; set; }
    public decimal Cantidad { get; set; }
    public string Observacion { get; set; }
}
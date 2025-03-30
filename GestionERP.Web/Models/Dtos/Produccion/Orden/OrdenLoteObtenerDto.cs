namespace GestionERP.Web.Models.Dtos.Produccion;

public class OrdenLoteObtenerDto
{  
    public Guid? Id { get; set; } 
    private string numeroLote;
    public string NumeroLote { get => numeroLote; set => numeroLote = value?.TrimEnd(); }
    public string CodigoMarca { get; set; }
    public string NombreMarca { get; set; }
    public DateTime? FechaFabricacion { get; set; }
    public DateTime? FechaVencimiento { get; set; }
    public string CodigoProductoEstado { get; set; }
    public string NombreProductoEstado { get; set; }
    public decimal? Cantidad { get; set; }
    public string Observacion { get; set; }
}
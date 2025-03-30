namespace GestionERP.Web.Models.Dtos.Produccion;

public class FuncionObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string CodigoMonedaPago { get; set; }
    public string NombreMonedaPago { get; set; }
    public decimal MontoPagoHora { get; set; } 
    public string Descripcion { get; set; }
    public bool Activo { get; set; }
}
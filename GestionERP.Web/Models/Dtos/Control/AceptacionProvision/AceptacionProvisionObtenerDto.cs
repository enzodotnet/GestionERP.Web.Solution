namespace GestionERP.Web.Models.Dtos.Control;

public class AceptacionProvisionObtenerDto
{
    public string CodigoPeriodo { get; set; }
    public string CodigoTransaccion { get; set; }
    public DateTime FechaOperacion { get; set; }
    public decimal ImporteBruto { get; set; }
}
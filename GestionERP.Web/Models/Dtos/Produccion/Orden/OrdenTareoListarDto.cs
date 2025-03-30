namespace GestionERP.Web.Models.Dtos.Produccion;

public class OrdenTareoListarDto
{ 
    public Guid Id { get; set; }
    public string CodigoPersonalProduccion { get; set; }
    public string NombrePersonalProduccion { get; set; }
    public string CodigoFuncion { get; set; }
    public string NombreFuncion { get; set; } 
    public DateTime FechaTrabajo { get; set; }
    public string CodigoMoneda { get; set; }
    public string NombreMoneda { get; set; }
    public string SimboloMoneda { get; set; }
    public string CodigoTipoCambioDia { get; set; }
    public decimal MontoTipoCambioDia { get; set; }
    public decimal CantidadHorasTrabajo { get; set; }
    public decimal MontoPagoHoraMN { get; set; }
    public decimal MontoPagoHoraME { get; set; }
    public decimal TotalMontoPagoMN { get; set; }
    public decimal TotalMontoPagoME { get; set; }
    public string Observacion { get; set; }
}
using FluentValidation;
namespace GestionERP.Web.Models.Dtos.Servicio;

public class ContratoCuotaGrid
{
    public Guid? Id { get; set; } 
    public int NumeroCuota { get; set; }
    public decimal? ImporteBruto { get; set; }
	public DateTime? FechaVencimiento { get; set; }
	public string Observacion { get; set; }
    public decimal ImporteBrutoAtendido { get; set; }
    public decimal ImporteBrutoRescindido { get; set; }
}
 
public class ContratoCuotaGridValidator : AbstractValidator<ContratoCuotaGrid>
{
	public ContratoCuotaGridValidator()
	{
		RuleFor(p => p.FechaVencimiento)
			.NotNull().WithMessage("La fecha de vencimiento es requerida");

		RuleFor(p => p.ImporteBruto)
			.Cascade(CascadeMode.Stop)
			.NotNull().WithMessage("El importe bruto es requerido")
			.GreaterThan(0).WithMessage("El importe bruto debe ser mayor a 0")
			.PrecisionScale(16, 2, true).WithMessage("El importe bruto debe contener como máximo 16 dígitos incluyendo 2 decimales");
	}
}
using FluentValidation;
using System.Text.Json.Serialization;

namespace GestionERP.Web.Models.Dtos.Servicio;

public class ContratoCuotaInsertarDto
{
	/// <summary>
	/// Esta propiedad no es editable, es un campo clave temporal
	/// </summary>
	[JsonIgnore]
	public int? NumeroCuota { get; set; }
	public decimal? ImporteBruto { get; set; }
    public DateTime? FechaVencimiento  { get; set; }
    public string Observacion { get; set; }
}

public class ContratoCuotaInsertarValidator : AbstractValidator<ContratoCuotaInsertarDto>
{
    public ContratoCuotaInsertarValidator()
    {
        RuleFor(p => p.ImporteBruto)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("El campo {PropertyName} es requerido") 
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a 0")
            .PrecisionScale(16, 2, true).WithMessage("El campo {PropertyName} debe contener como máximo 16 dígitos incluyendo 2 decimales");
        
        RuleFor(p => p.FechaVencimiento)
            .NotNull().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.Observacion)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
    }
}
using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Servicio;

public class ContratoDetalleEditarDto
{
    public Guid Id { get; set; } 
    public decimal? ImporteBruto { get; set; } 
    public decimal? ImporteImpuesto { get; set; }
    public decimal? ImporteNeto { get; set; }
    public string Observacion { get; set; }
}

public class ContratoDetalleEditarValidator : AbstractValidator<ContratoDetalleEditarDto>
{
    public ContratoDetalleEditarValidator()
    {
        RuleFor(p => p.ImporteBruto)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("El campo {PropertyName} es requerido") 
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a 0")
            .PrecisionScale(16, 2, true).WithMessage("El campo {PropertyName} debe contener como máximo 16 dígitos incluyendo 2 decimales");
        
        RuleFor(p => p.Observacion).MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
    }
}
using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Produccion;

public class VersionPlanEquipoInsertarDto
{ 
    public string CodigoEquipo { get; set; }
    public decimal? CantidadHorasUso { get; set; }
    public string Observacion { get; set; }
}

public class VersionPlanEquipoInsertarValidator : AbstractValidator<VersionPlanEquipoInsertarDto>
{
    public VersionPlanEquipoInsertarValidator()
    {
        RuleFor(p => p.CodigoEquipo).NotEmpty().WithMessage("El campo {PropertyName} es requerido"); 
         
        RuleFor(p => p.CantidadHorasUso)
            .NotNull().WithMessage("El campo {PropertyName} es requerido") 
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a 0")
            .PrecisionScale(5, 2, true).WithMessage("El campo {PropertyName} debe contener como máximo 5 dígitos incluyendo 2 decimales");
        
        RuleFor(p => p.Observacion).MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
    }
}
using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Produccion;

public class VersionPlanEquipoEditarDto
{  
    public Guid Id { get; set; } 
    public decimal? CantidadHorasUso { get; set; } 
    public string Observacion { get; set; }  
}

public class VersionPlanEquipoEditarValidator : AbstractValidator<VersionPlanEquipoEditarDto>
{
    public VersionPlanEquipoEditarValidator()
    { 
        RuleFor(p => p.CantidadHorasUso)
            .NotNull().WithMessage("El campo {PropertyName} es requerido en el editar") 
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a 0 en el editar")
            .PrecisionScale(5, 2, true).WithMessage("El campo {PropertyName} debe contener como máximo 5 dígitos incluyendo 2 decimales en el editar");
         
        RuleFor(p => p.Observacion).MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres en el editar"); 
    }
}
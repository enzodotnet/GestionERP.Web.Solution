using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Produccion;

public class VersionPlanFuncionInsertarDto
{  
    public string CodigoFuncion { get; set; }
    public decimal? CantidadHorasTrabajo { get; set; }
    public string Observacion { get; set; } 
}

public class VersionPlanFuncionInsertarValidator : AbstractValidator<VersionPlanFuncionInsertarDto>
{
    public VersionPlanFuncionInsertarValidator()
    {
        RuleFor(p => p.CodigoFuncion).NotEmpty().WithMessage("El campo {PropertyName} es requerido"); 
         
        RuleFor(p => p.CantidadHorasTrabajo)
            .NotNull().WithMessage("El campo {PropertyName} es requerido") 
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a 0")
            .PrecisionScale(5, 2, true).WithMessage("El campo {PropertyName} debe contener como máximo 5 dígitos incluyendo 2 decimales");
        
        RuleFor(p => p.Observacion).MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
    }
}
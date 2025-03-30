using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Produccion;

public class PlanEditarDto
{ 
    public string Nombre { get; set; }
    public string Descripcion { get; set; }  
    public decimal? Cantidad { get; set; }
    public string Observacion { get; set; }
}

public class PlanEditarValidator : AbstractValidator<PlanEditarDto>
{
    public PlanEditarValidator()
    { 
        RuleFor(p => p.Nombre)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .MaximumLength(150).WithMessage("El campo {PropertyName} debe tener como máximo 150 caracteres");
        
        RuleFor(p => p.Descripcion) 
            .MaximumLength(200).WithMessage("El campo {PropertyName} debe tener como máximo 200 caracteres");
 
        RuleFor(p => p.Cantidad)
            .NotNull().WithMessage("El campo {PropertyName} es requerido") 
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a 0")
            .PrecisionScale(10, 3, true).WithMessage("El campo {PropertyName} debe contener como máximo 10 dígitos incluyendo 3 decimales");
           
        RuleFor(p => p.Observacion)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
    }
}
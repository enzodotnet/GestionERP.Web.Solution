using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Produccion;

public class PlanInsertarDto
{  
    public string Nombre { get; set; }
    public DateTime? FechaRegistro { get; set; }
    public string Descripcion { get; set; }
    public string CodigoTipoProduccion { get; set; }
    public string CodigoArticuloTerminado { get; set; }
    public decimal? Cantidad { get; set; }
    public string Observacion { get; set; } 
}

public class PlanInsertarValidator : AbstractValidator<PlanInsertarDto>
{
    public PlanInsertarValidator()
    { 
        RuleFor(p => p.Nombre)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .MaximumLength(150).WithMessage("El campo {PropertyName} debe tener como máximo 150 caracteres");
        
        RuleFor(p => p.Descripcion) 
            .MaximumLength(200).WithMessage("El campo {PropertyName} debe tener como máximo 200 caracteres");

        RuleFor(p => p.FechaRegistro).NotNull().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.Cantidad)
            .NotNull().WithMessage("El campo {PropertyName} es requerido") 
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a 0")
            .PrecisionScale(10, 3, true).WithMessage("El campo {PropertyName} debe contener como máximo 10 dígitos incluyendo 3 decimales");
         
        RuleFor(p => p.CodigoTipoProduccion)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.CodigoArticuloTerminado)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");
 
        RuleFor(p => p.Observacion)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
    }
}
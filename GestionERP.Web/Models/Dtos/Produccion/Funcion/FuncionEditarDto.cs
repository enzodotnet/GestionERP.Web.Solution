using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Produccion;

public class FuncionEditarDto
{  
    public string Nombre { get; set; }
    public decimal MontoPagoHora { get; set; } 
    public string Descripcion { get; set; }
    public bool Activo { get; set; }
}

public class FuncionEditarValidator : AbstractValidator<FuncionEditarDto>
{
    public FuncionEditarValidator()
    { 
        RuleFor(p => p.Nombre)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres");
        
        RuleFor(p => p.MontoPagoHora)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .GreaterThan(0)
            .PrecisionScale(10, 2, true);

        RuleFor(p => p.Descripcion)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
    }
}
using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class TipoCambioDiaActualizarMontoDto
{ 
    public string Codigo { get; set; }
    public decimal? Monto { get; set; } 
}

public class TipoCambioDiaActualizarMontoValidator : AbstractValidator<TipoCambioDiaActualizarMontoDto>
{
    public TipoCambioDiaActualizarMontoValidator()
    {  
        RuleFor(p => p.Codigo)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.Monto)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .GreaterThan(0)
            .PrecisionScale(6, 4, true);
    }
}
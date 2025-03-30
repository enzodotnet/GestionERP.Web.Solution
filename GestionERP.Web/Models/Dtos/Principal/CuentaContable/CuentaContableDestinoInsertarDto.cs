using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class CuentaContableDestinoInsertarDto
{  
    public string FlagTipo { get; set; }  
    public string CodigoCuentaContableGenera { get; set; }
}

public class CuentaContableDestinoInsertarValidator : AbstractValidator<CuentaContableDestinoInsertarDto>
{
    public CuentaContableDestinoInsertarValidator()
    { 
        RuleFor(p => p.FlagTipo)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido en el insertar destinos");
            
        RuleFor(p => p.CodigoCuentaContableGenera)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido en el insertar destinos");
    }
}
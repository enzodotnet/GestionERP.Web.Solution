using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class CuentaContableDestinoEditarDto
{  
    public Guid Id { get; set; }
    public string CodigoCuentaContableGenera { get; set; }
}

public class CuentaContableDestinoEditarValidator : AbstractValidator<CuentaContableDestinoEditarDto>
{
    public CuentaContableDestinoEditarValidator()
    { 
        RuleFor(p => p.CodigoCuentaContableGenera)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido en el editar destinos");
    }
}
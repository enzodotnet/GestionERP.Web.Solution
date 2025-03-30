using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class CuentaContableDetalleEditarDto
{ 
    public Guid Id { get; set; }
    public string FlagTipo { get; set; }
    public string FlagTipoCambio { get; set; }
    public string CodigoCuentaContableGanancia { get; set; }
    public string CodigoCuentaContablePerdida { get; set; }
}

public class CuentaContableDetalleEditarValidator : AbstractValidator<CuentaContableDetalleEditarDto>
{
    public CuentaContableDetalleEditarValidator()
    { 
        RuleFor(p => p.FlagTipo)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido en el editar detalles");

        RuleFor(p => p.FlagTipoCambio)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido en el editar detalles");
    }
}
using FluentValidation; 

namespace GestionERP.Web.Models.Dtos.Principal;

public class CuentaContableDetalleInsertarDto
{  
    public string FlagRegistro { get; set; }
    public string FlagTipo { get; set; }
    public string FlagTipoCambio { get; set; } 
    public string CodigoCuentaContableGanancia { get; set; }
    public string CodigoCuentaContablePerdida { get; set; }
}

public class CuentaContableDetalleInsertarValidator : AbstractValidator<CuentaContableDetalleInsertarDto>
{
    public CuentaContableDetalleInsertarValidator()
    {
        RuleFor(p => p.FlagRegistro).NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.FlagTipo).NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.FlagTipoCambio).NotEmpty().WithMessage("El campo {PropertyName} es requerido"); 
    }
}
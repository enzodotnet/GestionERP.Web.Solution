using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class OperacionFinancieraEditarDto
{  
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public string FlagTipoCambio { get; set; }  
    public bool Activo { get; set; }
}

public class OperacionFinancieraEditarValidator : AbstractValidator<OperacionFinancieraEditarDto>
{
    public OperacionFinancieraEditarValidator()
    { 
        RuleFor(p => p.Nombre)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres");
         
        RuleFor(p => p.Descripcion)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");

        RuleFor(p => p.FlagTipoCambio)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");
    }
}
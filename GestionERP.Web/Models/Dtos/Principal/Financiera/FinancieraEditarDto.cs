using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class FinancieraEditarDto
{ 
    public string Nombre { get; set; } 
    public string CodigoTipoFinanciera { get; set; }
    public string AbreviacionNombre { get; set; }
    public bool Activo { get; set; } 
}

public class FinancieraEditarValidator : AbstractValidator<FinancieraEditarDto>
{
    public FinancieraEditarValidator()
    {
        RuleFor(p => p.Nombre)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres")
            .Matches(@"^[^""!@$%^&*(){}:;<>,.?/+_=|'~\\-]*$").WithMessage("El campo {PropertyName} no debe contener caracteres especiales");
         
        RuleFor(p => p.AbreviacionNombre)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(20).WithMessage("El campo {PropertyName} debe tener como máximo 20 caracteres");
    }
}
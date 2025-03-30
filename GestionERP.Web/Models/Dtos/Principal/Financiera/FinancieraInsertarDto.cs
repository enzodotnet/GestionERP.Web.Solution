using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class FinancieraInsertarDto
{ 
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string FlagRegistro { get; set; }
    public string CodigoTipoFinanciera { get; set; }
    public string AbreviacionNombre { get; set; } 
}

public class FinancieraInsertarValidator : AbstractValidator<FinancieraInsertarDto>
{
    public FinancieraInsertarValidator()
    {
        RuleFor(p => p.Codigo)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .Length(3).WithMessage("El campo {PropertyName} debe tener 3 caracteres") 
            .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos");

        RuleFor(p => p.Nombre)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres")
            .Matches(@"^[^""!@$%^&*(){}:;<>,.?/+_=|'~\\-]*$").WithMessage("El campo {PropertyName} no debe contener caracteres especiales");

        RuleFor(p => p.FlagRegistro)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");
        
        RuleFor(p => p.AbreviacionNombre)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(20).WithMessage("El campo {PropertyName} debe tener como máximo 20 caracteres");
    }
}
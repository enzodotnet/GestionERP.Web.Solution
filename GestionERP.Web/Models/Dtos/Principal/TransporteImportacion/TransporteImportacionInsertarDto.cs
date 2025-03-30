using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class TransporteImportacionInsertarDto
{ 
    public string Codigo { get; set; }
    public string Nombre { get; set; } 
}

public class TransporteImportacionInsertarValidator : AbstractValidator<TransporteImportacionInsertarDto>
{
    public TransporteImportacionInsertarValidator()
    {
        RuleFor(p => p.Codigo)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .Length(4).WithMessage("El campo {PropertyName} debe tener 4 caracteres") 
            .Matches("^[0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres numericos");
 
        RuleFor(p => p.Nombre)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres")
            .Matches(@"^[^""!@$%^&*(){}:;<>,.?/+_=|'~\\-]*$").WithMessage("El campo {PropertyName} no debe contener caracteres especiales");
    }
}
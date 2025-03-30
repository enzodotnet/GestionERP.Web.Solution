using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class AduanaEditarDto
{  
    public string Nombre { get; set; }
    public bool Activo { get; set; } 
}

public class AduanaEditarValidator : AbstractValidator<AduanaEditarDto>
{
    public AduanaEditarValidator()
    {         
        RuleFor(p => p.Nombre).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres")
            .Matches(@"^[^""!@$%^&*(){}:;<>,.?/+_=|'~\\-]*$").WithMessage("El campo {PropertyName} no debe contener caracteres especiales");
    }
}
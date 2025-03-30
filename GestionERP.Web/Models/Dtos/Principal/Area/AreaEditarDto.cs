using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class AreaEditarDto
{  
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public bool Activo { get; set; } 
}

public class AreaEditarValidator : AbstractValidator<AreaEditarDto>
{
    public AreaEditarValidator()
    {
        RuleFor(p => p.Nombre)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres")
            .Matches(@"^[^""!@$%^&*(){}:;<>,.?/+_=|'~\\-]*$").WithMessage("El campo {PropertyName} no debe contener caracteres especiales");
    
        RuleFor(p => p.Descripcion)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres"); 
    }
}
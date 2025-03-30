using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class GrupoDevengoEditarDto
{ 
    public string Nombre { get; set; }
    public bool Activo { get; set; } 
}

public class GrupoDevengoEditarValidator : AbstractValidator<GrupoDevengoEditarDto>
{
    public GrupoDevengoEditarValidator()
    {         
        RuleFor(p => p.Nombre)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres")
            .Matches(@"^[^""!@$%^&*(){}:;<>,.?/+_=|'~\\-]*$").WithMessage("El campo {PropertyName} no debe contener caracteres especiales");
    }
}
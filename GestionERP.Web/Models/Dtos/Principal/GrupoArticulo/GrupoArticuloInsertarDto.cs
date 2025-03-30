using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class GrupoArticuloInsertarDto
{ 
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
}

public class GrupoArticuloInsertarValidator : AbstractValidator<GrupoArticuloInsertarDto>
{
    public GrupoArticuloInsertarValidator()
    {
        RuleFor(p => p.Codigo)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .Length(2).WithMessage("El campo {PropertyName} debe tener 2 caracteres") 
            .Matches("^[A-Z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres numericos");

        RuleFor(p => p.Nombre)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres")
            .Matches(@"^[^""!@$%^&*(){}:;<>?/+_=|'~\\]*$").WithMessage("El campo {PropertyName} no debe contener caracteres especiales");
    
        RuleFor(p => p.Descripcion)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
    }
}
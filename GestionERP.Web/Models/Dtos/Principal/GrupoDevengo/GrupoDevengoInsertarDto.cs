using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class GrupoDevengoInsertarDto
{ 
    public string Codigo { get; set; }
    public string Nombre { get; set; }  
}

public class GrupoDevengoInsertarValidator : AbstractValidator<GrupoDevengoInsertarDto>
{
    public GrupoDevengoInsertarValidator()
    {
        RuleFor(p => p.Codigo)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .Length(2).WithMessage("El campo {PropertyName} debe tener 2 caracteres") 
            .Matches("^[0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres numéricos");

        RuleFor(p => p.Nombre)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres")
            .Matches(@"^[^""!@$%^&*(){}:;<>,.?/+_=|'~\\-]*$").WithMessage("El campo {PropertyName} no debe contener caracteres especiales"); 
    }
}
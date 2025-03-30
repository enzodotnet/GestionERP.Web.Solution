using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class ActividadComercialInsertarDto
{ 
    public string Codigo { get; set; } 
    public string Nombre { get; set; }
    public string Descripcion { get; set; } 
}

public class ActividadComercialInsertarValidator : AbstractValidator<ActividadComercialInsertarDto>
{
    public ActividadComercialInsertarValidator()
    {
        RuleFor(p => p.Codigo)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .Length(6).WithMessage("El campo {PropertyName} debe tener 6 caracteres") 
            .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanumericos");
         
        RuleFor(p => p.Nombre)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres")
            .Matches(@"^[^""!@$%^&*(){}:;<>,.?/+_=|'~\\-]*$").WithMessage("El campo {PropertyName} no debe contener caracteres especiales");
    
        RuleFor(p => p.Descripcion)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
    }
}
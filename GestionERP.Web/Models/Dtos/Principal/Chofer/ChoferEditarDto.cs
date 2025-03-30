using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class ChoferEditarDto
{ 
    public string Nombre { get; set; }
    public string TipoCategoria { get; set; }
    public DateTime? FechaExpiracionLicencia { get; set; }
    public string Observacion { get; set; }
    public bool Activo { get; set; } 
}

public class ChoferEditarValidator : AbstractValidator<ChoferEditarDto>
{
    public ChoferEditarValidator()
    {  
        RuleFor(p => p.Nombre).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(200).WithMessage("El campo {PropertyName} debe tener como máximo 200 caracteres")
            .Matches(@"^[^""!@$%^&*(){}:;<>,.?/+_=|'~\\-]*$").WithMessage("El campo {PropertyName} no debe contener caracteres especiales");

        RuleFor(p => p.TipoCategoria).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .Length(3).WithMessage("El campo {PropertyName} debe tener 3 caracteres") 
            .Matches("^[A-Z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos");

        RuleFor(p => p.Observacion)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
    }
}
using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class AduanaInsertarDto
{ 
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string CodigoRegion { get; set; }
}

public class AduanaInsertarValidator : AbstractValidator<AduanaInsertarDto>
{
    public AduanaInsertarValidator()
    {
        RuleFor(p => p.Codigo)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .Length(3).WithMessage("El campo {PropertyName} debe tener 3 caracteres") 
            .Matches("^[0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres numéricos");

        RuleFor(p => p.Nombre)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres")
            .Matches(@"^[^""!@$%^&*(){}:;<>,.?/+_=|'~\\-]*$").WithMessage("El campo {PropertyName} no debe contener caracteres especiales");

        RuleFor(p => p.CodigoRegion)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");
    }
}
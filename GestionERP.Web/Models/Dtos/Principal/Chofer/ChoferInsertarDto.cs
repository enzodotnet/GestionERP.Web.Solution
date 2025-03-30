using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class ChoferInsertarDto
{ 
    public string Codigo { get; set; }
    public string CodigoTipoIdentificacion { get; set; }
    public string NumeroTipoIdentificacion { get; set; }
    public string Nombre { get; set; }
    public string TipoCategoria { get; set; }
    public DateTime? FechaExpiracionLicencia { get; set; }
    public string Observacion { get; set; } 
}

public class ChoferInsertarValidator : AbstractValidator<ChoferInsertarDto>
{
    public ChoferInsertarValidator()
    {
        RuleFor(p => p.Codigo)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .Length(9).WithMessage("El campo {PropertyName} debe tener 9 caracteres") 
            .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos");

        RuleFor(p => p.CodigoTipoIdentificacion)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.NumeroTipoIdentificacion)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .MaximumLength(11).WithMessage("El campo {PropertyName} debe tener como máximo 11 caracteres")
            .Matches("^[0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres numericos");
        
        RuleFor(p => p.Nombre)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(200).WithMessage("El campo {PropertyName} debe tener como máximo 200 caracteres")
            .Matches(@"^[^""!@$%^&*(){}:;<>,.?/+_=|'~\\-]*$").WithMessage("El campo {PropertyName} no debe contener caracteres especiales");

        RuleFor(p => p.TipoCategoria)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .Length(3).WithMessage("El campo {PropertyName} debe tener 3 caracteres") 
            .Matches("^[A-Z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos");

        RuleFor(p => p.Observacion)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
    }
}
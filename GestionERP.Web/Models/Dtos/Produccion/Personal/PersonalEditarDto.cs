using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Produccion;

public class PersonalEditarDto
{   
    public string Nombre { get; set; }
    public string CodigoTipoIdentificacion { get; set; }
    public string NumeroTipoIdentificacion { get; set; }
    public string CodigoFuncion { get; set; } 
    public string Descripcion { get; set; }
    public bool Activo { get; set; } 
}

public class PersonalEditarValidator : AbstractValidator<PersonalEditarDto>
{
    public PersonalEditarValidator()
    { 
        RuleFor(p => p.Nombre)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .MaximumLength(200).WithMessage("El campo {PropertyName} debe tener como máximo 200 caracteres");
        
        RuleFor(p => p.CodigoTipoIdentificacion)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.NumeroTipoIdentificacion)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(11).WithMessage("El campo {PropertyName} debe tener como máximo 11 caracteres") 
            .Matches("^[0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres numericos");

        RuleFor(p => p.CodigoFuncion)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido"); 
 
        RuleFor(p => p.Descripcion)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
    }
}
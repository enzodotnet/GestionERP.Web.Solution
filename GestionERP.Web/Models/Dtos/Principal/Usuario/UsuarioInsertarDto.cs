using FluentValidation;
using FluentValidation.Validators;

namespace GestionERP.Web.Models.Dtos.Principal;

public class UsuarioInsertarDto
{ 
    public string Codigo { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string NickUser { get; set; }
    public string Nombre { get; set; }
    public string FlagTipoPerfil { get; set; }
    public string CodigoTipoIdentificacion { get; set; } 
    public string NumeroTipoIdentificacion { get; set; }
    public List<UsuarioModuloInsertarDto> Modulos { get; set; }
    public List<UsuarioServicioInsertarDto> Servicios { get; set; }
    public List<UsuarioPermisoInsertarDto> Permisos { get; set; } 
}

public class UsuarioInsertarValidator : AbstractValidator<UsuarioInsertarDto>
{
    [Obsolete("Validación del campo email de la clase es obsoleta, pero valida mejor.", true)]
    public UsuarioInsertarValidator()
    {
        RuleFor(p => p.Codigo)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .Length(3).WithMessage("El campo {PropertyName} debe tener 3 caracteres") 
            .Matches("^[0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres numericos");

        RuleFor(p => p.Nombre)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .Length(10,200)
            .Matches(@"^[^""!@$%^&*(){}:;<>,.?/+_=|'~\\-]*$").WithMessage("El campo {PropertyName} no debe contener caracteres especiales");
    
        RuleFor(p => p.Email)
            .EmailAddress(EmailValidationMode.Net4xRegex)
            .MaximumLength(100);
        
        RuleFor(p => p.Password)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .Length(8,16).WithMessage("El campo {PropertyName} debe contener entre 8 a 16 caracteres como máximo")
            .Matches(@"[A-Z]+").WithMessage("El campo {PropertyName} debe contener al menos un caracter en mayúscula")
            .Matches(@"[a-z]+").WithMessage("El campo {PropertyName} debe contener al menos un caracter en minúscula")
            .Matches(@"[0-9]+").WithMessage("El campo {PropertyName} debe contener al menos un número especial")
            .Matches(@"[""!@$%^&*(){}:;<>,.?/+_=|'~\\-]+").WithMessage("El campo {PropertyName} debe contener al menos un caracter especial")
            .Matches("^[^£# “”]*$").WithMessage("'{PropertyName}' no debe contener los siguiente caracteres £ # “” o espacios.");
        
        RuleFor(p => p.NickUser)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .Length(3).WithMessage("El campo {PropertyName} debe tener 3 caracteres") 
            .Matches(@"^[^""!@$%^&*(){}:;<>,.?/+_=|'~\\-]*$").WithMessage("El campo {PropertyName} no debe contener caracteres especiales");
        
        RuleFor(p => p.FlagTipoPerfil)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .Length(1)
            .Matches("^[ASU]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres S (SuperAdministrador), A (Administrador) o U (Usuario)"); 

        RuleFor(p => p.NumeroTipoIdentificacion)
            .Cascade(CascadeMode.Stop) 
            .MaximumLength(11).WithMessage("El campo {PropertyName} debe tener como máximo 11 caracteres") 
            .Matches("^[0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres numéricos");
    }
}
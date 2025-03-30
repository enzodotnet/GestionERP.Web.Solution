using FluentValidation;
using FluentValidation.Validators;

namespace GestionERP.Web.Models.Dtos.Principal;

public class UsuarioEditarDto
{  
    public string Email { get; set; }
    public string NickUser { get; set; }
    public string Nombre { get; set; }
    public bool Activo { get; set; } 
    public List<UsuarioModuloInsertarDto> ModulosInsertar { get; set; } = new();
    public List<UsuarioModuloEditarDto> ModulosEditar { get; set; } = new();
    public List<UsuarioServicioInsertarDto> ServiciosInsertar { get; set; } = new();
    public List<UsuarioServicioEditarDto> ServiciosEditar { get; set; } = new();
    public List<UsuarioPermisoInsertarDto> PermisosInsertar { get; set; } = new();
    public List<UsuarioPermisoEditarDto> PermisosEditar { get; set; } = [];
}

public class UsuarioEditarValidator : AbstractValidator<UsuarioEditarDto>
{
    [Obsolete("Validación del campo email de la clase es obsoleta, pero valida mejor.", true)]
    public UsuarioEditarValidator()
    {
         RuleFor(p => p.Nombre)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .Length(10,200)
            .Matches(@"^[^""!@$%^&*(){}:;<>,.?/+_=|'~\\-]*$").WithMessage("El campo {PropertyName} no debe contener caracteres especiales");
        
        RuleFor(p => p.Email)
            .EmailAddress(EmailValidationMode.Net4xRegex)
            .MaximumLength(100);

        RuleFor(p => p.NickUser)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .Length(3).WithMessage("El campo {PropertyName} debe tener 3 caracteres")
            .Matches(@"^[^""!@$%^&*(){}:;<>,.?/+_=|'~\\-]*$").WithMessage("El campo {PropertyName} no debe contener caracteres especiales");
    }
}
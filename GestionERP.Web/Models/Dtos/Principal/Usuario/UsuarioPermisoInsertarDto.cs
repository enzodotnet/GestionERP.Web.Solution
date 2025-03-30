using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class UsuarioPermisoInsertarDto
{   
    public string CodigoPermiso { get; set; } 
}

public class UsuarioPermisoInsertarValidator : AbstractValidator<UsuarioPermisoInsertarDto>
{
    public UsuarioPermisoInsertarValidator()
    {
        RuleFor(p => p.CodigoPermiso).NotEmpty().WithMessage("El campo {PropertyName} es requerido en el insertar permisos al servicio del módulo del usuario");
    }
}
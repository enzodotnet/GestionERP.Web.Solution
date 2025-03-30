using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class UsuarioServicioInsertarDto
{  
    public string CodigoServicio { get; set; }
}

public class UsuarioServicioInsertarValidator : AbstractValidator<UsuarioServicioInsertarDto>
{
    public UsuarioServicioInsertarValidator()
    {
        RuleFor(p => p.CodigoServicio).NotEmpty().WithMessage("El campo {PropertyName} es requerido en el insertar servicios al módulo del usuario");
    }
}
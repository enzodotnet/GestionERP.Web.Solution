using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class UsuarioModuloInsertarDto
{ 
    public string CodigoModulo { get; set; }
}

public class UsuarioModuloInsertarValidator : AbstractValidator<UsuarioModuloInsertarDto>
{
    public UsuarioModuloInsertarValidator()
    {
        RuleFor(p => p.CodigoModulo).NotEmpty().WithMessage("El campo {PropertyName} es requerido en el insertar módulos al usuario"); 
    }
}
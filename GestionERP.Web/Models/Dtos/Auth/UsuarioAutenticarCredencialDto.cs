using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Auth;

public class UsuarioAutenticarCredencialDto
{
    public string CodigoUsuario { get; set; }
    public string Password { get; set; }
}

public class UsuarioAutenticarCredencialValidator : AbstractValidator<UsuarioAutenticarCredencialDto>
{
    public UsuarioAutenticarCredencialValidator()
    {
        RuleFor(p => p.CodigoUsuario).NotEmpty().WithMessage("Es necesario que ingrese el código de usuario");
        RuleFor(p => p.Password).NotEmpty().WithMessage("Es necesario que ingrese la contraseña");
    }
}


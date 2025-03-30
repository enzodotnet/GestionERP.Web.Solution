using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Archivo;

public class PrincipalEmpresaInsertarDto
{
    public Guid OrigenArchivoId { get; set; }
    public string FlagTipoArchivo { get; set; }
    public string NombreArchivo { get; set; }
    public string Base64Archivo { get; set; }
    public string TipoContenido { get; set; }
}

public class PrincipalEmpresaInsertarValidator : AbstractValidator<PrincipalEmpresaInsertarDto>
{
    public PrincipalEmpresaInsertarValidator()
    { 
        RuleFor(p => p.OrigenArchivoId).NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.FlagTipoArchivo)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .Length(1).WithMessage("El campo {PropertyName} debe tener solo 1 caracter")
            .Matches("^[L]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres del tipo L: Logo principal del sistema");

        RuleFor(p => p.NombreArchivo)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .MaximumLength(200).WithMessage("El campo {PropertyName} debe tener como máximo 200 caracteres")
            .Matches(@"^[^""*:<>?/|\\]*$").WithMessage("El campo {PropertyName} no debe contener caracteres especiales {PropertyValue}");

        RuleFor(p => p.TipoContenido)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres");

        RuleFor(p => p.Base64Archivo)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");
    }
}
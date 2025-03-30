using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class TipoSituacionLetraEditarDto
{
    public string Nombre { get; set; }
    public string Abreviacion { get; set; }
    public bool EnCarteraGeneral { get; set; }
    public int? DocumentoGeneraId { get; set; }
    public string Descripcion { get; set; }
    public bool Activo { get; set; }
}

public class TipoSituacionLetraEditarValidator : AbstractValidator<TipoSituacionLetraEditarDto>
{
    public TipoSituacionLetraEditarValidator()
    {
        RuleFor(p => p.Nombre)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres");

        RuleFor(p => p.Abreviacion)
            .MaximumLength(10).WithMessage("El campo {PropertyName} debe tener como máximo 10 caracteres");

        RuleFor(p => p.Descripcion)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
    }
}
using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class TipoSituacionLetraInsertarDto
{
    public string CodigoEmpresa { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string FlagTipoLetra { get; set; }
    public string Abreviacion { get; set; }
    public bool EnCarteraGeneral { get; set; }
    public int? DocumentoGeneraId { get; set; }
    public string Descripcion { get; set; }
}

public class TipoSituacionLetraInsertarValidator : AbstractValidator<TipoSituacionLetraInsertarDto>
{
    public TipoSituacionLetraInsertarValidator()
    {
        RuleFor(p => p.CodigoEmpresa)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.Codigo)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .Length(2).WithMessage("El campo {PropertyName} debe tener 2 caracteres")
            .Matches("^[A-Z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos");

        RuleFor(p => p.Nombre)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres");

        RuleFor(p => p.FlagTipoLetra)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.Abreviacion)
            .MaximumLength(10).WithMessage("El campo {PropertyName} debe tener como máximo 10 caracteres");

        RuleFor(p => p.Descripcion)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
    }
}
using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class CierreContableEditarDto
{
    public string Nombre { get; set; }
    public int DiarioCierreId { get; set; }
    public int DiarioAperturaId { get; set; }
    public bool EsPredeterminado { get; set; }
    public string Descripcion { get; set; }
    public bool Activo { get; set; }
    public IEnumerable<CierreContableDetalleEliminarDto> DetallesEliminar { get; set; }
    public IEnumerable<CierreContableDetalleEditarDto> DetallesEditar { get; set; }
    public IEnumerable<CierreContableDetalleInsertarDto> DetallesInsertar { get; set; }
}

public class CierreContableEditarValidator : AbstractValidator<CierreContableEditarDto>
{
    public CierreContableEditarValidator()
    {
        RuleFor(p => p.Nombre)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres");

        RuleFor(p => p.Descripcion)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");

        RuleFor(p => p.DiarioCierreId).NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.DiarioAperturaId).NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleForEach(p => p.DetallesEliminar).SetValidator(new CierreContableDetalleEliminarValidator());

        RuleForEach(p => p.DetallesEditar).SetValidator(new CierreContableDetalleEditarValidator());

        RuleForEach(p => p.DetallesInsertar).SetValidator(new CierreContableDetalleInsertarValidator());
    }
}
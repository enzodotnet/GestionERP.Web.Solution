using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class CierreContableInsertarDto
{
    public string CodigoEmpresa { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public DateTime FechaRegistro { get; set; }
    public int DiarioCierreId { get; set; }
    public int DiarioAperturaId { get; set; }
    public bool EsPredeterminado { get; set; }
    public string Descripcion { get; set; }
    public IEnumerable<CierreContableDetalleInsertarDto> Detalles { get; set; }
}

public class CierreContableInsertarValidator : AbstractValidator<CierreContableInsertarDto>
{
    public CierreContableInsertarValidator()
    {
        RuleFor(p => p.CodigoEmpresa)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.Codigo)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .Length(3).WithMessage("El campo {PropertyName} debe tener 3 caracteres")
            .Matches("^[A-Z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos");

        RuleFor(p => p.Nombre)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres");

        RuleFor(p => p.DiarioCierreId).NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.DiarioAperturaId).NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.Descripcion)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");

        RuleForEach(p => p.Detalles).SetValidator(new CierreContableDetalleInsertarValidator());
    }
}
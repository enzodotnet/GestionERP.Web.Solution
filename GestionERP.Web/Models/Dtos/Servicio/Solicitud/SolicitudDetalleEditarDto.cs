using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Servicio;

public class SolicitudDetalleEditarDto
{ 
    public Guid Id { get; set; }
    public int? Cantidad { get; set; }
    public string Observacion { get; set; }
}

public class SolicitudDetalleEditarValidator : AbstractValidator<SolicitudDetalleEditarDto>
{
    public SolicitudDetalleEditarValidator()
    {
        RuleFor(p => p.Cantidad)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("El campo {PropertyName} es requerido") 
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a 0")
            .LessThanOrEqualTo(9999999).WithMessage("El campo {PropertyName} debe tener como máximo 7 dígitos");

        RuleFor(p => p.Observacion).MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
    }
}
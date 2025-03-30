using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Servicio;

public class OrdenDetalleEditarDto
{
    public Guid Id { get; set; }
    public int? Cantidad { get; set; }
    public decimal? PrecioUnitario { get; set; } 
    public decimal? ImporteBruto { get; set; } 
    public decimal? ImporteImpuesto { get; set; }
    public decimal? ImporteNeto { get; set; }
    public string Observacion { get; set; }
}

public class OrdenDetalleEditarValidator : AbstractValidator<OrdenDetalleEditarDto>
{
    public OrdenDetalleEditarValidator()
    {
        RuleFor(p => p.Cantidad)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("El campo {PropertyName} es requerido") 
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a 0")
            .LessThanOrEqualTo(9999999).WithMessage("El campo {PropertyName} debe tener como máximo 7 dígitos");

        RuleFor(p => p.PrecioUnitario)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("El campo {PropertyName} es requerido") 
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a 0")
            .PrecisionScale(13, 6, true).WithMessage("El campo {PropertyName} debe contener como máximo 13 dígitos incluyendo 6 decimales");
        
        RuleFor(p => p.Observacion).MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
    }
}
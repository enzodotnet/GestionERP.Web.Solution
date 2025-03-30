using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Importacion;

public class OrdenDetalleEditarDto
{ 
    public Guid Id { get; set; }
    public decimal? Cantidad { get; set; }
    public decimal? PrecioUnitario { get; set; } 
    public decimal? ImporteBruto { get; set; } 
    public string Observacion { get; set; }
}

public class OrdenDetalleEditarValidator : AbstractValidator<OrdenDetalleEditarDto>
{
    public decimal? UnidadConversion { get; set; }
    
    public OrdenDetalleEditarValidator()
    {
        RuleFor(p => p.Cantidad)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("El campo {PropertyName} es requerido") 
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a 0")
            .PrecisionScale(10, 3, true).WithMessage("El campo {PropertyName} debe contener como máximo 10 dígitos incluyendo 3 decimales")
            .Must(x => !UnidadConversion.HasValue || (x % UnidadConversion) == 0).WithMessage("La cantidad no es múltiplo entre la unidad de conversión del artículo");
        
        RuleFor(p => p.PrecioUnitario)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("El campo {PropertyName} es requerido") 
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a 0")
            .PrecisionScale(13, 6, true).WithMessage("El campo {PropertyName} debe contener como máximo 13 dígitos incluyendo 6 decimales");
        
        RuleFor(p => p.Observacion).MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
    }
}
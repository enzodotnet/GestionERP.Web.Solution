using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Importacion;

public class OrdenDetalleInsertarDto
{
    public string CodigoArticulo { get; set; }
    public decimal? Cantidad { get; set; }
    public decimal? PrecioUnitario { get; set; } 
    public decimal? ImporteBruto { get; set; } 
    public string Observacion { get; set; } 
    public string CodigoSolicitudReferencia { get; set; }
}

public class OrdenDetalleInsertarValidator : AbstractValidator<OrdenDetalleInsertarDto>
{
    public decimal? UnidadConversion { get; set; }
    public string MsgErrorArticulo { get; set; }
 
    public OrdenDetalleInsertarValidator()
    {
        RuleFor(p => p.CodigoArticulo)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos")
            .Must(x => string.IsNullOrEmpty(MsgErrorArticulo)).WithMessage(x => MsgErrorArticulo);
         
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
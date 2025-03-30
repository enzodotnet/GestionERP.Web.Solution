using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Compra;

public class SolicitudDetalleInsertarDto
{ 
    public string CodigoArticulo { get; set; }
    public decimal? Cantidad { get; set; } 
    public string Observacion { get; set; } 
}

public class SolicitudDetalleInsertarValidator : AbstractValidator<SolicitudDetalleInsertarDto>
{
    public decimal? UnidadConversion { get; set; }
    public string MsgErrorArticulo { get; set; }
     
    public SolicitudDetalleInsertarValidator()
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
         
        RuleFor(p => p.Observacion).MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres"); 
    }
}
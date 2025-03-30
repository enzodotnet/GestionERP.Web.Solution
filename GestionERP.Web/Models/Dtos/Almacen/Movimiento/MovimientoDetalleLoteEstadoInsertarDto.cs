using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Almacen;

public class MovimientoDetalleLoteEstadoInsertarDto
{
    public string CodigoProductoEstado { get; set; }
    public decimal Cantidad { get; set; }
}

public class MovimientoDetalleLoteEstadoInsertarValidator : AbstractValidator<MovimientoDetalleLoteEstadoInsertarDto>
{
    public MovimientoDetalleLoteEstadoInsertarValidator()
    { 
        RuleFor(p => p.CodigoProductoEstado)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido en el insertar lotes del Detalle al movimiento"); 
         
        RuleFor(p => p.Cantidad)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido en el insertar lotes del Detalle al movimiento") 
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a 0 en el insertar lotes del Detalle al movimiento")
            .PrecisionScale(10, 3, true).WithMessage("El campo {PropertyName} debe contener como máximo 10 dígitos incluyendo 3 decimales en el insertar lotes del Detalle al movimiento");
    }
}
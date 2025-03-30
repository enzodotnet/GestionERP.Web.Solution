using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Almacen;

public class MovimientoDetalleLoteInsertarDto
{
    public string NumeroLote { get; set; }
    public string CodigoMarca { get; set; }
    public DateTime FechaFabricacion { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public decimal Cantidad { get; set; }
    public List<MovimientoDetalleLoteEstadoInsertarDto> Estados { get; set; }
}

public class MovimientoDetalleLoteInsertarValidator : AbstractValidator<MovimientoDetalleLoteInsertarDto>
{
    public MovimientoDetalleLoteInsertarValidator()
    {
        RuleFor(p => p.NumeroLote)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido en el insertar lotes del Detalle al movimiento")
            .MaximumLength(20).WithMessage("El campo {PropertyName} debe tener como máximo 20 caracteres en el insertar lotes del Detalle al movimiento");
        
        RuleFor(p => p.CodigoMarca).NotEmpty().WithMessage("El campo {PropertyName} es requerido en el insertar lotes del Detalle al movimiento");
         
        RuleFor(p => p.Cantidad)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido en el insertar lotes del Detalle al movimiento") 
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a 0 en el insertar lotes del Detalle al movimiento")
            .PrecisionScale(10, 3, true).WithMessage("El campo {PropertyName} debe contener como máximo 10 dígitos incluyendo 3 decimales en el insertar lotes del Detalle al movimiento");

        RuleFor(p => p.FechaFabricacion)
            .NotNull().WithMessage("El campo {PropertyName} es requerido en el insertar lotes del Detalle al movimiento");

        RuleFor(p => p.FechaVencimiento)
            .NotNull().WithMessage("El campo {PropertyName} es requerido en el insertar lotes del Detalle al movimiento"); 

        RuleFor(p => p.Cantidad)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido en el insertar lotes del Detalle al movimiento") 
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a 0 en el insertar lotes del Detalle al movimiento")
            .PrecisionScale(10, 3, true).WithMessage("El campo {PropertyName} debe contener como máximo 10 dígitos incluyendo 3 decimales en el insertar lotes del Detalle al movimiento");
    }
}
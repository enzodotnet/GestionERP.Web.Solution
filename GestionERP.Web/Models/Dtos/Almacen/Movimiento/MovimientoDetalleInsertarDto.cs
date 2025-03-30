using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Almacen;

public class MovimientoDetalleInsertarDto
{
    public string CodigoAlmacen { get; set; }
    public string CodigoArticulo { get; set; }
    public decimal? Cantidad { get; set; }
    public decimal? CostoUnitarioMN { get; set; }
    public decimal? CostoUnitarioME { get; set; }
    public decimal? ValorCostoMN { get; set; }
    public decimal? ValorCostoME { get; set; }
    public List<MovimientoDetalleLoteInsertarDto> Lotes { get; set; }
}

public class MovimientoDetalleInsertarValidator : AbstractValidator<MovimientoDetalleInsertarDto>
{
    public decimal? UnidadConversion { get; set; }
    public string MsgErrorArticulo { get; set; }

    public MovimientoDetalleInsertarValidator()
    {
        RuleFor(p => p.CodigoAlmacen)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido en el insertar detalle al movimiento"); 

        RuleFor(p => p.CodigoArticulo)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido en el insertar detalle al movimiento"); 
         
        RuleFor(p => p.Cantidad)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido en el insertar detalle al movimiento") 
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a 0 en el insertar detalle al movimiento")
            .PrecisionScale(10, 3, true).WithMessage("El campo {PropertyName} debe contener como máximo 10 dígitos incluyendo 3 decimales en el insertar detalle al movimiento");
         
        RuleFor(p => p.CostoUnitarioMN) 
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a 0 en el insertar detalle al movimiento")
            .PrecisionScale(13, 6, true).WithMessage("El campo {PropertyName} debe contener como máximo 13 dígitos incluyendo 6 decimales en el insertar detalle al movimiento");
        
        RuleFor(p => p.CostoUnitarioME) 
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a 0 en el insertar detalle al movimiento")
            .PrecisionScale(13, 6, true).WithMessage("El campo {PropertyName} debe contener como máximo 13 dígitos incluyendo 6 decimales en el insertar detalle al movimiento");

        RuleFor(p => p.ValorCostoMN) 
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a 0 en el insertar detalle al movimiento")
            .PrecisionScale(19, 2, true).WithMessage("El campo {PropertyName} debe contener como máximo 19 dígitos incluyendo 2 decimales en el insertar detalle al movimiento");
        
        RuleFor(p => p.ValorCostoME) 
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a 0 en el insertar detalle al movimiento")
            .PrecisionScale(19, 2, true).WithMessage("El campo {PropertyName} debe contener como máximo 19 dígitos incluyendo 2 decimales en el insertar detalle al movimiento");
    }
}
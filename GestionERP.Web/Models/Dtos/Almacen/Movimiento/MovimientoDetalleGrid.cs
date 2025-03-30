using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Almacen;

public class MovimientoDetalleGrid
{
    public Guid? Id { get; set; } 
    public string CodigoTipoArticulo { get; set; }
    public string NombreTipoArticulo { get; set; }
    public string CodigoArticulo { get; set; }
    public string NombreArticulo { get; set; } 
    public string CodigoUnidadMedida { get; set; }
    public string NombreUnidadMedida { get; set; }
    public string Presentacion { get; set; }
    public decimal? UnidadConversion { get; set; }
    public decimal? Cantidad { get; set; }
    public decimal? CostoUnitarioMN { get; set; }
    public decimal? CostoUnitarioME { get; set; }
    public decimal? ValorCostoMN { get; set; }
    public decimal? ValorCostoME { get; set; }
}

public class MovimientoDetalleGridValidator : AbstractValidator<MovimientoDetalleGrid>
{
    public decimal? UnidadConversion { get; set; }

    public MovimientoDetalleGridValidator()
    {
        RuleFor(p => p.Cantidad)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("El campo {PropertyName} es requerido")
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a 0")
            .PrecisionScale(10, 3, true).WithMessage("El campo {PropertyName} debe contener como máximo 10 dígitos incluyendo 3 decimales")
            .Must(x => !UnidadConversion.HasValue || (x % UnidadConversion) == 0).WithMessage("La cantidad no es múltiplo entre la unidad de conversión del artículo");

        RuleFor(p => p.CostoUnitarioMN)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a 0 ")
            .PrecisionScale(13, 6, true).WithMessage("El campo {PropertyName} debe contener como máximo 13 dígitos incluyendo 6 decimales");

        RuleFor(p => p.CostoUnitarioME)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a 0 ")
            .PrecisionScale(13, 6, true).WithMessage("El campo {PropertyName} debe contener como máximo 13 dígitos incluyendo 6 decimales");
    }
}
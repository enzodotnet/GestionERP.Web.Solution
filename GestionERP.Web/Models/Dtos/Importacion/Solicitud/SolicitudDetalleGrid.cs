using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Importacion;

public class SolicitudDetalleGrid
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
    public string Observacion { get; set; }
    public decimal CantidadAtendida { get; set; }
    public decimal CantidadRescindida { get; set; }
}

public class SolicitudDetalleGridValidator : AbstractValidator<SolicitudDetalleGrid>
{
    public decimal? UnidadConversion { get; set; }
    
    public SolicitudDetalleGridValidator()
    {
        RuleFor(p => p.Cantidad)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("El campo {PropertyName} es requerido")
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a 0")
            .PrecisionScale(10, 3, true).WithMessage("El campo {PropertyName} debe contener como máximo 10 dígitos incluyendo 3 decimales")
            .Must(x => !UnidadConversion.HasValue || (x % UnidadConversion) == 0).WithMessage("La cantidad no es múltiplo entre la unidad de conversión del artículo");
    }
}
using FluentValidation; 

namespace GestionERP.Web.Models.Dtos.Servicio;

public class OrdenDetalleGrid
{
    public Guid? Id { get; set; } 
    public string CodigoTipoArticulo { get; set; }
    public string NombreTipoArticulo { get; set; }
    public string CodigoArticulo { get; set; }
    public string NombreArticulo { get; set; }
    public bool EsAfectoImpuesto { get; set; }
    public string CodigoUnidadMedida { get; set; }
    public string NombreUnidadMedida { get; set; }
    public int? Cantidad { get; set; }
    public decimal? PrecioUnitario { get; set; }
    public decimal? ImporteBruto { get; set; }
    public decimal? ImporteImpuesto { get; set; }
    public decimal? ImporteNeto { get; set; }
    public string Observacion { get; set; }
    public decimal ImporteBrutoAceptado { get; set; }
    public decimal ImporteBrutoAtendido { get; set; }
    public decimal ImporteBrutoRescindido { get; set; }
	public string CodigoSolicitudReferencia { get; set; }
}

public class OrdenDetalleGridValidator : AbstractValidator<OrdenDetalleGrid>
{
    public OrdenDetalleGridValidator()
    {
        RuleFor(p => p.Cantidad)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("La cantidad es requerida")
            .GreaterThan(0).WithMessage("La cantidad debe ser mayor a 0")
            .LessThanOrEqualTo(9999999).WithMessage("La cantidad debe tener como máximo 7 dígitos");

        RuleFor(p => p.PrecioUnitario)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("El precio unitario es requerido")
            .GreaterThan(0).WithMessage("El precio unitario debe ser mayor a 0")
            .PrecisionScale(13, 6, true).WithMessage("El precio unitario debe contener como máximo 13 dígitos incluyendo 6 decimales"); 
    }
}
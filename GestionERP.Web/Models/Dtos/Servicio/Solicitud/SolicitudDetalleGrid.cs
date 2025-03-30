using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Servicio;

public class SolicitudDetalleGrid
{
    public Guid? Id { get; set; } 
    public string CodigoTipoArticulo { get; set; }
    public string NombreTipoArticulo { get; set; }
    public string CodigoArticulo { get; set; }
    public string NombreArticulo { get; set; }
    public string CodigoUnidadMedida { get; set; }
    public string NombreUnidadMedida { get; set; }
    public int? Cantidad { get; set; }
    public string Observacion { get; set; }
    public int CantidadAtendida { get; set; }
    public int CantidadRescindida { get; set; }
}

public class SolicitudDetalleGridValidator : AbstractValidator<SolicitudDetalleGrid>
{
    public SolicitudDetalleGridValidator()
    {
        RuleFor(p => p.Cantidad)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("El campo {PropertyName} es requerido")
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a 0")
            .LessThanOrEqualTo(9999999).WithMessage("El campo {PropertyName} debe tener como máximo 7 dígitos");
    }
}
using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Servicio;

public class ContratoDetalleGrid
{
    public Guid? Id { get; set; } 
    public string CodigoTipoArticulo { get; set; }
    public string NombreTipoArticulo { get; set; }
    public string CodigoArticulo { get; set; }
    public string NombreArticulo { get; set; }
    public bool EsAfectoImpuesto { get; set; }
    public decimal? ImporteBruto { get; set; }
	public decimal? ImporteImpuesto { get; set; }
    public decimal? ImporteNeto { get; set; }
    public string Observacion { get; set; }
}

public class ContratoDetalleGridValidator : AbstractValidator<ContratoDetalleGrid>
{
	public ContratoDetalleGridValidator()
	{ 
		RuleFor(p => p.ImporteBruto)
			.Cascade(CascadeMode.Stop)
			.NotNull().WithMessage("El importe bruto es requerido")
			.GreaterThan(0).WithMessage("El importe bruto debe ser mayor a 0")
			.PrecisionScale(16, 2, true).WithMessage("El importe bruto debe contener como máximo 16 dígitos incluyendo 2 decimales");
	}
}
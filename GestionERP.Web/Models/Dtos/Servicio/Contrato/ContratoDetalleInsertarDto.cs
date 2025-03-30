using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Servicio;

public class ContratoDetalleInsertarDto
{ 
    public string CodigoArticulo { get; set; }
    public decimal? ImporteBruto { get; set; }
    public bool EsAfectoImpuesto { get; set; }
    public decimal? ImporteImpuesto { get; set; }
    public decimal? ImporteNeto { get; set; }
    public string Observacion { get; set; }
}

public class ContratoDetalleInsertarValidator : AbstractValidator<ContratoDetalleInsertarDto>
{
    public string MsgErrorArticulo { get; set; }

    public ContratoDetalleInsertarValidator()
    {
        RuleFor(p => p.CodigoArticulo)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos")
            .Must(x => string.IsNullOrEmpty(MsgErrorArticulo)).WithMessage(x => MsgErrorArticulo); 
          
        RuleFor(p => p.ImporteBruto)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("El campo {PropertyName} es requerido") 
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a 0")
            .PrecisionScale(16, 2, true).WithMessage("El campo {PropertyName} debe contener como máximo 16 dígitos incluyendo 2 decimales");
        
        RuleFor(p => p.Observacion).MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
    }
}
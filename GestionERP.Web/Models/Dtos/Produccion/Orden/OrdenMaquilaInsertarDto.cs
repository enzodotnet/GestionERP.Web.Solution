using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Produccion;

public class OrdenMaquilaInsertarDto
{ 
    public string CodigoOrdenServicio { get; set; }
    public DateTime? FechaEntrega { get; set; }
    public string CodigoTipoCambioDia { get; set; }
    public decimal? MontoTipoCambioDia { get; set; }
    public decimal? ImporteBrutoVinculadoMN { get; set; }
    public decimal? ImporteBrutoVinculadoME { get; set; }
    public string Observacion { get; set; }
}

public class OrdenMaquilaInsertarValidator : AbstractValidator<OrdenMaquilaInsertarDto>
{
    public OrdenMaquilaInsertarValidator()
    {
        RuleFor(p => p.CodigoOrdenServicio).NotEmpty().WithMessage("El campo {PropertyName} es requerido"); 

        RuleFor(p => p.FechaEntrega).NotNull().WithMessage("El campo {PropertyName} es requerido"); 

        RuleFor(p => p.CodigoTipoCambioDia).NotEmpty().WithMessage("El campo {PropertyName} es requerido"); 
 
        RuleFor(p => p.MontoTipoCambioDia)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("El campo {PropertyName} es requerido") 
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a 0")
            .PrecisionScale(6, 4, true).WithMessage("El campo {PropertyName} debe contener como máximo 6 dígitos incluyendo 4 decimales");

        RuleFor(p => p.ImporteBrutoVinculadoMN)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("El campo {PropertyName} es requerido") 
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a 0")
            .PrecisionScale(18, 2, true).WithMessage("El campo {PropertyName} debe contener como máximo 18 dígitos incluyendo 2 decimales");

        RuleFor(p => p.ImporteBrutoVinculadoME)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("El campo {PropertyName} es requerido") 
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a 0")
            .PrecisionScale(18, 2, true).WithMessage("El campo {PropertyName} debe contener como máximo 18 dígitos incluyendo 2 decimales");
         
        RuleFor(p => p.Observacion).MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
    }
}
using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Produccion;

public class OrdenLoteInsertarDto
{  
    public string NumeroLote { get; set; }
    public string CodigoMarca { get; set; }
    public DateTime? FechaFabricacion { get; set; }
    public DateTime? FechaVencimiento { get; set; }
    public string CodigoProductoEstado { get; set; }
    public decimal? Cantidad { get; set; }
    public string Observacion { get; set; }
}

public class OrdenLoteInsertarValidator : AbstractValidator<OrdenLoteInsertarDto>
{
    public OrdenLoteInsertarValidator()
    { 
        RuleFor(p => p.NumeroLote)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .MaximumLength(20).WithMessage("El campo {PropertyName} debe tener como máximo 20 caracteres");

        RuleFor(p => p.CodigoMarca).NotEmpty().WithMessage("El campo {PropertyName} es requerido"); 

        RuleFor(p => p.FechaFabricacion).NotNull().WithMessage("El campo {PropertyName} es requerido"); 

        RuleFor(p => p.FechaVencimiento).NotNull().WithMessage("El campo {PropertyName} es requerido"); 

        RuleFor(p => p.CodigoProductoEstado).NotEmpty().WithMessage("El campo {PropertyName} es requerido"); 

        RuleFor(p => p.Cantidad)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("El campo {PropertyName} es requerido") 
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a 0")
            .PrecisionScale(10, 3, true).WithMessage("El campo {PropertyName} debe contener como máximo 10 dígitos incluyendo 3 decimales");
         
        RuleFor(p => p.Observacion).MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
    }
}
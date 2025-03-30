using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Produccion;

public class OrdenLoteEditarDto
{  
    public Guid Id { get; set; }
    public string CodigoMarca { get; set; }
    public DateTime? FechaFabricacion { get; set; }
    public DateTime? FechaVencimiento { get; set; }
    public string CodigoProductoEstado { get; set; }
    public decimal? Cantidad { get; set; }
    public string Observacion { get; set; }
}

public class OrdenLoteEditarValidator : AbstractValidator<OrdenLoteEditarDto>
{
    public OrdenLoteEditarValidator()
    { 
        RuleFor(p => p.CodigoMarca).NotEmpty().WithMessage("El campo {PropertyName} es requerido en el editar lotes de la orden"); 

        RuleFor(p => p.FechaFabricacion).NotNull().WithMessage("El campo {PropertyName} es requerido en el editar lotes de la orden"); 

        RuleFor(p => p.FechaVencimiento).NotNull().WithMessage("El campo {PropertyName} es requerido en el editar lotes de la orden"); 

        RuleFor(p => p.CodigoProductoEstado).NotEmpty().WithMessage("El campo {PropertyName} es requerido en el editar lotes de la orden"); 

        RuleFor(p => p.Cantidad)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("El campo {PropertyName} es requerido en el editar lotes de la orden") 
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a 0")
            .PrecisionScale(10, 3, true).WithMessage("El campo {PropertyName} debe contener como máximo 10 dígitos incluyendo 3 decimales");
         
        RuleFor(p => p.Observacion).MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
    }
}
using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Servicio;

public class OrdenDetalleAceptacionInsertarDto
{  
    public Guid OrdenDetalleId { get; set; }
    public DateTime FechaAceptacion { get; set; }
    public decimal? ImporteBruto { get; set; }
    public string Observacion { get; set; }
    public string Motivo { get; set; }
}

public class OrdenDetalleAceptacionInsertarValidator : AbstractValidator<OrdenDetalleAceptacionInsertarDto>
{
    public OrdenDetalleAceptacionInsertarValidator()
    { 
        RuleFor(p => p.ImporteBruto)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("El campo {PropertyName} es requerido") 
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a 0")
            .PrecisionScale(18, 2, true).WithMessage("El campo {PropertyName} debe contener como máximo 18 dígitos incluyendo 2 decimales"); 
        
        RuleFor(p => p.Observacion) 
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");

        RuleFor(p => p.Motivo)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .MaximumLength(200).WithMessage("El campo {PropertyName} debe tener como máximo 200 caracteres");
    }
}
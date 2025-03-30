using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Produccion;

public class OrdenTareoInsertarDto
{  
    public string CodigoPersonal { get; set; } 
    public string CodigoFuncion { get; set; }
    public DateTime? FechaTrabajo { get; set; }
    public string CodigoTipoCambioDia { get; set; }
    public decimal? MontoTipoCambioDia { get; set; }
    public decimal? CantidadHorasTrabajo { get; set; }
    public decimal? MontoPagoHoraMN { get; set; }
    public decimal? MontoPagoHoraME { get; set; }
    public decimal? TotalMontoPagoMN { get; set; }
    public decimal? TotalMontoPagoME { get; set; }
    public string Observacion { get; set; }
}

public class OrdenTareoInsertarValidator : AbstractValidator<OrdenTareoInsertarDto>
{
    public OrdenTareoInsertarValidator()
    { 
        RuleFor(p => p.CodigoPersonal).NotEmpty().WithMessage("El campo {PropertyName} es requerido"); 

        RuleFor(p => p.CodigoFuncion).NotEmpty().WithMessage("El campo {PropertyName} es requerido"); 

        RuleFor(p => p.FechaTrabajo).NotNull().WithMessage("El campo {PropertyName} es requerido");
    
        RuleFor(p => p.CodigoTipoCambioDia).NotEmpty().WithMessage("El campo {PropertyName} es requerido");
    
        RuleFor(p => p.MontoTipoCambioDia)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("El campo {PropertyName} es requerido") 
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a 0")
            .PrecisionScale(6, 4, true).WithMessage("El campo {PropertyName} debe contener como máximo 6 dígitos incluyendo 4 decimales");

        RuleFor(p => p.CantidadHorasTrabajo)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("El campo {PropertyName} es requerido") 
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a 0")
            .PrecisionScale(5, 2, true).WithMessage("El campo {PropertyName} debe contener como máximo 5 dígitos incluyendo 2 decimales");

        RuleFor(p => p.MontoPagoHoraMN)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("El campo {PropertyName} es requerido") 
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a 0")
            .PrecisionScale(16, 2, true).WithMessage("El campo {PropertyName} debe contener como máximo 16 dígitos incluyendo 2 decimales");
         
        RuleFor(p => p.MontoPagoHoraME)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("El campo {PropertyName} es requerido")
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a 0")
            .PrecisionScale(16, 2, true).WithMessage("El campo {PropertyName} debe contener como máximo 16 dígitos incluyendo 2 decimales");
         
        RuleFor(p => p.Observacion).MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
    }
}
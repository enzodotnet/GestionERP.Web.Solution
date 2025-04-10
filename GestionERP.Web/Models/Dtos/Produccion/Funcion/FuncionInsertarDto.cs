using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Produccion;

public class FuncionInsertarDto
{ 
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string CodigoMonedaPago { get; set; }
    public decimal MontoPagoHora { get; set; } 
    public string Descripcion { get; set; }
}

public class FuncionInsertarValidator : AbstractValidator<FuncionInsertarDto>
{
    public FuncionInsertarValidator()
    {
        RuleFor(p => p.Codigo)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .Length(3).WithMessage("El campo {PropertyName} debe tener 3 caracteres") 
            .Matches("^[A-Z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos");
 
        RuleFor(p => p.Nombre)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres");
        
        RuleFor(p => p.CodigoMonedaPago)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.MontoPagoHora)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .GreaterThan(0)
            .PrecisionScale(10, 2, true);

        RuleFor(p => p.Descripcion)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
    }
}
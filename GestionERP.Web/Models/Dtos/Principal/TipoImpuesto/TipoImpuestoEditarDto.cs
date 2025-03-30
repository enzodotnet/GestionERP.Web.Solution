using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class TipoImpuestoEditarDto
{  
    public string Nombre { get; set; }
    public decimal Porcentaje { get; set; }
    public decimal MontoMinimoMN { get; set; } 
    public bool EsPredeterminado { get; set; }
    public bool EsOperacionVenta { get; set; }
    public bool Activo { get; set; } 
}

public class TipoImpuestoEditarValidator : AbstractValidator<TipoImpuestoEditarDto>
{
    public TipoImpuestoEditarValidator()
    {  
        RuleFor(p => p.Nombre) 
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres")
            .Matches(@"^[^""!@$^&*{}:;<>,.?/+_=|'~\\]*$").WithMessage("El campo {PropertyName} no debe contener caracteres especiales");
     
        RuleFor(p => p.Porcentaje)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .GreaterThanOrEqualTo(0)
            .PrecisionScale(5, 3, true);
        
        RuleFor(p => p.MontoMinimoMN)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .GreaterThanOrEqualTo(1)
            .PrecisionScale(10, 2, true); 
    }
}
using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class DiarioInsertarDto
{ 
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public string FlagTipoProceso { get; set; } 
    public string FlagTipoRegistro { get; set; }  
    public string FlagTipoCambio { get; set; }
}

public class DiarioInsertarValidator : AbstractValidator<DiarioInsertarDto>
{
    public DiarioInsertarValidator()
    {
        RuleFor(p => p.Codigo)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .Length(3).WithMessage("El campo {PropertyName} debe tener 3 caracteres") 
            .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos");
 
        RuleFor(p => p.Nombre)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
         
        RuleFor(p => p.Descripcion)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");

        RuleFor(p => p.FlagTipoProceso)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");
        
        RuleFor(p => p.FlagTipoRegistro) 
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.FlagTipoCambio) 
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");
    }
}
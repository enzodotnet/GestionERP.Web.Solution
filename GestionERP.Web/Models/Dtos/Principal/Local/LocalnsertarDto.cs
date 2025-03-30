using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class LocalInsertarDto
{ 
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string CodigoDistrito { get; set; }  
    public string Direccion { get; set; }
    public string Observacion { get; set; }
}

public class LocalInsertarValidator : AbstractValidator<LocalInsertarDto>
{
    public LocalInsertarValidator()
    {
        RuleFor(p => p.Codigo)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .Length(3).WithMessage("El campo {PropertyName} debe tener 3 caracteres") 
            .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos");
 
        RuleFor(p => p.Nombre)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
         
        RuleFor(p => p.CodigoDistrito).NotEmpty().WithMessage("El campo {PropertyName} es requerido");
        
        RuleFor(p => p.Direccion)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .MaximumLength(200).WithMessage("El campo {PropertyName} debe tener como máximo 200 caracteres");
        
        RuleFor(p => p.Observacion)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
    }
}
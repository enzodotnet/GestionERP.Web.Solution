using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class LocalEditarDto
{ 
    public string Nombre { get; set; }
    public string CodigoDistrito { get; set; }  
    public string Direccion { get; set; }
    public string Observacion { get; set; }
    public bool Activo { get; set; } 
}

public class LocalEditarValidator : AbstractValidator<LocalEditarDto>
{
    public LocalEditarValidator()
    {
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
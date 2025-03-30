using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class CentroCostoEditarDto
{  
    public string Nombre { get; set; }
    public string Abreviacion { get; set; }
    public string Descripcion { get; set; }
    public bool Activo { get; set; } 
}

public class CentroCostoEditarValidator : AbstractValidator<CentroCostoEditarDto>
{
    public CentroCostoEditarValidator()
    { 
        RuleFor(p => p.Nombre)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
         
        RuleFor(p => p.Descripcion)
            .MaximumLength(200).WithMessage("El campo {PropertyName} debe tener como máximo 200 caracteres");
        
        RuleFor(p => p.Abreviacion)
            .MaximumLength(20).WithMessage("El campo {PropertyName} debe tener como máximo 20 caracteres");
    }
}
using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Produccion;

public class EquipoEditarDto
{  
    public string Nombre { get; set; } 
    public string FlagTipoMaquinaria { get; set; } 
    public string Descripcion { get; set; }
    public bool Activo { get; set; }
}

public class EquipoEditarValidator : AbstractValidator<EquipoEditarDto>
{
    public EquipoEditarValidator()
    { 
        RuleFor(p => p.Nombre)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres");
   
        RuleFor(p => p.FlagTipoMaquinaria)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido"); 
 
        RuleFor(p => p.Descripcion)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
    }
}
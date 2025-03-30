using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class ZonaVentaEditarDto
{   
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public bool Activo { get; set; }
}

public class ZonaVentaEditarValidator : AbstractValidator<ZonaVentaEditarDto>
{
    public ZonaVentaEditarValidator()
    { 
        RuleFor(p => p.Nombre)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
        
        RuleFor(p => p.Descripcion)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
    }
}
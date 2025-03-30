using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class ZonaDespachoInsertarDto
{
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
}

public class ZonaDespachoInsertarValidator : AbstractValidator<ZonaDespachoInsertarDto>
{
    public ZonaDespachoInsertarValidator()
    { 
        RuleFor(p => p.Codigo)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .Length(3).WithMessage("El campo {PropertyName} debe tener 3 caracteres") 
            .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos");
 
        RuleFor(p => p.Nombre)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
        
        RuleFor(p => p.Descripcion)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
    }
}
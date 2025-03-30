using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class VehiculoEditarDto
{ 
    /// <summary>
    /// Esta propiedad no es editable, es un campo clave.
    /// </summary>
    public string Codigo { get; set; } 
    public string Marca { get; set; }
    public string Modelo { get; set; }
    public string NumeroCirculacion { get; set; }
    public string Descripcion { get; set; }
    public bool Activo { get; set; }
}

public class VehiculoEditarValidator : AbstractValidator<VehiculoEditarDto>
{
    public VehiculoEditarValidator()
    {        
        RuleFor(p => p.Marca)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(20).WithMessage("El campo {PropertyName} debe tener como máximo 20 caracteres");

        RuleFor(p => p.Modelo)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres");

        RuleFor(p => p.NumeroCirculacion)
            .MaximumLength(25).WithMessage("El campo {PropertyName} debe tener como máximo 25 caracteres");
    
        RuleFor(p => p.Descripcion)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
    }
}
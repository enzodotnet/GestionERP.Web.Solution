using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class GlosarioAnalisisEditarDto
{
    public string Descripcion { get; set; }
    public string DescripcionTraducida { get; set; }
    public bool Activo { get; set; }
}

public class GlosarioAnalisisEditarValidator : AbstractValidator<GlosarioAnalisisEditarDto>
{
    public GlosarioAnalisisEditarValidator()
    {         
        RuleFor(p => p.Descripcion)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(500).WithMessage("El campo {PropertyName} debe tener como máximo 500 caracteres");
        
        RuleFor(p => p.DescripcionTraducida)
            .MaximumLength(500).WithMessage("El campo {PropertyName} debe tener como máximo 500 caracteres");
    }
}
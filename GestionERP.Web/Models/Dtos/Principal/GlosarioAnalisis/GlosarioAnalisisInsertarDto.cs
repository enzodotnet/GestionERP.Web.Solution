using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class GlosarioAnalisisInsertarDto
{ 
    public string FlagRegistro { get; set; }
    public string FlagIdiomaOriginal { get; set; }
    public string Descripcion { get; set; }
    public string DescripcionTraducida { get; set; }
}

public class GlosarioAnalisisInsertarValidator : AbstractValidator<GlosarioAnalisisInsertarDto>
{
    public GlosarioAnalisisInsertarValidator()
    {
        RuleFor(p => p.FlagRegistro)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");
        
        RuleFor(p => p.FlagIdiomaOriginal)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");
    
        RuleFor(p => p.Descripcion)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(500).WithMessage("El campo {PropertyName} debe tener como máximo 500 caracteres");
        
        RuleFor(p => p.DescripcionTraducida)
            .MaximumLength(500).WithMessage("El campo {PropertyName} debe tener como máximo 500 caracteres");
    }
}
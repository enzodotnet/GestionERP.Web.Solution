using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class GrupoGastoImportacionEditarDto
{  
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public bool Activo { get; set; } 
}

public class GrupoGastoImportacionEditarValidator : AbstractValidator<GrupoGastoImportacionEditarDto>
{
    public GrupoGastoImportacionEditarValidator()
    {         
        RuleFor(p => p.Nombre)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres")
            .Matches(@"^[^""!@$%^&*(){}:;<>,.?/+_=|'~\\-]*$").WithMessage("El campo {PropertyName} no debe contener caracteres especiales");
    
        RuleFor(p => p.Descripcion)
            .MaximumLength(200).WithMessage("El campo {PropertyName} debe tener como máximo 200 caracteres");
    }
}
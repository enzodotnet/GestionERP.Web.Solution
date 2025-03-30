using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class LineaArticuloEditarDto
{ 
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public bool Activo { get; set; }
}

public class LineaArticuloEditarValidator : AbstractValidator<LineaArticuloEditarDto>
{
    public LineaArticuloEditarValidator()
    {
        RuleFor(p => p.Nombre)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres")
            .Matches(@"^[^""!@$%^&*(){}:;<>?/+_=|'~\\]*$").WithMessage("El campo {PropertyName} no debe contener caracteres especiales");
    
        RuleFor(p => p.Descripcion)
            .MaximumLength(200).WithMessage("El campo {PropertyName} debe tener como máximo 200 caracteres");
    }
}
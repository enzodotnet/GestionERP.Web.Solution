using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class EmpresaEditarDto
{
    public string Nombre { get; set; }
    public string Acronimo { get; set; } 
    public string Descripcion { get; set; }
    public bool Activo { get; set; }
    public EmpresaAtributoEditarDto AtributoEditar { get; set; }
    public EmpresaFacturacionEditarDto FacturacionEditar { get; set; }
}

public class EmpresaEditarValidator : AbstractValidator<EmpresaEditarDto>
{
    public EmpresaEditarValidator()
    {
        RuleFor(p => p.Nombre)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(200).WithMessage("El campo {PropertyName} debe tener como máximo 200 caracteres")
            .Matches(@"^[^""!@$%^&*{}:;<>?/+_=|~\\]*$").WithMessage("El campo {PropertyName} no debe contener caracteres especiales");

        RuleFor(p => p.Acronimo)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(15).WithMessage("El campo {PropertyName} debe tener como máximo 15 caracteres")
            .Matches(@"^[^""!@$%^&*{}:;<>?/+_=|~\\]*$").WithMessage("El campo {PropertyName} no debe contener caracteres especiales");
            
        RuleFor(p => p.Descripcion)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
 
        RuleFor(p => p.AtributoEditar)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("La colección {PropertyName} es requerido")
            .SetValidator(new EmpresaAtributoEditarValidator());

        RuleFor(p => p.FacturacionEditar)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("La colección {PropertyName} es requerido")
            .SetValidator(new EmpresaFacturacionEditarValidator());
    }
}
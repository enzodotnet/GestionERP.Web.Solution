using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class PermisoInsertarDto
{ 
    public string Codigo { get; set; }
    public string CodigoServicio { get; set; }
    public string Nombre { get; set; }
    public string CodigoEvento { get; set; }
    public string Descripcion { get; set; }
    public bool EsOpcionMenu { get; set; }
}

public class PermisoInsertarValidator : AbstractValidator<PermisoInsertarDto>
{
    public PermisoInsertarValidator()
    {
        RuleFor(p => p.CodigoServicio).NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.CodigoEvento).NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        When(p => !string.IsNullOrEmpty(p.CodigoServicio) & !string.IsNullOrEmpty(p.CodigoEvento), () => {
            RuleFor(p => p.Codigo)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
                .Length(10).WithMessage("El campo {PropertyName} debe tener 10 caracteres")
                .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos");
        });

        RuleFor(p => p.Nombre)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres")
            .Matches(@"^[^""!@$%^&*(){}:;<>,.?/+_=|'~\\-]*$").WithMessage("El campo {PropertyName} no debe contener caracteres especiales");
    
        RuleFor(p => p.Descripcion)
            .MaximumLength(200).WithMessage("El campo {PropertyName} debe tener como máximo 200 caracteres");
    }
}
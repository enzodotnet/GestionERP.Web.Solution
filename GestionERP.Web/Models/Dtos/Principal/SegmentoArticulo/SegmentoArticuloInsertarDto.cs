using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class SegmentoArticuloInsertarDto
{
    public string Codigo { get; set; }
    public string CodigoGrupoArticulo { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
}

public class SegmentoArticuloInsertarValidator : AbstractValidator<SegmentoArticuloInsertarDto>
{
    public SegmentoArticuloInsertarValidator()
    {
        RuleFor(p => p.CodigoGrupoArticulo)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        When(p => !string.IsNullOrEmpty(p.CodigoGrupoArticulo), () => {
            RuleFor(p => p.Codigo)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
                .Length(5).WithMessage("El campo {PropertyName} debe tener 5 caracteres") 
                .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos");
        });
 
        RuleFor(p => p.Nombre).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres")
            .Matches(@"^[^""!@$%^&*(){}:;<>?/+_=|'~\\]*$").WithMessage("El campo {PropertyName} no debe contener caracteres especiales");
    
        RuleFor(p => p.Descripcion)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
    }
}
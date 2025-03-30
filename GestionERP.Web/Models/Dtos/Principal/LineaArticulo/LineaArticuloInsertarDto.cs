using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class LineaArticuloInsertarDto
{ 
    public string Codigo { get; set; }
    public string CodigoSegmentoArticulo { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
}

public class LineaArticuloInsertarValidator : AbstractValidator<LineaArticuloInsertarDto>
{
    public LineaArticuloInsertarValidator()
    {
        When(p => !string.IsNullOrEmpty(p.CodigoSegmentoArticulo), () => {
            RuleFor(p => p.Codigo)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
                .Length(8).WithMessage("El campo {PropertyName} debe tener 8 caracteres") 
                .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres numericos");
        });

        RuleFor(p => p.CodigoSegmentoArticulo)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.Nombre)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres")
            .Matches(@"^[^""!@$%^&*(){}:;<>?/+_=|'~\\]*$").WithMessage("El campo {PropertyName} no debe contener caracteres especiales");
    
        RuleFor(p => p.Descripcion)
            .MaximumLength(200).WithMessage("El campo {PropertyName} debe tener como máximo 200 caracteres");
    }
}
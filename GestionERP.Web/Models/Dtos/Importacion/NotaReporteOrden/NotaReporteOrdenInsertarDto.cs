using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Importacion;

public class NotaReporteOrdenInsertarDto
{  
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string Contenido { get; set; }
    public string FlagSeccion { get; set; }
}

public class NotaReporteOrdenInsertarValidator : AbstractValidator<NotaReporteOrdenInsertarDto>
{
    public NotaReporteOrdenInsertarValidator()
    { 
        RuleFor(p => p.Codigo)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .Length(4).WithMessage("El campo {PropertyName} debe tener 4 caracteres") 
            .Matches("^[0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres numéricos");
 
        RuleFor(p => p.Nombre)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");

        RuleFor(p => p.FlagSeccion)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.Contenido)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");
    }
}
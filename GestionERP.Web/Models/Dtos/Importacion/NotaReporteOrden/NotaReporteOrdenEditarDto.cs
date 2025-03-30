using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Importacion;

public class NotaReporteOrdenEditarDto
{   
    public string Nombre { get; set; }
    public string Contenido { get; set; }
    public bool Activo { get; set; } 
}

public class NotaReporteOrdenEditarValidator : AbstractValidator<NotaReporteOrdenEditarDto>
{
    public NotaReporteOrdenEditarValidator()
    { 
        RuleFor(p => p.Nombre)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");

        RuleFor(p => p.Contenido)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .MaximumLength(500).WithMessage("El campo {PropertyName} debe tener como máximo 500 caracteres");
    }
}
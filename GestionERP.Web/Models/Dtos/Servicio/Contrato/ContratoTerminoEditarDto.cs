using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Servicio;

public class ContratoTerminoEditarDto
{ 
    public Guid Id { get; set; } 
    public string Descripcion { get; set; }
}

public class ContratoTerminoEditarValidator : AbstractValidator<ContratoTerminoEditarDto>
{
    public ContratoTerminoEditarValidator()
    {
        RuleFor(p => p.Descripcion)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .MaximumLength(500).WithMessage("El campo {PropertyName} debe tener como máximo 500 caracteres");
    }
}
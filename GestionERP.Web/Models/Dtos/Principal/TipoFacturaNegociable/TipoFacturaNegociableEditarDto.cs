using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class TipoFacturaNegociableEditarDto
{
    public string Nombre { get; set; }
    public string NumeroProductoBanco { get; set; }
    public int? DocumentoGeneraId { get; set; }
    public string Descripcion { get; set; }
    public bool Activo { get; set; }
}

public class TipoFacturaNegociableEditarValidator : AbstractValidator<TipoFacturaNegociableEditarDto>
{
    public TipoFacturaNegociableEditarValidator()
    {
        RuleFor(p => p.Nombre)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres");

        RuleFor(p => p.NumeroProductoBanco)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .MaximumLength(10).WithMessage("El campo {PropertyName} debe tener como máximo 10 caracteres");

        RuleFor(p => p.Descripcion)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
    }
}
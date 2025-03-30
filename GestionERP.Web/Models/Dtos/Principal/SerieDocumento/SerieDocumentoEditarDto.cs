using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class SerieDocumentoEditarDto
{
    public Guid Id { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public string RutaHostImpresora { get; set; } 
    public string NombreImpresora { get; set; }
    public bool Activo { get; set; }  
}

public class SerieDocumentoEditarValidator : AbstractValidator<SerieDocumentoEditarDto>
{
    public SerieDocumentoEditarValidator()
    {
        RuleFor(p => p.Id)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.Nombre)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");

        RuleFor(p => p.Descripcion)
            .MaximumLength(200).WithMessage("El campo {PropertyName} debe tener como máximo 200 caracteres");
        
        RuleFor(p => p.RutaHostImpresora)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
        
        RuleFor(p => p.NombreImpresora)
            .MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres");
    }
}
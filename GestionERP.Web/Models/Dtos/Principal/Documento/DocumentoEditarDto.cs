using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class DocumentoEditarDto
{
    public string Nombre { get; set; }
    public string CodigoTipoComprobante { get; set; } 
    public bool EsAfectoRetencion { get; set; }
    public bool EsElectronico { get; set; }
    public bool EsTransferenciaGratuita { get; set; }
    public bool ConservaTipoCambioOrigen { get; set; }
    public bool Activo { get; set; }
}

public class DocumentoEditarValidator : AbstractValidator<DocumentoEditarDto>
{
    public DocumentoEditarValidator()
    {
        RuleFor(p => p.Nombre)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
    }
}
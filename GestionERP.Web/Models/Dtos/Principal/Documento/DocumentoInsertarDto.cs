using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class DocumentoInsertarDto
{ 
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string FlagTipoEntidad { get; set; }
    public string CodigoTipoDocumento { get; set; }
    public string CodigoTipoComprobante { get; set; }
    public bool GeneraCuentaCorriente { get; set; } 
    public bool EsRegistroCompraVenta { get; set; }
    public bool EsAfectoRetencion { get; set; }
    public bool EsElectronico { get; set; }
    public bool EsTransferenciaGratuita { get; set; }
    public bool ConservaTipoCambioOrigen { get; set; }
}

public class DocumentoInsertarValidator : AbstractValidator<DocumentoInsertarDto>
{
    public DocumentoInsertarValidator()
    {
        RuleFor(p => p.Codigo)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(4).WithMessage("El campo {PropertyName} debe tener como máximo 4 caracteres") 
            .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos");
        
        RuleFor(p => p.Nombre)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");

        RuleFor(p => p.FlagTipoEntidad)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");
    
        RuleFor(p => p.CodigoTipoDocumento).NotEmpty().WithMessage("El campo {PropertyName} es requerido");
    }
}
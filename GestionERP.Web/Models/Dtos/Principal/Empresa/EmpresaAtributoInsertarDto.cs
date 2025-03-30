using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class EmpresaAtributoInsertarDto
{
    public string FlagTipoRubro { get; set; }
    public string DireccionFiscal { get; set; }
    public string DireccionDeposito { get; set; }
    public string TelefonoCorporativo { get; set; }
    public string TelefonoVenta { get; set; }
    public string EmailCorporativo { get; set; }
    public string EmailVenta { get; set; }
    public string EmailCompra { get; set; }
    public string EmailCobranza { get; set; }
    public string PaginaWeb { get; set; }
    public bool EsTransportista { get; set; }
    public bool EsAgenteRetenedor { get; set; }
    public bool EnFacturacionSunatPortal { get; set; }
    public bool EnFacturacionExternoOse { get; set; }
    public string CodigoCuentaContableImpuestoExtorno { get; set; }
    public string CodigoCuentaContableOrdenDeudor { get; set; }
    public string CodigoCuentaContableOrdenAcreedor { get; set; }
    public string CuentaCorrienteDetraccion { get; set; }
    public int? CantidadDiasPlazoFacturaVencida { get; set; }
    public bool EsEmisorCertificadoAnalisis { get; set; }
    public decimal? PorcentajeIndicadorVenta { get; set; }
}

public class EmpresaAtributoInsertarValidator : AbstractValidator<EmpresaAtributoInsertarDto>
{
    public EmpresaAtributoInsertarValidator()
    {
        RuleFor(p => p.FlagTipoRubro)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido en el insertar atributo");

        RuleFor(p => p.DireccionFiscal)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido en el insertar atributo") 
            .MaximumLength(200).WithMessage("El campo {PropertyName} debe tener como máximo 200 caracteres en el insertar atributo");

        RuleFor(p => p.DireccionDeposito)
            .MaximumLength(200).WithMessage("El campo {PropertyName} debe tener como máximo 200 caracteres en el insertar atributo");

        RuleFor(p => p.TelefonoCorporativo)
            .MaximumLength(20).WithMessage("El campo {PropertyName} debe tener como máximo 20 caracteres en el insertar atributo");
        
        RuleFor(p => p.TelefonoVenta)
            .MaximumLength(20).WithMessage("El campo {PropertyName} debe tener como máximo 20 caracteres en el insertar atributo");
        
        RuleFor(p => p.EmailCorporativo)
            .MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres en el insertar atributo");
        
        RuleFor(p => p.EmailVenta)
            .MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres en el insertar atributo");
        
        RuleFor(p => p.EmailCompra)
            .MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres en el insertar atributo");
        
        RuleFor(p => p.EmailCobranza)
            .MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres en el insertar atributo");
        
        RuleFor(p => p.PaginaWeb)
            .MaximumLength(30).WithMessage("El campo {PropertyName} debe tener como máximo 30 caracteres en el insertar atributo");

        RuleFor(p => p.CodigoCuentaContableOrdenDeudor)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido en el insertar atributo");

        RuleFor(p => p.CodigoCuentaContableOrdenAcreedor)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido en el insertar atributo");

        RuleFor(p => p.CuentaCorrienteDetraccion)
            .MaximumLength(20).WithMessage("El campo {PropertyName} debe tener como máximo 20 caracteres en el insertar atributo");

        RuleFor(p => p.CantidadDiasPlazoFacturaVencida)
            .GreaterThanOrEqualTo(0);

        RuleFor(p => p.PorcentajeIndicadorVenta)
            .Cascade(CascadeMode.Stop)
            .GreaterThanOrEqualTo(0)
            .PrecisionScale(6, 4, true);
    }
}
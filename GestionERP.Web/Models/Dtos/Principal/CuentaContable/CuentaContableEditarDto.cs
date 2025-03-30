using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class CuentaContableEditarDto
{   
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public string FlagTipo { get; set; }
    public string FlagFormatoFuncion { get; set; }
    public string FlagTipoNaturaleza { get; set; }
    public string FlagTipoEntidadFinanciera { get; set; }
    public string FlagTipoCuentaCorriente { get; set; }
    public bool ExigeEntidadComprobante { get; set; }
    public bool ExigeCentroCosto { get; set; }
    public bool ExigeMonedaExtranjera { get; set; }
    public bool ExigeCuentaExistencia { get; set; }
    public string CodigoTipoBienServicio { get; set; }
    public string CodigoCuentaBalance { get; set; }
    public string CodigoCasillaBalance { get; set; }
    public string CodigoCasillaBalanceDetalle { get; set; }
    public bool EsCuentaDestino { get; set; }
    public bool EsCuentaOrden { get; set; }
    public bool EsCuentaVinculoBanco { get; set; }
    public bool Activo { get; set; }
    public List<CuentaContableDetalleEliminarDto> DetallesEliminar { get; set; } = [];
    public List<CuentaContableDetalleEditarDto> DetallesEditar { get; set; } = [];
    public List<CuentaContableDetalleInsertarDto> DetallesInsertar { get; set; } = [];
    public List<CuentaContableDestinoEliminarDto> DestinosEliminar { get; set; } = [];
    public List<CuentaContableDestinoEditarDto> DestinosEditar { get; set; } = [];
    public List<CuentaContableDestinoInsertarDto> DestinosInsertar { get; set; } = [];  
}

public class CuentaContableEditarValidator : AbstractValidator<CuentaContableEditarDto>
{
    public CuentaContableEditarValidator()
    {
        RuleFor(p => p.Nombre)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres")
            .Matches(@"^[^""!@$%^&*(){}:;<>,?/+_=|'~\\]*$").WithMessage("El campo {PropertyName} no debe contener caracteres especiales");
    
        RuleFor(p => p.Descripcion)
            .MaximumLength(250).WithMessage("El campo {PropertyName} debe tener como máximo 250 caracteres");

        RuleFor(p => p.FlagTipo)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.FlagFormatoFuncion)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.FlagTipoNaturaleza)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.FlagTipoEntidadFinanciera)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.FlagTipoCuentaCorriente)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");
    }
}
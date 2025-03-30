using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Importacion;

public class OrdenInsertarDto
{
    public string FlagOrigen { get; set; }
    public string CodigoEntidad { get; set; }
    public string CodigoDocumento { get; set; }
    public string CodigoSerieDocumento { get; set; } 
    public DateTime? FechaEmision { get; set; }
    public string CodigoMoneda { get; set; }
    public string CodigoTipoImportacion { get; set; }
    public string CodigoAduana { get; set; }  
    public DateTime? FechaEstimadaETD { get; set; } 
    public DateTime? FechaEstimadaETA { get; set; } 
    public DateTime? FechaReposicionStock { get; set; }
    public string CodigoPaisOrigen { get; set; } 
    public string CodigoPaisProcedencia { get; set; } 
    public decimal? TotalImporteBruto { get; set; }
    public string CodigoModoPago { get; set; }
    public string CodigoPlazoCredito { get; set; }
    public string DescripcionLugarEntrega { get; set; }
    public string FlagMedioPago { get; set; }
    public string DescripcionFormaPago { get; set; }
    public string Observacion { get; set; }
    public List<OrdenDetalleInsertarDto> Detalles { get; set; } = [];
    public List<OrdenNotaInsertarDto> Notas { get; set; } = [];
}

public class OrdenInsertarValidator : AbstractValidator<OrdenInsertarDto>
{
    public string MsgErrorEntidad { get; set; }
    public string MsgErrorPaisOrigen { get; set; }
    public string MsgErrorPaisProcedencia { get; set; }
    public string MsgErrorTipoImportacion { get; set; }
    public string MsgErrorAduana { get; set; }

    public OrdenInsertarValidator()
    {  
        RuleFor(p => p.CodigoEntidad)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos")
            .Must(x => string.IsNullOrEmpty(MsgErrorEntidad)).WithMessage(x => MsgErrorEntidad);

		RuleFor(p => p.CodigoDocumento).NotEmpty().WithMessage("Es necesario que seleccione la serie del documento");

		RuleFor(p => p.FechaEmision).NotNull().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.CodigoMoneda).NotEmpty().WithMessage("El campo {PropertyName} es requerido");
        
        RuleFor(p => p.CodigoAduana)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .Matches("^[0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres numéricos")
            .Must(x => string.IsNullOrEmpty(MsgErrorAduana)).WithMessage(x => MsgErrorAduana);

        RuleFor(p => p.CodigoTipoImportacion)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .Matches("^[A-Za-z]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfabéticos")
            .Must(x => string.IsNullOrEmpty(MsgErrorTipoImportacion)).WithMessage(x => MsgErrorTipoImportacion);
        
        RuleFor(p => p.FechaEstimadaETA).NotNull().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.FechaEstimadaETD).NotNull().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.FechaReposicionStock).NotNull().WithMessage("El campo {PropertyName} es requerido");
        
        RuleFor(p => p.CodigoPaisOrigen)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .Matches("^[0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres numéricos")
            .Must(x => string.IsNullOrEmpty(MsgErrorPaisOrigen)).WithMessage(x => MsgErrorPaisOrigen);
        
        RuleFor(p => p.CodigoPaisProcedencia)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .Matches("^[0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres numéricos")
            .Must(x => string.IsNullOrEmpty(MsgErrorPaisProcedencia)).WithMessage(x => MsgErrorPaisProcedencia);

        RuleFor(p => p.DescripcionLugarEntrega).MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres");

        RuleFor(p => p.DescripcionFormaPago)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres");
        
        RuleFor(p => p.FlagMedioPago)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");
 
        RuleFor(p => p.Observacion).MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
    }
}
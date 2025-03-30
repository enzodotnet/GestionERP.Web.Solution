using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Importacion;

public class SolicitudEditarDto
{   
    public string CodigoEntidad { get; set; } 
    public DateTime? FechaEmision { get; set; } 
    public DateTime? FechaEstimadaETD { get; set; }
    public DateTime? FechaEstimadaETA { get; set; }
    public DateTime? FechaReposicionStock { get; set; }
    public string CodigoPaisOrigen { get; set; }
    public string CodigoPaisProcedencia { get; set; }
    public string FlagNivelPrioridad { get; set; }
    public string Observacion { get; set; }
    public string Motivo { get; set; }
    public List<SolicitudDetalleEliminarDto> DetallesEliminar { get; set; } = [];
    public List<SolicitudDetalleEditarDto> DetallesEditar { get; set; } = [];
    public List<SolicitudDetalleInsertarDto> DetallesInsertar { get; set; } = [];
}

public class SolicitudEditarValidator : AbstractValidator<SolicitudEditarDto>
{
    public string MsgErrorEntidad { get; set; }
    public string MsgErrorPaisOrigen { get; set; }
    public string MsgErrorPaisProcedencia { get; set; }

    public SolicitudEditarValidator()
    {  
        RuleFor(p => p.CodigoEntidad)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos")
            .Must(x => string.IsNullOrEmpty(MsgErrorEntidad)).WithMessage(x => MsgErrorEntidad);
         
        RuleFor(p => p.FlagNivelPrioridad)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");

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

        RuleFor(p => p.Observacion).MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");

        RuleFor(p => p.Motivo).MaximumLength(200).WithMessage("El campo {PropertyName} debe tener como máximo 200 caracteres");
    }
}
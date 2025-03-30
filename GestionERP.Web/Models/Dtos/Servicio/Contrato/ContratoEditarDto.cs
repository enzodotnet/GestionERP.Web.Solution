using FluentValidation; 

namespace GestionERP.Web.Models.Dtos.Servicio;

public class ContratoEditarDto
{
    public string CodigoEntidad { get; set; }
    public string CodigoTipoServicio { get; set; }
    public string CodigoTipoProvision { get; set; }
    public string CodigoTipoImpuesto { get; set; }
    public decimal? PorcentajeImpuesto { get; set; }
    public decimal? TotalImporteBruto { get; set; }
    public decimal? TotalImporteImpuesto { get; set; }
    public decimal? TotalImporteNeto { get; set; }
    public string CodigoModoPago { get; set; }
    public string CodigoPlazoCredito { get; set; }
    public DateTime? FechaInicio { get; set; }  
    public DateTime? FechaFin { get; set; }
    public string FlagIntervaloInforme { get; set; }
    public string CodigoUsuarioVerifica { get; set; } 
    public string CodigoUsuarioAutoriza { get; set; }
    public string Referencia { get; set; }
    public string FlagMedioPago { get; set; }
    public int? CantidadCuotas { get; set; }
    public bool EsGenerableDevengo { get; set; }
    public string CodigoTipoDevengo { get; set; }
    public string CodigoCentroCosto { get; set; }
    public string Observacion { get; set; }
    public List<ContratoDetalleEliminarDto> DetallesEliminar { get; set; } = [];
    public List<ContratoDetalleEditarDto> DetallesEditar { get; set; } = [];
    public List<ContratoDetalleInsertarDto> DetallesInsertar { get; set; } = [];
    public List<ContratoTerminoEliminarDto> TerminosEliminar { get; set; } = [];
    public List<ContratoTerminoEditarDto> TerminosEditar { get; set; } = [];
    public List<ContratoTerminoInsertarDto> TerminosInsertar { get; set; } = [];
    public List<ContratoCuotaEliminarDto> CuotasEliminar { get; set; } = [];
    public List<ContratoCuotaEditarDto> CuotasEditar { get; set; } = [];
    public List<ContratoCuotaInsertarDto> CuotasInsertar { get; set; } = [];
}

public class ContratoEditarValidator : AbstractValidator<ContratoEditarDto>
{
    public bool EsAfectoImpuesto { get; set; }
    public string MsgErrorEntidad { get; set; } 
    public string MsgErrorTipoServicio { get; set; }
    public string MsgErrorTipoProvision { get; set; }
    public string MsgErrorTipoImpuesto { get; set; }
    public string MsgErrorUsuarioVerifica { get; set; }
    public string MsgErrorUsuarioAutoriza { get; set; }
    public string MsgErrorCentroCosto { get; set; }
    public string MsgErrorTipoDevengo { get; set; }

    public ContratoEditarValidator()
    {    
        RuleFor(p => p.CodigoEntidad)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos")
            .Must(x => string.IsNullOrEmpty(MsgErrorEntidad)).WithMessage(x => MsgErrorEntidad);

        RuleFor(p => p.CodigoTipoProvision)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .Matches("^[A-Za-z]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfabéticos")
            .Must(x => string.IsNullOrEmpty(MsgErrorTipoProvision)).WithMessage(x => MsgErrorTipoProvision); 

        RuleFor(p => p.FechaInicio).NotNull().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.FechaFin).NotNull().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.Referencia).MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres");

        RuleFor(p => p.CantidadCuotas)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("El campo {PropertyName} es requerido")
            .GreaterThanOrEqualTo(1).WithMessage("El campo {PropertyName} debe ser mayor o igual a 1");

        When(p => p.EsGenerableDevengo, () => {
            RuleFor(p => p.CodigoTipoDevengo)
                .Cascade(CascadeMode.Stop) 
                .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
                .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos")
                .Must(x => string.IsNullOrEmpty(MsgErrorTipoDevengo)).WithMessage(x => MsgErrorTipoDevengo);
        });

        When(p => EsAfectoImpuesto, () => {
            RuleFor(p => p.CodigoTipoImpuesto)
                .Cascade(CascadeMode.Stop) 
                .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
                .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos")
                .Must(x => string.IsNullOrEmpty(MsgErrorTipoImpuesto)).WithMessage(x => MsgErrorTipoImpuesto);
        });

        RuleFor(p => p.CodigoTipoServicio)
            .Cascade(CascadeMode.Stop) 
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .Matches("^[0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres numéricos")
            .Must(x => string.IsNullOrEmpty(MsgErrorTipoServicio)).WithMessage(x => MsgErrorTipoServicio);

        RuleFor(p => p.CodigoUsuarioAutoriza)
            .Cascade(CascadeMode.Stop) 
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .Matches("^[0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres numéricos")
            .Must(x => string.IsNullOrEmpty(MsgErrorUsuarioAutoriza)).WithMessage(x => MsgErrorUsuarioAutoriza);

        RuleFor(p => p.CodigoUsuarioVerifica)
            .Cascade(CascadeMode.Stop) 
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .Matches("^[0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres numéricos")
            .Must(x => string.IsNullOrEmpty(MsgErrorUsuarioVerifica)).WithMessage(x => MsgErrorUsuarioVerifica); 

        RuleFor(p => p.CodigoCentroCosto)
            .Cascade(CascadeMode.Stop) 
            .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos")
            .Must(x => string.IsNullOrEmpty(MsgErrorCentroCosto)).WithMessage(x => MsgErrorCentroCosto);
                
        RuleFor(p => p.Observacion).MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
    }
}
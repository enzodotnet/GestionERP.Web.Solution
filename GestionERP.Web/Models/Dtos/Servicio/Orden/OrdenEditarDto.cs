using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Servicio;

public class OrdenEditarDto
{
    public string CodigoEntidad { get; set; }
    public string CodigoTipoProvision { get; set; }
    public string CodigoTipoImpuesto { get; set; }
    public decimal? PorcentajeImpuesto { get; set; }
    public decimal? TotalImporteBruto { get; set; }
    public decimal? TotalImporteImpuesto { get; set; }
    public decimal? TotalImporteNeto { get; set; }
    public string CodigoModoPago { get; set; }
    public string CodigoPlazoCredito { get; set; }
    public DateTime? FechaAtencion { get; set; }
    public string CodigoLocalAtencion { get; set; }
    public string DescripcionLugarAtencion { get; set; }
    public string FlagMedioPago { get; set; }
    public bool EsPagoAnticipado { get; set; }
    public bool EsVinculableProduccion { get; set; }
    public string CodigoCentroCosto { get; set; }
    public string Observacion { get; set; }
    public string Motivo { get; set; }
    public List<OrdenDetalleEliminarDto> DetallesEliminar { get; set; } = [];
    public List<OrdenDetalleEditarDto> DetallesEditar { get; set; } = [];
    public List<OrdenDetalleInsertarDto> DetallesInsertar { get; set; } = [];
}

public class OrdenEditarValidator : AbstractValidator<OrdenEditarDto>
{
    public bool EsAfectoImpuesto { get; set; }
    public string MsgErrorEntidad { get; set; }
    public string MsgErrorTipoProvision { get; set; }
    public string MsgErrorTipoImpuesto { get; set; }
    public string MsgErrorCentroCosto { get; set; }
    public string MsgErrorLocalAtencion { get; set; }
    public DateTime? FechaEmision { get; set; }

    public OrdenEditarValidator()
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

        When(p => EsAfectoImpuesto, () => {
            RuleFor(p => p.CodigoTipoImpuesto)
                .Cascade(CascadeMode.Stop) 
                .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
                .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos")
                .Must(x => string.IsNullOrEmpty(MsgErrorTipoImpuesto)).WithMessage(x => MsgErrorTipoImpuesto);
        });

        RuleFor(p => p.CodigoCentroCosto)
            .Cascade(CascadeMode.Stop) 
            .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos")
            .Must(x => string.IsNullOrEmpty(MsgErrorCentroCosto)).WithMessage(x => MsgErrorCentroCosto);

        RuleFor(p => p.CodigoLocalAtencion)
            .Cascade(CascadeMode.Stop) 
            .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos")
            .Must(x => string.IsNullOrEmpty(MsgErrorLocalAtencion)).WithMessage(x => MsgErrorLocalAtencion);
        
        RuleFor(p => p.FechaAtencion)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("El campo {PropertyName} es requerido")
            .GreaterThanOrEqualTo(x => FechaEmision).WithMessage("La fecha de atención debe ser mayor o igual a la fecha de emision");
        
        RuleFor(p => p.DescripcionLugarAtencion).MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres");
  
        RuleFor(p => p.Observacion).MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");

        RuleFor(p => p.Motivo).MaximumLength(200).WithMessage("El campo {PropertyName} debe tener como máximo 200 caracteres");
    }
}
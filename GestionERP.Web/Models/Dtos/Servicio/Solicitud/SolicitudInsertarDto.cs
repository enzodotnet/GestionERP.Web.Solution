using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Servicio;

public class SolicitudInsertarDto
{  
    public string CodigoEntidad { get; set; }
    public string ReferenciaProveedor { get; set; }
    public string CodigoDocumento { get; set; }
    public string CodigoSerieDocumento { get; set; }
    public DateTime? FechaEmision { get; set; }
    public DateTime? FechaAtencion { get; set; }
    public string CodigoLocalAtencion { get; set; }
    public string CodigoArea { get; set; }
    public string CodigoUsuarioSolicitante { get; set; }
    public string FlagNivelPrioridad { get; set; }
    public string Observacion { get; set; }
    public string Motivo { get; set; }
    public List<SolicitudDetalleInsertarDto> Detalles { get; set; } = [];
}

public class SolicitudInsertarValidator : AbstractValidator<SolicitudInsertarDto>
{
    public string MsgErrorEntidad { get; set; }
    public string MsgErrorLocalAtencion { get; set; }

    public SolicitudInsertarValidator()
    { 
        RuleFor(p => p.CodigoDocumento).NotEmpty().WithMessage("Es necesario que seleccione la serie del documento");

        RuleFor(p => p.CodigoEntidad)
            .Cascade(CascadeMode.Stop)
            .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos")
            .Must(x => string.IsNullOrEmpty(MsgErrorEntidad)).WithMessage(x => MsgErrorEntidad);

        RuleFor(p => p.CodigoLocalAtencion)
            .Cascade(CascadeMode.Stop) 
            .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos")
            .Must(x => string.IsNullOrEmpty(MsgErrorLocalAtencion)).WithMessage(x => MsgErrorLocalAtencion);

        RuleFor(p => p.FechaEmision).NotNull().WithMessage("Es necesario que seleccione la fecha de emisión.");
        
        RuleFor(p => p.ReferenciaProveedor).MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
        
        RuleFor(p => p.FechaAtencion)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("El campo {PropertyName} es requerido")
            .GreaterThanOrEqualTo(x => x.FechaEmision ?? new()).WithMessage("La fecha de atención debe ser mayor o igual a la fecha de emision");
        
        RuleFor(p => p.FlagNivelPrioridad)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.Observacion).MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");

        RuleFor(p => p.Motivo).MaximumLength(200).WithMessage("El campo {PropertyName} debe tener como máximo 200 caracteres");
    }
}
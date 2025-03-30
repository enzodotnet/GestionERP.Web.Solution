using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Compra;

public class SolicitudEditarDto
{  
    public string CodigoEntidad { get; set; }
    public string ReferenciaProveedor { get; set; } 
    public DateTime? FechaEntrega { get; set; } 
    public string CodigoLocalRecepcion { get; set; }
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
    public string MsgErrorLocalRecepcion { get; set; }
    public DateTime? FechaEmision { get; set; }

    public SolicitudEditarValidator()
    {
        RuleFor(p => p.CodigoEntidad)
            .Cascade(CascadeMode.Stop)
            .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos")
            .Must(x => string.IsNullOrEmpty(MsgErrorEntidad)).WithMessage(x => MsgErrorEntidad);

        RuleFor(p => p.CodigoLocalRecepcion)
            .Cascade(CascadeMode.Stop) 
            .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos")
            .Must(x => string.IsNullOrEmpty(MsgErrorLocalRecepcion)).WithMessage(x => MsgErrorLocalRecepcion);
            
        RuleFor(p => p.ReferenciaProveedor).MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
        
        RuleFor(p => p.FlagNivelPrioridad)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.FechaEntrega)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("El campo {PropertyName} es requerido")
            .GreaterThanOrEqualTo(x => FechaEmision).WithMessage("La fecha de entrega debe ser mayor o igual a la fecha de emision");
        
        RuleFor(p => p.Observacion).MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");

        RuleFor(p => p.Motivo).MaximumLength(200).WithMessage("El campo {PropertyName} debe tener como máximo 200 caracteres");
    }
}
using FluentValidation;
using System.Text.Json.Serialization;

namespace GestionERP.Web.Models.Dtos.Produccion;

public class OrdenInsertarDto
{ 
    public string CodigoEmpresa { get; set; }
    public string FlagOrigen { get; set; }
    public string FlagTipoRegistro { get; set; }
    public string CodigoEntidad { get; set; }
    public string CodigoDocumento { get; set; }
    public string CodigoSerieDocumento { get; set; } 
    public DateTime? FechaEmision { get; set; }

    [JsonIgnore]
    public string CodigoPlan { get; set; }
    public string CodigoVersionPlan { get; set; }
    public decimal? Cantidad { get; set; } 
    public DateTime? FechaInicio { get; set; } 
    public DateTime? FechaTermino { get; set; } 
    public string CodigoSolicitudReferencia { get; set; }
    public string CodigoLocalRecepcion { get; set; }
    public string Observacion { get; set; }
    public string Motivo { get; set; } 
}

public class OrdenInsertarValidator : AbstractValidator<OrdenInsertarDto>
{
    public string MsgErrorVersionPlan { get; set; }
    public string MsgErrorPlan { get; set; }
    public string MsgErrorEntidad { get; set; }
    public string MsgErrorLocalRecepcion { get; set; } 
    public decimal? UnidadConversionArticulo { get; set; }

    public OrdenInsertarValidator()
    {  
        RuleFor(p => p.CodigoDocumento).NotEmpty().WithMessage("Es necesario que seleccione la serie del documento");

        RuleFor(p => p.FechaEmision).NotNull().WithMessage("El campo {PropertyName} es requerido");
 
        When(p => !string.IsNullOrEmpty(p.CodigoDocumento) && p.FlagOrigen is "D", () =>
        {
            RuleFor(p => p.CodigoPlan)
                .Cascade(CascadeMode.Stop)
                .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos")
                .Must(x => string.IsNullOrEmpty(MsgErrorPlan)).WithMessage(x => MsgErrorPlan);

            When(p => !string.IsNullOrEmpty(p.CodigoPlan), () =>
            {
                RuleFor(p => p.CodigoEntidad)
                   .Cascade(CascadeMode.Stop)
                   .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
                   .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos")
                   .Must(x => string.IsNullOrEmpty(MsgErrorEntidad)).WithMessage(x => MsgErrorEntidad);
            });
        });

        When(p => !string.IsNullOrEmpty(p.CodigoDocumento), () =>
        {
            When(p => !string.IsNullOrEmpty(p.CodigoPlan), () =>
            {
                RuleFor(p => p.CodigoVersionPlan)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
                    .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos")
                    .Must(x => string.IsNullOrEmpty(MsgErrorVersionPlan)).WithMessage(x => MsgErrorVersionPlan);
            })
            .Otherwise(() => {
                RuleFor(p => p.CodigoVersionPlan).NotEmpty().WithMessage("La versión es requerida, pero antes es necesario el plan de producción");
            });
        });

        When(p => p.FlagOrigen is "S", () =>
        {
            RuleFor(p => p.CodigoSolicitudReferencia).NotEmpty().WithMessage("Es necesario que seleccione una solicitud de producción");
        });

        RuleFor(p => p.Cantidad)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("El campo {PropertyName} es requerido")
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a 0")
            .PrecisionScale(10, 3, true).WithMessage("El campo {PropertyName} debe contener como máximo 10 dígitos incluyendo 3 decimales")
            .Must(x => !UnidadConversionArticulo.HasValue || (x % UnidadConversionArticulo) == 0).WithMessage("La cantidad no es múltiplo entre la unidad de conversión del artículo");

        RuleFor(p => p.FechaInicio)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("El campo {PropertyName} es requerido")
            .GreaterThanOrEqualTo(x => x.FechaEmision ?? new()).WithMessage("La fecha de inicio debe ser mayor o igual a la fecha de emision");

        RuleFor(p => p.FechaTermino)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("El campo {PropertyName} es requerido")
            .GreaterThanOrEqualTo(x => x.FechaInicio ?? new()).WithMessage("La fecha de termino debe ser mayor o igual a la fecha de inicio");
        
        RuleFor(p => p.CodigoLocalRecepcion)
            .Cascade(CascadeMode.Stop) 
            .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos")
            .Must(x => string.IsNullOrEmpty(MsgErrorLocalRecepcion)).WithMessage(x => MsgErrorLocalRecepcion);
 
        RuleFor(p => p.Observacion).MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
        
        RuleFor(p => p.Motivo).MaximumLength(200).WithMessage("El campo {PropertyName} debe tener como máximo 200 caracteres"); 
    }
}
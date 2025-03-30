using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Produccion;

public class SolicitudInsertarDto
{  
    public string FlagTipoRegistro { get; set; }
    public string CodigoEntidad { get; set; }
    public string CodigoDocumento { get; set; }
    public string CodigoSerieDocumento { get; set; }
    public DateTime? FechaEmision { get; set; }
    public string CodigoPlan { get; set; }
    public decimal? Cantidad { get; set; }
    private DateTime? fechaEntrega;
    public DateTime? FechaEntrega
    {
        get { return fechaEntrega?.Date; }
        set { fechaEntrega = value; }
    }
    public string CodigoLocalRecepcion { get; set; }
    public string CodigoArea { get; set; }
    public string CodigoUsuarioSolicitante { get; set; }
    public string FlagNivelPrioridad { get; set; }
    public string Observacion { get; set; }
    public string Motivo { get; set; }
}

public class SolicitudInsertarValidator : AbstractValidator<SolicitudInsertarDto>
{
	public string MsgErrorPlan { get; set; }
	public string MsgErrorEntidad { get; set; }
    public string MsgErrorLocalRecepcion { get; set; }
    public decimal? UnidadConversionArticulo { get; set; }

    public SolicitudInsertarValidator()
    { 
        RuleFor(p => p.CodigoDocumento).NotEmpty().WithMessage("Es necesario que seleccione la serie del documento");

        When(p => !string.IsNullOrEmpty(p.CodigoDocumento), () =>
        {
            RuleFor(p => p.CodigoPlan)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
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

        RuleFor(p => p.Cantidad)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("El campo {PropertyName} es requerido")
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a 0")
            .PrecisionScale(10, 3, true).WithMessage("El campo {PropertyName} debe contener como máximo 10 dígitos incluyendo 3 decimales")
            .Must(x => !UnidadConversionArticulo.HasValue || (x % UnidadConversionArticulo) == 0).WithMessage("La cantidad no es múltiplo entre la unidad de conversión del artículo");

        RuleFor(p => p.CodigoLocalRecepcion)
            .Cascade(CascadeMode.Stop) 
            .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos")
            .Must(x => string.IsNullOrEmpty(MsgErrorLocalRecepcion)).WithMessage(x => MsgErrorLocalRecepcion);

        RuleFor(p => p.FechaEmision)
            .NotNull().WithMessage("Es necesario que seleccione la fecha de emisión.");

        RuleFor(p => p.FechaEntrega)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("El campo {PropertyName} es requerido")
            .GreaterThanOrEqualTo(x => x.FechaEmision ?? new()).WithMessage("La fecha de entrega debe ser mayor o igual a la fecha de emision");

        RuleFor(p => p.FlagNivelPrioridad)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.Observacion).MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");

        RuleFor(p => p.Motivo).MaximumLength(200).WithMessage("El campo {PropertyName} debe tener como máximo 200 caracteres");
    }
}
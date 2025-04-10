using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Almacen;

public class MovimientoInsertarDto
{
    public string FlagTipoRegistro { get; set; } 
    public string CodigoOperacionLogistica { get; set; }
    private DateTime? fechaHoraOperacion;
    public DateTime? FechaHoraOperacion { get => fechaHoraOperacion; set => fechaHoraOperacion = value?.Date.Add(DateTime.Now.TimeOfDay); }
    private string codigoEntidad; 
    public string CodigoEntidad { get => codigoEntidad; set => codigoEntidad = value?.TrimEnd(); }
    public string CodigoLocal { get; set; }
    public string CodigoAlmacenDestino { get; set; }
    public string CodigoMoneda { get; set; }
    public string CodigoTipoCambioDia { get; set; }
    public decimal? MontoTipoCambioDia { get; set; }
    public decimal? TotalValorCostoMN { get; set; }
    public decimal? TotalValorCostoME { get; set; }
    public string CodigoDocumentoReferencia { get; set; }
    public string SerieDocumentoReferencia { get; set; }
    public string NumeroDocumentoReferencia { get; set; }
    private string codigoCentroCosto;
    public string CodigoCentroCosto { get => codigoCentroCosto; set => codigoCentroCosto = value?.TrimEnd(); }
    public string Observacion { get; set; }
    public string Comentario { get; set; }
    public List<MovimientoDetalleInsertarDto> Detalles { get; set; } = [];
}

public class MovimientoInsertarValidator : AbstractValidator<MovimientoInsertarDto>
{
    public bool EsRequeridoReferencia { get; set; }
    public string MsgErrorLocal { get; set; }
    public string MsgErrorEntidad { get; set; }
    public string MsgErrorCentroCosto { get; set; }
    
    public MovimientoInsertarValidator()
    {
        RuleFor(p => p.CodigoOperacionLogistica).NotEmpty().WithMessage("Es necesario que seleccione la operación logística");

        RuleFor(p => p.CodigoEntidad)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos")
            .Must(x => string.IsNullOrEmpty(MsgErrorEntidad)).WithMessage(x => MsgErrorEntidad);

        RuleFor(p => p.CodigoLocal)
            .Cascade(CascadeMode.Stop)
             .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos")
            .Must(x => string.IsNullOrEmpty(MsgErrorLocal)).WithMessage(x => MsgErrorLocal);
         
        RuleFor(p => p.CodigoMoneda).NotEmpty().WithMessage("El campo {PropertyName} es requerido"); 

        RuleFor(p => p.FechaHoraOperacion).NotNull().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.Observacion).MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");

        When(p => p.FlagTipoRegistro == "T", () => { 
            RuleFor(p => p.CodigoAlmacenDestino).NotEmpty().WithMessage("El campo {PropertyName} es requerido para generar la transferencia"); 
        });

        RuleFor(p => p.CodigoCentroCosto)
            .Cascade(CascadeMode.Stop)
            .Matches(@"^[a-zA-Z0-9]+(-?[a-zA-Z0-9]+)*$").WithMessage("El centro de costo solo debe contener caracteres alfanuméricos y opcionalmente solo un guión entre ellos.")
            .Must(x => string.IsNullOrEmpty(MsgErrorCentroCosto)).WithMessage(x => MsgErrorCentroCosto);

        When(p => EsRequeridoReferencia, () => {
            RuleFor(p => p.CodigoDocumentoReferencia).NotEmpty().WithMessage("Es necesario el documento referencia") ;
        });
    }
}
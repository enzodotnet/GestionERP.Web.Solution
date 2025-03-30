using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Importacion;

public class PedidoEditarDto
{ 
    public string DescripcionFormaPago { get; set; }
    public DateTime? FechaCosto { get; set; }
    public string CodigoTipoCambioDia { get; set; }
    public decimal? MontoTipoCambioDia { get; set; }
    public string NumeroPoliza { get; set; }
    public DateTime? FechaEmisionPoliza { get; set; }
    public string NumeroDUA { get; set; }
    public DateTime? FechaEmisionDUA { get; set; }
    public DateTime? FechaCancelacionDUA { get; set; }
    public string ComentarioDUA { get; set; }
    public string FlagCanal { get; set; }
    public string NumeroBL { get; set; }
    public DateTime? FechaEmisionBL { get; set; }
    public string NumeroInvoice { get; set; }
    public DateTime? FechaEmisionInvoice { get; set; }
    public int? CantidadDiasVenceInvoice { get; set; }
    public DateTime? FechaVencimientoInvoice { get; set; }
    public decimal? TotalImporteBruto { get; set; }
	public decimal? TotalImporteNeto { get; set; }
	public bool EsAfectoLiberaArancel { get; set; }
    public bool EsPrevioAforo { get; set; }
    public bool EsSujetoReclamo { get; set; }
    public string ComentarioReclamo { get; set; }
    public DateTime? FechaPackList { get; set; }
    public string NumeroPackList { get; set; }
    public string Referencia { get; set; }
    public string Observacion { get; set; }
    public string Comentario { get; set; }
    public string DescripcionMarca { get; set; }
    public DateTime? FechaCopiaDocumento { get; set; }
    public DateTime? FechaOriginalDocumento { get; set; }
    public string ComentarioDocumentacion { get; set; }
    public int? CantidadDiasSobreestadia { get; set; }
    public int? CantidadDiasAlmacenaje { get; set; }
    public DateTime? FechaDireccionamiento { get; set; }
    public DateTime? FechaEntregaDocumento { get; set; }
    public DateTime? FechaVistoBueno { get; set; }
    public string CodigoSituacionImportacion { get; set; } 
    public DateTime? FechaEmbarque { get; set; } 
    public string DescripcionTramiteAduana { get; set; }
    public string DescripcionModalidad { get; set; }
    public string CodigoRegimenImportacion { get; set; } 
    public DateTime? FechaSobreestadia { get; set; }
    public DateTime? FechaLugarAlmacenaje { get; set; }
    public DateTime? FechaRecojoDocumentoAduana { get; set; }
    public string CreditoAgenteAduana { get; set; }
    public DateTime? FechaPagoAgenteAduana { get; set; }
    public string NumeroPresupuesto { get; set; }
    public DateTime? FechaAforoCanal { get; set; }
    public DateTime? FechaLevanteCanal { get; set; }
    public bool EsRequeridoInspeccion { get; set; }
    public DateTime? FechaInspeccion { get; set; }
    public DateTime? FechaLevanteInspeccion { get; set; }
    public string FlagTipoFinanciamiento { get; set; }
    public DateTime? FechaPagoFinanciamiento { get; set; }
    public string NumeroManifiesto { get; set; }
    public DateTime? FechaLlegada { get; set; }
    public DateTime? FechaDescarga { get; set; }
    public DateTime? FechaTarja { get; set; }
    public DateTime? FechaDesgloseCarga { get; set; }
    public DateTime? FechaEstimadaArribo { get; set; }
    public string NombreTipoEmbarque { get; set; }
    public string DescripcionEmpaque { get; set; }
    public bool EsTransbordo { get; set; }
    public string FlagModalidadEmbarque { get; set; } 
    public string CodigoPuertoEmbarque { get; set; }  
    public string CodigoPuertoLlegada { get; set; }
    public string CodigoTransporteImportacion { get; set; }
    public string FlagViaTransporte { get; set; }
    public string NombreConsolidador { get; set; }
    public string CodigoEntidadAgenteAduana { get; set; }
    public string CodigoEntidadMuelle { get; set; }
    public string CodigoEntidadAgenciaCarga { get; set; }
    public string CodigoEntidadTerminal { get; set; }
    public string CodigoEntidadAlmacen  { get; set; }
    public string CodigoEntidadNaviera { get; set; }
    public string NombreNave { get; set; }
    public string DescripcionTransporte { get; set; }
    public string DescripcionLocalDestino { get; set; }
    public bool EsVentaSucesiva { get; set; }
    public List<PedidoDetalleEliminarDto> DetallesEliminar { get; set; } = [];
    public List<PedidoDetalleEditarDto> DetallesEditar { get; set; } = [];
    public List<PedidoDetalleInsertarDto> DetallesInsertar { get; set; } = [];
}

public class PedidoEditarValidator : AbstractValidator<PedidoEditarDto>
{
    public string MsgErrorEntidadAgenteAduana { get; set; }
    public string MsgErrorEntidadMuelle { get; set; }
    public string MsgErrorEntidadAgenciaCarga { get; set; }
    public string MsgErrorEntidadTerminal { get; set; }
    public string MsgErrorEntidadAlmacen { get; set; }
    public string MsgErrorEntidadNaviera { get; set; }
    public string MsgErrorTransporte { get; set; }
    public string MsgErrorRegimen { get; set; }
    public string MsgErrorSituacion { get; set; }
    public string MsgErrorPuertoEmbarque { get; set; }
    public string MsgErrorPuertoLlegada { get; set; }

    public PedidoEditarValidator()
    {
        RuleFor(p => p.CodigoPuertoEmbarque)
            .Cascade(CascadeMode.Stop)
            .Matches("^[A-Za-z]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfabéticos")
            .Must(x => string.IsNullOrEmpty(MsgErrorPuertoEmbarque)).WithMessage(x => MsgErrorPuertoEmbarque);

        RuleFor(p => p.CodigoPuertoLlegada)
            .Cascade(CascadeMode.Stop)
            .Matches("^[A-Za-z]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfabéticos")
            .Must(x => string.IsNullOrEmpty(MsgErrorPuertoLlegada)).WithMessage(x => MsgErrorPuertoLlegada);

        RuleFor(p => p.CodigoTransporteImportacion)
            .Cascade(CascadeMode.Stop)
            .Matches("^[0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres numéricos")
            .Must(x => string.IsNullOrEmpty(MsgErrorTransporte)).WithMessage(x => MsgErrorTransporte);

        RuleFor(p => p.CodigoRegimenImportacion)
            .Cascade(CascadeMode.Stop)
            .Matches("^[0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres numéricos")
            .Must(x => string.IsNullOrEmpty(MsgErrorRegimen)).WithMessage(x => MsgErrorRegimen);

        RuleFor(p => p.CodigoSituacionImportacion)
            .Cascade(CascadeMode.Stop)
            .Matches("^[A-Za-z]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfabéticos")
            .Must(x => string.IsNullOrEmpty(MsgErrorSituacion)).WithMessage(x => MsgErrorSituacion);

        RuleFor(p => p.CodigoEntidadAgenteAduana)
            .Cascade(CascadeMode.Stop)
            .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos")
            .Must(x => string.IsNullOrEmpty(MsgErrorEntidadAgenteAduana)).WithMessage(x => MsgErrorEntidadAgenteAduana);

        RuleFor(p => p.CodigoEntidadMuelle)
            .Cascade(CascadeMode.Stop)
            .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos")
            .Must(x => string.IsNullOrEmpty(MsgErrorEntidadMuelle)).WithMessage(x => MsgErrorEntidadMuelle);

        RuleFor(p => p.CodigoEntidadAgenciaCarga)
            .Cascade(CascadeMode.Stop)
            .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos")
            .Must(x => string.IsNullOrEmpty(MsgErrorEntidadAgenciaCarga)).WithMessage(x => MsgErrorEntidadAgenciaCarga);

        RuleFor(p => p.CodigoEntidadTerminal)
            .Cascade(CascadeMode.Stop)
            .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos")
            .Must(x => string.IsNullOrEmpty(MsgErrorEntidadTerminal)).WithMessage(x => MsgErrorEntidadTerminal);

        RuleFor(p => p.CodigoEntidadAlmacen)
            .Cascade(CascadeMode.Stop)
            .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos")
            .Must(x => string.IsNullOrEmpty(MsgErrorEntidadAlmacen)).WithMessage(x => MsgErrorEntidadAlmacen);

        RuleFor(p => p.CodigoEntidadNaviera)
            .Cascade(CascadeMode.Stop)
            .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos")
            .Must(x => string.IsNullOrEmpty(MsgErrorEntidadNaviera)).WithMessage(x => MsgErrorEntidadNaviera);
  
        When(p => p.EsSujetoReclamo, () => {
            RuleFor(p => p.ComentarioReclamo)
                .NotEmpty().WithMessage("Es necesario que especifique alguún comentario del reclamo."); 
        }).Otherwise(() => {
            RuleFor(p => p.ComentarioReclamo)
                .Empty().WithMessage("Al no estar sujeto a reclamo no es necesario el comentario del reclamo."); 
        });

        RuleFor(p => p.Referencia).MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");

        RuleFor(p => p.DescripcionFormaPago).MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");

        RuleFor(p => p.NumeroPoliza).MaximumLength(25).WithMessage("El campo {PropertyName} debe tener como máximo 25 caracteres");

        RuleFor(p => p.NumeroDUA).MaximumLength(25).WithMessage("El campo {PropertyName} debe tener como máximo 25 caracteres");

        RuleFor(p => p.NumeroBL).MaximumLength(25).WithMessage("El campo {PropertyName} debe tener como máximo 25 caracteres");

        RuleFor(p => p.NumeroInvoice).MaximumLength(25).WithMessage("El campo {PropertyName} debe tener como máximo 25 caracteres");
        
        RuleFor(p => p.CantidadDiasVenceInvoice).GreaterThanOrEqualTo(0).WithMessage("El campo {PropertyName} debe ser mayor o igual a 0");
        
        RuleFor(p => p.NumeroPackList).MaximumLength(25).WithMessage("El campo {PropertyName} debe tener como máximo 25 caracteres");

        RuleFor(p => p.DescripcionMarca).MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres");

        RuleFor(p => p.CantidadDiasAlmacenaje).GreaterThanOrEqualTo(0).WithMessage("El campo {PropertyName} debe ser mayor o igual a 0");

        RuleFor(p => p.CantidadDiasSobreestadia).GreaterThanOrEqualTo(0).WithMessage("El campo {PropertyName} debe ser mayor o igual a 0");
        
        RuleFor(p => p.DescripcionTramiteAduana).MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres");

        RuleFor(p => p.DescripcionModalidad).MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres");

        RuleFor(p => p.CreditoAgenteAduana).MaximumLength(25).WithMessage("El campo {PropertyName} debe tener como máximo 25 caracteres");

        RuleFor(p => p.NumeroPresupuesto).MaximumLength(20).WithMessage("El campo {PropertyName} debe tener como máximo 20 caracteres");

        RuleFor(p => p.NumeroManifiesto).MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres");

        RuleFor(p => p.NombreTipoEmbarque).MaximumLength(20).WithMessage("El campo {PropertyName} debe tener como máximo 20 caracteres");
        
        RuleFor(p => p.DescripcionEmpaque).MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres");

        RuleFor(p => p.NombreConsolidador).MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres");

        RuleFor(p => p.NombreNave).MaximumLength(40).WithMessage("El campo {PropertyName} debe tener como máximo 40 caracteres");

        RuleFor(p => p.DescripcionTransporte).MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres");

        RuleFor(p => p.DescripcionLocalDestino).MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
        
        RuleFor(p => p.Observacion).MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
    }
}
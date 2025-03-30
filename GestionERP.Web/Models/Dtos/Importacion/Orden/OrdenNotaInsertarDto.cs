using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Importacion;

public class OrdenNotaInsertarDto
{
    public string CodigoNotaReporteOrden { get; set; }
    public string Contenido { get; set; }
}

public class OrdenNotaInsertarValidator : AbstractValidator<OrdenNotaInsertarDto>
{
    public string MsgErrorNotaReporteOrden { get; set; }
    
    public OrdenNotaInsertarValidator()
    {
        RuleFor(p => p.CodigoNotaReporteOrden)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .Matches("^[0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres numéricos")
            .Must(x => string.IsNullOrEmpty(MsgErrorNotaReporteOrden)).WithMessage(x => MsgErrorNotaReporteOrden);
          
        RuleFor(p => p.Contenido).NotEmpty().WithMessage("El campo {PropertyName} es requerido");
    }
}
using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class CierreContableDetalleEliminarDto
{
    public Guid Id { get; set; }
}

public class CierreContableDetalleEliminarValidator : AbstractValidator<CierreContableDetalleEliminarDto>
{
    public CierreContableDetalleEliminarValidator()
    {
        RuleFor(p => p.Id).NotEmpty().WithMessage("El campo {PropertyName} es requerido en el eliminar Detalle del cierre contable");
    }
}
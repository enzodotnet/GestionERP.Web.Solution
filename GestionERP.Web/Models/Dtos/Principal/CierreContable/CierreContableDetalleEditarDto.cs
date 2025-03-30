using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class CierreContableDetalleEditarDto
{
    public Guid Id { get; set; }
    public string NumeroCuentaInicio { get; set; }
    public string NumeroCuentaFinal { get; set; }
    public int CuentaContableDestinoId { get; set; }
    public int NumeroOrden { get; set; }
}

public class CierreContableDetalleEditarValidator : AbstractValidator<CierreContableDetalleEditarDto>
{
    public CierreContableDetalleEditarValidator()
    {
        RuleFor(p => p.Id).NotEmpty().WithMessage("El campo {PropertyName} es requerido en el editar Detalle del cierre contable");

        RuleFor(p => p.NumeroCuentaInicio)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido en el editar Detalle del cierre contable")
            .MaximumLength(10).WithMessage("El campo {PropertyName} debe tener máximo 10 caracteres en el editar Detalle del cierre contable")
            .Matches("^[0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres numéricos en el editar Detalle del cierre contable");

        RuleFor(p => p.NumeroCuentaFinal)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido en el editar Detalle del cierre contable")
            .MaximumLength(10).WithMessage("El campo {PropertyName} debe tener máximo 10 caracteres en el editar Detalle del cierre contable")
            .Matches("^[0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres numéricos en el editar Detalle del cierre contable");

        RuleFor(p => p.CuentaContableDestinoId).NotEmpty().WithMessage("El campo {PropertyName} es requerido en el editar Detalle del cierre contable");

        RuleFor(p => p.NumeroOrden)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido en el editar Detalle del cierre contable")
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a cero en el editar Detalle del cierre contable");
    }
}
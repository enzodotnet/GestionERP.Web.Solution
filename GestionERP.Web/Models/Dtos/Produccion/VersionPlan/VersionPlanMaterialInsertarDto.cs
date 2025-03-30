using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Produccion;

public class VersionPlanMaterialInsertarDto
{ 
    public string FlagTipo { get; set; }
    public string CodigoArticulo { get; set; }
    public string FlagInsumo { get; set; }
    public decimal? Cantidad { get; set; }
    public string Observacion { get; set; }
}

public class VersionPlanMaterialInsertarValidator : AbstractValidator<VersionPlanMaterialInsertarDto>
{
    public VersionPlanMaterialInsertarValidator()
    {
        RuleFor(p => p.CodigoArticulo).NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.FlagTipo).NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.Cantidad)
            .NotNull().WithMessage("El campo {PropertyName} es requerido") 
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a 0")
            .PrecisionScale(10, 3, true).WithMessage("El campo {PropertyName} debe contener como máximo 10 dígitos incluyendo 3 decimales");

        RuleFor(p => p.FlagInsumo).NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.Observacion).MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
    }
}
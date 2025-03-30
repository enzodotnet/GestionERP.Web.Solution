using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class ArticuloInsertarDto
{ 
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public string CodigoUnidadMedida { get; set; } 
    public string FlagCategoria { get; set; } 
    public string CodigoLineaArticulo { get; set; } 
    public string CodigoFamiliaProducto { get; set; }
    public bool EsAfectoImpuesto { get; set; }
}

public class ArticuloInsertarValidator : AbstractValidator<ArticuloInsertarDto>
{
    public ArticuloInsertarValidator()
    { 
        RuleFor(p => p.Nombre)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");

        RuleFor(p => p.CodigoUnidadMedida).NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.FlagCategoria).NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.CodigoLineaArticulo).NotEmpty().WithMessage("El campo {PropertyName} es requerido");
 
        RuleFor(p => p.Descripcion)
            .MaximumLength(250).WithMessage("El campo {PropertyName} debe tener como máximo 250 caracteres");
    }
}
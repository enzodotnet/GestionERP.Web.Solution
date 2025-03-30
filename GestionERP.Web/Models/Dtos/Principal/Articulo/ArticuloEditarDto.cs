using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class ArticuloEditarDto
{  
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public string CodigoFamiliaProducto { get; set; }
    public bool EsAfectoImpuesto { get; set; }
    public bool Activo { get; set; }
}

public class ArticuloEditarValidator : AbstractValidator<ArticuloEditarDto>
{
    public ArticuloEditarValidator()
    {
        RuleFor(p => p.Nombre)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
 
        RuleFor(p => p.Descripcion)
            .MaximumLength(250).WithMessage("El campo {PropertyName} debe tener como máximo 250 caracteres");
    }
}
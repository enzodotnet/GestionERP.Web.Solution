using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class OperacionLogisticaInsertarDto
{ 
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public string CodigoTipoMovimiento { get; set; } 
    public string CodigoTipoOperacionAlmacen { get; set; }
    public string CodigoOperacionLogisticaDestino { get; set; }
}

public class OperacionLogisticaInsertarValidator : AbstractValidator<OperacionLogisticaInsertarDto>
{
    public OperacionLogisticaInsertarValidator()
    {
        RuleFor(p => p.Codigo)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .Length(3).WithMessage("El campo {PropertyName} debe tener 3 caracteres") 
            .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos");
 
        RuleFor(p => p.Nombre)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres");
         
        RuleFor(p => p.Descripcion)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");

        RuleFor(p => p.CodigoTipoMovimiento).NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.CodigoTipoOperacionAlmacen).NotEmpty().WithMessage("El campo {PropertyName} es requerido");
    }
}
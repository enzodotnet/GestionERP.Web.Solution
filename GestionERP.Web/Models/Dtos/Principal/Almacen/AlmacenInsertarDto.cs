using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class AlmacenInsertarDto
{ 
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public string FlagAtributo { get; set; }
    public string CodigoCuentaContable { get; set; }
    public string CodigoTipoAlmacen { get; set; }
    public bool EsDestinadoVenta { get; set; }
    public bool EsDestinadoCompra { get; set; }
    public bool EsDestinadoProduccion { get; set; }
    public bool EsDestinadoTraslado { get; set; }
    public bool EsDestinadoControl { get; set; } 
}

public class AlmacenInsertarValidator : AbstractValidator<AlmacenInsertarDto>
{
    public AlmacenInsertarValidator()
    {
        RuleFor(p => p.Codigo)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .Length(3).WithMessage("El campo {PropertyName} debe tener 3 caracteres") 
            .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos");
 
        RuleFor(p => p.Nombre)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
         
        RuleFor(p => p.Descripcion)
            .MaximumLength(250).WithMessage("El campo {PropertyName} debe tener como máximo 250 caracteres");

        RuleFor(p => p.FlagAtributo)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");
    }
}
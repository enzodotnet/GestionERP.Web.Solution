using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class AlmacenEditarDto
{  
    public string Nombre { get; set; }
    public string Descripcion { get; set; } 
    public string CodigoCuentaContable { get; set; }
    public string CodigoTipoAlmacen { get; set; }
    public bool EsDestinadoVenta { get; set; }
    public bool EsDestinadoCompra { get; set; }
    public bool EsDestinadoProduccion { get; set; }
    public bool EsDestinadoTraslado { get; set; }
    public bool EsDestinadoControl { get; set; }
    public bool Activo { get; set; } 
}

public class AlmacenEditarValidator : AbstractValidator<AlmacenEditarDto>
{
    public AlmacenEditarValidator()
    {
        RuleFor(p => p.Nombre)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
         
        RuleFor(p => p.Descripcion)
            .MaximumLength(250).WithMessage("El campo {PropertyName} debe tener como máximo 250 caracteres"); 
    }
}
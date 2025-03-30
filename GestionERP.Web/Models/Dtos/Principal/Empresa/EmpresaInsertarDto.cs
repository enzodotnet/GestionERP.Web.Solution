using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class EmpresaInsertarDto
{ 
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string CodigoTipoIdentificacion { get; set; } 
    public string NumeroTipoIdentificacion { get; set; }
    public string CodigoWeb { get; set; }
    public string Acronimo { get; set; } 
    public string Descripcion { get; set; }
    public DateTime? FechaInicioSistema { get; set; }
    public EmpresaAtributoInsertarDto Atributo { get; set; }
    public EmpresaFacturacionInsertarDto Facturacion { get; set; }
}

public class EmpresaInsertarValidator : AbstractValidator<EmpresaInsertarDto>
{
    public EmpresaInsertarValidator()
    {
        RuleFor(p => p.Codigo)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(2).WithMessage("El campo {PropertyName} debe tener como máximo 2 caracteres") 
            .Matches("^[0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres numericos");
        
        RuleFor(p => p.Nombre)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(200).WithMessage("El campo {PropertyName} debe tener como máximo 200 caracteres")
            .Matches(@"^[^""!@$%^&*{}:;<>?/+_=|~\\]*$").WithMessage("El campo {PropertyName} no debe contener caracteres especiales");

        RuleFor(p => p.CodigoTipoIdentificacion).NotEmpty().WithMessage("El campo {PropertyName} es requerido");
 
        RuleFor(p => p.NumeroTipoIdentificacion)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(11).WithMessage("El campo {PropertyName} debe tener como máximo 11 caracteres") 
            .Matches("^[0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres numéricos");

        RuleFor(p => p.CodigoWeb)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Es necesario que ingrese la sección del módulo en el menú")
            .MaximumLength(15).WithMessage("El campo {PropertyName} debe tener como máximo 15 caracteres")
            .Matches("^[a-z]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfabético en minúsculas")
            .Matches(@"^[^""!@$%^&*(){}:;<>,.?/+_=|'~\\-]*$").WithMessage("El campo {PropertyName} no debe contener caracteres especiales");

        RuleFor(p => p.Acronimo)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(15).WithMessage("El campo {PropertyName} debe tener como máximo 15 caracteres")
            .Matches(@"^[^""!@$%^&*{}:;<>?/+_=|~\\]*$").WithMessage("El campo {PropertyName} no debe contener caracteres especiales");

        RuleFor(p => p.Descripcion)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
        
        RuleFor(p => p.FechaInicioSistema)
            .NotNull().WithMessage("El campo {PropertyName} es requerido");
 
        RuleFor(p => p.Atributo)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("La colección {PropertyName} es requerido")
            .SetValidator(new EmpresaAtributoInsertarValidator());

        RuleFor(p => p.Facturacion)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("La colección {PropertyName} es requerido")
            .SetValidator(new EmpresaFacturacionInsertarValidator());  
    }
}
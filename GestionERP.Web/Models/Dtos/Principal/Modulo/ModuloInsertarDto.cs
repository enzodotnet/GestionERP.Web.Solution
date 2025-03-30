using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class ModuloInsertarDto
{ 
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public string FlagTipoAcceso { get; set; }
    public bool EsOperacional { get; set; }
    public string CodigoWeb { get; set; }
}

public class ModuloInsertarValidator : AbstractValidator<ModuloInsertarDto>
{
    public ModuloInsertarValidator()
    {
        RuleFor(p => p.Codigo)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Es necesario que ingrese el código de módulo")
            .Length(2).WithMessage("El código del módulo debe tener necesariamente 2 caracteres") 
            .Matches("^[0-9]*$").WithMessage("El código del módulo solo debe contener caracteres numéricos");  
        
        RuleFor(p => p.Nombre)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres")
            .Matches(@"^[^""!@$%^&*(){}:;<>,.?/+_=|'~\\-]*$").WithMessage("El campo {PropertyName} no debe contener caracteres especiales");
 
        RuleFor(p => p.FlagTipoAcceso).NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.Descripcion).MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres"); 

        RuleFor(p => p.CodigoWeb)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Es necesario que ingrese la sección del módulo en el menú")
            .MaximumLength(20).WithMessage("El campo {PropertyName} debe tener como máximo 20 caracteres")
            .Matches(@"^[^""!@$%^&*(){}:;<>,.?/+_=|'~\\-]*$").WithMessage("El campo {PropertyName} no debe contener caracteres especiales"); 
    }
}
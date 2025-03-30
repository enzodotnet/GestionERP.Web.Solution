using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class CentroCostoInsertarDto
{ 
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string Abreviacion { get; set; } 
    public string Descripcion { get; set; } 
}

public class CentroCostoInsertarValidator : AbstractValidator<CentroCostoInsertarDto>
{
    public CentroCostoInsertarValidator()
    {
        RuleFor(p => p.Codigo)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .Length(5,15).WithMessage("El campo {PropertyName} debe tener entre 5 a 15 caracteres") 
            .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos");
 
        RuleFor(p => p.Nombre)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
         
        RuleFor(p => p.Descripcion)
            .MaximumLength(200).WithMessage("El campo {PropertyName} debe tener como máximo 200 caracteres");
        
        RuleFor(p => p.Abreviacion)
            .MaximumLength(20).WithMessage("El campo {PropertyName} debe tener como máximo 20 caracteres");
    }
}
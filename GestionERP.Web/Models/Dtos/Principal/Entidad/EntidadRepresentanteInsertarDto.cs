using FluentValidation; 
namespace GestionERP.Web.Models.Dtos.Principal;

public class EntidadRepresentanteInsertarDto
{ 
    public string CodigoTipoIdentificacion { get; set; }
    public string NumeroTipoIdentificacion { get; set; } 
    public string Nombre { get; set; }
    public string DescripcionCargo { get; set; }
    public DateTime? FechaInicioCargo { get; set; }  
}

public class EntidadRepresentanteInsertarValidator : AbstractValidator<EntidadRepresentanteInsertarDto>
{
    public EntidadRepresentanteInsertarValidator()
    {
        RuleFor(p => p.CodigoTipoIdentificacion).NotEmpty().WithMessage("El campo {PropertyName} es requerido en el insertar representantes");
        
        RuleFor(p => p.NumeroTipoIdentificacion)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido en el insertar representantes")
            .MaximumLength(11).WithMessage("El campo {PropertyName} debe tener como máximo 11 caracteres en el insertar representantes")
            .Matches("^[0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres numericos en el insertar representantes");
         
        RuleFor(p => p.Nombre)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido en el insertar representantes")
            .MaximumLength(200).WithMessage("El campo {PropertyName} debe tener como máximo 200 caracteres en el insertar representantes");
         
        RuleFor(p => p.DescripcionCargo)
            .MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres en el insertar representantes");
    }
}
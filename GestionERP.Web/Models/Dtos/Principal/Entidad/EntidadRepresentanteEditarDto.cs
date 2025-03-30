using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class EntidadRepresentanteEditarDto
{  
    public Guid Id { get; set; }   
    public string Nombre { get; set; }
    public string DescripcionCargo { get; set; }
    public DateTime? FechaInicioCargo { get; set; }  
    public bool Activo { get; set; }
}

public class EntidadRepresentanteEditarValidator : AbstractValidator<EntidadRepresentanteEditarDto>
{
    public EntidadRepresentanteEditarValidator()
    { 
        RuleFor(p => p.Nombre)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido en el editar representantes")
            .MaximumLength(200).WithMessage("El campo {PropertyName} debe tener como máximo 200 caracteres en el editar representantes");
         
        RuleFor(p => p.DescripcionCargo)
            .MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres en el editar representantes");
    }
}
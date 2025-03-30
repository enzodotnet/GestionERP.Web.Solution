using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class EntidadVehiculoEditarDto
{ 
    public Guid Id { get; set; }
    public string Descripcion { get; set; } 
    public bool Activo { get; set; }
}

public class EntidadVehiculoEditarValidator : AbstractValidator<EntidadVehiculoEditarDto>
{
    public EntidadVehiculoEditarValidator()
    { 
        RuleFor(p => p.Descripcion).MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres en el editar vehículos transportista");
    }
}
using FluentValidation; 

namespace GestionERP.Web.Models.Dtos.Principal;

public class EntidadVehiculoInsertarDto
{ 
    public string CodigoVehiculo { get; set; }
    public string Descripcion { get; set; } 
}

public class EntidadVehiculoInsertarValidator : AbstractValidator<EntidadVehiculoInsertarDto>
{
    public EntidadVehiculoInsertarValidator()
    {
        RuleFor(p => p.CodigoVehiculo)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido en el insertar vehículos transportista");
        
        RuleFor(p => p.Descripcion)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres en el insertar vehículos transportista");
    }
}
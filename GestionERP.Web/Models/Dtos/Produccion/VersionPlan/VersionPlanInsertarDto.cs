using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Produccion;

public class VersionPlanInsertarDto
{ 
    public string CodigoEmpresa { get; set; } 
    public string CodigoPlan { get; set; } 
    public DateTime? FechaRegistro { get; set; }
    public string Descripcion { get; set; } 
    public string Observacion { get; set; }
    public List<VersionPlanMaterialInsertarDto> Materiales { get; set; } = [];
    public List<VersionPlanFuncionInsertarDto> Funciones { get; set; } = [];
    public List<VersionPlanEquipoInsertarDto> Equipos { get; set; } = [];
}

public class VersionPlanInsertarValidator : AbstractValidator<VersionPlanInsertarDto>
{
    public VersionPlanInsertarValidator()
    { 
        RuleFor(p => p.CodigoPlan)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");
         
        RuleFor(p => p.FechaRegistro).NotNull().WithMessage("El campo {PropertyName} es requerido");
  
        RuleFor(p => p.Descripcion)
            .MaximumLength(200).WithMessage("El campo {PropertyName} debe tener como máximo 200 caracteres");
 
        RuleFor(p => p.Observacion)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
    }
}
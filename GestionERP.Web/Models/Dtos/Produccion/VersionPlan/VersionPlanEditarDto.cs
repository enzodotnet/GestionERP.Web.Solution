using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Produccion;

public class VersionPlanEditarDto
{
    public string Descripcion { get; set; } 
    public string Observacion { get; set; }
    public List<VersionPlanMaterialEliminarDto> MaterialesEliminar { get; set; } = [];
    public List<VersionPlanMaterialEditarDto> MaterialesEditar { get; set; } = [];
    public List<VersionPlanMaterialInsertarDto> MaterialesInsertar { get; set; } = [];
    public List<VersionPlanFuncionEliminarDto> FuncionesEliminar { get; set; } = [];
    public List<VersionPlanFuncionEditarDto> FuncionesEditar { get; set; } = [];
    public List<VersionPlanFuncionInsertarDto> FuncionesInsertar { get; set; } = [];
    public List<VersionPlanEquipoEliminarDto> EquiposEliminar { get; set; } = [];
    public List<VersionPlanEquipoEditarDto> EquiposEditar { get; set; } = [];
    public List<VersionPlanEquipoInsertarDto> EquiposInsertar { get; set; } = [];
}

public class VersionPlanEditarValidator : AbstractValidator<VersionPlanEditarDto>
{
    public VersionPlanEditarValidator()
    { 
        RuleFor(p => p.Observacion)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
        
        RuleFor(p => p.Descripcion) 
            .MaximumLength(200).WithMessage("El campo {PropertyName} debe tener como máximo 200 caracteres");
    }
}
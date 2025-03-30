using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalPlanContable
{
    Task<IEnumerable<PlanContableListarDto>> Listar();
    Task<IEnumerable<PlanContableCatalogoDto>> Catalogo();
    Task<PlanContableObtenerDto> Obtener(Guid id);
}
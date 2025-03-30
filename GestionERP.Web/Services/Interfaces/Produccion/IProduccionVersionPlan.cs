using GestionERP.Web.Models.Dtos.Produccion;
using GestionERP.Web.Models.Requests;

namespace GestionERP.Web.Services.Interfaces;

public interface IProduccionVersionPlan
{
    Task<IEnumerable<VersionPlanListarDto>> Listar(string codigoEmpresa, string codigoPlan = null);
    Task<Guid> Insertar(string codigoEmpresa, VersionPlanInsertarDto versionPlan);
    Task Editar(string codigoEmpresa, Guid id, VersionPlanEditarDto versionPlan);
    Task<VersionPlanObtenerDto> Obtener(string codigoEmpresa, Guid id);
    Task Eliminar(string codigoEmpresa, Guid id);
    Task ActualizarEstado(string codigoEmpresa, EstadoActualizarRequest estadoActualizar);
    Task<IEnumerable<VersionPlanCatalogoDto>> Catalogo(string codigoEmpresa, string codigoPlan = null);
    Task<VersionPlanObtenerPorCodigoDto> ObtenerPorCodigo(string codigoEmpresa, string codigoVersionPlan, string codigoPlan = null);
    Task<IEnumerable<VersionPlanMaterialConsultaDto>> ConsultaMateriales(string codigoEmpresa, string codigoVersionPlan);
}
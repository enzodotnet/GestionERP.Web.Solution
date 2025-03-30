using GestionERP.Web.Models.Dtos.Produccion;
using GestionERP.Web.Models.Requests;

namespace GestionERP.Web.Services.Interfaces;

public interface IProduccionPlan
{
    Task<IEnumerable<PlanListarDto>> Listar(string codigoEmpresa);
    Task<Guid> Insertar(string codigoEmpresa, PlanInsertarDto plan);
    Task Editar(string codigoEmpresa, Guid id, PlanEditarDto plan);
    Task<PlanObtenerDto> Obtener(string codigoEmpresa, Guid id);
    Task Eliminar(string codigoEmpresa, Guid id);
    Task ActualizarEstado(string codigoEmpresa, EstadoActualizarRequest estadoActualizar);
    Task<IEnumerable<PlanCatalogoDto>> Catalogo(string codigoEmpresa, string flagTipoProceso = null);
    Task<PlanObtenerPorCodigoDto> ObtenerPorCodigo(string codigoEmpresa, string codigoPlan, string flagTipoProceso = null);
    Task<PlanConsultaPorCodigoDto> ConsultaPorCodigo(string codigoEmpresa, string codigoPlan);
}
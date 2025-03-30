using GestionERP.Web.Models.Dtos.Importacion;
using GestionERP.Web.Models.Requests;

namespace GestionERP.Web.Services.Interfaces;

public interface IImportacionPedido
{
    Task<IEnumerable<PedidoListarDto>> Listar(string codigoEmpresa, string codigoEjercicio, string codigoPeriodo = null);
    Task<Guid> Insertar(string codigoEmpresa, PedidoInsertarDto orden);
    Task Editar(string codigoEmpresa, Guid id, PedidoEditarDto orden);
    Task<PedidoObtenerDto> Obtener(string codigoEmpresa, Guid id);
    Task Eliminar(string codigoEmpresa, Guid id);
    Task ActualizarEstado(string codigoEmpresa, EstadoActualizarRequest estadoActualizar);
}
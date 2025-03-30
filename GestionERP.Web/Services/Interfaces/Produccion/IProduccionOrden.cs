using GestionERP.Web.Models.Dtos.Produccion;
using GestionERP.Web.Models.Requests;

namespace GestionERP.Web.Services.Interfaces;

public interface IProduccionOrden
{
    Task<IEnumerable<OrdenListarDto>> Listar(string codigoEmpresa, string codigoEjercicio, string codigoPeriodo = null, string flagTipoRegistro = null, string flagOrigen = null);
    Task<Guid> Insertar(string codigoEmpresa, OrdenInsertarDto orden);
    Task Editar(string codigoEmpresa, Guid id, OrdenEditarDto orden);
    Task<OrdenObtenerDto> Obtener(string codigoEmpresa, Guid id);
    Task Eliminar(string codigoEmpresa, Guid id);
    Task<IEnumerable<OrdenCatalogoActualizarEstadoDto>> CatalogoActualizarEstado(string codigoEmpresa, string codigoEstado);
    Task ActualizarEstado(string codigoEmpresa, EstadoActualizarRequest estadoActualizar);
    Task ActualizarComentarioProceso(string codigoEmpresa, OrdenActualizarComentarioProcesoDto orden);
    Task ActualizarFechaCosto(string codigoEmpresa, OrdenActualizarFechaCostoDto orden);
    Task<IEnumerable<OrdenLoteListarDto>> ListarLotes(string codigoEmpresa, string codigoOrden);
    Task InsertarLote(string codigoEmpresa, OrdenLoteInsertarDto lote);
    Task EditarLote(string codigoEmpresa, Guid id, OrdenLoteEditarDto lote);
    Task EliminarLote(string codigoEmpresa, Guid id);
    Task<IEnumerable<OrdenMaquilaListarDto>> ListarMaquilas(string codigoEmpresa, string codigoOrden);
    Task InsertarMaquila(string codigoEmpresa, OrdenMaquilaInsertarDto maquila);
    Task EditarMaquila(string codigoEmpresa, Guid id, OrdenMaquilaEditarDto maquila);
    Task EliminarMaquila(string codigoEmpresa, Guid id);
    Task<IEnumerable<OrdenTareoListarDto>> ListarTareos(string codigoEmpresa, string codigoOrden);
    Task InsertarTareo(string codigoEmpresa, OrdenTareoInsertarDto tareo);
    Task EditarTareo(string codigoEmpresa, Guid id, OrdenTareoEditarDto tareo);
    Task EliminarTareo(string codigoEmpresa, Guid id);
}
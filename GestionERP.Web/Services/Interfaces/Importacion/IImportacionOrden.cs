using GestionERP.Web.Models.Dtos.Importacion;
using GestionERP.Web.Models.Requests;

namespace GestionERP.Web.Services.Interfaces;

public interface IImportacionOrden
{
    Task<IEnumerable<OrdenListarDto>> Listar(string codigoEmpresa, string codigoEjercicio, string codigoPeriodo = null, string flagRegistro = null);
    Task<Guid> Insertar(string codigoEmpresa, OrdenInsertarDto orden);
    Task Editar(string codigoEmpresa, Guid id, OrdenEditarDto orden);
    Task<OrdenObtenerDto> Obtener(string codigoEmpresa, Guid id);
    Task Eliminar(string codigoEmpresa, Guid id);
    Task<IEnumerable<OrdenCatalogoActualizarEstadoDto>> CatalogoActualizarEstado(string codigoEmpresa, string codigoEstado);
    Task ActualizarEstado(string codigoEmpresa, EstadoActualizarRequest estadoActualizar);
    Task<IEnumerable<OrdenCatalogoAtenderDto>> CatalogoAtender(string codigoEmpresa, string codigoEjercicio);
    Task<IEnumerable<OrdenDetalleCatalogoAtenderDto>> CatalogoDetallesAtender(string codigoEmpresa, string codigoOrden);
}
using GestionERP.Web.Models.Dtos.Compra;
using GestionERP.Web.Models.Requests;

namespace GestionERP.Web.Services.Interfaces;

public interface ICompraOrden
{
    Task<IEnumerable<OrdenListarDto>> Listar(string codigoEmpresa, string codigoEjercicio, string codigoPeriodo = null, string flagOrigen = null);
    Task<Guid> Insertar(string codigoEmpresa, OrdenInsertarDto orden);
    Task Editar(string codigoEmpresa, Guid id, OrdenEditarDto orden);
    Task<OrdenObtenerDto> Obtener(string codigoEmpresa, Guid id);
    Task Eliminar(string codigoEmpresa, Guid id);
    Task<IEnumerable<OrdenCatalogoActualizarEstadoDto>> CatalogoActualizarEstado(string codigoEmpresa, string codigoEstado);
    Task ActualizarEstado(string codigoEmpresa, EstadoActualizarRequest estadoActualizar);
    Task<IEnumerable<OrdenCatalogoIngresarDto>> CatalogoIngresar(string codigoEmpresa, string codigoEjercicio);
    Task<IEnumerable<OrdenDetalleCatalogoIngresarDto>> CatalogoDetallesIngresar(string codigoEmpresa, string codigoOrden);
    Task<IEnumerable<OrdenCatalogoAnticiparDto>> CatalogoAnticipar(string codigoEmpresa); 
}
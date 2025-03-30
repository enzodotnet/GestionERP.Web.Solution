using GestionERP.Web.Models.Dtos.Servicio;
using GestionERP.Web.Models.Requests;

namespace GestionERP.Web.Services.Interfaces;

public interface IServicioOrden
{
    Task<IEnumerable<OrdenListarDto>> Listar(string codigoEmpresa, string codigoEjercicio, string codigoPeriodo = null, string flagOrigen = null);
    Task<Guid> Insertar(string codigoEmpresa, OrdenInsertarDto orden);
    Task Editar(string codigoEmpresa, Guid id, OrdenEditarDto orden);
    Task<OrdenObtenerDto> Obtener(string codigoEmpresa, Guid id);
    Task Eliminar(string codigoEmpresa, Guid id);
    Task<IEnumerable<OrdenCatalogoActualizarEstadoDto>> CatalogoActualizarEstado(string codigoEmpresa, string codigoEstado);
    Task ActualizarEstado(string codigoEmpresa, EstadoActualizarRequest estadoActualizar);
    Task<IEnumerable<OrdenCatalogoAceptarDto>> CatalogoAceptar(string codigoEmpresa, string codigoEjercicio, string flagEstadoAceptacion);
    Task<OrdenObtenerAceptarDto> ObtenerAceptar(string codigoEmpresa, Guid ordenId);
    Task<IEnumerable<OrdenCatalogoProvisionarDto>> CatalogoProvisionar(string codigoEmpresa, string flagRegistro, bool esRegularizableAnticipo = false);
    Task<IEnumerable<OrdenCatalogoAnticiparDto>> CatalogoAnticipar(string codigoEmpresa);
    Task<IEnumerable<OrdenCatalogoProducirDto>> CatalogoProducir(string codigoEmpresa, string codigoEjercicio);
    Task InsertarDetalleAceptacion(string codigoEmpresa, OrdenDetalleAceptacionInsertarDto aceptacionInsertar);
    Task EliminarDetalleAceptacion(string codigoEmpresa, OrdenDetalleAceptacionEliminarDto aceptacionEliminar);
}
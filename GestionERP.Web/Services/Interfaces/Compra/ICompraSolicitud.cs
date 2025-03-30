using GestionERP.Web.Models.Dtos.Compra;
using GestionERP.Web.Models.Requests;

namespace GestionERP.Web.Services.Interfaces;

public interface ICompraSolicitud
{
    Task<IEnumerable<SolicitudListarDto>> Listar(string codigoEmpresa, string codigoEjercicio, string codigoPeriodo = null);
    Task<Guid> Insertar(string codigoEmpresa, SolicitudInsertarDto solicitud);
    Task Editar(string codigoEmpresa, Guid id, SolicitudEditarDto solicitud);
    Task<SolicitudObtenerDto> Obtener(string codigoEmpresa, Guid id);
    Task Eliminar(string codigoEmpresa, Guid id);
    Task<IEnumerable<SolicitudCatalogoActualizarEstadoDto>> CatalogoActualizarEstado(string codigoEmpresa, string codigoEstado);
    Task ActualizarEstado(string codigoEmpresa, EstadoActualizarRequest estadoActualizar);
    Task<IEnumerable<SolicitudCatalogoAtenderDto>> CatalogoAtender(string codigoEmpresa, string codigoEjercicio, string codigoProcesoDocumento, string codigoLocal = null);
}
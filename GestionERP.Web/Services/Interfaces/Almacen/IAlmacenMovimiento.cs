using GestionERP.Web.Models.Dtos.Almacen;
using GestionERP.Web.Models.Requests;

namespace GestionERP.Web.Services.Interfaces;

public interface IAlmacenMovimiento
{
    Task<IEnumerable<MovimientoListarDto>> Listar(string codigoEmpresa, string codigoEjercicio, string codigoPeriodo = null, string flagTipoRegistro = null);
    Task<Guid> Insertar(string codigoEmpresa, MovimientoInsertarDto movimiento); 
    Task<MovimientoObtenerDto> Obtener(string codigoEmpresa, Guid id);
    Task Eliminar(string codigoEmpresa, Guid id); 
    Task ActualizarEstado(string codigoEmpresa, EstadoActualizarRequest estadoActualizar);  
    Task<IEnumerable<MovimientoCatalogoProvisionarDto>> CatalogoProvisionar(string codigoEmpresa, string codigoEjercicio, bool regularizaAnticipo = false);
}
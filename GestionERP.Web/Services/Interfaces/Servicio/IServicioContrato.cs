using GestionERP.Web.Models.Dtos.Servicio;
using GestionERP.Web.Models.Requests;

namespace GestionERP.Web.Services.Interfaces;

public interface IServicioContrato
{
    Task<IEnumerable<ContratoListarDto>> Listar(string codigoEmpresa, string codigoEjercicio, string codigoPeriodo = null, string flagTipoRegistro = null, string flagEstadoDevengo = null);
    Task<Guid> Insertar(string codigoEmpresa, ContratoInsertarDto contrato);
    Task Editar(string codigoEmpresa, Guid id, ContratoEditarDto contrato);
    Task<ContratoObtenerDto> Obtener(string codigoEmpresa, Guid id);
    Task Eliminar(string codigoEmpresa, Guid id);
    Task<IEnumerable<ContratoCatalogoActualizarEstadoDto>> CatalogoActualizarEstado(string codigoEmpresa, string codigoEstado);
    Task ActualizarEstado(string codigoEmpresa, EstadoActualizarRequest estadoActualizar);
    Task<IEnumerable<ContratoCatalogoProvisionarDto>> CatalogoProvisionar(string codigoEmpresa, string flagRegistroProvision, string flagRegistroOrigen);
    Task<IEnumerable<ContratoCatalogoDevengarDto>> CatalogoDevengar(string codigoEmpresa);
}
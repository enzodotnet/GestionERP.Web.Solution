using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalTipoImpuesto
{
    Task<IEnumerable<TipoImpuestoListarDto>> Listar();
    Task<Guid> Insertar(TipoImpuestoInsertarDto tipoImpuesto);
    Task Editar(Guid id, TipoImpuestoEditarDto tipoImpuesto);
    Task<TipoImpuestoObtenerDto> Obtener(Guid id);
    Task Eliminar(Guid id);
    Task<IEnumerable<TipoImpuestoCatalogoDto>> Catalogo(bool? esOperacionVenta = null);
    Task<decimal?> ObtenerPorcentaje(string codigoTipoImpuesto);
    Task<TipoImpuestoConsultaEsPredeterminadoDto> ConsultaEsPredeterminado();
    Task<TipoImpuestoObtenerPorCodigoDto> ObtenerPorCodigo(string codigoTipoImpuesto, bool? esOperacionVenta = null);
}
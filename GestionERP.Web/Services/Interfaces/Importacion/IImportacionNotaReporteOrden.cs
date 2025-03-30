using GestionERP.Web.Models.Dtos.Importacion;
namespace GestionERP.Web.Services.Interfaces;

public interface IImportacionNotaReporteOrden
{
    Task<IEnumerable<NotaReporteOrdenListarDto>> Listar(string codigoEmpresa);
    Task<Guid> Insertar(string codigoEmpresa, NotaReporteOrdenInsertarDto notaReporteOrden);
    Task Editar(string codigoEmpresa, Guid id, NotaReporteOrdenEditarDto notaReporteOrden);
    Task<NotaReporteOrdenObtenerDto> Obtener(string codigoEmpresa, Guid id);
	Task<NotaReporteOrdenObtenerPorCodigoDto> ObtenerPorCodigo(string codigoEmpresa, string codigoNotaReporteOrden);
	Task Eliminar(string codigoEmpresa, Guid id);
    Task<IEnumerable<NotaReporteOrdenCatalogoDto>> Catalogo(string codigoEmpresa); 
}
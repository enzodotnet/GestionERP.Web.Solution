using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;
public interface IPrincipalArticulo
{
    Task<IEnumerable<ArticuloListarDto>> Listar();
    Task <Guid>Insertar(ArticuloInsertarDto articulo);
    Task Editar(Guid id, ArticuloEditarDto articulo);
    Task<ArticuloObtenerDto> Obtener(Guid id);
    Task Eliminar(Guid id); 
    Task<IEnumerable<ArticuloCatalogoPorEmpresaDto>> CatalogoPorEmpresa(string codigoEmpresa); 
	Task<IEnumerable<ArticuloCatalogoPorEmpresaProcesoDocumentoDto>> CatalogoPorEmpresaProcesoDocumento(string codigoEmpresa, string codigoProcesoDocumento);
	Task<ArticuloObtenerPorCodigoEmpresaDto> ObtenerPorCodigoEmpresa(string codigoArticulo, string codigoEmpresa);
    Task<ArticuloObtenerPorCodigoEmpresaProcesoDocumentoDto> ObtenerPorCodigoEmpresaProcesoDocumento(string codigoArticulo, string codigoEmpresa, string codigoProcesoDocumento);
}
using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;
public interface IPrincipalDocumento
{
    Task<IEnumerable<DocumentoListarDto>> Listar();
    Task<Guid> Insertar(DocumentoInsertarDto documento);
    Task Editar(Guid id, DocumentoEditarDto documento);
    Task<DocumentoObtenerDto> Obtener(Guid id);
    Task<DocumentoObtenerPorCodigoDto> ObtenerPorCodigo(string codigoDocumento);
    Task Eliminar(Guid id);
    Task<IEnumerable<DocumentoCatalogoDto>> Catalogo();
    Task<IEnumerable<DocumentoCatalogoPorTipoEntidadDto>> CatalogoPorTipoEntidad(string flagTipoEntidad);
}
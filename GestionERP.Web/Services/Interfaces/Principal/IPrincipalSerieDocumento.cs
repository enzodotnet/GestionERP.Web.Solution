using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalSerieDocumento
{
    Task<IEnumerable<DocumentoListarDto>> Listar(string codigoEmpresa, string flagTipoEntidad = null);
    Task<Guid> Insertar(string codigoEmpresa, DocumentoInsertarDto documento);
    Task Editar(string codigoEmpresa, Guid id, DocumentoEditarDto documento);
    Task<DocumentoObtenerDto> Obtener(string codigoEmpresa, Guid id);
    Task Eliminar(string codigoEmpresa, Guid id); 
 
    Task<SerieDocumentoConsultaPorCodigoEmpresaDto> ConsultaPorCodigoEmpresa(string codigoSerieDocumento, string codigoDocumento, string codigoEmpresa);
    Task<IEnumerable<SerieDocumentoCatalogoPorEmpresaDto>> CatalogoPorEmpresa(string codigoEmpresa);
    Task<IEnumerable<SerieDocumentoCatalogoPorEmpresaSesionDto>> CatalogoPorEmpresaSesion(string codigoEmpresa, string flagTipoAccion, string codigoTipoDocumento);
}
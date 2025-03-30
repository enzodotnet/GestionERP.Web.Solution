using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalProcesoDocumento
{
    Task<IEnumerable<ProcesoDocumentoListarDto>> Listar(string codigoTipoDocumento);
    Task<IEnumerable<ProcesoDocumentoCatalogoPorTipoDocumentoDto>> CatalogoPorTipoDocumento(string codigoTipoDocumento);
    Task<ProcesoDocumentoObtenerDto> Obtener(Guid id);
    Task<IEnumerable<ProcesoDocumentoTipoArticuloCatalogoDto>> CatalogoTiposArticulo(string codigoProcesoDocumento);
}
using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalTipoDocumento
{
    Task<IEnumerable<TipoDocumentoListarDto>> Listar();
    Task<IEnumerable<TipoDocumentoCatalogoDto>> Catalogo();
    Task<TipoDocumentoObtenerDto> Obtener(Guid id);
}
using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalTipoTributo
{
    Task<IEnumerable<TipoTributoListarDto>> Listar();
    Task<IEnumerable<TipoTributoCatalogoDto>> Catalogo();
    Task<TipoTributoObtenerEsPredeterminadoDto> ObtenerEsPredeterminado();
    Task<TipoTributoObtenerDto> Obtener(Guid id);
}
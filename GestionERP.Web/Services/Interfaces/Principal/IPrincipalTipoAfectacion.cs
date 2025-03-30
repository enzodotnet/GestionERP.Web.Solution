using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalTipoAfectacion
{
    Task<IEnumerable<TipoAfectacionListarDto>> Listar();
    Task<IEnumerable<TipoAfectacionCatalogoPorTipoTributoDto>> CatalogoPorTipoTributo(string codigoTipoTributo);
    Task<TipoAfectacionObtenerEsPredeterminadoDto> ObtenerEsPredeterminado();
    Task<TipoAfectacionObtenerDto> Obtener(Guid id);
}
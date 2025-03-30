using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;
public interface IPrincipalRegion
{
    Task<IEnumerable<RegionListarDto>> Listar(string codigoPais);
    Task<IEnumerable<RegionCatalogoPorPaisDto>> CatalogoPorPais(string codigoPais);
    Task<IEnumerable<RegionCatalogoDto>> Catalogo();
    Task<RegionObtenerDto> Obtener(Guid id);
}
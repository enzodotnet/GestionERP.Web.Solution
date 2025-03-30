using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;
public interface IPrincipalProvincia
{
    Task<IEnumerable<ProvinciaListarDto>> Listar(string codigoRegion);
    Task<IEnumerable<ProvinciaCatalogoPorRegionDto>> CatalogoPorRegion(string codigoRegion);
    Task<ProvinciaObtenerDto> Obtener(Guid id);
}
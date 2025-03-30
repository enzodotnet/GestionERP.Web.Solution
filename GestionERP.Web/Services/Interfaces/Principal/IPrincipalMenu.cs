using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalMenu
{
    Task<IEnumerable<MenuListarDto>> Listar();
    Task<MenuObtenerDto> Obtener(Guid id);
    Task<IEnumerable<MenuCatalogoPorSesionDto>> CatalogoPorSesion(string codigoEmpresa = null);
    Task<IEnumerable<MenuObtenerItemRutaPorServicioDto>> ObtenerItemsRutaPorServicio(string codigoServicio, string codigoWebEmpresa = null);
}
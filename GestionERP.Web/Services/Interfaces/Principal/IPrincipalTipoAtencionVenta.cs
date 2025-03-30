using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalTipoAtencionVenta
{
    Task<IEnumerable<TipoAtencionVentaListarDto>> Listar();
    Task<IEnumerable<TipoAtencionVentaCatalogoDto>> Catalogo();
    Task<TipoAtencionVentaObtenerDto> Obtener(Guid id);
}
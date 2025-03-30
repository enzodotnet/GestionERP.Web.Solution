using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalTipoOperacionProveedor
{
    Task<IEnumerable<TipoOperacionProveedorListarDto>> Listar();
    Task<IEnumerable<TipoOperacionProveedorCatalogoDto>> Catalogo();
    Task<TipoOperacionProveedorObtenerDto> Obtener(Guid id);
}
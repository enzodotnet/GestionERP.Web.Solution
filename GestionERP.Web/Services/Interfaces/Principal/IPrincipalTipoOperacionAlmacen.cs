using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalTipoOperacionAlmacen
{
    Task<IEnumerable<TipoOperacionAlmacenListarDto>> Listar();
    Task<IEnumerable<TipoOperacionAlmacenCatalogoDto>> Catalogo();
    Task<TipoOperacionAlmacenObtenerDto> Obtener(Guid id);
}
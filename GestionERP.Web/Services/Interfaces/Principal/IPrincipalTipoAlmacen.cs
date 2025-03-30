using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalTipoAlmacen
{
    Task<IEnumerable<TipoAlmacenListarDto>> Listar();
    Task<IEnumerable<TipoAlmacenCatalogoDto>> Catalogo();
    Task<TipoAlmacenObtenerDto> Obtener(Guid id);
}
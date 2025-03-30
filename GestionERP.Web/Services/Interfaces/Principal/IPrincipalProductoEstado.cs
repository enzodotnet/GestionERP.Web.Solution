using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalProductoEstado
{
    Task<IEnumerable<ProductoEstadoListarDto>> Listar();
    Task<IEnumerable<ProductoEstadoCatalogoDto>> Catalogo();
    Task<ProductoEstadoObtenerEsPredeterminadoDto> ObtenerEsPredeterminado();
    Task<ProductoEstadoObtenerDto> Obtener(Guid id);
}
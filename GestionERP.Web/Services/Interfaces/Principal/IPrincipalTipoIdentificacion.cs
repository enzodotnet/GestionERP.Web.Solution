using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalTipoIdentificacion
{
    Task<IEnumerable<TipoIdentificacionListarDto>> Listar();
    Task<IEnumerable<TipoIdentificacionCatalogoDto>> Catalogo();
    Task<IEnumerable<TipoIdentificacionCatalogoPorTipoOrigenDto>> CatalogoPorTipoOrigen(string flagTipoOrigen);
    Task<TipoIdentificacionObtenerDto> Obtener(Guid id);
}
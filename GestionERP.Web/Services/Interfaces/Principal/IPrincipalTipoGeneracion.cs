using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalTipoGeneracion
{
    Task<IEnumerable<TipoGeneracionListarDto>> Listar();
    Task<IEnumerable<TipoGeneracionCatalogoDto>> Catalogo();
    Task<TipoGeneracionObtenerDto> Obtener(Guid id);
}
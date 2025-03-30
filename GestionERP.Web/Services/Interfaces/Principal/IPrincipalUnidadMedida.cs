using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalUnidadMedida
{
    Task<IEnumerable<UnidadMedidaListarDto>> Listar();
    Task<IEnumerable<UnidadMedidaCatalogoDto>> Catalogo();
    Task<UnidadMedidaObtenerDto> Obtener(Guid id);
}
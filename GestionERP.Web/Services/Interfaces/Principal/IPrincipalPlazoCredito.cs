using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalPlazoCredito
{
    Task<IEnumerable<PlazoCreditoListarDto>> Listar();
    Task<IEnumerable<PlazoCreditoCatalogoDto>> Catalogo();
    Task<PlazoCreditoObtenerDto> Obtener(Guid id);
}
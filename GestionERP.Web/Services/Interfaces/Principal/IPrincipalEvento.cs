using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalEvento
{
    Task<IEnumerable<EventoListarDto>> Listar();
    Task<IEnumerable<EventoCatalogoDto>> Catalogo();
    Task<EventoObtenerDto> Obtener(Guid id);
}
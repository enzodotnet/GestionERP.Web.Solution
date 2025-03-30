using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalEstado
{
    Task<IEnumerable<EstadoListarDto>> Listar();
    Task<EstadoObtenerDto> Obtener(Guid id);
}
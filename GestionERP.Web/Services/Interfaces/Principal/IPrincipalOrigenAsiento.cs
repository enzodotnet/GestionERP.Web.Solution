using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalOrigenAsiento
{
    Task<IEnumerable<OrigenAsientoListarDto>> Listar();
    Task<OrigenAsientoObtenerDto> Obtener(Guid id);
}
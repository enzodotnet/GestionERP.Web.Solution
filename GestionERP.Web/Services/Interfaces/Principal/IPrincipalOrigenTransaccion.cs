using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalOrigenTransaccion
{
    Task<IEnumerable<OrigenTransaccionListarDto>> Listar();
    Task<OrigenTransaccionObtenerDto> Obtener(Guid id);
}
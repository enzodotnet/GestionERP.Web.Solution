using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalMedioPago
{
    Task<IEnumerable<MedioPagoListarDto>> Listar();
    Task<IEnumerable<MedioPagoCatalogoDto>> Catalogo();
    Task<MedioPagoObtenerDto> Obtener(Guid id);
}
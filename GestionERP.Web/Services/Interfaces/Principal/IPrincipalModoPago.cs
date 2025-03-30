using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalModoPago
{
    Task<IEnumerable<ModoPagoListarDto>> Listar();
    Task<IEnumerable<ModoPagoCatalogoDto>> Catalogo(bool esConfigurable = false);
    Task<ModoPagoObtenerDto> Obtener(Guid id);
}
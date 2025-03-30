using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalTipoTransaccion
{
    Task<IEnumerable<TipoTransaccionListarDto>> Listar();
    Task<IEnumerable<TipoTransaccionCatalogoDto>> Catalogo();
    Task<TipoTransaccionObtenerDto> Obtener(Guid id);
}
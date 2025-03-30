using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalCasillaBalance
{
    Task<IEnumerable<CasillaBalanceListarDto>> Listar();
    Task<IEnumerable<CasillaBalanceCatalogoDto>> Catalogo();
    Task<CasillaBalanceObtenerDto> Obtener(Guid id);
}
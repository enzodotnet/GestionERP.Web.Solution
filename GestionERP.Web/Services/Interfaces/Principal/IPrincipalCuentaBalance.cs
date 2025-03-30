using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalCuentaBalance
{
    Task<IEnumerable<CuentaBalanceListarDto>> Listar();
    Task<IEnumerable<CuentaBalanceCatalogoDto>> Catalogo();
    Task<CuentaBalanceObtenerDto> Obtener(Guid id);
}
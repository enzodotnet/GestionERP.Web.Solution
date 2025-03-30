using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalTipoFacturaNegociable
{
    Task<IEnumerable<TipoFacturaNegociableListarDto>> Listar();
    Task<Guid> Insertar(TipoFacturaNegociableInsertarDto tipoFacturaNegociable);
    Task Editar(Guid id, TipoFacturaNegociableEditarDto tipoFacturaNegociable);
    Task<TipoFacturaNegociableObtenerDto> Obtener(Guid id);
    Task Eliminar(Guid id);
    Task<IEnumerable<TipoFacturaNegociableCatalogoPorCuentaBancariaDto>> CatalogoPorCuentaBancaria(int cuentaBancariaId); 
}
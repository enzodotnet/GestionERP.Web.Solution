using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;
public interface IPrincipalCuentaContable
{
    Task<IEnumerable<CuentaContableListarDto>> Listar();
    Task<Guid> Insertar(CuentaContableInsertarDto cuentaContable);
    Task Editar(Guid id, CuentaContableEditarDto cuentaContable);
    Task<CuentaContableObtenerDto> Obtener(Guid id);
    Task Eliminar(Guid id);
    Task<IEnumerable<CuentaContableCatalogoDto>> Catalogo(bool esCuentaDestino = false);
}
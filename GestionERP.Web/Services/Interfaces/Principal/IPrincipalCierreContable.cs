using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalCierreContable
{
    Task<IEnumerable<CierreContableListarDto>> Listar();
    Task<Guid> Insertar(CierreContableInsertarDto cierreContable);
    Task Editar(Guid id, CierreContableEditarDto cierreContable);
    Task<CierreContableObtenerDto> Obtener(Guid id);
    Task Eliminar(Guid id); 
}
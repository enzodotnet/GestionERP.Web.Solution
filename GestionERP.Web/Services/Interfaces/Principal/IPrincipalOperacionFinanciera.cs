using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;
public interface IPrincipalOperacionFinanciera
{
    Task<IEnumerable<OperacionFinancieraListarDto>> Listar();
    Task<Guid> Insertar(OperacionFinancieraInsertarDto operacionFinanciera);
    Task Editar(Guid id, OperacionFinancieraEditarDto operacionFinanciera);
    Task<OperacionFinancieraObtenerDto> Obtener(Guid id);
    Task Eliminar(Guid id);
    Task<IEnumerable<OperacionFinancieraCatalogoDto>> Catalogo();
}
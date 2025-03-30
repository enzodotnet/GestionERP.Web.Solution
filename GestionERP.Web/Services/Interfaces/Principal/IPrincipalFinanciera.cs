using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalFinanciera
{
    Task<IEnumerable<FinancieraListarDto>> Listar();
    Task<Guid> Insertar(FinancieraInsertarDto financiera);
    Task Editar(Guid id, FinancieraEditarDto financiera);
    Task<FinancieraObtenerDto> Obtener(Guid id);
    Task Eliminar(Guid id);
    Task<IEnumerable<FinancieraCatalogoDto>> Catalogo();
}
using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalGrupoDevengo
{
    Task<IEnumerable<GrupoDevengoListarDto>> Listar();
    Task<Guid> Insertar(GrupoDevengoInsertarDto grupoDevengo);
    Task Editar(Guid id, GrupoDevengoEditarDto grupoDevengo);
    Task<GrupoDevengoObtenerDto> Obtener(Guid id);
    Task Eliminar(Guid id);
    Task<IEnumerable<GrupoDevengoCatalogoDto>> Catalogo();
}
using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;
public interface IPrincipalGrupoArticulo
{
    Task<IEnumerable<GrupoArticuloListarDto>> Listar();
    Task<Guid> Insertar(GrupoArticuloInsertarDto grupo);
    Task Editar(Guid id, GrupoArticuloEditarDto grupo);
    Task<GrupoArticuloObtenerDto> Obtener(Guid id);
    Task Eliminar(Guid id);
    Task<IEnumerable<GrupoArticuloCatalogoDto>> Catalogo();
}
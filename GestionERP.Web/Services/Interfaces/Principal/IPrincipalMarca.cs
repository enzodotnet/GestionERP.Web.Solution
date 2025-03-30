using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalMarca
{
    Task<IEnumerable<MarcaListarDto>> Listar();
    Task<Guid> Insertar(MarcaInsertarDto marca);
    Task Editar(Guid id, MarcaEditarDto marca);
    Task<MarcaObtenerDto> Obtener(Guid id);
    Task Eliminar(Guid id);
    Task<IEnumerable<MarcaCatalogoDto>> Catalogo();
}
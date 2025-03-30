using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalGlosarioAnalisis
{
    Task<IEnumerable<GlosarioAnalisisListarDto>> Listar();
    Task<Guid> Insertar(GlosarioAnalisisInsertarDto glosarioAnalisis);
    Task Editar(Guid id, GlosarioAnalisisEditarDto glosarioAnalisis);
    Task<GlosarioAnalisisObtenerDto> Obtener(Guid id);
    Task<GlosarioAnalisisObtenerDescripcionTraducidaDto> ObtenerTraducidaPorDescripcion(string descripcion);
    Task Eliminar(Guid id);
    Task<IEnumerable<GlosarioAnalisisCatalogoPorRegistroIdiomaDto>> CatalogoPorRegistroIdioma(string flagRegistro, string flagIdiomaOriginal);
}
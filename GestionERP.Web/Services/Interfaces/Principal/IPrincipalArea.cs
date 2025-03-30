using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalArea
{
    Task<IEnumerable<AreaListarDto>> Listar();
    Task<Guid> Insertar(AreaInsertarDto area);
    Task Editar(Guid id, AreaEditarDto area);
    Task<AreaObtenerDto> Obtener(Guid id);
    Task<AreaObtenerPorCodigoDto> ObtenerPorCodigo(string codigoArea);
    Task Eliminar(Guid id);
    Task<IEnumerable<AreaCatalogoDto>> Catalogo();
}
using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalAduana
{
    Task<IEnumerable<AduanaListarDto>> Listar();
    Task<Guid> Insertar(AduanaInsertarDto aduana);
    Task Editar(Guid id, AduanaEditarDto aduana);
    Task<AduanaObtenerDto> Obtener(Guid id);
    Task<AduanaObtenerPorCodigoDto> ObtenerPorCodigo(string codigoAduana);
    Task Eliminar(Guid id);
    Task<IEnumerable<AduanaCatalogoDto>> Catalogo();
}
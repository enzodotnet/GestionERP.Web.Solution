using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalTipoExistencia
{
    Task<IEnumerable<TipoExistenciaListarDto>> Listar();
    Task<IEnumerable<TipoExistenciaCatalogoDto>> Catalogo();
    Task<TipoExistenciaObtenerDto> Obtener(Guid id);
}
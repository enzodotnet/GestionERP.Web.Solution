using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalTipoSituacionLetra
{
    Task<IEnumerable<TipoSituacionLetraListarDto>> Listar();
    Task<Guid> Insertar(TipoSituacionLetraInsertarDto tipoSituacionLetra);
    Task Editar(Guid id, TipoSituacionLetraEditarDto tipoSituacionLetra);
    Task<TipoSituacionLetraObtenerDto> Obtener(Guid id);
    Task Eliminar(Guid id);
    Task<IEnumerable<TipoSituacionLetraCatalogoDto>> Catalogo(); 
}
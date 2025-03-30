using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;
public interface IPrincipalDiario
{
    Task<IEnumerable<DiarioListarDto>> Listar();
    Task<Guid> Insertar(DiarioInsertarDto diario);
    Task Editar(Guid id, DiarioEditarDto diario);
    Task<DiarioObtenerDto> Obtener(Guid id);
    Task Eliminar(Guid id);
    Task<IEnumerable<DiarioCatalogoDto>> Catalogo();
}
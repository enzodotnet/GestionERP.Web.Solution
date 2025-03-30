using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalGrupoGastoImportacion
{
    Task<IEnumerable<GrupoGastoImportacionListarDto>> Listar();
    Task<Guid> Insertar(GrupoGastoImportacionInsertarDto grupoGastoImportacion);
    Task Editar(Guid id, GrupoGastoImportacionEditarDto grupoGastoImportacion);
    Task<GrupoGastoImportacionObtenerDto> Obtener(Guid id);
    Task Eliminar(Guid id);
    Task<IEnumerable<GrupoGastoImportacionCatalogoDto>> Catalogo();
}
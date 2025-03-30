using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalTipoGastoImportacion
{
    Task<IEnumerable<TipoGastoImportacionListarDto>> Listar();
    Task<Guid> Insertar(TipoGastoImportacionInsertarDto tipoGastoImportacion);
    Task Editar(Guid id, TipoGastoImportacionEditarDto tipoGastoImportacion);
    Task<TipoGastoImportacionObtenerDto> Obtener(Guid id);
    Task Eliminar(Guid id);
    Task<IEnumerable<TipoGastoImportacionCatalogoDto>> Catalogo();
}
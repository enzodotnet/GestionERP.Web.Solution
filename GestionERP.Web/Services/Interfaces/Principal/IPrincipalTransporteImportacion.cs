using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalTransporteImportacion
{
    Task<IEnumerable<TransporteImportacionListarDto>> Listar();
    Task<Guid> Insertar(TransporteImportacionInsertarDto transporteImportacion);
    Task Editar(Guid id, TransporteImportacionEditarDto transporteImportacion);
    Task<TransporteImportacionObtenerDto> Obtener(Guid id);
    Task<TransporteImportacionObtenerPorCodigoDto> ObtenerPorCodigo(string codigoTransporteImportacion);
    Task Eliminar(Guid id);
    Task<IEnumerable<TransporteImportacionCatalogoDto>> Catalogo();
}
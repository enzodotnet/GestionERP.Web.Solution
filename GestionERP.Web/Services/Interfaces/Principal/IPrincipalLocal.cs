using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalLocal
{
    Task<IEnumerable<LocalListarDto>> Listar();
    Task<Guid> Insertar(LocalInsertarDto local);
    Task Editar(Guid id, LocalEditarDto local);
    Task<LocalObtenerDto> Obtener(Guid id);
    Task Eliminar(Guid id);
    Task<IEnumerable<LocalCatalogoPorEmpresaDto>> CatalogoPorEmpresa(string codigoEmpresa);
    Task<LocalObtenerPorCodigoEmpresaDto> ObtenerPorCodigoEmpresa(string codigoLocal, string codigoEmpresa);
}
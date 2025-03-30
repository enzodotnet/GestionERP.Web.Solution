using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalOperacionLogistica
{
    Task<IEnumerable<OperacionLogisticaListarDto>> Listar();
    Task<Guid> Insertar(OperacionLogisticaInsertarDto operacionLogistica);
    Task Editar(Guid id, OperacionLogisticaEditarDto operacionLogistica);
    Task<OperacionLogisticaObtenerDto> Obtener(Guid id);
    Task Eliminar(Guid id);
    Task<IEnumerable<OperacionLogisticaCatalogoDto>> Catalogo();
    Task<IEnumerable<OperacionLogisticaCatalogoPorEmpresaSesionDto>> CatalogoPorEmpresaSesion(string codigoEmpresa, string flagTipoAccion);
}
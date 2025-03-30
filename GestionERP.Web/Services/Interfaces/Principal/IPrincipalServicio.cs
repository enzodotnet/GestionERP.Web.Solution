using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalServicio
{
    Task<IEnumerable<ServicioListarDto>> Listar(string codigoModulo);
    Task<Guid> Insertar(ServicioInsertarDto servicio);
    Task Editar(Guid id, ServicioEditarDto servicio);
    Task<ServicioObtenerDto> Obtener(Guid id);
    Task<ServicioObtenerPorCodigoDto> ObtenerPorCodigo(string codigoServicio);
    Task<ServicioConsultaPorCodigoDto> ConsultaPorCodigo(string codigoServicio);
    Task Eliminar(Guid id);
    Task<IEnumerable<ServicioCatalogoPorModuloDto>> CatalogoPorModulo(string codigoModulo);
    Task<IEnumerable<ServicioCatalogoDto>> Catalogo();
}
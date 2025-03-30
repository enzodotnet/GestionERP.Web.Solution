using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalTipoDevengo
{
    Task<IEnumerable<TipoDevengoListarDto>> Listar();
    Task<Guid> Insertar(TipoDevengoInsertarDto tipoDevengo);
    Task Editar(Guid id, TipoDevengoEditarDto tipoDevengo);
    Task<TipoDevengoObtenerDto> Obtener(Guid id);
    Task Eliminar(Guid id);
	Task<IEnumerable<TipoDevengoCatalogoDto>> Catalogo();
	Task<TipoDevengoObtenerPorCodigoDto> ObtenerPorCodigo(string codigoTipoDevengo);
}
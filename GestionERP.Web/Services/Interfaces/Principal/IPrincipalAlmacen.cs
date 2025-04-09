using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalAlmacen
{
    Task<IEnumerable<AlmacenListarDto>> Listar();
    Task<Guid> Insertar(AlmacenInsertarDto almacen);
    Task Editar(Guid id, AlmacenEditarDto almacen);
    Task<AlmacenObtenerDto> Obtener(Guid id);
    Task Eliminar(Guid id);
    Task<IEnumerable<AlmacenCatalogoDto>> Catalogo();
    Task<IEnumerable<AlmacenCatalogoPorEmpresaOperacionLogisticaSesionDto>> CatalogoPorEmpresaOperacionLogisticaSesion(string codigoEmpresa, string codigoOperacionLogistica, string codigoTipoArticulo = null, string codigoAlmacenDestino = null);
    Task<AlmacenObtenerPorCodigoEmpresaOperacionLogisticaSesionDto> ObtenerPorCodigoEmpresaOperacionLogisticaSesion(string codigoAlmacen, string codigoEmpresa, string codigoOperacionLogistica, string codigoTipoArticulo = null, string codigoAlmacenDestino = null);
}
using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalLineaArticulo
{
    Task<IEnumerable<LineaArticuloListarDto>> Listar(string codigoSegmentoArticulo);
    Task<Guid> Insertar(LineaArticuloInsertarDto lineaArticulo);
    Task Editar(Guid id, LineaArticuloEditarDto lineaArticulo);
    Task<LineaArticuloObtenerDto> Obtener(Guid id);
    Task Eliminar(Guid id);
    Task<IEnumerable<LineaArticuloCatalogoPorSegmentoDto>> CatalogoPorSegmento(string codigoSegmentoArticulo);
}
using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalSegmentoArticulo
{
    Task<IEnumerable<SegmentoArticuloListarDto>> Listar(string codigoGrupoArticulo);
    Task<Guid> Insertar(SegmentoArticuloInsertarDto linea);
    Task Editar(Guid id, SegmentoArticuloEditarDto linea);
    Task<SegmentoArticuloObtenerDto> Obtener(Guid id);
    Task Eliminar(Guid id);
    Task<IEnumerable<SegmentoArticuloCatalogoPorGrupoDto>> CatalogoPorGrupo(string codigoGrupoArticulo);
    Task<IEnumerable<SegmentoArticuloCatalogoDto>> Catalogo();
}
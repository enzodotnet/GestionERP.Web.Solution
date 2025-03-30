using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalTipoArticulo
{
    Task<IEnumerable<TipoArticuloListarDto>> Listar();
    Task<IEnumerable<TipoArticuloCatalogoDto>> Catalogo();
    Task<TipoArticuloObtenerPorCodigoDto> ObtenerPorCodigo(string codigoTipoArticulo);
    Task<TipoArticuloObtenerDto> Obtener(Guid id);
}
using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalTipoServicio
{
    Task<IEnumerable<TipoServicioListarDto>> Listar();    
    Task<IEnumerable<TipoServicioCatalogoDto>> Catalogo(string flagRegistro = null);
    Task<TipoServicioObtenerDto> Obtener(Guid id);
    Task<TipoServicioObtenerPorCodigoDto> ObtenerPorCodigo(string codigoTipoServicio, string flagRegistro = null);
}
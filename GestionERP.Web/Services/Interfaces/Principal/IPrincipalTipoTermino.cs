using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalTipoTermino
{
    Task<IEnumerable<TipoTerminoListarDto>> Listar();    
    Task<IEnumerable<TipoTerminoCatalogoDto>> Catalogo(string flagRegistro = null);
    Task<TipoTerminoObtenerDto> Obtener(Guid id);
    Task<TipoTerminoObtenerPorCodigoDto> ObtenerPorCodigo(string codigoTipoTermino, string flagRegistro = null);
}
using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalPuerto
{
    Task<IEnumerable<PuertoListarDto>> Listar();    
    Task<IEnumerable<PuertoCatalogoDto>> Catalogo();
    Task<IEnumerable<PuertoCatalogoPorPaisDto>> CatalogoPorPais(string codigoPais);
    Task<PuertoObtenerDto> Obtener(Guid id);
    Task<PuertoObtenerPorCodigoDto> ObtenerPorCodigo(string codigoPuerto);
}
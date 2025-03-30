using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalTipoAdjuntoRecepcion
{
    Task<IEnumerable<TipoAdjuntoRecepcionListarDto>> Listar();    
    Task<IEnumerable<TipoAdjuntoRecepcionCatalogoPorSeccionDto>> CatalogoPorSeccion(string flagSeccion);
    Task<TipoAdjuntoRecepcionObtenerDto> Obtener(Guid id);
}
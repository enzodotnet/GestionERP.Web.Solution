using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalTipoAdjuntoCertificado
{
    Task<IEnumerable<TipoAdjuntoCertificadoListarDto>> Listar();    
    Task<IEnumerable<TipoAdjuntoCertificadoCatalogoPorOrigenDto>> CatalogoPorOrigen(string flagOrigen);
    Task<TipoAdjuntoCertificadoObtenerDto> Obtener(Guid id);
}
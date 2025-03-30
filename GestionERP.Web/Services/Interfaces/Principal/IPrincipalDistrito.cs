using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;
public interface IPrincipalDistrito
{
    Task<IEnumerable<DistritoListarDto>> Listar(string codigoProvincia);
    Task<IEnumerable<DistritoCatalogoPorProvinciaDto>> CatalogoPorProvincia(string codigoProvincia);
    Task<DistritoObtenerDto> Obtener(Guid id);
}
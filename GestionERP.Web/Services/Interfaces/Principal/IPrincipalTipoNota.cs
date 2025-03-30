using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalTipoNota
{
    Task<IEnumerable<TipoNotaListarDto>> Listar();
    Task<IEnumerable<TipoNotaCatalogoPorTipoDto>> CatalogoPorTipo(string flagTipo);
    Task<TipoNotaObtenerDto> Obtener(Guid id);
}
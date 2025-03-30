using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalTipoFinanciera
{
    Task<IEnumerable<TipoFinancieraListarDto>> Listar();
    Task<IEnumerable<TipoFinancieraCatalogoDto>> Catalogo();
    Task<TipoFinancieraObtenerDto> Obtener(Guid id);
}
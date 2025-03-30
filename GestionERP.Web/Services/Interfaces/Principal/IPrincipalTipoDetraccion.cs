using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalTipoDetraccion
{
    Task<IEnumerable<TipoDetraccionListarDto>> Listar();
    Task<IEnumerable<TipoDetraccionCatalogoDto>> Catalogo();
    Task<TipoDetraccionObtenerDto> Obtener(Guid id);
}
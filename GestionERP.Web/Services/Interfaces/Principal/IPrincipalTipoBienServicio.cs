using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalTipoBienServicio
{
    Task<IEnumerable<TipoBienServicioListarDto>> Listar();
    Task<IEnumerable<TipoBienServicioCatalogoDto>> Catalogo();
    Task<TipoBienServicioObtenerDto> Obtener(Guid id);
}
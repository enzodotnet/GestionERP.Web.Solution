using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalFamiliaProducto
{
    Task<IEnumerable<FamiliaProductoListarDto>> Listar();
    Task<IEnumerable<FamiliaProductoCatalogoDto>> Catalogo();
    Task<FamiliaProductoObtenerDto> Obtener(Guid id);
}
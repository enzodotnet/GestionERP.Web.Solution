using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;
public interface IPrincipalChofer
{
    Task<IEnumerable<ChoferListarDto>> Listar();
    Task<Guid> Insertar(ChoferInsertarDto chofer);
    Task Editar(Guid id, ChoferEditarDto chofer);
    Task<ChoferObtenerDto> Obtener(Guid id);
    Task Eliminar(Guid id);
    Task<IEnumerable<ChoferCatalogoDto>> Catalogo();
}
using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalFabricante
{
    Task<IEnumerable<FabricanteListarDto>> Listar();
    Task<Guid> Insertar(FabricanteInsertarDto fabricante);
    Task Editar(Guid id, FabricanteEditarDto fabricante);
    Task<FabricanteObtenerDto> Obtener(Guid id);
    Task Eliminar(Guid id);
    Task<IEnumerable<FabricanteCatalogoDto>> Catalogo();
}
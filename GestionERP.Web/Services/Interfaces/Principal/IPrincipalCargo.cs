using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalCargo
{
    Task<IEnumerable<CargoListarDto>> Listar();
    Task<Guid> Insertar(CargoInsertarDto cargo);
    Task Editar(Guid id, CargoEditarDto cargo);
    Task<CargoObtenerDto> Obtener(Guid id);
    Task<CargoObtenerPorCodigoDto> ObtenerPorCodigo(string codigoCargo);
    Task Eliminar(Guid id);
    Task<IEnumerable<CargoCatalogoDto>> Catalogo();
}
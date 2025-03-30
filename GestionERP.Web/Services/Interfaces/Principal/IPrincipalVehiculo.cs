using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalVehiculo
{
    Task<IEnumerable<VehiculoListarDto>> Listar();
    Task<Guid> Insertar(VehiculoInsertarDto vehiculo);
    Task Editar(Guid id, VehiculoEditarDto vehiculo);
    Task<VehiculoObtenerDto> Obtener(Guid id);
    Task Eliminar(Guid id);
    Task<IEnumerable<VehiculoCatalogoDto>> Catalogo();
}
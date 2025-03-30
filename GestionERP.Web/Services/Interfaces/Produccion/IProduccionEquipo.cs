using GestionERP.Web.Models.Dtos.Produccion; 

namespace GestionERP.Web.Services.Interfaces;

public interface IProduccionEquipo
{
    Task<IEnumerable<EquipoListarDto>> Listar(string codigoEmpresa);
    Task<Guid> Insertar(string codigoEmpresa, EquipoInsertarDto plan);
    Task Editar(string codigoEmpresa, Guid id, EquipoEditarDto plan);
    Task<EquipoObtenerDto> Obtener(string codigoEmpresa, Guid id);
    Task Eliminar(string codigoEmpresa, Guid id);
    Task<IEnumerable<EquipoCatalogoDto>> Catalogo(string codigoEmpresa);
}
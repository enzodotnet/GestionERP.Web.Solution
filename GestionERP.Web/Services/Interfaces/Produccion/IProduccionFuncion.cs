using GestionERP.Web.Models.Dtos.Produccion; 

namespace GestionERP.Web.Services.Interfaces;

public interface IProduccionFuncion
{
    Task<IEnumerable<FuncionListarDto>> Listar(string codigoEmpresa);
    Task<Guid> Insertar(string codigoEmpresa, FuncionInsertarDto plan);
    Task Editar(string codigoEmpresa, Guid id, FuncionEditarDto plan);
    Task<FuncionObtenerDto> Obtener(string codigoEmpresa, Guid id);
    Task Eliminar(string codigoEmpresa, Guid id);
    Task<IEnumerable<FuncionCatalogoDto>> Catalogo(string codigoEmpresa);
}
using GestionERP.Web.Models.Dtos.Produccion; 

namespace GestionERP.Web.Services.Interfaces;

public interface IProduccionPersonal
{
    Task<IEnumerable<PersonalListarDto>> Listar(string codigoEmpresa);
    Task<Guid> Insertar(string codigoEmpresa, PersonalInsertarDto plan);
    Task Editar(string codigoEmpresa, Guid id, PersonalEditarDto plan);
    Task<PersonalObtenerDto> Obtener(string codigoEmpresa, Guid id);
    Task Eliminar(string codigoEmpresa, Guid id);
    Task<IEnumerable<PersonalCatalogoDto>> Catalogo(string codigoEmpresa);
    Task<PersonalObtenerPorCodigoDto> ObtenerPorCodigo(string codigoEmpresa, string codigoPersonal);
}
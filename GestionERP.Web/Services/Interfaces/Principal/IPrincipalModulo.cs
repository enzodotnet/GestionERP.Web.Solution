using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;
public interface IPrincipalModulo
{
    Task<IEnumerable<ModuloListarDto>> Listar();
    Task<Guid> Insertar(ModuloInsertarDto modulo);
    Task Editar(Guid id, ModuloEditarDto modulo);
    Task<ModuloObtenerDto> Obtener(Guid id);
    Task<ModuloObtenerPorCodigoDto> ObtenerPorCodigo(string codigoModulo);
    Task Eliminar(Guid id);
    Task<IEnumerable<ModuloCatalogoDto>> Catalogo(string flagTipoAcceso = null);
}
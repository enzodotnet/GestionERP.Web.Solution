using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalUsuario
{
    Task<IEnumerable<UsuarioListarDto>> Listar();
    Task<UsuarioObtenerDto> Obtener(Guid id);
    Task<IEnumerable<UsuarioCatalogoDto>> Catalogo();
    Task<Guid> Insertar(UsuarioInsertarDto usuario);
    Task Editar(Guid id, UsuarioEditarDto usuario);
    Task Eliminar(Guid id);
    Task<UsuarioObtenerPorSesionDto> ObtenerPorSesion();
    Task<UsuarioConsultaAccesoPorSesionDto> ConsultaAccesoPorSesion(string codigoWebEmpresa = null, string codigoModulo = null, string codigoServicio = null);
    Task<IEnumerable<UsuarioEmpresaCatalogoPorSesionDto>> CatalogoEmpresasPorSesion();
    Task<IEnumerable<UsuarioCatalogoPorEmpresaDto>> CatalogoPorEmpresa(string codigoEmpresa);
    Task<UsuarioConsultaPorEmpresaSesionDto> ConsultaPorEmpresaSesion(string codigoEmpresa);
    Task<UsuarioObtenerPorCodigoEmpresaDto> ObtenerPorCodigoEmpresa(string codigoUsuario, string codigoEmpresa);
    Task<IEnumerable<UsuarioModuloCatalogoPorEmpresaSesionDto>> CatalogoModulosPorEmpresaSesion(string codigoEmpresa);
}
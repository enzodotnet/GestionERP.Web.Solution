using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;
public interface IPrincipalPermiso
{
    Task<IEnumerable<PermisoListarDto>> Listar(string codigoServicio);
    Task<Guid> Insertar(PermisoInsertarDto permiso);
    Task Editar(Guid id, PermisoEditarDto permiso);
    Task<PermisoObtenerDto> Obtener(Guid id);
    Task Eliminar(Guid id);
    Task<IEnumerable<PermisoCatalogoDto>> Catalogo();
    Task<bool> ConsultaEsAsignadoPorSesion(string codigoPermiso, string codigoEmpresa = null);
}
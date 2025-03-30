using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalZonaDespacho
{
    Task<IEnumerable<ZonaDespachoListarDto>> Listar();
    Task<Guid> Insertar(ZonaDespachoInsertarDto zonaDespacho);
    Task Editar(Guid id, ZonaDespachoEditarDto zonaDespacho);
    Task<ZonaDespachoObtenerDto> Obtener(Guid id);
    Task Eliminar(Guid id);
    Task<IEnumerable<ZonaDespachoCatalogoDto>> Catalogo();
}
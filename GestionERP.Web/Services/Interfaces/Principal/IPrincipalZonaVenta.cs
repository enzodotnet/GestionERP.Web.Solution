using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalZonaVenta
{
    Task<IEnumerable<ZonaVentaListarDto>> Listar();
    Task<Guid> Insertar(ZonaVentaInsertarDto zonaVenta);
    Task Editar(Guid id, ZonaVentaEditarDto zonaVenta);
    Task<ZonaVentaObtenerDto> Obtener(Guid id);
    Task Eliminar(Guid id);
    Task<IEnumerable<ZonaVentaCatalogoDto>> Catalogo();
}
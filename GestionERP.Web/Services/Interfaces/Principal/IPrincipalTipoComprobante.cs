using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalTipoComprobante
{
    Task<IEnumerable<TipoComprobanteListarDto>> Listar();
    Task<IEnumerable<TipoComprobanteCatalogoDto>> Catalogo();
    Task<TipoComprobanteObtenerDto> Obtener(Guid id);
}
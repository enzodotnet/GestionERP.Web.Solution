using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalTipoMovimiento
{
    Task<IEnumerable<TipoMovimientoListarDto>> Listar();
    Task<IEnumerable<TipoMovimientoCatalogoDto>> Catalogo();
    Task<TipoMovimientoObtenerDto> Obtener(Guid id);
    Task<TipoMovimientoConsultaPorCodigoDto> ConsultaPorCodigo(string codigoTipoMovimiento);
}
using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalMoneda
{
    Task<IEnumerable<MonedaListarDto>> Listar();
    Task<IEnumerable<MonedaCatalogoDto>> Catalogo();
    Task<MonedaObtenerDto> Obtener(Guid id);
    Task<MonedaObtenerPorCodigoDto> ObtenerPorCodigo(string codigoMoneda);
    Task<MonedaObtenerPorTipoDto> ObtenerPorTipo(string flagTipo);
}
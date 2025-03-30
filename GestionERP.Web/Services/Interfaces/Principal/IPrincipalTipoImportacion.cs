using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalTipoImportacion
{
    Task<IEnumerable<TipoImportacionListarDto>> Listar();
    Task<IEnumerable<TipoImportacionCatalogoDto>> Catalogo();
    Task<TipoImportacionObtenerDto> Obtener(Guid id);
    Task<TipoImportacionObtenerPorCodigoDto> ObtenerPorCodigo(string codigoTipoImportacion);
}
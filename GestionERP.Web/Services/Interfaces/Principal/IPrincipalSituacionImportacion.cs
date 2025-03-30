using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalSituacionImportacion
{
    Task<IEnumerable<SituacionImportacionListarDto>> Listar();
    Task<IEnumerable<SituacionImportacionCatalogoDto>> Catalogo();
    Task<SituacionImportacionObtenerDto> Obtener(Guid id);
    Task<SituacionImportacionObtenerPorCodigoDto> ObtenerPorCodigo(string codigoSituacionImportacion);
}
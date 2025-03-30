using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalRegimenImportacion
{
    Task<IEnumerable<RegimenImportacionListarDto>> Listar();
    Task<IEnumerable<RegimenImportacionCatalogoDto>> Catalogo();
    Task<RegimenImportacionObtenerDto> Obtener(Guid id);
    Task<RegimenImportacionObtenerPorCodigoDto> ObtenerPorCodigo(string codigoRegimenImportacion);
}
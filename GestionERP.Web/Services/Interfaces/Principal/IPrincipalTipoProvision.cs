using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalTipoProvision
{
    Task<IEnumerable<TipoProvisionListarDto>> Listar();
    Task<IEnumerable<TipoProvisionCatalogoDto>> Catalogo();
    Task<TipoProvisionObtenerDto> Obtener(Guid id);
	Task<TipoProvisionObtenerPorCodigoDto> ObtenerPorCodigo(string codigoTipoProvision);
}
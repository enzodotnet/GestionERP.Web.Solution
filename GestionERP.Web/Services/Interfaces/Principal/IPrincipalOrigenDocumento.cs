using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalOrigenDocumento
{
    Task<IEnumerable<OrigenDocumentoListarDto>> Listar();
    Task<OrigenDocumentoObtenerDto> Obtener(Guid id);
}
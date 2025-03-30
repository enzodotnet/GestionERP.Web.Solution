using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalModalidadTraslado
{
    Task<IEnumerable<ModalidadTrasladoListarDto>> Listar();
    Task<IEnumerable<ModalidadTrasladoCatalogoDto>> Catalogo();
    Task<ModalidadTrasladoObtenerDto> Obtener(Guid id);
}
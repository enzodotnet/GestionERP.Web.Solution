using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalMotivoTraslado
{
    Task<IEnumerable<MotivoTrasladoListarDto>> Listar();
    Task<IEnumerable<MotivoTrasladoCatalogoDto>> Catalogo();
    Task<MotivoTrasladoObtenerDto> Obtener(Guid id);
}
using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalPais
{
    Task<IEnumerable<PaisListarDto>> Listar();
    Task<IEnumerable<PaisCatalogoDto>> Catalogo();
    Task<PaisObtenerDto> Obtener(Guid id);
    Task<PaisObtenerPorCodigoDto> ObtenerPorCodigo(string codigoPais);
}
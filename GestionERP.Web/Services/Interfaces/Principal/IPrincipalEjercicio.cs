using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalEjercicio
{
    Task<IEnumerable<EjercicioListarDto>> Listar(); 
    Task<EjercicioObtenerDto> Obtener(Guid id);
    Task<string> ObtenerCodigoPorAnio(int anio);
}
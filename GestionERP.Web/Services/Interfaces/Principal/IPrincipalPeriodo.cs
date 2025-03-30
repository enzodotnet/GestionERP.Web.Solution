using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalPeriodo
{
    Task<IEnumerable<PeriodoListarDto>> Listar(string codigoEjercicio);
    Task<PeriodoObtenerDto> Obtener(Guid id);
    Task<IEnumerable<PeriodoCatalogoPorEjercicioDto>> CatalogoPorEjercicio(string codigoEjercicio);
    Task<string> ConsultaCodigoPorFecha(DateTime fecha);
    Task<PeriodoDiaObtenerPorCodigoDto> ObtenerDiaPorCodigo(string codigoPeriodoDia);
    Task<IEnumerable<PeriodoDiaCatalogoPorPeriodoDto>> CatalogoDiasPorPeriodo(string codigoPeriodo);
    Task<string> ObtenerDiaCodigoPorFecha(DateTime fecha);
}
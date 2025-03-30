using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalTipoCambioDia
{
    Task<IEnumerable<TipoCambioDiaListarDto>> Listar(string codigoPeriodo);
    Task ActualizarMonto(TipoCambioDiaActualizarMontoDto tipoCambioDia);
    Task<TipoCambioDiaObtenerDto> Obtener(Guid id);
	Task<TipoCambioDiaObtenerPorFechaDto> ObtenerPorFecha(DateTime fecha, string flagTipo);
	Task<string> ObtenerCodigoPorFecha(DateTime fecha, string flagTipo);
    Task<decimal?> ObtenerMonto(string codigoTipoCambioDia);
}
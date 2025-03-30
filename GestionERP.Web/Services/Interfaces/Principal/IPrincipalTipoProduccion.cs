using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalTipoProduccion
{
    Task<IEnumerable<TipoProduccionListarDto>> Listar();
    Task<Guid> Insertar(TipoProduccionInsertarDto tipoProduccion);
    Task Editar(Guid id, TipoProduccionEditarDto tipoProduccion);
    Task<TipoProduccionObtenerDto> Obtener(Guid id);
    Task Eliminar(Guid id);
	Task<IEnumerable<TipoProduccionCatalogoDto>> Catalogo(string flagTipoProceso = null);
	Task<TipoProduccionObtenerPorCodigoDto> ObtenerPorCodigo(string codigoTipoProduccion, string flagTipoProceso = null);
    Task<TipoProduccionConsultaPorCodigoDto> ConsultaPorCodigo(string codigoTipoProduccion);
}
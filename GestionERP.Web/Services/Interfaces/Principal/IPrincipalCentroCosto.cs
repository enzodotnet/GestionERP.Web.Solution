using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalCentroCosto
{
    Task<IEnumerable<CentroCostoListarDto>> Listar();
    Task<Guid> Insertar(CentroCostoInsertarDto centroCosto);
    Task Editar(Guid id, CentroCostoEditarDto centroCosto);
    Task<CentroCostoObtenerDto> Obtener(Guid id);
    Task Eliminar(Guid id);
    Task<IEnumerable<CentroCostoCatalogoDto>> Catalogo(string codigoEmpresa = null); 
    Task<CentroCostoObtenerPorCodigoDto> ObtenerPorCodigo(string codigoCentroCosto, string codigoEmpresa = null);
}
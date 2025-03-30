using GestionERP.Web.Models.Dtos.Principal;
namespace GestionERP.Web.Services.Interfaces;
public interface IPrincipalEntidad
{
    Task<IEnumerable<EntidadListarDto>> Listar(bool esTransportista);
    Task<Guid> Insertar(EntidadInsertarDto entidad);
    Task Editar(Guid id, EntidadEditarDto entidad);
    Task<EntidadObtenerDto> Obtener(Guid id);
    Task Eliminar(Guid id);
    Task<IEnumerable<EntidadCatalogoDto>> Catalogo(bool esTransportista);
    Task<EntidadObtenerEmpresaGestionadaDto> ObtenerEmpresaGestionada(string codigoEmpresa);
    Task<IEnumerable<EntidadDireccionCatalogoDto>> CatalogoDirecciones(string codigoEntidad);
    Task<IEnumerable<EntidadVehiculoCatalogoDto>> CatalogoVehiculos(string codigoEntidad);
    Task<EntidadFichaSunatObtenerDto> ObtenerFichaSunat(string codigoEntidad);
    Task<EntidadDireccionObtenerEsPredeterminadoDto> ObtenerDireccionEsPredeterminado(string codigoEntidad);
    
    Task<IEnumerable<EntidadCatalogoPorEmpresaDto>> CatalogoPorEmpresa(string codigoEmpresa, string flagTipo = null);
    Task<EntidadConsultaPorEmpresaEsGestionadaDto> ConsultaPorEmpresaEsGestionada(string codigoEmpresa);
    Task<EntidadObtenerPorCodigoEmpresaDto> ObtenerPorCodigoEmpresa(string codigoEntidad, string codigoEmpresa, string flagTipo = null);
    Task<IEnumerable<EntidadProveedorCatalogoPorEmpresaDto>> CatalogoProveedoresPorEmpresa(string codigoEmpresa, string flagAmbito = null);
    Task<EntidadProveedorObtenerPorCodigoEmpresaDto> ObtenerProveedorPorCodigoEmpresa(string codigoEntidad, string codigoEmpresa, string flagAmbito = null); 
}
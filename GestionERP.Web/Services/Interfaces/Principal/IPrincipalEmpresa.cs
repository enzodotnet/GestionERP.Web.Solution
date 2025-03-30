using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Services.Interfaces;

public interface IPrincipalEmpresa
{
    Task<IEnumerable<EmpresaListarDto>> Listar();
    Task<Guid> Insertar(EmpresaInsertarDto empresa);
    Task Editar(Guid id, EmpresaEditarDto empresa);
    Task<EmpresaObtenerDto> Obtener(Guid id);
    Task<EmpresaObtenerPorCodigoDto> ObtenerPorCodigo(string codigoEmpresa);
    Task<EmpresaConsultaPorCodigoWebDto> ConsultaPorCodigoWeb(string codigoWebEmpresa);
    Task Eliminar(Guid id);
    Task<IEnumerable<EmpresaCatalogoDto>> Catalogo(); 
    Task<EmpresaAtributoObtenerDto> ObtenerAtributo(string codigoEmpresa);
    Task<EmpresaFacturacionObtenerDto> ObtenerFacturacion(string codigoEmpresa);
    
    Task<IEnumerable<EmpresaEjercicioCatalogoDto>> CatalogoEjercicios(string codigoEmpresa);
    Task<string> ConsultaEjercicioCodigoPorAnio(string codigoEmpresa, int anio);
    Task<EmpresaEjercicioConsultaIntervaloFechaDto> ConsultaEjercicioIntervaloFecha(string codigoEmpresa);
    Task<string> ConsultaPeriodoCodigoPorFecha(string codigoEmpresa, DateTime fecha);
    Task<IEnumerable<EmpresaModuloPeriodoConsultaFechaEsCerradoOperacionDto>> ConsultaModuloPeriodoFechasEsCerradoOperacion(string codigoEmpresa, string codigoModulo);
    Task<IEnumerable<EmpresaPeriodoCatalogoDto>> CatalogoPeriodos(string codigoEmpresa, string codigoEjercicio);
}
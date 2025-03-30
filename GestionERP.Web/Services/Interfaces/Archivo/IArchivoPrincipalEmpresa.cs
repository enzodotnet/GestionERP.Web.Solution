using GestionERP.Web.Models.Dtos.Archivo; 

namespace GestionERP.Web.Services.Interfaces;

public interface IArchivoPrincipalEmpresa
{
    Task<IEnumerable<PrincipalEmpresaListarDto>> Listar(Guid origenArchivoId);
    Task<Guid> Insertar(string codigoEmpresa, PrincipalEmpresaInsertarDto archivo);
    Task Eliminar(Guid archivoId);
    Task<PrincipalEmpresaObtenerDto> Obtener(Guid archivoId);
}
using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Services.Interfaces;
public interface IPrincipalActividadComercial
{
    Task<IEnumerable<ActividadComercialListarDto>> Listar();
    Task<Guid> Insertar(ActividadComercialInsertarDto actividad);
    Task Editar(Guid id, ActividadComercialEditarDto actividad);
    Task<ActividadComercialObtenerDto> Obtener(Guid id);
    Task Eliminar(Guid id); 
    Task<IEnumerable<ActividadComercialCatalogoDto>> Catalogo();
}
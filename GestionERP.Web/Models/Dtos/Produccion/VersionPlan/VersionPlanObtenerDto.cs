using GestionERP.Web.Models.Objects;

namespace GestionERP.Web.Models.Dtos.Produccion;

public class VersionPlanObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public int NumeroVersion { get; set; }
    public DateTime FechaRegistro { get; set; }
    public string Descripcion { get; set; }
    public string Observacion { get; set; }
    public string CodigoPlan { get; set; }
    public string NombrePlan { get; set; }
    public string CodigoTipoProduccion { get; set; }
    public string NombreTipoProduccion { get; set; }
    public string CodigoArticuloTerminado { get; set; }
    public string NombreArticuloTerminado { get; set; }
    public string CodigoUnidadMedida { get; set; }
    public string NombreUnidadMedida { get; set; }
    public string PresentacionArticulo { get; set; }
    public decimal Cantidad { get; set; }
    public string CodigoEstado { get; set; }
    public string NombreEstado { get; set; }
    public List<VersionPlanMaterialObtenerDto> Materiales { get; set; }
    public List<VersionPlanFuncionObtenerDto> Funciones { get; set; }
    public List<VersionPlanEquipoObtenerDto> Equipos { get; set; }
    public IEnumerable<AuditoriaObject> Auditorias { get; set; }
}
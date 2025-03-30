using GestionERP.Web.Models.Objects;

namespace GestionERP.Web.Models.Dtos.Produccion;

public class PlanObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public int NumeroPlan { get; set; }
    public DateTime FechaRegistro { get; set; }
    public string Descripcion { get; set; }
    public string CodigoTipoProduccion { get; set; }
    public string NombreTipoProduccion { get; set; }
    public string CodigoArticuloTerminado { get; set; }
    public string NombreArticuloTerminado { get; set; }
    public string CodigoUnidadMedida { get; set; }
    public string NombreUnidadMedida { get; set; }
    public string PresentacionArticulo { get; set; }
    public decimal Cantidad { get; set; }
    public string Observacion { get; set; }
    public string CodigoEstado { get; set; }
    public string NombreEstado { get; set; }
    public IEnumerable<AuditoriaObject> Auditorias { get; set; }
}
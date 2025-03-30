using GestionERP.Web.Models.Dtos.Control;

namespace GestionERP.Web.Models.Dtos.Servicio;

public class ContratoCuotaObtenerDto
{
    public Guid? Id { get; set; } 
    public int NumeroCuota { get; set; }
    public decimal? ImporteBruto { get; set; }
	public DateTime? FechaVencimiento { get; set; }
	public string Observacion { get; set; }
    public decimal ImporteBrutoAtendido { get; set; }
    public decimal ImporteBrutoRescindido { get; set; }
    public IEnumerable<CuotaProvisionObtenerDto> Provisiones { get; set; }
}
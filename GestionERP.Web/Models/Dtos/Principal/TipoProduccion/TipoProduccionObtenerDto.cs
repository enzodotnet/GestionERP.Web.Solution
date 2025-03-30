namespace GestionERP.Web.Models.Dtos.Principal;

public class TipoProduccionObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string FlagTipoProceso { get; set; }
    public string FlagAmbito { get; set; }
    public string FlagTipoEncargo { get; set; }
	public string CodigoAlmacenProceso { get; set; }
	public string NombreAlmacenProceso { get; set; }
	public bool EsExigibleTareo { get; set; }
    public bool EsExigibleMaquila { get; set; }
    public bool EsValidadorIngreso { get; set; }
    public bool EsValidadorSobrante { get; set; }
    public string Descripcion { get; set; }
    public bool Activo { get; set; } 
}
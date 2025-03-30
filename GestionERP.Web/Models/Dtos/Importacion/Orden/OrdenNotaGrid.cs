namespace GestionERP.Web.Models.Dtos.Importacion;

public class OrdenNotaGrid
{
    public Guid? Id { get; set; }
    public string CodigoNotaReporteOrden { get; set; }
    public string NombreNotaReporteOrden { get; set; }
	public string FlagSeccion { get; set; }
	public string Contenido { get; set; }
}
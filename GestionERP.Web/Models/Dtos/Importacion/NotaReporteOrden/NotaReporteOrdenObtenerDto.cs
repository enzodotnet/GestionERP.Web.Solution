namespace GestionERP.Web.Models.Dtos.Importacion;

public class NotaReporteOrdenObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string Contenido { get; set; } 
    public string FlagSeccion { get; set; }
    public bool Activo { get; set; }
}
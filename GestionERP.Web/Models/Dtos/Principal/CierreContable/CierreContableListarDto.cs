namespace GestionERP.Web.Models.Dtos.Principal;

public class CierreContableListarDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public DateTime FechaRegistro { get; set; }
    public int DiarioCierreId { get; set; }
    public string CodigoDiarioCierre { get; set; }
    public string ClaseDiarioCierre { get; set; }
    public string NombreClaseDiarioCierre { get; set; }
    public int DiarioAperturaId { get; set; }
    public string CodigoDiarioApertura { get; set; }
    public string ClaseDiarioApertura { get; set; }
    public string NombreClaseDiarioApertura { get; set; }
    public bool EsPredeterminado { get; set; }
    public string Descripcion { get; set; }
    public bool Activo { get; set; }
}
namespace GestionERP.Web.Models.Dtos.Principal;

public class EstadoListarDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public bool EsPrimario { get; set; }
    public string VerboAccion { get; set; }
    public string Descripcion { get; set; }  
}
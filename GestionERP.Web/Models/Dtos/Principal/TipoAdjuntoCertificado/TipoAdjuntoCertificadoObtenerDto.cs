namespace GestionERP.Web.Models.Dtos.Principal;

public class TipoAdjuntoCertificadoObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string FlagOrigen { get; set; }
    public int NumeroPrioridad { get; set; }
    public bool Activo { get; set; } 
}
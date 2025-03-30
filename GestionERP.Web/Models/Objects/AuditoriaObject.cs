namespace GestionERP.Web.Models.Objects;

public class AuditoriaObject
{ 
    public string CodigoEstado{ get; set; }
    public string NombreEstado { get; set; }
    public DateTime FechaHoraAccion { get; set; }
    public string CodigoUsuario { get; set; }
    public string NombreUsuario { get; set; }
    public string Motivo { get; set; }
}
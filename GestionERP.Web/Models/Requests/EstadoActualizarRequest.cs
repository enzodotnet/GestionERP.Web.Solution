namespace GestionERP.Web.Models.Requests;

public class EstadoActualizarRequest
{   
    public Guid RegistroId { get; set; }
    public string CodigoEstado { get; set; } 
    public string Motivo { get; set; }
}
namespace GestionERP.Web.Models.Dtos.Principal;

public class UsuarioConsultaAccesoPorSesionDto
{   
    public bool EsAccesoValido { get; set; }
    public bool EsSesionValida { get; set; }
    public string MensajeAlerta { get; set; }
    public string TipoAlerta { get; set; }
    public string UrlRetorno { get; set; }
}

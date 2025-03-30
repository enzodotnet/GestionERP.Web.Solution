namespace GestionERP.Web.Models.Responses;

public class ClaimsResponse
{
    public string Codigo { get; set; }
    public string Email { get; set; }
    public string Nick { get; set; }
    public string Nombre { get; set; }
    public string Perfil { get; set; }
    public ClaimsResponse()
    {
        Codigo = "";
        Email = "";
        Nick = "";
        Nombre = "";
        Perfil = "";
    }
}

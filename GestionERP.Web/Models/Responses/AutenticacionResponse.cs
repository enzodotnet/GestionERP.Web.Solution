namespace GestionERP.Web.Models.Responses;

public class AutenticacionResponse
{
    public string AuthToken { get; set; }
    public string IdToken { get; set;}
    public DateTime? ExpiryTime { get; set; }
    public AutenticacionResponse()
    {
        AuthToken = "";
        IdToken = "";
        ExpiryTime = null;
    }
}

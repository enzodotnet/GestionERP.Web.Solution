namespace GestionERP.Web.Models.Responses;

public class ErrorEndpointResponse
{  
    public string Code { get; set; }
    public string Message { get; set; }
    public ErrorEndpointResponse()
    { 
        Code = ""; 
        Message = "";
    }
}
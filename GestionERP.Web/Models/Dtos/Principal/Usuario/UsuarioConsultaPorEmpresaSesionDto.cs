namespace GestionERP.Web.Models.Dtos.Principal;

public class UsuarioConsultaPorEmpresaSesionDto
{ 
    public string CodigoUsuario { get; set; }
    public string NombreUsuario { get; set; }  
    public string CodigoArea { get; set; }
    public string NombreArea { get; set; } 
    public string CodigoCargo { get; set; }
    public string NombreCargo { get; set; } 
    public string CodigoLocal { get; set; }
    public string NombreLocal { get; set; } 
    public bool Activo { get; set; }
}
namespace GestionERP.Web.Models.Dtos.Principal;

public class MenuCatalogoPorSesionDto
{
    public string NumeroOrden { get; set; }
    public string CodigoMenu { get; set; }
    public string NombreMenu { get; set; } 
    public string Nivel { get; set; } 
    public string RutaWeb { get; set; }
    public string Icono { get; set; }
    public bool EsGrupo { get; set; }  
    public string CodigoMenuReferencia { get; set; }
    public bool EsVisible { get; set; }  
    public bool EsExpandido { get; set; }  
}
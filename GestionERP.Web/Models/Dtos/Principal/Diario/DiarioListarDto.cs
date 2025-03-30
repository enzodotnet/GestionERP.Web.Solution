namespace GestionERP.Web.Models.Dtos.Principal;

public class DiarioListarDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public string FlagTipoProceso { get; set; } 
    public string FlagTipoRegistro { get; set; }
    public string FlagTipoCambio { get; set; }  
    public bool Activo { get; set; }
}
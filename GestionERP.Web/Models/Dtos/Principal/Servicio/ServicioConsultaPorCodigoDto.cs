namespace GestionERP.Web.Models.Dtos.Principal;

public class ServicioConsultaPorCodigoDto
{  
    public string Nombre { get; set; }
    public string FlagOrigenAcceso { get; set; }
    public string FlagTipoFuncion { get; set; }
    public string CodigoModulo { get; set; }
    public string NombreModulo { get; set; }
    public bool EsAdjuntableArchivo { get; set; } 
}
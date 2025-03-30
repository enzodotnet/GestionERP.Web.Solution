namespace GestionERP.Web.Models.Dtos.Produccion;

public class PersonalObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string CodigoTipoIdentificacion { get; set; }
    public string SiglaTipoIdentificacion { get; set; }
    public string NombreTipoIdentificacion { get; set; }
    public string NumeroTipoIdentificacion { get; set; } 
    public string CodigoFuncion { get; set; }
    public string NombreFuncion { get; set; }
    public string Observacion { get; set; }
    public bool Activo { get; set; }
}
namespace GestionERP.Web.Models.Dtos.Servicio;

public class ContratoTerminoObtenerDto
{
    public Guid? Id { get; set; } 
    public string CodigoTipoTermino { get; set; }
    public string NombreTipoTermino { get; set; }
    public int NumeroTermino { get; set; }
    public string Descripcion { get; set; } 
}
using System.Text.Json.Serialization;

namespace GestionERP.Web.Models.Dtos.Principal;

public class EntidadRepresentanteObtenerDto
{
    public Guid? Id { get; set; }
    public string CodigoTipoIdentificacion { get; set; }
    public string SiglaTipoIdentificacion { get; set; } 
    public string NombreTipoIdentificacion { get; set; }
    public string NumeroTipoIdentificacion { get; set; }
    public string Nombre { get; set; }
    public string DescripcionCargo { get; set; }
    public DateTime? FechaInicioCargo { get; set; }
    public bool Activo { get; set; } 
}
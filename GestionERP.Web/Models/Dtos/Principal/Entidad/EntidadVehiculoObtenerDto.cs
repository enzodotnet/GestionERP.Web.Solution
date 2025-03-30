using System.Text.Json.Serialization;

namespace GestionERP.Web.Models.Dtos.Principal;

public class EntidadVehiculoObtenerDto
{
    public Guid? Id { get; set; }
    public string CodigoVehiculo { get; set; }
    public string MarcaVehiculo { get; set; } 
    public string Descripcion { get; set; } 
    public bool Activo { get; set; } 
}
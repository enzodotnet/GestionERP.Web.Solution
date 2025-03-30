using System.Text.Json.Serialization;

namespace GestionERP.Web.Models.Dtos.Principal;

public class CuentaContableDestinoObtenerDto
{
    public Guid? Id { get; set; }
    public string FlagTipo { get; set; } 
    public string CodigoCuentaContableGeneraTemp { get; set; }
    public string CodigoCuentaContableGenera { get; set; }
    public string NombreCuentaContableGenera { get; set; }
}
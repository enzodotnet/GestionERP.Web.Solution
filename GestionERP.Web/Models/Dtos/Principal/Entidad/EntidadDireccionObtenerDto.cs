using System.Text.Json.Serialization;

namespace GestionERP.Web.Models.Dtos.Principal;

public class EntidadDireccionObtenerDto
{
    public Guid? Id { get; set; }
    public string Codigo { get; set; }
    public int NumeroItem { get; set; }
    public string CodigoRegion { get; set; }
    public string NombreRegion { get; set; }
    public string CodigoProvincia { get; set; }
    public string NombreProvincia { get; set; }
    public string CodigoDistrito { get; set; }
    public string NombreDistrito { get; set; }
    public string Nombre { get; set; }
    public string Referencia { get; set; }
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
    public bool EsOficina { get; set; }
    public bool EsPuntoLlegada { get; set; }
    public bool EsNormal { get; set; }
    public bool EsPredeterminado { get; set; }
    public bool Activo { get; set; }
}
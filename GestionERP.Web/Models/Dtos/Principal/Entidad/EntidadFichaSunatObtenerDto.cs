using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class EntidadFichaSunatObtenerDto
{ 
    public string NombreComercial { get; set; }
    public string ActividadPrincipal { get; set; }
    public string ActividadSecundariaN1 { get; set; }
    public string ActividadSecundariaN2 { get; set; }
    public string DescripcionTipoContribuyente { get; set; }
    public string EstadoContribuyente { get; set; } 
    public string FlagCondicionContribuyente { get; set; } 
    public bool EsAfectoNuevoRus { get; set; }
    public bool EsAgenteRetenedor { get; set; }
    public bool EsEmisorElectronico { get; set; }
    public DateTime? FechaInscripcion { get; set; }
    public DateTime? FechaInicioActividad { get; set; }
    public DateTime? FechaCondicionNohabido { get; set; }
}
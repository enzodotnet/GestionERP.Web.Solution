namespace GestionERP.Web.Models.Dtos.Principal;

public class TipoAdjuntoRecepcionObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string FlagSeccion { get; set; }
    public bool EsTipoInforme { get; set; }
    public bool EsObligatorio { get; set; }
    public bool Activo { get; set; }
}
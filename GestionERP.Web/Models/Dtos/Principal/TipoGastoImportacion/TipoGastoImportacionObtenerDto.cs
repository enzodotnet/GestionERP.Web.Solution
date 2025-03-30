namespace GestionERP.Web.Models.Dtos.Principal;

public class TipoGastoImportacionObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public string FlagTipo { get; set; }
    public bool Activo { get; set; }
}
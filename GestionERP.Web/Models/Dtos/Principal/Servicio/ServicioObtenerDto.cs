namespace GestionERP.Web.Models.Dtos.Principal;

public class ServicioObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string CodigoModulo { get; set; }
    public string NombreModulo { get; set; }
    public string FlagTipoFuncion { get; set; }
    public bool EsAdjuntableArchivo { get; set; }
    public string Descripcion { get; set; }
    public bool Activo { get; set; }
}
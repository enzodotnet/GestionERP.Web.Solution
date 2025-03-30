namespace GestionERP.Web.Models.Dtos.Principal;

public class GlosarioAnalisisObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string FlagRegistro { get; set; }
    public string FlagIdiomaOriginal { get; set; }
    public string Descripcion { get; set; }
    public string DescripcionTraducida { get; set; }
    public bool Activo { get; set; }
}
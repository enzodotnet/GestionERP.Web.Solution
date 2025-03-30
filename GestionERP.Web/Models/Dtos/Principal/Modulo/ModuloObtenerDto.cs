namespace GestionERP.Web.Models.Dtos.Principal;

public class ModuloObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public string FlagTipoAcceso { get; set; }
    public bool EsOperacional { get; set; }
    public string CodigoWeb { get; set; }
    public bool Activo { get; set; }
}
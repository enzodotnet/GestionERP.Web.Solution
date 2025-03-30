namespace GestionERP.Web.Models.Dtos.Principal;

public class LineaArticuloObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string CodigoSegmentoArticulo { get; set; }
    public string NombreSegmentoArticulo { get; set; }
    public string CodigoGrupoArticulo { get; set; }
    public string NombreGrupoArticulo { get; set; }
    public string Descripcion { get; set; }
    public bool Activo { get; set; }
}
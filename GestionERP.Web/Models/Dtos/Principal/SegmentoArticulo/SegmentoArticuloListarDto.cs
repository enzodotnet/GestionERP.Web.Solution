namespace GestionERP.Web.Models.Dtos.Principal;

public class SegmentoArticuloListarDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string CodigoGrupoArticulo { get; set; }
    public string NombreGrupoArticulo { get; set; }
    public string Descripcion { get; set; }
    public bool Activo { get; set; }
}
namespace GestionERP.Web.Models.Dtos.Principal;

public class ArticuloObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public string FlagCategoria { get; set; }
    public string CodigoUnidadMedida { get; set; }
    public string NombreUnidadMedida { get; set; }
    public string CodigoGrupoArticulo { get; set; }
    public string NombreGrupoArticulo { get; set; }
    public string CodigoSegmentoArticulo { get; set; }
    public string NombreSegmentoArticulo { get; set; }
    public string CodigoLineaArticulo { get; set; }
    public string NombreLineaArticulo { get; set; }
    public int NumeroLineaArticulo { get; set; } 
    public string CodigoFamiliaProducto { get; set; }
    public string NombreFamiliaProducto { get; set; }
    public bool EsAfectoImpuesto { get; set; }
    public bool Activo { get; set; }
}
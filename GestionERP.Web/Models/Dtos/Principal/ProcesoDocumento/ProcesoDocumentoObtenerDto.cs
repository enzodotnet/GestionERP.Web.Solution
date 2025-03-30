namespace GestionERP.Web.Models.Dtos.Principal;

public class ProcesoDocumentoObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; } 
    public string CodigoTipoDocumento { get; set; }
    public string NombreTipoDocumento { get; set; } 
    public string FlagTipoAccion { get; set; }
    public bool EsAgregableTipoArticulo { get; set; }
    public bool EsProcesoReferencia { get; set; }
    public bool Activo { get; set; }
    public IEnumerable<ProcesoDocumentoTipoArticuloObtenerDto> TiposArticulo { get; set; }
    public IEnumerable<ProcesoDocumentoReferenciaObtenerDto> Referencias { get; set; }
}
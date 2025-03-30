namespace GestionERP.Web.Models.Dtos.Principal;

public class EntidadProveedorCatalogoPorEmpresaDto
{ 
    public string CodigoEntidad { get; set; }
    public string NombreEntidad { get; set; }
    public string CodigoTipoIdentificacion { get; set; }
    public string SiglaTipoIdentificacion { get; set; }
    public string FlagAmbito { get; set; }
    public string CodigoModoPago { get; set; }
    public string NombreModoPago { get; set; }
    public string CodigoPlazoCredito { get; set; }
    public string NombrePlazoCredito { get; set; }
    public int? CantidadDiasCredito { get; set; }
    public string FlagVinculo { get; set; } 
}
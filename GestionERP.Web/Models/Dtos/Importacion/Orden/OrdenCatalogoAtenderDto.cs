namespace GestionERP.Web.Models.Dtos.Importacion;

public class OrdenCatalogoAtenderDto
{
	public string CodigoPeriodo { get; set; }
    public string CodigoEntidad { get; set; }
    public string NombreEntidad { get; set; }
    public string CodigoOrden { get; set; }
    public string NombreSerieDocumentoOrden { get; set; } 
    public string CodigoTipoImportacion { get; set; }
    public string NombreTipoImportacion { get; set; }
    public string CodigoMoneda { get; set; }
    public string NombreMoneda { get; set; }
    public string SimboloMoneda { get; set; } 
    public DateTime FechaEmision { get; set; } 
    public DateTime FechaEstimadaETD { get; set; }
    public DateTime FechaEstimadaETA { get; set; }
    public DateTime FechaReposicionStock { get; set; }
    public string DescripcionFormaPago { get; set; }
    public string CodigoAduana { get; set; }
    public string NombreAduana { get; set; }
    public string CodigoPaisOrigen { get; set; }
    public string NombrePaisOrigen { get; set; }
    public string CodigoPaisProcedencia { get; set; }
    public string NombrePaisProcedencia { get; set; }
}
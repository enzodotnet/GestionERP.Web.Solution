namespace GestionERP.Web.Models.Dtos.Principal;

public class SerieDocumentoConsultaPorCodigoEmpresaDto
{
    public string NombreSerieDocumento { get; set; }
    public int SecuenciaNumero { get; set; }
    public string CodigoProcesoDocumento { get; set; }
    public string NombreProcesoDocumento { get; set; }
    public string CodigoLocal { get; set; }
    public string NombreLocal { get; set; }
    public string RutaHostImpresora { get; set; }
    public string NombreImpresora { get; set; }
}
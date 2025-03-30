namespace GestionERP.Web.Models.Dtos.Principal;

public class SerieDocumentoCatalogoPorEmpresaSesionDto
{  
    public string CodigoDocumento { get; set; }
    public string CodigoSerieDocumento { get; set; }
    public string NombreSerieDocumento { get; set; } 
    public int SecuenciaNumero { get; set; }
    public string CodigoProcesoDocumento { get; set; }
    public string NombreProcesoDocumento { get; set; }
    public string CodigoLocal { get; set; }
    public string NombreLocal { get; set; } 
}
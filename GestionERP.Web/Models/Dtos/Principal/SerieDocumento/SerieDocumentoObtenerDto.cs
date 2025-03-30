namespace GestionERP.Web.Models.Dtos.Principal;

public class SerieDocumentoObtenerDto
{
    public Guid? Id { get; set; }
    public string Serie { get; set; }
    public string Nombre { get; set; }
    public int SecuenciaNumero { get; set; }
    public int NumeroInicio { get; set; }
    public int NumeroFinal { get; set; }
    public string CodigoProcesoDocumento { get; set; }
    public string NombreProcesoDocumento { get; set; }
    public string Descripcion { get; set; }
    public string CodigoLocal { get; set; }
    public string NombreLocal { get; set; }
    public string RutaHostImpresora { get; set; }
    public string NombreImpresora { get; set; }
    public bool Activo { get; set; } 
}
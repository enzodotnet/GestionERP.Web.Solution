namespace GestionERP.Web.Models.Dtos.Principal;

public class TipoSituacionLetraListarDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string FlagTipoLetra { get; set; }
    public string Abreviacion { get; set; }
    public bool EnCarteraGeneral { get; set; }
    public int? DocumentoGeneraId { get; set; }
    public string CodigoDocumentoGenera { get; set; }
    public string NombreDocumentoGenera { get; set; }
    public string Descripcion { get; set; }
    public bool Activo { get; set; }
}
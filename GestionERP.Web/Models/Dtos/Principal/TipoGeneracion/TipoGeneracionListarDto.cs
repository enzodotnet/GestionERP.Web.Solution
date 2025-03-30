namespace GestionERP.Web.Models.Dtos.Principal;

public class TipoGeneracionListarDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public bool CreaDocumento { get; set; }
    public bool CreaOperacionFinanciera { get; set; }
    public bool CreaOperacionLogistica { get; set; }
    public bool CreaOperacionLogisticaDestino { get; set; }
    public bool CreaDiario { get; set; }
    public bool Activo { get; set; }
}
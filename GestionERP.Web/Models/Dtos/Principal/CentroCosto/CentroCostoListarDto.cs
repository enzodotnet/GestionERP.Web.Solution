namespace GestionERP.Web.Models.Dtos.Principal;

public class CentroCostoListarDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string CodigoGrupoCosto { get; set; }
    public string NombreGrupoCosto { get; set; }
    public string Abreviacion { get; set; }
    public string Descripcion { get; set; }
    public bool EsAutomatico { get; set; }
    public string FlagOrigen { get; set; }
    public bool Activo { get; set; } 
}
namespace GestionERP.Web.Models.Dtos.Principal;

public class FinancieraListarDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string FlagRegistro { get; set; }
    public string CodigoTipoFinanciera { get; set; }
    public string NombreTipoFinanciera { get; set; }
    public string AbreviacionNombre { get; set; }
    public bool Activo { get; set; }
}